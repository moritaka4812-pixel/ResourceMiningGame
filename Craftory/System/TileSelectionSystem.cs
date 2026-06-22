
using Craftory.Controller;
using Craftory.Input;
using Craftory.Maps.Tiles;
using Craftory.Core;
using Craftory.Maps;

namespace Craftory.System
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
