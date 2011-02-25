using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using PCRemote.Core.Configuration;

namespace PCRemote.Core.Utilities
{
    /// <summary>
    /// ������صľ�̬����
    /// </summary>
    public class NetworkUtility
    {
        /// <summary>
        /// ������Ƶ�ļ�
        /// </summary>
        public static bool DownloadFile(DownloadParameter para)
        {
            //�������ٵ�Tick
            Int32 privateTick = 0;
            //�������ݰ���С = 1KB
            var buffer = new byte[1024];
            var request = HttpWebRequest.Create(para.Url);
            var response = request.GetResponse();
            para.TotalLength = response.ContentLength; //�ļ�����

            #region ����ļ��Ƿ����ع�

            //���Ҫ���ص��ļ�����
            if (File.Exists(para.FilePath))
            {
                var filelength = new FileInfo(para.FilePath).Length;
                //����ļ�������ͬ
                if (filelength == para.TotalLength)
                {
                    //�������سɹ�
                    return true;
                }
            }

            #endregion

            para.DoneBytes = 0; //����ֽ���
            para.LastTick = Environment.TickCount; //ϵͳ����
            Stream st, fs; //���������ļ���
            BufferedStream bs; //������
            int t, limitcount = 0;
            //ȷ�����峤��
            if (GlobalSettings.GetSettings().CacheSizeMb > 256 || GlobalSettings.GetSettings().CacheSizeMb < 1)
                GlobalSettings.GetSettings().CacheSizeMb = 1;
            //��ȡ������
            using (st = response.GetResponseStream())
            {
                //���ļ���
                using (fs = new FileStream(para.FilePath, FileMode.Create, FileAccess.Write, FileShare.Read, 8))
                {
                    //ʹ�û�����
                    using (bs = new BufferedStream(fs, GlobalSettings.GetSettings().CacheSizeMb*1024))
                    {
                        //��ȡ��һ������
                        var osize = st.Read(buffer, 0, buffer.Length);
                        //��ʼѭ��
                        while (osize > 0)
                        {
                            #region �ж��Ƿ�ȡ������

                            //����û���ֹ�򷵻�false
                            if (para.IsStop)
                            {
                                //�ر���
                                bs.Close();
                                st.Close();
                                fs.Close();
                                return false;
                            }

                            #endregion

                            //����������ֽ���
                            para.DoneBytes += osize;

                            //д�ļ�(����)
                            bs.Write(buffer, 0, osize);

                            //����
                            if (GlobalSettings.GetSettings().SpeedLimit > 0)
                            {
                                //���ؼ�����һcount++
                                limitcount++;
                                //����1KB
                                osize = st.Read(buffer, 0, buffer.Length);
                                //�ۻ���limit KB��
                                if (limitcount >= GlobalSettings.GetSettings().SpeedLimit)
                                {
                                    t = Environment.TickCount - privateTick;
                                    //����Ƿ����һ��
                                    if (t < 1000) //���С��һ����ȴ���һ��
                                        Thread.Sleep(1000 - t);
                                    //����count�ͼ�ʱ������������
                                    limitcount = 0;
                                    privateTick = Environment.TickCount;
                                }
                            }
                            else //���������
                            {
                                osize = st.Read(buffer, 0, buffer.Length);
                            }
                        } //end while
                    } //end bufferedstream
                } // end filestream
            } //end netstream

            //һ��˳������true
            return true;
        }

        /// <summary>
        /// ������Ļ�ļ�
        /// </summary>
        public static bool DownloadSub(DownloadParameter para)
        {
            try
            {
                //���绺��(100KB)
                var buffer = new byte[102400];
                var request = HttpWebRequest.Create(para.Url);
                var response = request.GetResponse();

                //��ȡ������
                var st = response.GetResponseStream();
                if (!para.UseDeflate)
                {
                    //���ļ���
                    using (var so = new FileStream(para.FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 8))
                    {
                        //��ȡ����
                        var osize = st.Read(buffer, 0, buffer.Length);
                        while (osize > 0)
                        {
                            //д������
                            so.Write(buffer, 0, osize);
                            osize = st.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
                else
                {
                    //deflate��ѹ��
                    var deflate = new DeflateStream(st, CompressionMode.Decompress);
                    using (var reader = new StreamReader(deflate))
                    {
                        File.WriteAllText(para.FilePath, reader.ReadToEnd());
                    }
                }
                //�ر���
                st.Close();
                //һ��˳������true
                return true;
            }
            catch
            {
                //�������󷵻�False
                return false;
            }
        }

        /// <summary>
        /// ȡ����ҳԴ����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource2(string url, Encoding encode)
        {
            var req = HttpWebRequest.Create(url);
            var res = req.GetResponse();
            var strm = new StreamReader(res.GetResponseStream(), encode);
            var sline = strm.ReadToEnd();
            strm.Close();
            return sline;
        }

        public static string GetHtmlSource(string url, Encoding encode)
        {
            var wc = new WebClient();
            var data = wc.DownloadData(url);
            return encode.GetString(data);
        }
    }

    /// <summary>
    /// ���ز���
    /// </summary>
    public class DownloadParameter
    {
        /// <summary>
        /// ��Դ������λ��
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Ҫ�����ı����ļ�λ��
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// ��Դ����
        /// </summary>
        public Int64 TotalLength { get; set; }

        /// <summary>
        /// ����ɵ��ֽ���
        /// </summary>
        public Int64 DoneBytes { get; set; }

        /// <summary>
        /// �ϴ�Tick����ֵ
        /// </summary>
        public Int64 LastTick { get; set; }

        /// <summary>
        /// �Ƿ�ֹͣ����(���������ع����н������ã������������ع��̵�ֹͣ)
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// ����ʱ�Ƿ�ʹ��Deflate��ѹ��
        /// </summary>
        public bool UseDeflate { get; set; }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// ��Ч�ַ�����
        /// </summary>
        /// <param name="input">��Ҫ���˵��ַ���</param>
        /// <param name="replace">�滻Ϊ���ַ���</param>
        /// <returns></returns>
        public static string InvalidCharacterFilter(string input, string replace)
        {
            if (replace == null)
                replace = "";

            input = Path.GetInvalidFileNameChars().Aggregate(input, (current, item) => current.Replace(item.ToString(), replace));
            return Path.GetInvalidPathChars().Aggregate(input, (current, item) => current.Replace(item.ToString(), replace));
        }
    }
}