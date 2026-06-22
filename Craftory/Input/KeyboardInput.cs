using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Craftory.Input
{
    public class KeyboardInput
    {
        public KeyboardState Current { get; private set; } //現在のキーボード状態
        public KeyboardState Previous { get; private set; } //前フレームのキーボード状態

        public void Update() //Updateでキーボードの状態更新
        {
            Previous = Current;
            Current = Keyboard.GetState();
        }

        public bool IsDown(Keys key) => Current.IsKeyDown(key); //キーが押し込まれているか
        public bool Pressed(Keys key) =>
            Current.IsKeyDown(key) && Previous.IsKeyUp(key); //キーが押された瞬間か
    }
}
