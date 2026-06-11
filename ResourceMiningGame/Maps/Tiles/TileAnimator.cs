
using ResourceMiningGame.Core;

namespace ResourceMiningGame.Maps.Tiles
{
    public class TileAnimator
    {
        private readonly IMap map;

        public TileAnimator(IMap map)
        {
            this.map = map;
        }

        public void UpdateVisibleTiles(GameTime gameTime, Camera camera, GraphicsDevice device)
        {
            var range = map.GetVisibleRange(camera, device);

            for ( int y = range.StartY; y < range.EndY; y++)
            {
                for (int x = range.StartX; x < range.EndX; x++)
                {
                    map.MapTiles[x, y].Update(gameTime);
                }
            }
        }
    }
}
