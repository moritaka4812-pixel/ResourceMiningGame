
using ResourceMiningGame.Controller;
using ResourceMiningGame.Input;
using ResourceMiningGame.Maps.Tiles;
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps;

namespace ResourceMiningGame.System
{
    public class TileSelectionSystem
    {
        private TileSelectionController controller;
        public Tile SelectedTile { get; private set; }

        public TileSelectionSystem(TileSelectionController controller)
        {
            this.controller = controller;
        }

        public void Update(MouseInput mouse, Camera camera)
        {
            var result = controller.SelectTile(mouse, camera);

            if (result.Type == TileSelectionResultType.Selected)
                SelectedTile = result.Tile;

            else if (result.Type == TileSelectionResultType.Outside)
                SelectedTile = null;
        }
    }
}
