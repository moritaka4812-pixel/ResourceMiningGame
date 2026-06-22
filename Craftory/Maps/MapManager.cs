
using Craftory.Core;
using Craftory.Maps.Buildings;
using Craftory.Maps.Shadow;
using Craftory.Maps.Tiles;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps
{
    public class MapManager
    {
        public IMap Map { get; set; }
        public List<BuildingInstance> Buildings { get; private set; }
        private TileAnimator tileAnimator;
        public MapShadowGenerator shadowGenerator {  get; private set; }

        public MapManager(IMap map, GraphicsDevice graphics)
        {
            Map = map;
            Buildings = new();
            tileAnimator = new TileAnimator(map);
            shadowGenerator = new MapShadowGenerator(map, graphics);

        }

        public void AddBuilding(BuildType type, Point tilePos)
        {
            var building = new BuildingInstance(type, tilePos);
            
            foreach(var pos in building.OccupiedTiles)
            {
                var tile = Map.GetTile(pos.X, pos.Y);
                tile.Occupant = building;
                tile.ShadowSources.Add(new ShadowSource
                {
                    Type = ShadowSourceType.Building,
                    Strength = 2
                });
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
            shadowGenerator.Draw(sb);

            foreach (var b in Buildings)
                b.Draw(sb, camera);
        }
    }
}
