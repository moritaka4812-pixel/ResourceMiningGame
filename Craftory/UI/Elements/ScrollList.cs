using Craftory.Input;
using Craftory.UI.Core;
using Craftory.UI.Elements;
using ScrollBar = Craftory.UI.Elements.ScrollBar;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Disp = System.Diagnostics.Debug;

namespace Craftory.UI.Elements
{
    public enum PositionMode
    {
        Local, //親座標を足さない(画面直下の配置)
        Relative  //親の座標を足す(Panelなどの子の場合)
    }

    public class ScrollList : UIContainer
    {
        public ScrollBar? ScrollBar { get; set; }
        public int ItemSpacing { get; set; }
        public int LeftMargin { get; set; }
        public int RightMargin { get; set; }
        public PositionMode Mode { get; set; }

        private int contentHeight;
        private int scrollOffset;

        private RasterizerState scissorRaster = new RasterizerState() { ScissorTestEnable = true };
        private RasterizerState defaultRaster = new RasterizerState() { ScissorTestEnable = false };
        private Rectangle previousScissor;

        public ScrollList(PositionMode mode,int spacing = 5, int leftMargin = 20, int rightMargin = 20)
        {
            this.Mode = mode;
            ItemSpacing = spacing;
            LeftMargin = leftMargin;
            RightMargin = rightMargin;
        }

        public void SetScrollBar(ScrollBar scroll)
        {
            ScrollBar = scroll;
        }

        public void Add(UIElement child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public override bool Update(MouseInput mouse)
        {
            if (!Visible) return false;

            // スクロール処理
            UpdateScrollOffset(mouse);

            // ★ 子 UI の Update を呼ぶ（絶対に必要）
            bool consumed = false;
            foreach (var child in Children)
            {
                consumed |= child.Update(mouse);
            }

            ScrollBar?.UpdateScroll(scrollOffset, rect.Height);

            return consumed;
        }


        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;

            Disp.WriteLine($"ScrollList Rect = {rect}");
            sb.Draw(whiteTex, rect, Color.Red * 0.2f);


            BeginClip(sb);

            foreach (var child in Children)
            {

                child.Draw(sb);

            }

            EndClip(sb);
        }


        public override void RecalculateLayout()
        {
            base.RecalculateLayout();
            ScrollBar?.RecalculateLayout();

            LayoutChildrenVertically();
            UpdateContentHeight();
            ApplyScrollOffset();
        }

        private void LayoutChildrenVertically()
        {
            int x = LeftMargin;
            int y = 0;

            foreach (var child in Children)
            {
                if (Mode == PositionMode.Relative)
                {
                    child.X = rect.X + x;
                    child.Y = rect.Y + y;
                }
                else
                {
                    child.X = x;
                    child.Y = y;
                }

                child.Width = rect.Width - (LeftMargin + RightMargin);
                y += child.Height + ItemSpacing;
            }

            contentHeight = y;
        }

        private void UpdateContentHeight()
        {
            ScrollBar?.SetContentHeight(contentHeight, rect.Height);
        }

        private void UpdateScrollOffset(MouseInput mouse)
        {
            int wheelDelta = mouse.ScrollDelta();
            scrollOffset -= (int)(wheelDelta * 0.1f);
            scrollOffset = Math.Clamp(scrollOffset, 0, Math.Max(0, contentHeight - rect.Height));
        }

        private void ApplyScrollOffset()
        {
            int y = -scrollOffset;

            foreach (var child in Children)
            {
                if (Mode == PositionMode.Relative)
                    child.Y = rect.Y + y;
                else
                    child.Y = y;

                y += child.Height + ItemSpacing;
            }
        }



        public void BeginClip(SpriteBatch sb)
        {
            var abs = GetAbsolutePosition();
            var device = sb.GraphicsDevice;

            // 現在の ScissorRectangle を保存
            previousScissor = device.ScissorRectangle;

            // ScrollList の範囲を設定
            device.ScissorRectangle = new Rectangle(abs.X, abs.Y, this.Width, this.Height);
        }

        public void EndClip(SpriteBatch sb)
        {
            // 元の ScissorRectangle を戻す
            sb.GraphicsDevice.ScissorRectangle = previousScissor;
        }


        protected override void RecalculateChildrenLayout() { }
    }
}
