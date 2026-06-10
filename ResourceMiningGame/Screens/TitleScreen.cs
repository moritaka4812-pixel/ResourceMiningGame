using Rect = Microsoft.Xna.Framework.Rectangle;
using MyUI = ResourceMiningGame.UI;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.Screens
{
    public class TitleScreen : ScreenBase
    {
        MyUI.Button startButton;

        public TitleScreen(Game1 game) : base(game) // 親のコンストラクタを先に呼んでから実行
        {
            font = game.Content.Load<SpriteFont>("Fonts\\MyFont"); // フォントデータをロード
            startButton = new MyUI.Button( // ボタンを生成
                game.GraphicsDevice,
                font,
                new Rect(300, 400, 200, 80),
                "Start");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(); // spriteBatchで描画
            startButton.Draw(spriteBatch); // スタートボタンを描画
            spriteBatch.DrawString(font, "My Resource Mining Game", new Vector2(200, 100), Color.AliceBlue); // タイトルテキストを表示
            spriteBatch.End(); // Batchの設定をフラッシュして終了
        }

        public override void Update(GameTime gameTime)
        {
            if (startButton.Update(game.mouseInput)) // スタートボタンが押された
            {
                game.ChangeScreen(new LevelSelectScreen(game));
            }
        }
    }
}
