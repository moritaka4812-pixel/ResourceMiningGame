using ResourceMiningGame.Input;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace ResourceMiningGame.UI
{
    public abstract class UIElement
    {
        protected Rect rect;
        public Rect Rect => rect;
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
        public static void Initialize(GraphicsDevice device)
        {
            whiteTex = new Texture2D(device, 1, 1); //白テクスチャをセット
            whiteTex.SetData(new[] { Color.White });
        }
        public abstract void Draw(SpriteBatch sb);
        public abstract bool Update(MouseInput mouse);
    }
}
