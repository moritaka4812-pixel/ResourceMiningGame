using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.Maps.Tiles
{
    public class Tile
    {
        public TileType Type;
        public float MiningRate; //資源タイルの採掘速度
        public bool IsBuildable; //建設可能かどうか
        public Vector2 Position; //タイルの位置（ワールド座標）

        //アニメーション用
        public Texture2D SpriteSheet; //スプライトシート
        public int FrameCount; //フレーム数(1ならアニメーションなし)
        public int FrameWidth; //フレームの幅
        public int FrameHeight; //フレームの高さ

        public float FrameTime; //1フレームの時間
        private float timer;
        private int currentFrame;

        public Tile(TileType Type,　//タイルはType, Positionのみでnew可能
                    Vector2 Position,
                    int FrameCount = 1, //その他の初期設定
                    int FrameWidth = 16,
                    int FrameHeight = 16,
                    float FrameTime = 0.25f)
        {
            this.Type = Type;
            this.Position = Position;
            this.FrameCount = FrameCount;
            this.FrameWidth = FrameWidth;
            this.FrameHeight = FrameHeight;
            this.FrameTime = FrameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (FrameCount <= 1) return; //アニメなし

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= FrameTime)
            {
                timer -= FrameTime;
                currentFrame = (currentFrame + 1) % FrameCount;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (SpriteSheet == null) return; //スプライトシートがない

            var source = new Rectangle(
                currentFrame * FrameWidth,
                0,
                FrameWidth,
                FrameHeight
             );

            sb.Draw(SpriteSheet, Position, source, Color.White);

        }

    }
}
