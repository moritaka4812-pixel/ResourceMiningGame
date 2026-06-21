using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Buildings
{
    public class Drill : BuildingInstance
    {
        public Drill(Point pos) :
            base(BuildType.Drill, pos)
        {

        }

        public override void UpdateLogic(GameTime gameTime)
        {

        }
    }
}
