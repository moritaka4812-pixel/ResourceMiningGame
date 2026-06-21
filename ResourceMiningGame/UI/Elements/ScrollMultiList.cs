using ResourceMiningGame.Input;
using ResourceMiningGame.UI.Core;
using Point = Microsoft.Xna.Framework.Point;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.UI.Elements
{
    public class ScrollMultiList : UIElement
    {
        public List<UIElement> Children { get; private set; } = new();
        private int scrollY = 0;

        //列数(今は固定)
        public int Columns { get; set;  } = 3;

        //セルのサイズ
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        //セル間の余白
        public int SpacingX { get; set; }
        public int SpacingY { get; set; }

        public Color? BackgroundColor;


        public ScrollMultiList(int CellWidth = 56, int CellHeight = 56, int SpacingX = 8, int SpacingY = 8)
        {
            this.CellWidth = CellWidth;
            this.CellHeight = CellHeight;
            this.SpacingX = SpacingX;
            this.SpacingY = SpacingY;
        }

        public void Add(UIElement element)
        {
            element.Parent = this;
            Children.Add(element);
        }

        public override bool OnWheel(MouseInput mouse, int delta)
        {
            scrollY -= delta * 20;
            scrollY = Math.Clamp(scrollY, 0 , GetMaxScroll());
            return true; //ホイールを吸収
        }

        public override bool Update(MouseInput mouse)
        {
            if (!Visible) return false;

            RecalculateLayout();

            bool consumed = base.Update(mouse); //ホイール処理などを呼ぶ

            foreach(var child in Children)
            {
                var localMouse = ConvertToLocal(mouse);
                consumed |= child.Update(localMouse);
            }

            return consumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            if(!Visible) return;
            if(BackgroundColor != null)
                sb.Draw(whiteTex, this.Rect, (Color)BackgroundColor);


            //子要素描画
            foreach (var child in Children)
                child.Draw(sb);

        }

        public override void RecalculateLayout()
        {
            base.RecalculateLayout();

            //列数を動的に決めたい場合は
            Columns = Math.Max(1, (Rect.Width + SpacingX) / (CellWidth + SpacingX));

            for (int i = 0; i< Children.Count; i++)
            {
                int row = i / Columns;
                int col = i % Columns;

                int x = Rect.X + col * (CellWidth + SpacingX);
                int y = Rect.Y + row * (CellHeight + SpacingY) - scrollY;

                Children[i].X = x;
                Children[i].Y = y;
                Children[i].Width = CellWidth; 
                Children[i].Height = CellHeight;
            }
        }

        private int GetMaxScroll() //スクロールできる範囲を求める
        {
            int rows = (int)Math.Ceiling(Children.Count / (float)Columns);
            int contentHeight = rows * (CellHeight + SpacingY);
            return Math.Max(0, contentHeight - Rect.Height);
        }

        private MouseInput ConvertToLocal(MouseInput mouse)
        {
            return mouse.WithOffset(0, -scrollY);
        }
    }
}
