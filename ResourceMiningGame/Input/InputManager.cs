
namespace ResourceMiningGame.Input
{
    public class InputManager //入力デバイスの状態を管理するクラス
    {
        public MouseInput Mouse { get; }
        public KeyboardInput keyboard { get; }

        public InputManager()
        {
            Mouse = new MouseInput();
            keyboard = new KeyboardInput();
        }

        public void Update() //各入力情報を前フレームから更新
        {
            Mouse.Update();
            keyboard.Update();
        }
    }
}
