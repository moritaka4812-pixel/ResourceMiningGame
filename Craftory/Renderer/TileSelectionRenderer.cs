using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Craftory.Maps.Tiles;

namespace Craftory.Renderer
{
    public class TileSelectionRenderer
    {
        private Texture2D pixel;

        public TileSelectionRenderer(GraphicsDevice device)
        {
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch sb, Tile selectedTile)
        {
            if (selectedTile == null) return;

            var pos = selectedTile.Position;
            int size = 32;

            sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, size, 1), Color.Yellow);
            sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y + size - 1, size, 1), Color.Yellow);
            sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, 1, size), Color.Yellow);
            sb.Draw(pixel, new Rectangle((int)pos.X + size - 1, (int)pos.Y, 1, size), Color.Yellow);
        }
    }
}
