using rect = Microsoft.Xna.Framework.Rectangle;
using Button = ResourceMiningGame.UI.Button;

namespace ResourceMiningGame.UI
{
    public class UIFactory
    {
        private readonly Game1 game;
        private SpriteFont defaultFont;

        public UIFactory(Game1 game)
        {
            this.game = game;
            defaultFont = game.Content.Load<SpriteFont>("Fonts/MyFont");
        }

        public void ChangeFont(string file)
        {
            defaultFont = game.Content.Load<SpriteFont>(file);
        }

        public Button CreateImageButton(int x, int y, int width, int height, string file)
        {
            return new Button(
                game.GraphicsDevice,
                game.Content.Load<Texture2D>(file),
                new rect(x, y, width, height));
        }

        public Button CreateTextButton(int x, int y, int width, int height, string text)
        {
            return new Button(
                game.GraphicsDevice,
                defaultFont,
                new rect(x, y, width, height),
                text);
        }
    }
}
