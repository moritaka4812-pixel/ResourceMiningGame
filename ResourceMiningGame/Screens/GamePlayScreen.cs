
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.UI.Setup;
using ResourceMiningGame.Controller;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Button = ResourceMiningGame.UI.Elements.Button;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.UI.Elements;
using ResourceMiningGame.ScreenUI;
using ResourceMiningGame.System;
using ResourceMiningGame.Renderer;

namespace ResourceMiningGame.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        private Texture2D pixel; //Draw用のテクスチャ
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
            camera = new Camera(new Vector2(0f, 0f)); //カメラの初期位置
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

            var builder = new GamePlayUIScreenBuilder(game);
            (settingsButton, toolPanel) = builder.BuildUI();

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

            sb.End();

            //UIの描画（スクリーン座標）
            sb.Begin();

            toolPanel.Draw(sb);

            settingsButton.Draw(sb);

            sb.End();
        }
    }
}