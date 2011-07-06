using PCRemote.Core;
using PCRemote.Core.Contracts;
using PCRemote.UI.Properties;

namespace PCRemote.UI.Factories
{
    public class WeiboServiceFactory
    {
        public static IWeiboService CreateInstance()
        {
            var weiboType = Settings.Default.WeiboType.ToLower();
            var accessToken = Settings.Default.AccessToken;
            var accessTokenSecret = Settings.Default.AccessTokenSecret;

            IWeiboService weiboService = null;
            switch (weiboType)
            {
                case "����΢��":
                    weiboService = new SinaWeiboService(accessToken, accessTokenSecret);
                    break;
                case "��Ѷ΢��":
                    weiboService = new QQWeiboService(accessToken, accessTokenSecret);
                    break;
                default:
                    break;
            }

            return weiboService;
        }
    }
}