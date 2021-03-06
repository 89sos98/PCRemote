#region using

using System.Runtime.InteropServices;
using PCRemote.Core.Contracts;

#endregion

namespace PCRemote.Core.Commands
{
    public class DarkScreenCommand : ICommand
    {
        [DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        #region Implementation of ICommand

        public void Execute()
        {
            SendMessage(0xFFFF, 0x112, 0xF170, 2);
        }

        #endregion
    }
}