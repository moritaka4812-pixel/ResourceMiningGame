
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.UI;
using ResourceMiningGame.Controller;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Button = ResourceMiningGame.UI.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ResourceMiningGame.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        private Texture2D pixel; //Draw用のテクスチャ
        public Tile selectedTile = null; //選択したタイルを格納
        Camera camera; //画面表示用のカメラ
        CameraController controller; //カメラの移動を管理する
        TileAnimator tileAnimator; //タイルのアニメーションを管理
        TileSelectionController tileSelectionController; //タイル選択を処理
        IMap map; //マップ情報
        Button settingsButton; //セッティングボタン

        public GamePlayScreen(Game1 game) : base(game)
        {
            camera = new Camera(new Vector2(0f, 0f)); //カメラの初期位置
            controller = new CameraController();
            map = new Map1();
            tileAnimator = new TileAnimator(map);
            tileSelectionController = new TileSelectionController(map);
            this.LoadContent();
        }
        public override bool IsTransparent => true;
        public void LoadContent()
        {
            var ui = new UIFactory(game); //UIを生成するインスタンス
            map.LoadContent(game.Content); //マップをロード

            settingsButton = ui.CreateImageButton(760, 20, 32, 32, "UI/gear"); //セッティングボタンを生成
            settingsButton.SetBackgroundColor(Color.White); //背景色を再設定

            pixel = new Texture2D(game.GraphicsDevice, 1, 1); //Draw用のテクスチャ作成
            pixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //フレーム間の変化量

            controller.Update(game.Input); //入力からカメラの意図[移動・ズーム・ドラッグ]の状態を更新
            controller.ApplyToCamera(camera, dt); //更新された操作意図をCameraに適用して動かす


            //セッティングボタンが押されたかの処理
            if (settingsButton.Update(game.Input.Mouse))
            {
                game.PushScreen(new GameSettingScreen(game));
            }

            //タイル更新(アニメーション)
            tileAnimator.UpdateVisibleTiles(gameTime, camera, game.GraphicsDevice);

            //左クリックでタイル選択
            selectedTile = tileSelectionController.SelectTile(game.Input.Mouse, camera);

        }

        public override void Draw(SpriteBatch sb)
        {
            //ワールド座標での描画
            sb.Begin(transformMatrix: camera.GetViewMatrix()); //描画座標を指定してDrawをワールド座標基準で描画できるようにする

            var range = map.GetVisibleRange(camera, game.GraphicsDevice); //描画範囲内のレンジを取得
            map.Draw(sb, range); //範囲内のマップをDraw

            // 選択タイルのハイライト
            if (selectedTile != null)
            {
                var pos = selectedTile.Position;
                int size = 16;

                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, size, 1), Color.Yellow); //上
                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y + size - 1, size, 1), Color.Yellow); //下
                sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, 1, size), Color.Yellow); //左
                sb.Draw(pixel, new Rectangle((int)pos.X + size - 1, (int)pos.Y, 1, size), Color.Yellow); //右

            }
            sb.End();

            //UIの描画（スクリーン座標）
            sb.Begin();

            settingsButton.Draw(sb);

            sb.End();
        }
    }
}