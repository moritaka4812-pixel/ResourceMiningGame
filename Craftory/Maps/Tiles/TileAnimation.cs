using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.Maps.Tiles
{
    public class TileAnimation
    {
        public Texture2D Texture;
        public int FrameCount;
        public int FrameWidth;
        public int FrameHeight;
        public float FrameTime;

        private float timer;
        private int currentFrame;

        public TileAnimation(Texture2D tex, int count, int w, int h, float time)
        {
            Texture = tex; ;
            FrameCount = count;
            FrameWidth = w;
            FrameHeight = h;
            FrameTime = time;
        }

        public void Update(GameTime gameTime)
        {
            if (FrameCount <= 1) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= FrameTime)
            {
                timer -= FrameTime;
                currentFrame = (currentFrame + 1) % FrameCount;
            }
        }

        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            var src = new Rect(
                currentFrame * FrameWidth,
                0,
                FrameWidth,
                FrameHeight
                );
            sb.Draw(Texture, pos, src, Color.White);
        }

        public void Draw(SpriteBatch sb, Vector2 pos, Color color) //プレビュー用表示
        {
            var src = new Rect(
                currentFrame * FrameWidth,
                0,
                FrameWidth,
                FrameHeight
                );

            sb.Draw(Texture, pos, src, color);
        }
    }
}
