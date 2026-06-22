
using Craftory.Core;

namespace Craftory.Maps.Tiles
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
            var range = map.GetVisibleRange(camera, device); //映るタイル範囲を取得

            for ( int y = range.StartY; y < range.EndY; y++) //映る範囲だけアニメーション
            {
                for (int x = range.StartX; x < range.EndX; x++)
                {
                    map.MapTiles[x, y].Update(gameTime);
                }
            }
        }
    }
}
