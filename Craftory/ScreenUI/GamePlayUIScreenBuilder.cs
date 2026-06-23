using Color = Microsoft.Xna.Framework.Color;
using Button = Craftory.UI.Elements.Button;
using Panel = Craftory.UI.Elements.Panel;
using Craftory.GameUI;
using Craftory.UI.Core;
using Craftory.Screens;
using Craftory.UI.Elements;
using Craftory.Maps.Buildings;
using Craftory.Core;
using Craftory.Maps.Resource;

namespace Craftory.ScreenUI
{
    public class GamePlayUIScreenBuilder
    {
        private Game1 game;
        private UIFactory ui;
        private WorldUIFactory worldui;
        private GamePlayScreen screen;
        private ResourceManager resourceManager;

        public GamePlayUIScreenBuilder(Game1 game, GamePlayScreen screen, Camera camera, ResourceManager resourceManager)
        {
            this.game = game;
            this.ui = new UIFactory(game);
            this.screen = screen;
            this.worldui = new WorldUIFactory(game, camera);
            this.resourceManager = resourceManager;
        }

        public (Button settingsButton, ToolPanel toolPanel, InformationPanel informationPanel) BuildUI() //GamePlayScreenのUI
        {
            var settingsButton = ui.CreateImageButton("UI/gear", 760, 20, 32, 32);
            settingsButton.SetBackgroundColor(Color.White);
            settingsButton.Anchor = UIAnchor.TopRight;
            settingsButton.PaddingX = 10;
            settingsButton.PaddingY = 10;
            settingsButton.OnClicked += () => game.PushScreen(new GameSettingScreen(game));

            var toolPanel = new ToolPanel(ui);

            toolPanel.OnBuildRequested += (type) => screen.buildModeController.Start(type);

            var informationPanel = new InformationPanel(ui, resourceManager);

            return (settingsButton, toolPanel, informationPanel);
        }
    }
}
