using System.Diagnostics;

namespace PCRemote.Core.Utilities
{
    public class DosCommandUtility
    {
        public static string RunCmd(string command)
        {
            //ʵ��һ��Process�࣬����һ����������
            //Process����һ��StartInfo���ԣ������ProcessStartInfo�࣬������һЩ���Ժͷ��������������õ������ļ������ԣ�
            var p = new Process
                        {
                            StartInfo =
                                {
                                    FileName = "cmd.exe",
                                    Arguments = "/c " + command,
                                    UseShellExecute = false,
                                    RedirectStandardInput = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    CreateNoWindow = true
                                }
                        };
            p.Start();   //����
            p.StandardInput.WriteLine("exit");        //����Ҫ�ǵü���ExitҪ��Ȼ��һ�г�ʽִ�е�ʱ��ᵱ��
            return p.StandardOutput.ReadToEnd();      //�������ȡ������ִ�н��
        }
    }
}