using Color = Microsoft.Xna.Framework.Color;
using Button = ResourceMiningGame.UI.Elements.Button;
using Panel = ResourceMiningGame.UI.Elements.Panel;
using ResourceMiningGame.GameUI;
using ResourceMiningGame.UI.Core;
using ResourceMiningGame.Screens;
using ResourceMiningGame.UI.Elements;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.Core;

namespace ResourceMiningGame.ScreenUI
{
    public class GamePlayUIScreenBuilder
    {
        private Game1 game;
        private UIFactory ui;
        private WorldUIFactory worldui;
        private GamePlayScreen screen;

        public GamePlayUIScreenBuilder(Game1 game, GamePlayScreen screen, Camera camera)
        {
            this.game = game;
            this.ui = new UIFactory(game);
            this.screen = screen;
            this.worldui = new WorldUIFactory(game, camera);
        }

        public (Button settingsButton, ToolPanel toolPanel, WorldPanel confirmPanel) BuildUI() //GamePlayScreenのUI
        {
            var settingsButton = ui.CreateImageButton("UI/gear", 760, 20, 32, 32);
            settingsButton.SetBackgroundColor(Color.White);
            settingsButton.Anchor = UIAnchor.TopRight;
            settingsButton.PaddingX = 10;
            settingsButton.PaddingY = 10;
            settingsButton.OnClicked += () => game.PushScreen(new GameSettingScreen(game));

            var toolPanel = new ToolPanel(ui);

            toolPanel.OnBuildRequested += (type) => screen.EnterBuildMode(type);

            var confirmPanel = worldui.CreateWorldPanel(80, 40);

            var okButton = worldui.CreateWorldTextButton("o", 0, 0, 40, 40);
            var cancelButton = worldui.CreateWorldTextButton("x", 40, 0, 40, 40);

            okButton.LeftClicked += () => screen.ConfirmBuild();
            cancelButton.LeftClicked += () => screen.CancelBuild();

            confirmPanel.AddChild(okButton);
            confirmPanel.AddChild(cancelButton);

            return (settingsButton, toolPanel, confirmPanel);
        }
    }
}
