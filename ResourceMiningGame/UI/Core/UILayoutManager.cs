namespace ResourceMiningGame.UI.Core
{
    public class UILayoutManager
    {
        public static Vector2 AnchorTopRight(int screenW, int screenH, int uiW, int uiH, int paddingX, int paddingY)
        {
            return new Vector2( //右上アンカーの座標を返す
                screenW - uiW - paddingX,
                paddingY
                );
        }
        public static Vector2 AnchorBottomRight(int screenW, int screenH, int uiW, int uiH, int paddingX, int paddingY)
        {
            return new Vector2( //右下アンカーの座標を返す
                screenW - uiW - paddingX,
                screenH - uiH - paddingY
                );
        }
        public static Vector2 AnchorTopLeft(int screenW, int screenH, int uiW, int uiH, int paddingX, int paddingY)
        {
            return new Vector2( //左上アンカーの座標を返す
                paddingX,
                paddingY
                );
        }
        public static Vector2 AnchorBottomLeft(int screenW, int screenH, int uiW, int uiH, int paddingX, int paddingY)
        {
            return new Vector2( //左下のアンカー座標を返す
                paddingX,
                screenH - uiH - paddingY
                );
        }
        public static Vector2 AnchorCenter(int screenW, int screenH, int uiW, int uiH, int offsetX, int offsetY)
        {
            return new Vector2( //中央アンカー座標を返す
                screenW / 2 - uiW / 2 + offsetX,
                screenH / 2 - uiH / 2 + offsetY
                );
        }

        public static Vector2 AnchorLeftCenter(int screenW, int screenH, int uiW, int uiH, int paddingX, int paddingY)
        {
            return new Vector2(
                paddingX,
                screenH / 2 - uiH / 2 + paddingY
                );
        }

        public static Vector2 GetPositionForAnchor( //各アンカーに合わせて配置位置を返す
            UIAnchor anchor,
            int screenW, int screenH,
            int uiW, int uiH,
            int paddingX, int paddingY)
        {
            switch (anchor) 
            {
                case UIAnchor.TopLeft:
                    return AnchorTopLeft(screenW, screenH, uiW, uiH, paddingX, paddingY);

                case UIAnchor.TopRight:
                    return AnchorTopRight(screenW, screenH, uiW, uiH, paddingX, paddingY);

                case UIAnchor.BottomLeft:
                    return AnchorBottomLeft(screenW, screenH, uiW, uiH, paddingX, paddingY);

                case UIAnchor.BottomRight:
                    return AnchorBottomRight(screenW, screenH, uiW, uiH, paddingX, paddingY);

                case UIAnchor.Center:
                    return AnchorCenter(screenW, screenH, uiW, uiH, paddingX, paddingY);

                case UIAnchor.LeftCenter:
                    return AnchorLeftCenter(screenW, screenH, uiW, uiH, paddingX, paddingY);
                default:
                    return Vector2.Zero;
            }
        }
    }
}
