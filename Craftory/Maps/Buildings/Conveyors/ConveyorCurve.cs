using Point = Microsoft.Xna.Framework.Point;

namespace Craftory.Maps.Buildings.Conveyors
{
    public class ConveyorCurve : BuildingInstance
    {
        public BuildingDirection InDirection;
        public BuildingDirection OutDirection;

        public ConveyorCurve(Point pos, BuildingDirection inDir, BuildingDirection outDir)
            :base(BuildType.ConveyorCurve, pos, outDir)
        {
            InDirection = inDir;
            OutDirection = outDir;
        }
    }
}
