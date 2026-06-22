using Craftory.Screens;
using Craftory.UI.Core;
using Button = Craftory.UI.Elements.Button;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.Screens
{
    public class GameSettingScreen : ScreenBase
    {
        private Button backButton;
        private Button backToTitleButton;
        private Texture2D pixel;

        public GameSettingScreen(Game1 game) : base(game) 
        {
            LoadContent();
        }

        public override void LoadContent()
        {
            var ui = new UIFactory(game); //UIを作るインスタンスを生成

            backButton = ui.CreateTextButton("Back", 350, 350, 350, 100);
            backButton.RelativeX = 0.2f;
            backButton.RelativeY = 0.2f;
            backButton.RelativeHeight = 0.1f;
            backButton.RelativeWidth = 0.6f;

            backToTitleButton = ui.CreateTextButton("Back to title", 350, 150, 350, 100);
            backToTitleButton.RelativeX = 0.2f;
            backToTitleButton.RelativeY = 0.4f;
            backToTitleButton.RelativeHeight = 0.1f;
            backToTitleButton.RelativeWidth = 0.6f;

            backButton.RecalculateLayout();
            backToTitleButton.RecalculateLayout();

            uiSet.Add(backButton);
            uiSet.Add(backToTitleButton);

            pixel = new Texture2D(game.GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }
        public override bool IsTransparent => true; //画面の透過設定

        public override void Update(GameTime gameTime)
        {
            if (backButton.Update(game.Input.Mouse)) //バックボタンが押されたら
            {
                game.PopScreen(); //前の画面に戻る
            }

            if (backToTitleButton.Update(game.Input.Mouse)) //タイトルに戻るボタンが押されたら
            {
                game.ChangeScreen(new TitleScreen(game));
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(pixel, new Rectangle(0, 0, 
                game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
                new Color(0, 0, 0, 150));

            backButton.Draw(sb);
            backToTitleButton.Draw(sb);
            sb.End();
        }
    }
}
