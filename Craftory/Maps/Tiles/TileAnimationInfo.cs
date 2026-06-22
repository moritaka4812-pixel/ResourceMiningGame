

namespace Craftory.Maps.Tiles
{
    public class TileAnimationInfo
    {
        public string TexturePath;
        public int FrameCount;
        public float FrameTime;

        public TileAnimation CreateTileAnimation()
        {
            var tex = ContentLoader.LoadTexture(TexturePath);
            return new TileAnimation(tex, FrameCount, tex.Width / FrameCount, tex.Height, FrameTime);
        }
    }
}
