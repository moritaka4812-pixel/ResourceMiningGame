using Rect = Microsoft.Xna.Framework.Rectangle;
using MyUI = ResourceMiningGame.UI;
using Color = Microsoft.Xna.Framework.Color;
using ResourceMiningGame.UI.Elements;
using ResourceMiningGame.UI.Core;

namespace ResourceMiningGame.Screens
{
    public class TitleScreen : ScreenBase
    {
        MyUI.Elements.Button startButton;
        TextLabel titleLabel;

        public TitleScreen(Game1 game) : base(game) // 親のコンストラクタを先に呼んでから実行
        {
            var ui = new UIFactory(game);

            startButton = ui.CreateRelativeTextButton("Start", relX: 0.5f, relY: 0.8f, relW: 0.4f, relH: 0.2f); // ボタンを生成
            uiSet.Add(startButton);
            titleLabel = ui.CreateRelativeTextLabel("My Resource Mining Game", relX: 0.5f, relY: 0.2f); // タイトルラベルを生成
            uiSet.Add(titleLabel);
            base.LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(); // spriteBatchで描画
            startButton.Draw(spriteBatch); // スタートボタンを描画
            titleLabel.Draw(spriteBatch); //タイトルラベルを描画
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
