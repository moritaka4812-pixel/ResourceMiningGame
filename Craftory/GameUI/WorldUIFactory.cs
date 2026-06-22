
using Craftory.Core;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.GameUI
{
    public class WorldUIFactory
    {
        private readonly Game1 game;
        private SpriteFont defaultFont;
        private Camera camera;

        public WorldUIFactory(Game1 game, Camera camera)
        {
            this.game = game;
            this.camera = camera;
            defaultFont = game.Content.Load<SpriteFont>("Fonts/MyFont");
        }

        public WorldButton CreateWorldTextButton(string text, int x, int y, int w, int h)
        {
            return new WorldButton(
                camera,
                defaultFont,
                text,
                new Rect(x, y, w, h)
                );
        }

        public WorldPanel CreateWorldPanel(int w, int h)
        {
            return new WorldPanel(w, h, camera);
        }
    }
}
