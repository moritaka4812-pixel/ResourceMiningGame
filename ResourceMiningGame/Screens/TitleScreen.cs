using Rect = Microsoft.Xna.Framework.Rectangle;
using MyUI = ResourceMiningGame.UI;
using Color = Microsoft.Xna.Framework.Color;
using ResourceMiningGame.UI;

namespace ResourceMiningGame.Screens
{
    public class TitleScreen : ScreenBase
    {
        MyUI.Button startButton;

        public TitleScreen(Game1 game) : base(game) // 親のコンストラクタを先に呼んでから実行
        {
            var ui = new UIFactory(game);
            startButton = ui.CreateTextButton(300, 400, 200, 80, "Start"); // ボタンを生成
            startButton.RelativeX = 0.5f;
            startButton.RelativeY = 0.8f;
            startButton.RelativeWidth = 0.4f;
            startButton.RelativeHeight = 0.2f;
            uiSet.Add(startButton);
            base.LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = game.Content.Load<SpriteFont>("Fonts/MyFont");

            spriteBatch.Begin(); // spriteBatchで描画
            startButton.Draw(spriteBatch); // スタートボタンを描画
            spriteBatch.DrawString(font, "My Resource Mining Game", new Vector2(200, 100), Color.AliceBlue); // タイトルテキストを表示
            spriteBatch.End(); // Batchの設定を送信して終了
        }

        public override void Update(GameTime gameTime)
        {
            if (startButton.Update(game.Input.Mouse)) // スタートボタンが押された
            {
                game.ChangeScreen(new LevelSelectScreen(game));
            }
        }
    }
}
