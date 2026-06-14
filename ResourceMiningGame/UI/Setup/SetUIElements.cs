using ResourceMiningGame.UI.Core;

namespace ResourceMiningGame.UI.Setup
{
    public class SetUIElements //UIをウィンドウサイズで再配置するクラス
    {
        private List<UIElement> elements = new(); //UIエレメントを格納する

        public void Add(UIElement element) //画面のUIを追加するメソッド
        {
            elements.Add(element);
        }

        public void UpdateLayout(int screenW, int screenH) //再配置
        {
            foreach(var e in elements)
            {
                //相対サイズの計算
                if (e.RelativeWidth.HasValue)
                    e.Width = (int) (screenW * e.RelativeWidth.Value);
                if (e.RelativeHeight.HasValue)
                    e.Height = (int)(screenH * e.RelativeHeight.Value);

                //アンカーを基準にした再配置
                var pos = UILayoutManager.GetPositionForAnchor(
                    e.Anchor,
                    screenW, screenH,
                    e.Width, e.Height,
                    paddingX: e.PaddingX,
                    paddingY: e.PaddingY
                    );

                if (e.RelativeX.HasValue) //相対配置のXの値があれば(中央基準)
                    e.X = (int)(screenW * e.RelativeX.Value - e.Width / 2);
                else e.X = (int)pos.X;

                if (e.RelativeY.HasValue) //相対配置のYの値があれば(中央基準)
                    e.Y = (int)(screenH * e.RelativeY.Value - e.Height / 2);
                else e.Y = (int)pos.Y;
            }
        }
    }
}
