
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
        IMap map; //マップ情報
        Button settingsButton; //セッティングボタン
        Button backButton; //バックボタン
        Button backToTitleButton; //タイトルに戻るボタン
        bool isSettingsOpen = false; //セッティング状態か

        public GamePlayScreen(Game1 game) : base (game) 
        {
            camera = new Camera(new Vector2(0f,0f)); //カメラの初期位置
            controller = new CameraController();

            map = new Map1(); 
            this.LoadContent();
        }

        public void LoadContent()
        {
            map.LoadContent(game.Content); //マップをロード
            settingsButton = new Button(
                game.GraphicsDevice,
                game.Content.Load<Texture2D>("UI/gear"),
                new Rectangle(760, 20, 32, 32)
                );
            settingsButton.SetBackgroundColor(Color.White);

            backButton = new Button(game.GraphicsDevice,
                game.Content.Load<SpriteFont>("Fonts/MyFont"),
                new Rectangle(350,350, 350, 100),
                "back");

            backToTitleButton = new Button(
                game.GraphicsDevice,
                game.Content.Load<SpriteFont>("Fonts/MyFont"),
                new Rectangle(350, 150, 350, 100),
                "Back To Title");

            pixel = new Texture2D(game.GraphicsDevice, 1, 1); //Draw用のテクスチャ作成
            pixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; //フレーム間の変化量

            controller.Update(game.Input); //入力でカメラコントローラーの状態を更新

            camera.ZoomBy(controller.ZoomDelta); //Zoom量だけZoom

            if (controller.MoveDirection != Vector2.Zero) //キーが押されていたら
                camera.Move(controller.MoveDirection * 500f * dt/ camera.Zoom); //その方向に移動(500fはワールド座標での補正量 1f = 1px)
            
            if (controller.DragDelta != Vector2.Zero) //マウスがミドルキーを押しながらドラッグされていたら
                camera.Drag(controller.DragDelta); //カメラ自体がZoomを補正して移動


            //セッティングボタンが押されたかの処理
            if (settingsButton.Update(game.Input.Mouse))
            {
                isSettingsOpen = true;
            }
            //セッティングメニューの処理
            if (isSettingsOpen)
            {
                if (backButton.Update(game.Input.Mouse))
                {
                    isSettingsOpen = false;
                }

                if (backToTitleButton.Update(game.Input.Mouse))
                {
                    game.ChangeScreen(new TitleScreen(game));
                }
            }

            //タイル更新(アニメーション)
            var range = map.GetVisibleRange(camera, game.GraphicsDevice); //画面内のタイルの範囲を取得

            for (int y = range.StartY; y < range.EndY; y++) //画面内のタイルのみ
            {
                for (int x = range.StartX; x < range.EndX; x++)
                {
                    map.MapTiles[x, y].Update(gameTime); //画面内のタイルのアニメーションを行う
                }
            }

            //左クリックでタイル選択
            if (game.Input.Mouse.LeftClicked())
            {
                Vector2 screenPos = game.Input.Mouse.Current.Position.ToVector2(); //スクリーン座標を取得

                Matrix inverse = Matrix.Invert(camera.GetViewMatrix()); //カメラ行列の逆行列を取得
                Vector2 worldPos = Vector2.Transform(screenPos, inverse);　//逆行列でスクリーン座標をワールド座標に変換

                //タイル座標に変換
                int tileX = (int)(worldPos.X / 16);
                int tileY = (int)(worldPos.Y / 16);

                //範囲チェック
                if(tileX >= 0 && tileX < map.MapSizeX && tileY >= 0 && tileY < map.MapSizeY)
                {
                    selectedTile = map.GetTile(tileX, tileY);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            //ワールド座標での描画
            sb.Begin(transformMatrix : camera.GetViewMatrix()); //描画座標を指定してDrawをワールド座標基準で描画できるようにする

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

            if (isSettingsOpen)
            {
                sb.Draw(pixel, new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                    new Color(0, 0, 0, 150));
                backButton.Draw(sb);
                backToTitleButton.Draw(sb);
            }
            sb.End();
        }
    }
}
