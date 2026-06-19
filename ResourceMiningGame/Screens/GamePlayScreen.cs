
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
        private List<Point> buildTargets = new(); //建設する位置
        private Vector2 confirmButtonWorldPos; //建設を決定するかのボタン位置
        private BuildType currentBuildType = BuildType.None; //現在の建設モードでの建設する建物
        private WorldPanel confirmPanel;
        public Tile selectedTile = null; //選択したタイルを格納
        Camera camera; //画面表示用のカメラ
        CameraController cameraController;
        CameraSystem cameraSystem; //カメラを管理するインスタンス
        TileAnimator tileAnimator; //タイルのアニメーションを管理
        TileSelectionController tileSelectionController; //タイル選択を処理
        TileSelectionSystem tileSelectionSystem;
        TileSelectionRenderer tileSelectionRenderer;
        IMap map; //マップ情報
        Button settingsButton; //セッティングボタン
        ToolPanel toolPanel; //左に表示されるツールパネル

        public GamePlayScreen(Game1 game) : base(game)
        {
            camera = new Camera(new Vector2(0f, 0f), game); //カメラの初期位置
            cameraController = new CameraController();
            cameraSystem = new CameraSystem(camera, cameraController);
            map = new Map1();
            tileAnimator = new TileAnimator(map);
            tileSelectionController = new TileSelectionController(map);
            tileSelectionSystem = new TileSelectionSystem(tileSelectionController);
            tileSelectionRenderer = new TileSelectionRenderer(game.GraphicsDevice);
            uiSet = new SetUIElements();
            this.LoadContent();
        }
        public override bool IsTransparent => true;
        public override void LoadContent()
        {
            var ui = new UIFactory(game); //UIを生成するインスタンス
            map.LoadContent(game.Content); //マップをロード

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

            if (isBuildMode)
            {
                UpdateBuildMode(game.Input.Mouse, camera);
                uiConsumed |= confirmPanel.UpdateWorld(game.Input.Mouse);
                return;
            }

            //UIがホイールの入力を吸収していないときだけカメラ操作
            if (!uiConsumed)
                cameraSystem.Update(game, dt);

                //UIが入力を吸収していないときだけタイル操作
            if (!uiConsumed)
                tileSelectionSystem.Update(game.Input.Mouse, camera);

            //タイル更新(アニメーション)
            tileAnimator.UpdateVisibleTiles(gameTime, camera, game.GraphicsDevice);

        }

        public override void Draw(SpriteBatch sb)
        {
            //ワールド座標での描画
            sb.Begin(transformMatrix: camera.GetViewMatrix()); //描画座標を指定してDrawをワールド座標基準で描画できるようにする

            var range = map.GetVisibleRange(camera, game.GraphicsDevice); //描画範囲内のレンジを取得
            map.Draw(sb, range); //範囲内のマップをDraw

            // 選択タイルのハイライト
            tileSelectionRenderer.Draw(sb, tileSelectionSystem.SelectedTile);

            if (isBuildMode)
                confirmPanel.DrawWorld(sb);

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
        }

        private void UpdateBuildMode(MouseInput mouse, Camera camera)
        {
            var worldPos = camera.ScreenToWorld(mouse.Current.Position.ToVector2());
            if (confirmPanel.HitTestWorld(worldPos)) return;

            var tilePos = map.WorldToTile(worldPos);

            if(mouse.LeftClicked() || mouse.RightDragging())
            {
                buildTargets.Add(tilePos);
                confirmButtonWorldPos = worldPos;
            }

            confirmPanel.X = (int)confirmButtonWorldPos.X;
            confirmPanel.Y = (int)confirmButtonWorldPos.Y;
        }
    }
}