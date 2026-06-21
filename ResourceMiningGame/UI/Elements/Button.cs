using ResourceMiningGame.UI.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
using Color = Microsoft.Xna.Framework.Color;
using MouseInput = ResourceMiningGame.Input.MouseInput;
using Point = Microsoft.Xna.Framework.Point;
using Rect = Microsoft.Xna.Framework.Rectangle;

namespace ResourceMiningGame.UI.Elements
{
    public class Button : UIElement
    {
        public string Text;
        public Color TextColor;
        public Color FillColor;
        public Color BorderColor;
        public Color NormalFillColor;
        public Color HoverFillColor;
        public Texture2D Icon; //アイコン画像
        public bool IsImageButton = false; //テキストボタンか画像ボタンか
        public event Action? OnClicked;

        public bool IsToggle { get; set; } = false;

        protected SpriteFont font; //フォントデータ

        public Button(GraphicsDevice device, SpriteFont font, Rect rect, string text) //テキスト付きボタン
        {
            this.rect = rect;　//引数受け渡しとテクスチャの初期化
            this.Text = text;
            this.font = font;
            this.TextColor = Color.White;
            this.FillColor = Color.DarkSlateGray;
            this.BorderColor = Color.White;
            this.NormalFillColor = Color.DarkSlateGray;
            this.HoverFillColor = Color.Gray;
        }

        public Button(GraphicsDevice device, Texture2D icon, Rect rect) //イメージ付きボタン
        {
            this.Icon = icon;
            this.rect = rect;
            this.FillColor = Color.DarkSlateGray;
            this.BorderColor= Color.White;
            this.NormalFillColor = Color.DarkSlateGray;
            this.HoverFillColor= Color.Gray;
            IsImageButton = true;
        }

        public override bool OnLeftClick(MouseInput mouse)
        {
            OnClicked?.Invoke();
            return true; //クリック吸収
        }

        public override bool OnHover(MouseInput mouse) //ホバー時
        {
            ColorChangeWithHover(mouse.Current.Position);
            return false;
        }

        public override void OnHoverExit() //非ホバー時
        {
            FillColor = NormalFillColor;
            BorderColor = Color.White;
        }

        public override bool UpdateWithOffset(int offsetX, int offsetY, MouseInput mouse) //画面が移動した時のUpdate処理
        {
            if (!Visible) return false;
            var pos = new Point(mouse.Current.Position.X - offsetX, mouse.Current.Position.Y - offsetY); //内部の相対座標を計算（ミシンの縫物のイメージ）描画と内部のズレがoffset

            bool consumed = false;

            //ホバー
            ColorChangeWithHover(pos);

            //左クリック判定
            if(Rect.Contains(pos) && mouse.LeftClicked())
            {
                OnClicked?.Invoke();
                consumed = true;
            }

            return consumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (!Visible) return;
            //背景
            sb.Draw(whiteTex, Rect, FillColor);
            //ボタンの枠
            if (IsToggle == false)
                DrawRectangle(sb, Rect, 3, BorderColor);
            else
                DrawRectangle(sb, Rect, 3, Color.Yellow);

            if(IsImageButton && Icon != null)
            {
                var iconPos = new Vector2(
                    Rect.X + (Rect.Width - Icon.Width) / 2,
                    Rect.Y + (Rect.Height - Icon.Height) / 2
                    );
                sb.Draw(Icon, iconPos, Color.White);
            }
            else
            {
                // ボタンテキストサイズを取得
                var size = font.MeasureString(Text);
                // テキストの位置計算
                var pos = new Vector2(
                    Rect.X + (Rect.Width - size.X) / 2,
                    Rect.Y + (Rect.Height - size.Y) / 2
                    );

                sb.DrawString(font, Text, pos, TextColor);
            }
        }

        public void SetBackgroundColor(Color color) //背景色変更メソッド
        {
            NormalFillColor = color;
            FillColor = color;
        }

        public void SetHoverColor(Color color) //ホバー時の色変更メソッド
        {
            HoverFillColor = color;
        }

        bool ColorChangeWithHover(Point point)
        {
            bool hover = Rect.Contains(point);

            if (hover) //ボタン上にマウスがホバーしているか
            {
                FillColor = HoverFillColor;
                BorderColor = Color.Yellow;
            }
            else
            {
                FillColor = NormalFillColor;
                BorderColor = Color.White;
            }
            return hover;
        }

        protected void DrawRectangle(SpriteBatch sb, Rect rect, int thickness, Color color)
        {
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, rect.Width, thickness), color); // 上
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y, thickness, rect.Height), color); // 左
            sb.Draw(whiteTex, new Rect(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // 下
            sb.Draw(whiteTex, new Rect(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // 右
        }
    }
}
