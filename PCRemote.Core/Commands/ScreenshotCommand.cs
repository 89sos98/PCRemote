#region using

using System;
using PCRemote.Core.Contracts;
using PCRemote.Core.Utilities;
using WeiboSDK.Contracts;

#endregion

namespace PCRemote.Core.Commands
{
    public class ScreenshotCommand : ICommand
    {
        readonly IWeiboService _service;

        public ScreenshotCommand(IWeiboService service)
        {
            _service = service;
        }

        #region Implementation of ICommand

        public void Execute()
        {
            var temp = Environment.GetEnvironmentVariable("TEMP");
            var picPath = temp + "\\" + Guid.NewGuid() + ".jpg";
            ImageUtility.CaptureDesktop(picPath);

            _service.SendWeiboWithPicture("������ʹ��#PCң����#�����ҵ���Ļ��ͼ " + DateTime.Now.ToLongTimeString(), picPath);
        }

        #endregion
    }
}