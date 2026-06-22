
using Microsoft.VisualBasic.Devices;
using ResourceMiningGame.Controller;
using ResourceMiningGame.Core;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.Input;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.Maps.Shadow;
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
        public BuildModeController buildModeController;

        public GamePlayScreen(Game1 game) : base(game)
        {
            camera = new Camera(new Vector2(0f, 0f), game); //カメラの初期位置
            cameraController = new CameraController();
            cameraSystem = new CameraSystem(camera, cameraController);
            mapManager = new MapManager(new Map1(), game.GraphicsDevice);
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
            (settingsButton, toolPanel) = builder.BuildUI();

            buildModeController = new BuildModeController(mapManager, toolPanel, game, camera, this);

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

            if (buildModeController.IsActive && !uiConsumed)
            {
                uiConsumed |= buildModeController.confirmPanel.UpdateWorld(game.Input.Mouse);
                if(!uiConsumed)
                    buildModeController.Update(game.Input.Mouse, camera);

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

            if (buildModeController.IsActive)
            {
                buildModeController.Draw(sb);
            }

            sb.End();

            //UIの描画（スクリーン座標）
            sb.Begin();

            toolPanel.Draw(sb);

            settingsButton.Draw(sb);

            sb.End();
        }

    }
}