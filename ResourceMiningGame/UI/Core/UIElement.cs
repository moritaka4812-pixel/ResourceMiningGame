using ResourceMiningGame.Input;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.UI.Core
{
    public abstract class UIElement
    {
        protected Rect rect;
        //全UIが共有する画面サイズの四角
        public static Rect RootRect;
        public Rect Rect => rect;
        //レイアウトを無視するか
        public bool IgnoreLayoutX { get; set; } = false;
        public bool IgnoreLayoutY { get; set; } = false;
        //表示するかしないか
        public bool Visible { get; set; } = true;
        //UIContainerで親があれば
        public UIElement? Parent { get; set; } = null;
        //UI更新の時のパディング
        public int PaddingX { get; set; } = 0;
        public int PaddingY { get; set; } = 0;
        //相対配置のX、Y比率
        public float? RelativeX { get; set; } = null; // 0.0 ~ 1.0
        public float? RelativeY { get; set; } = null; // 0.0 ~ 1.0
        //相対の大きさのWidth,Height比率
        public float? RelativeWidth { get; set; } = null; // 0.0 ~ 1.0
        public float? RelativeHeight { get; set; } = null; // 0.0 ~ 1.0
        //UIのRectangleを設定するプロパティ
        public int X { get => rect.X; set => rect = new Rect(value, rect.Y, rect.Width, rect.Height);  }
        public int Y { get => rect.Y; set => rect = new Rect(rect.X, value, rect.Width, rect.Height);  }
        public int Width { get => rect.Width; set => rect = new Rect(rect.X, rect.Y, value, rect.Height);  }
        public int Height { get => rect.Height; set => rect = new Rect(rect.X, rect.Y, rect.Width, value);  }

        //アンカープロパティ
        public UIAnchor Anchor { get; set; } = UIAnchor.TopLeft;
        protected static Texture2D whiteTex; //全UI共通で利用

        public virtual bool OnWheel(MouseInput mouse, int delta) { return false;  }
        public static void Initialize(GraphicsDevice device)
        {
            whiteTex = new Texture2D(device, 1, 1); //白テクスチャをセット
            whiteTex.SetData(new[] { Color.White });

            RootRect = new Rect(0, 0, device.Viewport.Width, device.Viewport.Height); //画面全体の四角
        }

        //親要素の四角上での配置を見て位置を更新する
        public virtual void RecalculateLayout()
        {
            //親がいない場合は画面全体を親とする
            Rect parent = Parent?.Rect ?? RootRect;

            int newWidth = Width;
            int newHeight = Height;
            //サイズ計算
            if (RelativeWidth.HasValue)
                newWidth = (int)(parent.Width * RelativeWidth.Value);
            if (RelativeHeight.HasValue)
                newHeight = (int)(parent.Height * RelativeHeight.Value);
            //親の中でのAnchorによる位置計算
            Vector2 pos = UILayoutManager.GetPositionForAnchor(
                Anchor,
                parent.Width,
                parent.Height,
                newWidth,
                newHeight,
                PaddingX,
                PaddingY
                );
            //親の座標を足す(親の左上が原点でないから)
            pos.X += parent.X;
            pos.Y += parent.Y;
            //RelativeX / RelativeYがあれば上書き
            if (RelativeX.HasValue)
                pos.X = parent.X + (int)(parent.Width * RelativeX.Value);
            if (RelativeY.HasValue)
                pos.Y = parent.Y + (int)(parent.Height * RelativeY.Value);
            //それぞれのX,Yレイアウトを無視するなら元のX,Yを代入
            int finalX = IgnoreLayoutX ? rect.X : (int)pos.X;
            int finalY = IgnoreLayoutY ? rect.Y : (int)pos.Y;

            rect = new Rect(finalX, finalY, newWidth, newHeight);
        }

        public abstract void Draw(SpriteBatch sb);
        public virtual bool Update(MouseInput mouse)
        {
            if(!Visible) return false;
            bool consume = false;

            //クリック
            if (HitTest(mouse.Current.Position) && mouse.LeftClicked())
                consume = true;

            //ホイール
            int wheel = mouse.ScrollDelta();
            if(wheel != 0 && HitTest(mouse.Current.Position))
            {
                bool wheelConsumed = OnWheel(mouse, wheel);

                if (wheelConsumed)
                    return true;
            }

            return consume;
        }

        public virtual bool UpdateWithOffset(int offsetX, int offsetY, MouseInput mouse)
        {
            //デフォルトは通常のUpdate
            return Update(mouse);
        }

        public bool HitTest(Point p)
        {
            return Rect.Contains(p);
        }
    }
}
