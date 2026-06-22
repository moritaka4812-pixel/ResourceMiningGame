using Craftory.Core;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Tiles
{
    public interface ITileOccupant
    {
        Point TilePosition { get; }
        Point SizeInTiles { get; }

        void UpdateLogic(GameTime gameTime);
        void UpdateVisual(GameTime gameTime);
        void Draw(SpriteBatch sb, Camera camera);
    }
}
