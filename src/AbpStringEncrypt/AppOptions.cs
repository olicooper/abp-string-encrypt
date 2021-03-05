namespace AbpStringEncrypt
{
    public class AppOptions
    {
        public static Mode DefaultMode = Mode.Encrypt;

        public Mode Mode { get; set; } = Mode.Unset;
    }

    public enum Mode
    {
        /// <summary>
        /// Uses the 'DefaultMode' defined in the app configuration
        /// </summary>
        Unset = 0,
        Encrypt,
        Decrypt
    }
}
