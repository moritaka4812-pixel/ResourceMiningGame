using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Craftory.Core;

namespace Craftory.Maps.Shadow
{
    public class MapShadowGenerator
    {
        private IMap map;
        private Texture2D pixel;
        private RenderTarget2D shadowTexture;
        private bool needUpdate = true;

        public void MarkDirty() => needUpdate = true;

        public MapShadowGenerator(IMap map, GraphicsDevice graphics) 
        {
            this.map = map;
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new[] {Color.White});
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(shadowTexture, Vector2.Zero, Color.White);
        }

        public void UpdateShadowTexture(SpriteBatch sb)
        {
            if (needUpdate)
                RebuildShadowTexture(sb);
        }

        public void RebuildShadowTexture(SpriteBatch sb)
        {
            int w = map.MapSizeX * 32;
            int h = map.MapSizeY * 32;

            if (shadowTexture == null ||
                shadowTexture.Width != w ||
                shadowTexture.Height != h)
            {
                shadowTexture = new RenderTarget2D(
                    sb.GraphicsDevice,
                    w,
                    h,
                    false,
                    SurfaceFormat.Color,
                    DepthFormat.None
                );
            }


            sb.GraphicsDevice.SetRenderTarget(shadowTexture);
            sb.GraphicsDevice.Clear(Color.Transparent);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            for (int x = 0; x < map.MapSizeX; x++)
                for (int y = 0; y < map.MapSizeY; y++)
                    DrawTileShadow(sb, x, y);

            sb.End();

            sb.GraphicsDevice.SetRenderTarget(null);

            needUpdate = false;
        }



        private void DrawTileShadow(SpriteBatch sb, int x, int y)
        {
            var tile = map.GetTile(x, y);
            if (tile == null) return;

            int strength = tile.ShadowSources.Sum(s => s.Strength);

            // 上下左右の比較
            CompareAndDraw(sb, x, y, x, y - 1, Direction.Up);
            CompareAndDraw(sb, x, y, x, y + 1, Direction.Down);
            CompareAndDraw(sb, x, y, x - 1, y, Direction.Left);
            CompareAndDraw(sb, x, y, x + 1, y, Direction.Right);
        }

        private void CompareAndDraw(SpriteBatch sb, int ax, int ay, int bx, int by, Direction dir)
        {
            var A = map.GetTile(ax, ay);
            var B = map.GetTile(bx, by);
            if (A == null || B == null) return;

            int strengthA = A.ShadowSources.Sum(s => s.Strength);
            int strengthB = B.ShadowSources.Sum(s => s.Strength);

            if (strengthB > strengthA)
            {
                DrawShadow(sb, ax, ay, dir, strengthB - strengthA);
            }
        }

        private void DrawShadow(SpriteBatch sb, int x, int y, Direction dir, int strength)
        {
            var pos = new Vector2(x * 32, y * 32);
            int size = 6;

            Color c = Color.Black * (0.2f * strength);

            switch (dir)
            {
                case Direction.Up:
                    sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, 32, size), c);
                    break;
                case Direction.Down:
                    sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y + 32 - size, 32, size), c);
                    break;
                case Direction.Left:
                    sb.Draw(pixel, new Rectangle((int)pos.X, (int)pos.Y, size, 32), c);
                    break;
                case Direction.Right:
                    sb.Draw(pixel, new Rectangle((int)pos.X + 32 - size, (int)pos.Y, size, 32), c);
                    break;
            }
        }
    }
}
