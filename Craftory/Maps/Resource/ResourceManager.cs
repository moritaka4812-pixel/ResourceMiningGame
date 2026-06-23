
namespace Craftory.Maps.Resource
{
    public class ResourceManager
    {
        private Dictionary<ResourceType, int> _resources;

        public ResourceManager() 
        {
            _resources = new Dictionary<ResourceType, int>();

            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                _resources[type] = 0;
            }
        }

        public int Get(ResourceType type)
        {
            return _resources[type];
        }

        public void Add(ResourceType type, int amount)
        {
            _resources[type] += amount;
        }

        public void Set(ResourceType type, int amount)
        {
            _resources[type] = amount;
        }

        public IReadOnlyDictionary<ResourceType, int> GetAll()
        {
            return _resources;
        }
    }
}
