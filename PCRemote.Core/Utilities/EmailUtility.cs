using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PCRemote.Core.Utilities
{
    /// <summary>
    ///  �ʼ����ȼ���high���ߣ���low(��)��normal(����)
    /// </summary>
    public enum EmailPriorityEnum
    {
        #region///�ʼ����ȼ�

        /// <summary>
        /// ��
        /// </summary>
        [Description("��")] High,
        /// <summary>
        /// ��
        /// </summary>
        [Description("��")] Low,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")] Normal

        #endregion
    }

    public class EmailUtility
    {
        /// <summary>
        /// �ʼ�����
        /// </summary>
        /// <param name="toEmailAddress">����Ŀ�������б�</param>
        /// <param name="toCcEmailAddress"></param>
        /// <param name="attachmentList">�����ʼ������б�</param>
        /// <param name="fromEmailAddress">�����˻�</param>
        /// <param name="fromEmailPassword">��������</param>
        /// <param name="emailPersonName">��������</param>
        /// <param name="emailSubject">�ʼ�����</param>
        /// <param name="emailBody">�ʼ�����</param>
        /// <param name="isBodyHtml">�Ƿ�HTML���� Ĭ��Ϊ��</param>
        /// <param name="emailPriority">�ʼ����ȼ�</param>
        /// <param name="port">����˿ں�</param>
        /// <param name="emailHostName">�����������ַ</param>
        /// <param name="isEnableSsl">�ʼ��Ƿ����:true(����),false(������)  Ĭ��Ϊtrue</param>
        /// <param name="encodingType">�����ʽ</param>
        public void Send(IList<string> toEmailAddress, IList<string> toCcEmailAddress, IList<Attachment> attachmentList,
                         string fromEmailAddress, string fromEmailPassword, string emailPersonName, string emailSubject,
                         string emailBody, bool isBodyHtml, EmailPriorityEnum emailPriority, int port,
                         string emailHostName, bool isEnableSsl, Encoding encodingType)
        {
            #region///�ʼ�����

            var mails = new MailMessage();

            //��������
            var emaiEncodingType = encodingType;

            //��ӷ��͵�ַ
            foreach (var to in toEmailAddress)
            {
                mails.To.Add(to);
            }

            // ��ӳ��͵�ַ
            foreach (var to in toCcEmailAddress)
            {
                mails.CC.Add(to);
            }

            //��Ӹ���  
            foreach (var attachment in attachmentList)
            {
                mails.Attachments.Add(attachment);
            }

            mails.From = new MailAddress(fromEmailAddress, emailPersonName, emaiEncodingType);
            mails.Subject = emailSubject;
            mails.SubjectEncoding = emaiEncodingType;
            mails.Body = HttpUtility.HtmlDecode(emailBody);
            mails.BodyEncoding = emaiEncodingType;
            //�����ʼ��Ƿ�ΪHTML��ʽ
            mails.IsBodyHtml = isBodyHtml;
            //�����ʼ��ż��ȼ�
            switch (emailPriority)
            {
                case EmailPriorityEnum.Normal:
                    mails.Priority = MailPriority.Normal;
                    break;
                case EmailPriorityEnum.Low:
                    mails.Priority = MailPriority.Low;
                    break;
                default:
                    mails.Priority = MailPriority.High;
                    break;
            }

            var client = new SmtpClient
                             {
                                 Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword),
                                 Port = port,
                                 Host = emailHostName,
                                 EnableSsl = isEnableSsl
                             };

            client.Send(mails);

            #endregion
        }
    }
}