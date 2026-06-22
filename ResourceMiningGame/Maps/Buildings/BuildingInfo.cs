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

        public BuildType Type;
        public int Width;  //タイル準拠の幅
        public int Height; //タイル準拠の高さ

        public TileAnimation CreateTileAnimation()
        {
            var tex = ContentLoader.LoadTexture(TexturePath);
            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime);
        }
    }
}
