//スクリーン座標＝物理的な画面全体の座標
//内部座標＝サイズだけ変わる、内部の画面全体の座標
//描画座標＝SpriteBatch.Begin(transformMatrix)の内部で使われる座標系。
//          スクリーンに表示するための内部座標上に存在する座標。原点はtransformMatrixの影響を受ける

using MyUI = Craftory.UI;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using ScrollBar = Craftory.UI.Elements.ScrollBar;
using Craftory.UI.Core;
using Craftory.Core;
using Craftory.Maps;
using Craftory.UI.Elements;

namespace Craftory.Screens
{
    public class LevelSelectScreen : ScreenBase
    {
        MyUI.Elements.ScrollBar bar; //スクロールバーUI
        ScrollList listUI; //スクロール表示するUIリスト
        

        public LevelSelectScreen(Game1 game) : base(game)
        {
            var ui = new UIFactory(game); //UI生成インスタンス
            listUI = new ScrollList(PositionMode.Local, spacing: 20) { 
                RelativeX = 0.1f, RelativeY = 0.05f, RelativeWidth = 0.9f, RelativeHeight = 0.95f}; //スクロール表示のUIインスタンス

            bar = new ScrollBar(50, 50, 20, 800) { RelativeX = 0.02f, RelativeY = 0.05f, RelativeWidth = 0.03f, RelativeHeight = 0.95f}; //スクロールバーUIを初期化

            listUI.SetScrollBar(bar); //対応するスクロールバーを指定

            for(int i= 0; i < 10; i++) 
            {

                var btn = ui.CreateTextButton($"stage {i+1}", 0 , 0, 400, 50);
                btn.LeftClicked += () => game.ChangeScreen(new GamePlayScreen(game));
                listUI.Add(btn);
            }

        }

        public override void Update(GameTime gameTime)
        {
            listUI.RecalculateLayout();
            bar.RecalculateLayout();

            listUI.Update(game.Input.Mouse);
            bar.Update(game.Input.Mouse);
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Begin();

            listUI.Draw(sb);
            //スクロールバーの描画
            bar.Draw(sb);

            sb.End();
        }
    }
}
