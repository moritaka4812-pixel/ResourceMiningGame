using rect = Microsoft.Xna.Framework.Rectangle;
using Button = ResourceMiningGame.UI.Elements.Button;
using ResourceMiningGame.UI.Elements;

namespace ResourceMiningGame.UI.Core
{
    public class UIFactory
    {
        private readonly Game1 game;
        private SpriteFont defaultFont;

        public UIFactory(Game1 game)
        {
            this.game = game;
            defaultFont = game.Content.Load<SpriteFont>("Fonts/MyFont"); //デフォルトフォント
        }

        public void ChangeFont(string file) //フォント変更
        {
            defaultFont = game.Content.Load<SpriteFont>(file);
        }

        public Button CreateImageButton(string file, int x, int y, int width, int height)
        {
            return new Button(
                game.GraphicsDevice,
                game.Content.Load<Texture2D>(file),
                new rect(x, y, width, height));
        }

        public Button CreateTextButton(string text, int x, int y, int width, int height)
        {
            return new Button(
                game.GraphicsDevice,
                defaultFont,
                new rect(x, y, width, height),
                text);
        }
        public Button CreateRelativeImageButton( //基本的にx,y,width,heightは無視
            string file,
            float? relX = null, float? relY = null,
            float? relW = null, float? relH = null,
            int x = 0, int y = 0,
            int width = 1, int height = 1
            )
        {
            var btn = new Button(
                game.GraphicsDevice,
                game.Content.Load<Texture2D>(file),
                new rect(x, y, width, height));

            btn.RelativeX = relX;
            btn.RelativeY = relY;
            btn.RelativeWidth = relW;
            btn.RelativeHeight = relH;
            return btn;
        }

        public Button CreateRelativeTextButton( //基本的にx,y,width,heightは無視
            string text,
            float? relX = null, float? relY = null,
            float? relW = null, float? relH = null,
            int x = 0, int y = 0,
            int width = 1, int height = 1
            )
        {
            var btn = new Button(
                game.GraphicsDevice,
                defaultFont,
                new rect(x, y, width, height),
                text);

            btn.RelativeX = relX;
            btn.RelativeY = relY;
            btn.RelativeWidth = relW;
            btn.RelativeHeight = relH;
            return btn;
        }

        public TextLabel CreateRelativeTextLabel( //基本的にx,t,width,heightは無視
            string text,
            float? relX = null, float? relY = null,
            float? relW = null, float? relH = null,
            int x = 0, int y = 0,
            int width = 1, int height = 1
            )
        {
            var label = new TextLabel(defaultFont, text, new rect(x, y, width, height));
            label.RelativeX = relX;
            label.RelativeY = relY;
            label.RelativeWidth = relW;
            label.RelativeHeight = relH;
            return label;
        }

    }
}
