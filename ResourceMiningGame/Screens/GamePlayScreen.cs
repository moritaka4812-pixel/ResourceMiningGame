
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps;
using ResourceMiningGame.Maps.Tiles;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.Screens
{
    public class GamePlayScreen : ScreenBase
    {
        private Texture2D pixel; //Draw用のテクスチャ
        public Tile selectedTile = null; //選択したタイルを格納
        Camera camera; //カメラの視点移動用
        IMap map; //マップ情報
        public GamePlayScreen(Game1 game) : base (game) 
        {
            camera = new Camera(new Vector2(0f,0f)); //カメラの初期位置
            map = new Map1(); 
            this.LoadContent();
        }

        public void LoadContent()
        {
            map.LoadContent(game.Content); //マップをロード

            pixel = new Texture2D(game.GraphicsDevice, 1, 1); //Draw用のテクスチャ作成
            pixel.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime); //カメラ系のUpdate

            var mouse = Mouse.GetState(); //ウィンドウ用のマウス位置取得

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
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                Vector2 screenPos = mouse.Position.ToVector2(); //スクリーン座標を取得

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
        }
    }
}
