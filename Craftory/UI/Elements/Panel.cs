using Craftory.Input;
using Craftory.UI.Core;
using Color = Microsoft.Xna.Framework.Color;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace Craftory.UI.Elements
{
    public class Panel : UIElement
    {
        public Color BackgroundColor { get; set; }

        public UIContainer container;

        public Panel(int width, int height)
        {
            Width = width;
            Height = height;
            BackgroundColor = Color.Gray;

            container = new UIContainer();
            container.Parent = this; //コンテナの親をこのPanelに
        }

        public void AddChild(UIElement child)
        {
            container.Add(child);
        }

        public void RemoveChild(UIElement child)
        {
            container.Remove(child);
        }

        public void SetBackground(Color color)
        {
            this.BackgroundColor = color;
        }

        public override bool OnWheel(MouseInput mouse, int delta)
        {
            if (HitTest(mouse.Current.Position) && !container.HitTest(mouse.Current.Position)) 
                return true;

            return false;
        }

        public override bool Update(MouseInput mouse)
        {
            if (!Visible) return false;

            bool consumed = false;

            // 1. レイアウト更新
            this.RecalculateLayout();
            container.Width = this.Width;
            container.Height = this.Height;
            container.X = this.X;
            container.Y = this.Y;
            container.RecalculateLayout();

            // 2. Panel 自身のホイール処理（背景吸収）
            //    base.Update を先に呼ぶと OnWheel が正しく動く
            consumed |= base.Update(mouse);

            // 3. 子要素の更新（ボタンやスクロールリスト）
            consumed |= container.Update(mouse);

            // 4. 背景クリック吸収
            if (HitTest(mouse.Current.Position) && mouse.LeftClicked())
                consumed = true;

            return consumed;
        }


        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;

            //背景
            sb.Draw(whiteTex, new Rect((int)X, (int)Y, Width, Height), BackgroundColor);
            //子要素の描画
            container.Draw(sb);
        }
    }
}
