using Craftory.Input;
using Craftory.Maps.Resource;
using Craftory.UI.Core;
using Craftory.UI.Elements;
using Panel = Craftory.UI.Elements.Panel;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Button = Craftory.UI.Elements.Button;
using Disp = System.Diagnostics.Debug;

namespace Craftory.GameUI
{
    public class InformationPanel : UIElement
    {
        private Panel panel;
        private ScrollList list;
        private TextLabel infoLabel;
        private Button handleButton;

        private ResourceManager resourceManager;
        private SpriteFont font;

        private bool isOpen;
        private float targetX;
        private float openX;
        private float closedX;

        public InformationPanel(UIFactory ui, ResourceManager manager)
        {
            resourceManager = manager;

            panel = new Panel(200, 300);
            panel.RelativeWidth = 0.25f;
            panel.RelativeHeight = 0.6f;
            panel.RelativeX = 1f;
            panel.RelativeY = 0.2f;

            list = new ScrollList(PositionMode.Relative, spacing: 10);
            list.RelativeX = 0;
            list.RelativeY = 0.1f;
            list.RelativeWidth = 1f;
            list.RelativeHeight = 0.9f;

            panel.AddChild(list);

            font = ContentLoader.LoadFont("Fonts/MyFont");

            infoLabel = new TextLabel(font, "Resources", new Rect(0, 0, 1, 1));
            infoLabel.RelativeX = 0f;
            infoLabel.RelativeY = 0f;
            infoLabel.RelativeWidth = 1f;
            infoLabel.RelativeHeight = 0.1f;

            panel.AddChild(infoLabel);

            panel.RecalculateLayout();

            handleButton = ui.CreateTextButton("i", 0, 0, 40, 40);
            handleButton.OnClicked += Toggle;


            isOpen = false;
            openX = 1f - (float)panel.RelativeWidth;
            closedX = 1f;
            targetX = closedX;

        }

        private void Toggle()
        {
            isOpen = !isOpen;
            targetX = isOpen ? openX : closedX;
        }

        public override bool Update(MouseInput mouse)
        {
            bool consumed = false;

            // パネルの開閉アニメーション
            panel.RelativeX = MathHelper.Lerp((float)panel.RelativeX, targetX, 0.2f);

            // ★ パネルのレイアウトを先に確定
            panel.RecalculateLayout();

            // ★ リストを構築
            list.Clear();
            foreach (var pair in resourceManager.GetAll())
            {
                var label = new TextLabel(font, $"{pair.Key}: {pair.Value}", new Rect(0, 0, 1, 1));
                list.Add(label);
            }

            // ★ パネルの Update（内部で ScrollList.Update が呼ばれる）
            consumed |= panel.Update(mouse);

            // ★ パネル Update の後に ScrollList のレイアウトを確定
            list.RecalculateLayout();

            // ★ ScrollList の Update
            consumed |= list.Update(mouse);

            // 取っ手ボタンの位置
            var rect = panel.Rect;
            handleButton.X = rect.X - handleButton.Width;
            handleButton.Y = rect.Y + (rect.Height - handleButton.Height) / 2;

            consumed |= handleButton.Update(mouse);

            //Disp.WriteLine(list.Rect);

            return consumed;
        }




        public override void Draw(SpriteBatch sb)
        {
            panel.Draw(sb);
            handleButton.Draw(sb);
        }

    }
}
