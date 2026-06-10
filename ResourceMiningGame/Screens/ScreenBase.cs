
namespace ResourceMiningGame.Screens
{
    public abstract class ScreenBase // すべてのスクリーンが継承するクラス
    {
        protected Game1 game; // main gameクラスへの参照
        protected SpriteFont font; // スクリーンに文字を表示するためのフォント
        public ScreenBase(Game1 game) // gameインスタンスの受け渡し
        {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual bool IsTransparent => false; //画面を透過するか
    }
}
