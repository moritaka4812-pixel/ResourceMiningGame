using ResourceMiningGame.UI.Elements;
using Panel = ResourceMiningGame.UI.Elements.Panel;
using Button = ResourceMiningGame.UI.Elements.Button;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.Input;

namespace ResourceMiningGame.GameUI
{
    public class ToolPanel
    {
        private Panel panel; //UI.Elements.Panel
        private Button handleButton; //取って
        private bool isOpen;
        private float currentX;
        private float targetX;

        public ToolPanel(UIFactory ui)
        {
            panel = new Panel(200, 300);
            panel.Anchor = UIAnchor.LeftCenter;
            panel.RelativeHeight = 1f;
            panel.RelativeWidth = 0.3f;
            panel.IgnoreLayoutX = true;

            handleButton = ui.CreateTextButton("T", 0, 0, 20, 40);

            handleButton.OnClicked += Toggle;

            panel.RecalculateLayout();

            isOpen = false;
            currentX = -panel.Width;
            targetX = currentX;
            panel.X = (int)currentX;
        }

        private void Toggle()
        {
            isOpen = !isOpen;
            targetX = isOpen ? 0 : -panel.Width;
        }

        public bool Update(MouseInput mouse)
        {
            //スライドアニメーション
            currentX = MathHelper.Lerp(currentX, targetX, 0.2f);
            panel.X = (int)currentX;
            //取ってボタンの一をパネルの右中央に合わせる
            handleButton.X = panel.X + panel.Width;
            handleButton.Y = panel.Y + (panel.Height - handleButton.Height) / 2;

            bool clicked = false;

            clicked |= panel.Update(mouse);
            clicked |= handleButton.Update(mouse);

            //クリックを受け取ったらTrueを返す
            return clicked;
        }

        public void Draw(SpriteBatch sb)
        {
            panel.Draw(sb);
            handleButton.Draw(sb);
        }
    }
}
