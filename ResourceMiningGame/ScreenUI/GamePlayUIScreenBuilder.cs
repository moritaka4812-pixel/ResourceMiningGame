using Color = Microsoft.Xna.Framework.Color;
using Button = ResourceMiningGame.UI.Elements.Button;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.Screens;
using ResourceMiningGame.UI.Elements;

namespace ResourceMiningGame.ScreenUI
{
    public class GamePlayUIScreenBuilder
    {
        private Game1 game;
        private UIFactory ui;

        public GamePlayUIScreenBuilder(Game1 game)
        {
            this.game = game;
            this.ui = new UIFactory(game);
        }

        public (Button settingsButton, ToolPanel toolPanel) BuildUI() //GamePlayScreenのUI
        {
            var settingsButton = ui.CreateImageButton("UI/gear", 760, 20, 32, 32);
            settingsButton.SetBackgroundColor(Color.White);
            settingsButton.Anchor = UIAnchor.TopRight;
            settingsButton.PaddingX = 10;
            settingsButton.PaddingY = 10;
            settingsButton.OnClicked += () => game.PushScreen(new GameSettingScreen(game));

            var toolPanel = new ToolPanel(ui);
            var list = new ScrollMultiList();
            list.RelativeY = 0.15f;
            list.RelativeHeight = 0.7f;
            list.RelativeWidth = 1f;

            list.Add(ui.CreateTextButton("a", 0, 0, 1, 1));
            list.Add(ui.CreateTextButton("b", 0, 0, 1, 1));
            list.Add(ui.CreateTextButton("c", 0, 0, 1, 1));
            list.Add(ui.CreateTextButton("d", 0, 0, 1, 1));

            toolPanel.panel.AddChild(list);

            return (settingsButton, toolPanel);
        }
    }
}
