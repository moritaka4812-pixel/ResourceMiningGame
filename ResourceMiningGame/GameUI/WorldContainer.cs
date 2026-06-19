using ResourceMiningGame.Core;
using ResourceMiningGame.Input;

namespace ResourceMiningGame.GameUI
{
    public class WorldContainer : WorldUIElement
    {
        public List<WorldUIElement> Children = new();

        public WorldContainer(Camera camera) : base(camera) { }

        public void AddChild(WorldUIElement child)
        {
            child.ParentWorld = this;
            Children.Add(child);
        }

        public override bool UpdateWorld(MouseInput mouse)
        {
            bool consumed = false;
            foreach (var child in Children)
                consumed |= child.UpdateWorld(mouse);
            return consumed;
        }

        public override void DrawWorld(SpriteBatch sb)
        {
            foreach (var child in Children)
                child.DrawWorld(sb);
        }
    }
}

