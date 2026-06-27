
namespace Craftory.Item
{
    public static class ItemRegistry
    {
        public static readonly Dictionary<ItemType, ItemData> Data =
            new()
            {
                {
                    ItemType.CopperOre,
                    new ItemData
                    {
                        Type = ItemType.CopperOre,
                        Category = ItemCategory.Ore,
                        TexturePath = "Items/Ore/CopperOre"
                    }
                },
                /*
                {
                    ItemType.CopperIngot,
                    new ItemData
                    {
                        Type = ItemType.CopperIngot,
                        Category = ItemCategory.Ingot,
                        TexturePath = "Items/Ingot/CopperIngot"
                    }
                }*/
            };

        public static void LoadTextures()
        {
            foreach (var kv in Data)
            {
                var data = kv.Value;
                data.Texture = ContentLoader.LoadTexture(data.TexturePath);
            }
        }
    }
}
