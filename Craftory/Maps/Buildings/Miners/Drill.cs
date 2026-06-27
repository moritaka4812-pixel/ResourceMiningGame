using Craftory.Conversion;
using Craftory.Core;
using Craftory.Maps.Buildings.Conveyors;
using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Miners
{
    public class Drill : BuildingInstance
    {
        public Drill(Point pos) :
            base(BuildType.Drill, pos, BuildingDirection.None)
        {

        }

        public override void UpdateLogic(GameTime gameTime)
        {
            if(!IsActive) return;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer >= 1f / WorkSpeed)
            {
                timer -= 1f / WorkSpeed;

                var tile = GameCore.Instance.MapManager.Map.GetTile(TilePosition.X, TilePosition.Y);

                if(tile.Resource != Resource.TileResourceType.None)
                {
                    TryOutputToConveyor(tile.Resource);
                }
            }
        }

        private void TryOutputToConveyor(Resource.TileResourceType type)
        {
            var dirs = new (int x, int y)[]
            {
                (1,0), (-1,0), (0,1), (0,-1)
            };

            var itemType = ResourceToItemConvertor.Convert(type);

            foreach(var dir in dirs)
            {
                var nextPos = new Point(TilePosition.X + dir.x, TilePosition.Y + dir.y);
                var tile = GameCore.Instance.MapManager.Map.GetTile(nextPos.X, nextPos.Y);

                if(tile?.Occupant is Conveyor conveyor)
                {
                    var item = new ConveyorItem { Type = itemType, GlobalPosition = 0f };

                    if (conveyor.TileLogic.TryAccept(item))
                        return; // 最初に受け取ってくれたコンベアに渡して終了
                }
            }
        }
    }
}
