namespace AITetris.Classes
{
    public class Settings
    {
        // Constructor for Settings with default values.
        public Settings()
        {
            GameSpeed = 10.0;
            StartSpeed = 1000.0;
            EnableSwapBlock = false;
            EnableNextBlock = true;
            Volume = 50;
            EnableTraining = false;
            KeyBinds = new KeyBinds();
        }

        public double GameSpeed { get; set; }
        public double StartSpeed { get; set; }
        public bool EnableSwapBlock { get; set; }
        public bool EnableNextBlock { get; set; }
        public int Volume { get; set; }
        public bool EnableTraining { get; set; }
        public KeyBinds KeyBinds { get; set; }
    }
}
