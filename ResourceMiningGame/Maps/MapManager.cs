
using ResourceMiningGame.Core;
using ResourceMiningGame.Maps.Buildings;
using ResourceMiningGame.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps
{
    public class MapManager
    {
        public IMap Map { get; set; }
        public List<BuildingInstance> Buildings { get; private set; }
        private TileAnimator tileAnimator;

        public MapManager(IMap map)
        {
            Map = map;
            Buildings = new();
            tileAnimator = new TileAnimator(map);
        }

        public void AddBuilding(BuildType type, Point tilePos)
        {
            var building = new BuildingInstance(type, tilePos);
            
            foreach(var pos in building.OccupiedTiles)
            {
                var tile = Map.GetTile(pos.X, pos.Y);
                tile.Occupant = building;
            }

            Buildings.Add(building);
        }

        public void Update(GameTime gameTime, Camera camera, GraphicsDevice device)
        {
            tileAnimator.UpdateVisibleTiles(gameTime, camera, device);
            Map.Update(gameTime);

            var range = Map.GetVisibleRange(camera, device);

            foreach(var b in Buildings)
            {
                b.UpdateLogic(gameTime);

                if (range.Contains(b.TilePosition))
                    b.UpdateVisual(gameTime);
            }
        }

        public void Draw(SpriteBatch sb, Camera camera)
        {
            var range = Map.GetVisibleRange(camera, sb.GraphicsDevice);
            Map.Draw(sb, range);

            foreach (var b in Buildings)
                b.Draw(sb, camera);
        }
    }
}
