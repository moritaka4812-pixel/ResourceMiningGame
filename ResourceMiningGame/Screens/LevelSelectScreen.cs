//スクリーン座標＝物理的な画面全体の座標
//内部座標＝サイズだけ変わる、内部の画面全体の座標
//描画座標＝SpriteBatch.Begin(transformMatrix)の内部で使われる座標系。
//          スクリーンに表示するための内部座標上に存在する座標。原点はtransformMatrixの影響を受ける

using MyUI = ResourceMiningGame.UI;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

using ResourceMiningGame.Core;
using ResourceMiningGame.UI;

namespace ResourceMiningGame.Screens
{
    public class LevelSelectScreen : ScreenBase
    {
        ScrollView scroll; //スクロール画面のための処理クラス
        MyUI.ScrollBar bar; //スクロールバーUI
        List<MyUI.Button> levelButtons; //レベル選択のためのボタンリスト
        Texture2D white; //１ドットの白テクスチャ
        

        public LevelSelectScreen(Game1 game) : base(game)
        {
            white = new Texture2D(game.GraphicsDevice, 1, 1); //1ドットのテクスチャ作成
            white.SetData(new[] { Color.White }); //白いテクスチャを割り当て
            var ui = new UIFactory(game); //UI生成インスタンス

            scroll = new ScrollView( //スクロール画面を初期化
                game.GraphicsDevice,
                new Rectangle(100, 100, 600, 500),
                contentHeight: 1000
                );

            bar = new MyUI.ScrollBar //スクロールバーUIを初期化
            {
                BarRect = new Rectangle(50, 100, 20, 500),
                HandleRect = new Rectangle(50, 100, 20, 80)
            };

            levelButtons = new List<MyUI.Button>(); //リスト初期化

            for(int i= 0; i < 10; i++) //ボタンのリストを作成
            {
                levelButtons.Add(ui.CreateTextButton(120, 120 + i * 100, 500, 80, $"Stage {i + 1}")); //テキストボタンを生成
            }

        }

        public override void Update(GameTime gameTime)
        {
            scroll.Update(); //スクロール画面の更新
            bar.Update(scroll.ScrollY, scroll.ContentHeight, scroll.ViewRect.Height); //スクロール画面の更新

            foreach( var btn in levelButtons) //各ボタンが押されたかの確認
            {
                if (btn.UpdateWithOffset(0, - scroll.ScrollY, game.Input.Mouse))　//内部座標に変換して押されたか(-scroll.ScrollYはScrollViewのtransformMatrixに依存するもの)
                {
                    // Load level
                    game.ChangeScreen(new GamePlayScreen(game)); //ゲームプレイスクリーンに切り替える
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            //スクロールバーの描画
            bar.Draw(sb, white);

            sb.End();
            //GPUのラスタリズ設定のうちにScissorを使って描画範囲を切り抜くように指示する設定
            var raster = new RasterizerState() { ScissorTestEnable = true };
            //spriteBatchに改めてScissorを使うことを指示し、使うMatrixを指定
            sb.Begin(rasterizerState: raster, transformMatrix: scroll.GetMatrix());
            //描画範囲のScissorRectgangleをスクロール画面のViewRectに設定
            game.GraphicsDevice.ScissorRectangle = scroll.ViewRect;
            //内部座標に各ボタンを描画
            foreach (var btn in levelButtons) btn.Draw(sb);

            sb.End();
        }
    }
}
