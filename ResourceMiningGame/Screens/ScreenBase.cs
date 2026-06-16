using ResourceMiningGame.UI.Setup;

namespace ResourceMiningGame.Screens
{
    public abstract class ScreenBase // すべてのスクリーンが継承するクラス
    {
        protected Game1 game; // main gameクラスへの参照
        protected SpriteFont font; // スクリーンに文字を表示するためのフォント
        protected SetUIElements uiSet; //UIをウィンドウサイズで再配置するインスタンス
        public ScreenBase(Game1 game) // gameインスタンスの受け渡し
        {
            this.game = game;
            this.uiSet = new SetUIElements();
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public virtual bool IsTransparent => false; //画面を透過するか
        public virtual void OnWindowSizeChanged(int width, int height) //ウィンドウサイズが変更されたとき
        {
            uiSet.UpdateLayout(width, height);
        }

        public virtual void LoadContent()
        {
            //派生クラスがUIを追加した後に呼ぶ
            uiSet.UpdateLayout(
                game.GraphicsDevice.Viewport.Width,
                game.GraphicsDevice.Viewport.Height
                );
        }
    }
}
