using Craftory.Input;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.UI.Core
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
        //マウス入力の吸収処理に使う
        //各インスタンスごとにデリゲードで入力の吸収を設定する時につかう
        public Func<MouseInput, bool>? OnLeftClickHandler { get; set; }
        public Func<MouseInput, bool>? OnRightClickHandler { get; set; }
        public Func<MouseInput, bool>? OnMiddleClickHandler { get; set; }
        public Func<MouseInput, int, bool>? OnWheelHandler { get; set; }
        public Func<MouseInput, Point, bool>? OnDragHandler { get; set; }
        public Func<MouseInput, bool>? OnHoverHandler { get; set;  }
        //各エレメントごとに設定されるマウス入力吸収設定
        public virtual bool OnLeftClick(MouseInput mouse) { return false; }
        public virtual bool OnRightClick(MouseInput mouse) { return false; }
        public virtual bool OnMiddleClick(MouseInput mouse) { return false; }
        public virtual bool OnWheel(MouseInput mouse, int delta) { return false; }
        public virtual bool OnDrag(MouseInput mouse, Point delta) { return false; }
        public virtual bool OnHover(MouseInput mouse) { return false; }
        //ホバー解除用
        public virtual void OnHoverExit() {}
        // 外部に操作を伝えるイベント
        public event Action? LeftClicked;
        public event Action? RightClicked;
        public event Action? MiddleClicked;
        public event Action? Hovered;
        public event Action? HoveredExited;
        public event Action<int>? WheelScrolled;
        public event Action<Point>? Dragged;

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
        public virtual bool Update(MouseInput mouse) //ベースとしてのマウス入力の吸収処理
        {
            if(!Visible) return false;

            bool consumed = false;
            bool hit = HitTest(mouse.Current.Position);

            //左クリック
            if (hit && mouse.LeftClicked())
            {
                LeftClicked?.Invoke();
                if(OnLeftClickHandler != null) //左クリックの代替デリゲード処理があれば
                    consumed |= OnLeftClickHandler(mouse);
                
                else
                    consumed |= OnLeftClick(mouse);
            }

            //右クリック
            if (hit && mouse.RightClicked())
            {
                RightClicked?.Invoke(); 
                if(OnRightClickHandler != null) //右クリックの代替デリゲード処理があれば
                    consumed |= OnRightClickHandler(mouse);

                else
                    consumed |= OnRightClick(mouse);

                
            }

            //中クリック
            if (hit && mouse.MiddleClicked())
            {
                MiddleClicked?.Invoke();
                if (OnMiddleClickHandler != null) //中クリックの代替デリゲード処理があれば
                    consumed |= OnMiddleClickHandler(mouse);

                else
                    consumed |= OnMiddleClick(mouse);

                
            }

            //ホイール
            int wheel = mouse.ScrollDelta();
            if(wheel != 0 && hit)
            {
                WheelScrolled?.Invoke(wheel);
                if (OnWheelHandler != null) //ホイールの代替デリゲード処理があれば
                    consumed |= OnWheelHandler(mouse, wheel);

                else
                    consumed |= OnWheel(mouse, wheel);
            }

            //ホバー
            if (hit)
            {
                Hovered?.Invoke();
                if (OnHoverHandler != null)
                    consumed |= OnHoverHandler(mouse);

                else
                    consumed |= OnHover(mouse);

                
            }
            else
            {
                HoveredExited?.Invoke();
                OnHoverExit();
            }

            return consumed;
        }

        protected void RaiseLeftClicked()
        {
            LeftClicked?.Invoke();
        }

        protected void RaiseRightClicked()
        {
            RightClicked?.Invoke();
        }

        protected void RaiseMiddleClicked()
        {
            MiddleClicked?.Invoke();
        }

        protected void RaiseWheelScrolled(int wheel)
        {
            WheelScrolled?.Invoke(wheel);
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
