using Microsoft.Xna.Framework;
using ResourceMiningGame.Input;
using ResourceMiningGame.Screens;

namespace ResourceMiningGame
{
    public class Game1 : Game //ゲームクラスから継承
    {
        private GraphicsDeviceManager _graphics; //グラフィック設定を管理
        private SpriteBatch _spriteBatch; //テキストとイメージを描画
        private ScreenBase currentScreen; //表示する画面
        public MouseInput mouseInput; //マウスの状態を管理

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); //グラフィック設オブジェクトを生成（プログラム上で一度のみ生成）
            Content.RootDirectory = "Content"; // コンテントディレクトリのルートを指定
            IsMouseVisible = true; // マウスカーソルを表示
            mouseInput = new MouseInput();
        }

        protected override void Initialize() // ゲームオブジェクトの初期化
        {
            // TODO: Add your initialization logic here
            base.Initialize(); // ベースクラス(親クラス)のInitialize()を実行
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // テキスト描画などを行うSpriteBatchを生成
            currentScreen = new TitleScreen (this); // タイトルスクリーンからスタート

            // TODO: use this.Content to load your game content here
            

        }

        public void ChangeScreen(ScreenBase next)// 画面変更では共通のGame1インスタンスのメソッドを使う
        {
            mouseInput.Update();
            currentScreen = next;
        }

        // GameTime : ゲーム世界の時間情報（前フレームからの経過時間および累計時間）
        // Updateロジックをフレームレートに依存させないために使う。
        protected override void Update(GameTime gameTime) // 内部の情報を更新するために毎フレームごとに呼ばれる
        {
            mouseInput.Update(); //マウス情報更新

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentScreen.Update(gameTime);

            base.Update(gameTime); // ベースクラスのUpdate()を行う
        }

        protected override void Draw(GameTime gameTime) // 毎フレームごとにUpdate()のあとに呼ばれる。スクリーンに描画をする
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            // TODO: Add your drawing code here
            currentScreen.Draw(_spriteBatch);

            base.Draw(gameTime); // ベースクラスのDraw()
        }
    }
}
