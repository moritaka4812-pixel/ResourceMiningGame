
using Microsoft.VisualBasic.Devices;
using Craftory.Controller;
using Craftory.Core;
using Craftory.GameUI;
using Craftory.Input;
using Craftory.Maps;
using Craftory.Maps.Buildings;
using Craftory.Maps.Shadow;
using Craftory.Maps.Tiles;
using Craftory.Renderer;
using Craftory.ScreenUI;
using Craftory.System;
using Craftory.UI.Core;
using Craftory.UI.Elements;
using Craftory.UI.Setup;
using Button = Craftory.UI.Elements.Button;
using Color = Microsoft.Xna.Framework.Color;
using Panel = Craftory.UI.Elements.Panel;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.Screens
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
        Button settingsButton; //セッティングボタン
        ToolPanel toolPanel; //左に表示されるツールパネル
        InformationPanel informationPanel; //右に表示される情報パネル
        public BuildModeController buildModeController;

        public GamePlayScreen(Game1 game) : base(game)
        {
            camera = new Camera(new Vector2(0f, 0f), game); //カメラの初期位置
            cameraController = new CameraController();
            cameraSystem = new CameraSystem(camera, cameraController);
            GameCore.Instance.MapManager.MapSet(new Map1(), game.GraphicsDevice);
            tileSelectionController = new TileSelectionController(GameCore.Instance.MapManager.Map);
            tileSelectionSystem = new TileSelectionSystem(tileSelectionController);
            tileSelectionRenderer = new TileSelectionRenderer(game.GraphicsDevice);
            uiSet = new SetUIElements();
            this.LoadContent();
        }
        public override bool IsTransparent => true;
        public override void LoadContent()
        {
            var ui = new UIFactory(game); //UIを生成するインスタンス
            GameCore.Instance.MapManager.Map.LoadContent(game.Content); //マップをロード

            var builder = new GamePlayUIScreenBuilder(game, this, camera);
            (settingsButton, toolPanel, informationPanel) = builder.BuildUI();

            buildModeController = new BuildModeController(GameCore.Instance.MapManager, toolPanel, game, camera, this);

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
            uiConsumed |= informationPanel.Update(game.Input.Mouse);

            if (buildModeController.IsActive && !uiConsumed)
            {
                uiConsumed |= buildModeController.confirmPanel.UpdateWorld(game.Input.Mouse);
                if (!uiConsumed)
                    buildModeController.Update(game.Input.Mouse);

            }

            //UIがホイールの入力を吸収していないときだけカメラ操作
            if (!uiConsumed)
                cameraSystem.Update(game, dt);

            //UIが入力を吸収していないときだけタイル操作
            if (!uiConsumed)
                tileSelectionSystem.Update(game.Input.Mouse, camera);

            //タイル更新(アニメーション)
            GameCore.Instance.MapManager.Update(gameTime, camera, game.GraphicsDevice);

        }

        public override void Draw(SpriteBatch sb)
        {

            // 影更新
            GameCore.Instance.MapManager.shadowGenerator.UpdateShadowTexture(sb);



            //ワールド座標での描画
            sb.Begin(
            SpriteSortMode.Deferred,      // ← ソートしない（安定）
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            camera.GetViewMatrix()        // ← ワールド座標で描画
            );

            var range = GameCore.Instance.MapManager.Map.GetVisibleRange(camera, game.GraphicsDevice); //描画範囲内のレンジを取得
            GameCore.Instance.MapManager.Draw(sb, camera); //範囲内のマップをDraw

            if (buildModeController.IsActive)
            {
                buildModeController.Draw(sb);
            }

            sb.End();

            // 選択枠（ズームなし）
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            // 選択タイルのハイライト
            tileSelectionRenderer.DrawScreenSpace(sb, tileSelectionSystem.SelectedTile, camera);
            sb.End();

            //UIの描画（スクリーン座標）
            var raster = new RasterizerState() { ScissorTestEnable = true };

            sb.Begin(SpriteSortMode.Deferred,
                     BlendState.AlphaBlend,
                     SamplerState.PointClamp,
                     DepthStencilState.None,
                     raster);

            toolPanel.Draw(sb);
            settingsButton.Draw(sb);
            informationPanel.Draw(sb);

            sb.End();

        }

    }
}