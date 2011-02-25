namespace PCRemote.Core.Configuration
{
    /// <summary>
    /// �ṩ�������ȫ������
    /// </summary>
    public class GlobalSettings
    {
        private static readonly GlobalSettings Settings;

        static GlobalSettings()
        {
            Settings = new GlobalSettings();
        }

        /// <summary>
        /// ����
        /// </summary>
        public int CacheSizeMb { get; set; }

        /// <summary>
        /// �ٶ�����
        /// </summary>
        public int SpeedLimit { get; set; }

        /// <summary>
        /// ȡ��ȫ������
        /// </summary>
        /// <returns></returns>
        public static GlobalSettings GetSettings()
        {
            return Settings;
        }
    }
}