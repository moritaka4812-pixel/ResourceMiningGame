using ResourceMiningGame.Screens;
using ResourceMiningGame.UI.Core;
using Button = ResourceMiningGame.UI.Elements.Button;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.Screens
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
            backToTitleButton = ui.CreateTextButton("Back to title", 350, 150, 350, 100);

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
