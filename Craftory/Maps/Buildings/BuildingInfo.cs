using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings
{
    public class BuildingInfo
    {
        public Dictionary<BuildingDirection, string> TexturePaths;
        public Dictionary<BuildingDirection, Texture2D> CachedTextures = new();
        public int FrameCount;
        public float FrameTime;
        public Point SizeInTiles;
        public float WorkSpeed;

        public BuildType Type;
        public int Width;  //タイル準拠の幅
        public int Height; //タイル準拠の高さ

        public Func<Point, BuildingDirection, BuildingInstance> Create;

        public TileAnimation CreateTileAnimation(BuildingDirection dir)
        {
            Texture2D tex;
            if (CachedTextures.TryGetValue(dir, out var t))
                tex = t;
            else
                tex = CachedTextures[BuildingDirection.None];

            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime);
        }

        public IEnumerable<Point> GetArea(Point origin)
        {
            for(int x = 0; x < SizeInTiles.X; x++)
                for(int y = 0; y < SizeInTiles.Y; y++)
                    yield return new Point(origin.X + x, origin.Y + y);
        }
    }
}
