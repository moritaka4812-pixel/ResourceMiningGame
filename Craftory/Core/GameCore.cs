using Craftory.Maps;
using Craftory.Maps.Resource;

namespace Craftory.Core
{
    public class GameCore
    {
        public MapManager MapManager;
        public ResourceManager ResourceManager;

        public GameCore()
        {
            MapManager = new MapManager();
            ResourceManager = new ResourceManager();
        }
    }
}
