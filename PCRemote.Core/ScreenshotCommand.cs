using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using PCRemote.Core.Utilities;
using WeiboSDK.Contracts;

namespace PCRemote.Core
{
    public class ScreenshotCommand : ICommand
    {
        readonly IWeiboClient _client;

        public ScreenshotCommand(IWeiboClient client)
        {
            _client = client;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            var temp = Environment.GetEnvironmentVariable("TEMP");
            var picPath = temp + "\\" + Guid.NewGuid() + ".jpg";
            ImageUtility.CaptureDesktop(picPath);

            _client.UploadStatus("������ʹ��#PCң����#�����ҵ���Ļ��ͼ " + DateTime.Now.ToLongTimeString(), picPath); 
        }

        #endregion
    }
}