using Craftory.Item;
using Craftory.Maps.Resource;

namespace Craftory.Conversion
{
    public class ResourceToItemConvertor
    {
        private static readonly Dictionary<TileResourceType, ItemType> map =
            new()
            {
                {TileResourceType.Copper,  ItemType.CopperOre},
                
            };

        public static ItemType Convert(TileResourceType resource)
        {
            return map.TryGetValue(resource, out var item)
                ? item
                : ItemType.None;
        }
    }
}
