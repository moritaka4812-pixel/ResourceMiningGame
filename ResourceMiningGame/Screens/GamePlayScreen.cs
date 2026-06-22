
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
        bool[,] previewOccupied; //プレビューでの占有マップ
        List<Point>[,] previewOwner; //占有タイルからorigin
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

                foreach (var origin in buildTargets)
                {
                    Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

                    previewAnim.Draw(sb, worldPos, Color.White * 0.5f);
                }
                foreach(var origin in invalidTargets)
                {
                    Vector2 worldPos = new Vector2(origin.X * 32, origin.Y * 32);

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
            previewOccupied = new bool[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            previewOwner = new List<Point>[mapManager.Map.MapSizeX, mapManager.Map.MapSizeY];
            for (int x = 0; x < mapManager.Map.MapSizeX; x++)
                for (int y = 0; y < mapManager.Map.MapSizeY; y++)
                    previewOwner[x, y] = new List<Point>();
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
            var info = BuildingRegistry.Data[currentBuildType];

            // pが既存のプレビュー建物の占有タイルか
            var owners = previewOwner[p.X, p.Y];
            if (owners.Count > 0)
            {
                //最後に追加された建物を優先して消去
                var origin = owners.Last();
                RemovePreviewBuilding(origin, info);
                return;
            }

            //以下新規追加の処理
            List<Point> area = new();
            for (int x = 0; x < info.SizeInTiles.X; x++)
            {
                for (int y = 0; y < info.SizeInTiles.Y; y++)
                {
                    area.Add(new Point(p.X + x, p.Y + y));
                }
            }

            //実マップの判定
            bool canPlace = true;
            foreach(var pos in area)
            {
                var t = mapManager.Map.GetTile(pos.X, pos.Y);
                if(t == null || t.IsOccupied || !t.IsBuildable)
                {
                    canPlace = false;
                    break;
                }
            }

            //プレビュー内の重複判定
            foreach(var pos in area)
            {
                if (previewOccupied[pos.X, pos.Y])
                {
                    canPlace = false;
                    break;
                }
            }

            if (canPlace)
                buildTargets.Add(p);
            else 
                invalidTargets.Add(p);

            //仮想マップに追加
            foreach(var pos in area)
            {
                previewOccupied[pos.X, pos.Y] = true;
                previewOwner[pos.X, pos.Y].Add(p);
            }

            confirmButtonWorldPos = worldPos + new Vector2(10, 10);
        }

        private void RemovePreviewBuilding(Point origin, BuildingInfo info)
        {
            buildTargets.Remove(origin);
            invalidTargets.Remove(origin);

            for(int x = 0; x < info.SizeInTiles.X; x++)
            {
                for(int y = 0; y < info.SizeInTiles.Y; y++)
                {
                    var pos = new Point(origin.X + x, origin.Y + y);

                    previewOwner[pos.X, pos.Y].Remove(origin);

                    //他の建物が残っていればoccupiedのまま
                    previewOccupied[pos.X, pos.Y] = previewOwner[pos.X, pos.Y].Count > 0;
                }
            }
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