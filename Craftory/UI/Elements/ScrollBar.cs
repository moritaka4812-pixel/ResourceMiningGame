using Craftory.Input;
using Craftory.UI.Core;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace Craftory.UI.Elements
{
    public class ScrollBar : UIElement
    {
        public int ContentHeight;
        public int HandleHeight;
        public int ScrollOffset;

        public Rectangle HandleRectLocal; // ローカル座標（ScrollBar 内）

        public ScrollBar(int x, int y, int width, int height)
        {
            rect = new Rectangle(x, y, width, height);
        }

        public void SetContentHeight(int contentHeight, int viewHeight)
        {
            ContentHeight = contentHeight;
            rect.Height = viewHeight;
        }

        public void UpdateScroll(int scrollOffset, int viewHeight)
        {
            ScrollOffset = scrollOffset;

            // つまみの高さ
            HandleHeight = (int)((float)viewHeight / ContentHeight * rect.Height);
            HandleHeight = Math.Max(20, HandleHeight);

            // スクロール割合
            float ratio = (float)scrollOffset / (ContentHeight - viewHeight);

            // ローカル座標での Y
            int localY = (int)(ratio * (rect.Height - HandleHeight));

            HandleRectLocal = new Rectangle(0, localY, rect.Width, HandleHeight);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;

            // 絶対座標へ変換
            var abs = GetAbsolutePosition();
            var barAbs = new Rectangle(abs.X, abs.Y, rect.Width, rect.Height);
            var handleAbs = new Rectangle(abs.X + HandleRectLocal.X,
                                          abs.Y + HandleRectLocal.Y,
                                          HandleRectLocal.Width,
                                          HandleRectLocal.Height);

            sb.Draw(whiteTex, barAbs, Color.DarkGray);
            sb.Draw(whiteTex, handleAbs, Color.White);
        }

        public override bool Update(MouseInput mouse)
        {
            return false;
        }
    }
}
