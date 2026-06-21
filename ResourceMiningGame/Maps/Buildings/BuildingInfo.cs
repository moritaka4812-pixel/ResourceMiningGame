using ResourceMiningGame.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Buildings
{
    public class BuildingInfo
    {
        public string TexturePath;
        public int FrameCount;
        public float FrameTime;
        public Point SizeInTiles;
        public float WorkSpeed;

        public TileAnimation CreateTileAnimation()
        {
            var tex = ContentLoader.LoadTexture(TexturePath);
            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime);
        }
    }
}
