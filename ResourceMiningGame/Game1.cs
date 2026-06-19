using Microsoft.Xna.Framework;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.Input;
using ResourceMiningGame.Screens;
using ResourceMiningGame.UI.Core;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame
{
    public class Game1 : Game //ゲームクラスから継承
    {
        private GraphicsDeviceManager _graphics; //グラフィック設定を管理
        private SpriteBatch _spriteBatch; //テキストとイメージを描画
        private Stack<ScreenBase> screens = new Stack<ScreenBase>(); //表示する画面のスタック
        public InputManager Input { get; private set; } //入力の状態を管理

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); //グラフィック設定を管理するオブジェクトを生成（ゲーム内で一度のみ生成）
            Content.RootDirectory = "Content"; // コンテントディレクトリのルートを指定
            IsMouseVisible = true; // マウスカーソルを表示
            Input = new InputManager(); //インプット管理をするインスタンスを生成

            Window.AllowUserResizing = true; //ウィンドウサイズを変更可能に
            _graphics.PreferredBackBufferWidth = 800; //最小画面横幅
            _graphics.PreferredBackBufferHeight = 600; //最小画面縦幅
            Window.ClientSizeChanged += OnResize;
            Window.ClientSizeChanged += OnClientSizeChanged; //ウィンドウが変更されたらイベントを呼ぶ
        }

        protected override void Initialize() // ゲームオブジェクトの初期化
        {
            // TODO: Add your initialization logic here
            UIElement.Initialize(GraphicsDevice);
            WorldUIElement.Initialize(GraphicsDevice);
            base.Initialize(); // ベースクラス(親クラス)のInitialize()を実行
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // テキスト描画などを行うSpriteBatchを生成
            screens.Push(new TitleScreen (this)); // タイトルスクリーンからスタート

            // TODO: use this.Content to load your game content here
            

        }

        public void PushScreen(ScreenBase screen) //Stackにスクリーンをプッシュ
        {
            screens.Push(screen);
        }

        public void PopScreen() //Stackからスクリーンをポップ
        {
            if (screens.Count > 1)
                screens.Pop();
        }

        public void ChangeScreen(ScreenBase screen)// 画面遷移を行うための共通メソッド(currentScreenを差し替える)
        {
            Input.Update(); //画面切り替え時に入力を更新して、クリックなどの入力誤検知を防ぐ
            screens.Clear(); //画面をクリア
            screens.Push(screen); //次の画面を表示
        }

        // GameTime : ゲーム世界の時間情報（前フレームからの経過時間および累計時間）
        // Updateロジックをフレームレートに依存させないために使う。
        protected override void Update(GameTime gameTime) // 内部の情報を更新するために毎フレームごとに呼ばれる
        {
            Input.Update(); //入力デバイスの情報更新

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            screens.Peek().Update(gameTime); //最も後から追加されたスクリーンをUpdate()

            base.Update(gameTime); // ベースクラスのUpdate()を行う
        }

        protected override void Draw(GameTime gameTime) // 毎フレームごとにUpdate()のあとに呼ばれる。スクリーンに描画をする
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            // TODO: Add your drawing code here
            foreach (var screen in screens.Reverse()) //最も後から追加されたスクリーンから描画
            {
                screen.Draw(_spriteBatch);

                if (!screen.IsTransparent) break; //スクリーン透過ができない
            }
            base.Draw(gameTime); // ベースクラスのDraw()
        }
        
        public void OnResize(object sender, EventArgs e)
        {
            int minWidth = 800;
            int minHeight = 600;

            if(Window.ClientBounds.Width < minWidth ||
                Window.ClientBounds.Height < minHeight)
            {
                _graphics.PreferredBackBufferWidth = Math.Max(Window.ClientBounds.Width, minWidth);
                _graphics.PreferredBackBufferHeight = Math.Max(Window.ClientBounds.Height, minHeight);
                _graphics.ApplyChanges();
            }
        }

        private void OnClientSizeChanged(object sendeer, EventArgs e) //ウィンドウサイズ変更されたら呼ばれる
        {
            screens.Peek()?.OnWindowSizeChanged( //表示ウィンドウのUI配置関数を呼ぶ
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                );
            UIElement.RootRect = new Rect(0,0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height
                );
        }
    }
}
