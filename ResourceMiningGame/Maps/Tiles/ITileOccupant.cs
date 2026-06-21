using ResourceMiningGame.Core;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Tiles
{
    public interface ITileOccupant
    {
        Point TilePosition { get; }
        Point SizeInTiles { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch sb, Camera camera);
    }
}
