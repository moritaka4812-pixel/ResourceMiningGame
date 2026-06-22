using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using Craftory.Input;
using Craftory.UI.Core;

namespace Craftory.UI.Elements
{
    public class ScrollBar : UIElement
    {
        public Rectangle HandleRect; // 映している範囲を示すスクロールバー（つまみ）
        public int HandleHeight; //つまみの高さ
        public int contentHeight; //映せるコンテンツ全体の高さ

        public ScrollBar(int x, int y, int width, int height)
        {
            rect = new Rectangle(x, y, width, height); //BarRect（映せる画面全体の長形）として扱う
        }

        public void SetContentHeight(int contentHeight, int rectHeight)
        {
            this.contentHeight = contentHeight;
            rect.Height = rectHeight;
        }

        public void Update(int scrollY, int viewHeight) // スクロールバーの位置を更新
        {
            if (!Visible) return;
            //つまみの高さ(長さ)を自動計算
            HandleHeight = (int)((float)viewHeight / contentHeight * rect.Height);
            HandleHeight = Math.Max(20, HandleHeight); //最短サイズ

            float ratio = (float)scrollY / (contentHeight - viewHeight); // コンテンツのどの位置にスクロールバーがあるか（0~1.0）
            int handleY = rect.Y + (int)(ratio * (rect.Height - HandleRect.Height)); // スクロールバーがコンテンツ全体のどの高さにあるかを計算

            HandleRect = new Rectangle(
                rect.X,
                handleY,
                rect.Width,
                HandleHeight
                );
        }

        public override void  Draw(SpriteBatch sb)
        {
            if (!Visible) return;
            sb.Draw(whiteTex, rect, Color.DarkGray); // BarRect
            sb.Draw(whiteTex, HandleRect, Color.White); //HandleRect
        }

        public override bool Update(MouseInput mouse)
        {
            //スクロールバーはクリック処理がない
            return false;
        }
    }
}
