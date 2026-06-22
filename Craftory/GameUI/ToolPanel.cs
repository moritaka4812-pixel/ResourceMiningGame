using Craftory.UI.Elements;
using Panel = Craftory.UI.Elements.Panel;
using Button = Craftory.UI.Elements.Button;
using Rect = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Craftory.UI.Core;
using Craftory.Input;
using System.DirectoryServices;
using Craftory.Maps.Buildings;

namespace Craftory.GameUI
{
    public class ToolPanel : UIElement
    {
        public Panel panel; //UI.Elements.Panel
        public event Action<BuildType> OnBuildRequested;
        private Button handleButton; //取って
        private bool isOpen;
        private float targetX;
        private Button? activeButton = null;
        float openX;
        float closedX;

        public ToolPanel(UIFactory ui)
        {
            panel = new Panel(200, 300);
            panel.RelativeHeight = 1f;
            panel.RelativeWidth = 0.35f;
            panel.RelativeX = -panel.RelativeWidth; //初期の閉じている状態
            panel.RelativeY = 0f;
            panel.OnLeftClickHandler = (mouse) => { return true; };

            handleButton = ui.CreateTextButton("T", 0, 0, 40, 40);
            panel.AddChild(handleButton);
            handleButton.RelativeX = 1f;
            handleButton.RelativeY = 0.50f;
            handleButton.OnClicked += Toggle;

            panel.RecalculateLayout();
            handleButton.RecalculateLayout();

            var list = new ScrollMultiList();
            list.RelativeY = 0.15f;
            list.RelativeHeight = 0.7f;
            list.RelativeWidth = 1f;
            list.BackgroundColor = new Color(30, 30, 30, 200);

            AddBuildButton(ui, list, "Buildings/drill", BuildType.Drill, 32);
            AddBuildButton(ui, list, "Buildings/Test", BuildType.Test, 32);

            panel.AddChild(list);

            isOpen = false;
            openX = 0f;
            closedX = (float) - panel.RelativeWidth;
        }

        private void AddBuildButton(UIFactory ui, ScrollMultiList list, string label, BuildType type, int size)
        {
            var btn = ui.CreateImageButtonFrame(label, new Rect(0, 0, size, size));
            btn.OnClicked += () =>
            {
                if(activeButton != null)
                    activeButton.IsToggle = false;

                btn.IsToggle = true;
                activeButton = btn;

                OnBuildRequested?.Invoke(type);
            };
            list.Add(btn);
        }

        private void Toggle()
        {
            isOpen = !isOpen;
            targetX = isOpen ? 0 : (float) -panel.RelativeWidth;
        }

        public override bool Update(MouseInput mouse)
        {
            base.Update(mouse);

            panel.RelativeX = MathHelper.Lerp((float)panel.RelativeX, isOpen ? openX : closedX, 0.2f);

            return panel.Update(mouse);
        }

        public override void Draw(SpriteBatch sb)
        {
            panel.Draw(sb);

        }

        public void ClearActiveButton()
        {
            if(activeButton != null)
            {
                activeButton.IsToggle = false;
                activeButton = null;
            }
        }
    }
}
