using ResourceMiningGame.UI.Elements;
using Panel = ResourceMiningGame.UI.Elements.Panel;
using Button = ResourceMiningGame.UI.Elements.Button;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.Input;
using System.DirectoryServices;

namespace ResourceMiningGame.GameUI
{
    public class ToolPanel
    {
        public Panel panel; //UI.Elements.Panel
        private Button handleButton; //取って
        private bool isOpen;
        private float targetX;
        float openX;
        float closedX;

        public ToolPanel(UIFactory ui)
        {
            panel = new Panel(200, 300);
            panel.RelativeHeight = 1f;
            panel.RelativeWidth = 0.35f;
            panel.RelativeX = -panel.RelativeWidth; //初期の閉じている状態
            panel.RelativeY = 0f;
            panel.OnLeftClickHandler = (MouseInput) => true;

            handleButton = ui.CreateTextButton("T", 0, 0, 40, 40);
            panel.AddChild(handleButton);
            handleButton.RelativeX = 1f;
            handleButton.RelativeY = 0.50f;
            handleButton.OnClicked += Toggle;

            panel.RecalculateLayout();
            handleButton.RecalculateLayout();

            isOpen = false;
            openX = 0f;
            closedX = (float) - panel.RelativeWidth;
        }

        private void Toggle()
        {
            isOpen = !isOpen;
            targetX = isOpen ? 0 : (float) -panel.RelativeWidth;
        }

        public bool Update(MouseInput mouse)
        {
            panel.RelativeX = MathHelper.Lerp((float)panel.RelativeX, isOpen ? openX : closedX, 0.2f);

            return panel.Update(mouse);
        }

        public void Draw(SpriteBatch sb)
        {
            panel.Draw(sb);
        }
    }
}
