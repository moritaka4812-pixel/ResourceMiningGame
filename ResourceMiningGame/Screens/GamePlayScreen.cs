
using Microsoft.VisualBasic.Devices;
using ResourceMiningGame.Controller;
using ResourceMiningGame.Core;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.Input;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.Renderer;
using ResourceMiningGame.ScreenUI;
using ResourceMiningGame.System;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.UI.Elements;
using ResourceMiningGame.UI.Setup;
using Button = ResourceMiningGame.UI.Elements.Button;
using Color = Microsoft.Xna.Framework.Color;
using Panel = ResourceMiningGame.UI.Elements.Panel;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        private Texture2D pixel; //Draw用のテクスチャ
        private bool isBuildMode = false; //建設モードかを管理
        private List<Point> buildTargets = new(); //建設する位置の一時リスト
        private List<Point> invalidTargets = new(); //建設不可の一時リスト
        private Vector2 confirmButtonWorldPos; //建設を決定するかのボタン位置
        private BuildType currentBuildType = BuildType.None; //現在の建設モードでの建設する建物
        private WorldPanel confirmPanel; //確認ボタンをのせるPanel
        public Tile selectedTile = null; //選択したタイルを格納
        Camera camera; //画面表示用のカメラ
        CameraController cameraController;
        CameraSystem cameraSystem; //カメラを管理するインスタンス
        TileSelectionController tileSelectionController; //タイル選択を処理
        TileSelectionSystem tileSelectionSystem;
        TileSelectionRenderer tileSelectionRenderer;
        MapManager mapManager; //マップ情報などを管理
        Button settingsButton; //セッティングボタン
        ToolPanel toolPanel; //左に表示されるツールパネル

        public GamePlayScreen(Game1 game) : base(game)
        {
            camera = new Camera(new Vector2(0f, 0f), game); //カメラの初期位置
            cameraController = new CameraController();
            cameraSystem = new CameraSystem(camera, cameraController);
            mapManager = new MapManager(new Map1());
            tileSelectionController = new TileSelectionController(mapManager.Map);
            tileSelectionSystem = new TileSelectionSystem(tileSelectionController);
            tileSelectionRenderer = new TileSelectionRenderer(game.GraphicsDevice);
            uiSet = new SetUIElements();
            this.LoadContent();
        }
        public override bool IsTransparent => true;
        public override void LoadContent()
        {
            var ui = new UIFactory(game); //UIを生成するインスタンス
            mapManager.Map.LoadContent(game.Content); //マップをロード

            var builder = new GamePlayUIScreenBuilder(game, this, camera);
            (settingsButton, toolPanel, confirmPanel) = builder.BuildUI();

            uiSet.Add(settingsButton);

            pixel = new Texture2D(game.GraphicsDevice, 1, 1); //Draw用のテクスチャ作成
            pixel.SetData(new[] { Color.White });

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //フレーム間の変化量

            //UIのマウス入力判定
            bool uiConsumed = false;
            uiConsumed |= toolPanel.Update(game.Input.Mouse);
            uiConsumed |= settingsButton.Update(game.Input.Mouse);

            if (isBuildMode && !uiConsumed)
            {
                uiConsumed |= confirmPanel.UpdateWorld(game.Input.Mouse);
                if(!uiConsumed)
                    UpdateBuildMode(game.Input.Mouse, camera);

            }

            //UIがホイールの入力を吸収していないときだけカメラ操作
            if (!uiConsumed)
                cameraSystem.Update(game, dt);

                //UIが入力を吸収していないときだけタイル操作
            if (!uiConsumed)
                tileSelectionSystem.Update(game.Input.Mouse, camera);

            //タイル更新(アニメーション)
            mapManager.Update(gameTime, camera, game.GraphicsDevice);

        }

        public override void Draw(SpriteBatch sb)
        {
            //ワールド座標での描画
            sb.Begin(transformMatrix: camera.GetViewMatrix()); //描画座標を指定してDrawをワールド座標基準で描画できるようにする
            
            var range = mapManager.Map.GetVisibleRange(camera, game.GraphicsDevice); //描画範囲内のレンジを取得
            mapManager.Draw(sb, camera); //範囲内のマップをDraw

            // 選択タイルのハイライト
            tileSelectionRenderer.Draw(sb, tileSelectionSystem.SelectedTile);

            if (isBuildMode)
            {
                var info = BuildingRegistry.Data[currentBuildType];
                var previewAnim = info.CreateTileAnimation();

                foreach (var tile in buildTargets)
                {
                    Vector2 worldPos = new Vector2(tile.X * 32, tile.Y * 32);

                    previewAnim.Draw(sb, worldPos, Color.White * 0.5f);
                }
                foreach(var tile in invalidTargets)
                {
                    Vector2 worldPos = new Vector2(tile.X * 32, tile.Y * 32);

                    previewAnim.Draw(sb, worldPos, Color.Red * 0.4f);
                }
                confirmPanel.DrawWorld(sb);
            }

            sb.End();

            //UIの描画（スクリーン座標）
            sb.Begin();

            toolPanel.Draw(sb);

            settingsButton.Draw(sb);

            sb.End();
        }

        public void EnterBuildMode(BuildType type)
        {
            isBuildMode = true;
            currentBuildType = type;
            buildTargets.Clear();
            invalidTargets.Clear();
            confirmPanel.Visible = false;
        }

        private void UpdateBuildMode(MouseInput mouse, Camera camera)
        {
            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            if (confirmPanel.HitTestWorld(worldPos)) return;

            var tilePos = mapManager.Map.WorldToTile(worldPos);
            if (tilePos == null) return;

            var p = tilePos.Value;
            var tile = mapManager.Map.GetTile(p.X, p.Y);

            //左クリック
            if (mouse.LeftClicked())
            {
                HandleBuildTarget(p, tile, worldPos);
            }

            //左ドラッグ
            if(mouse.LeftDragging())
            {
                if (!buildTargets.Contains(p) && !invalidTargets.Contains(p)) //連続で同じタイルを追加しない
                    HandleBuildTarget(p, tile, worldPos);
            }

            confirmPanel.X = (int)confirmButtonWorldPos.X;
            confirmPanel.Y = (int)confirmButtonWorldPos.Y;
        }

        private void HandleBuildTarget(Point p, Tile tile, Vector2 worldPos)
        {
            confirmPanel.Visible = true;
            if (!tile.IsOccupied && tile.IsBuildable)
                if(buildTargets.Contains(p))
                    buildTargets.Remove(p);

                 else buildTargets.Add(p);
            else
                if(invalidTargets.Contains(p))
                    invalidTargets.Remove(p);

                else invalidTargets.Add(p);

            confirmButtonWorldPos = new Vector2(worldPos.X + 10, worldPos.Y + 10);
        }

        public void ConfirmBuild()
        {
            if (buildTargets.Count == 0) return;
            foreach(var tile in buildTargets)
            {
                mapManager.AddBuilding(currentBuildType, tile);
            }

            confirmPanel.Visible = false;
            isBuildMode = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
        }

        public void CancelBuild()
        {
            confirmPanel.Visible = false;
            isBuildMode = false;
            buildTargets.Clear();
            invalidTargets.Clear();
            toolPanel.ClearActiveButton();
        }
    }
}