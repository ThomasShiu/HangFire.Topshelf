using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Hangfire.Topshelf.Jobs.Model;
namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 訊息中心類別
    /// </summary>
    public class MC
    {
        /// <summary>
        /// Gets or sets the paras.
        /// </summary>
        /// <value>The paras.</value>
        public IMailConfig Paras { get; set; }
      
        /// <summary>
        /// 任務管制碼
        /// </summary>
        /// <value>任務管制碼</value>
        public string SRNO { get; set; }

        /// <summary>
        /// 通知類型
        /// </summary>
        /// <value>MA:電子信件;SM:簡訊</value>
        public string NTYPE { get; set; }

        /// <summary>
        /// 訊息類型
        /// </summary>
        /// <value>M:資料庫維護;C:資料檢查;A:應用程式呼叫</value>
        public string MTYPE { get; set; }

        /// <summary>
        /// 設定或取得 管制碼所對應的標題
        /// </summary>
        /// <value>管制碼標題</value>
        public string SRNOTITL { get; set; }

        /// <summary>
        /// 是否有附件的旗標
        /// </summary>
        /// <value><c>true</c> 表示有附件; 反之為<c>false</c>.</value>
        public bool ATTACHFLAG { get; set; }

        /// <summary>
        /// 附件檔案位置
        /// </summary>
        /// <value>包含檔名及路徑</value>
        public string ATTACHDIR { get; set; }

        /// <summary>
        /// Gets or sets the mail body.
        /// </summary>
        /// <value>The mail body.</value>
        public string MailBody { get; set; }

        /// <summary>
        /// Gets or sets to users.
        /// </summary>
        /// <value>To users.</value>
        public List<SS_MUR> ToUsers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MC" /> class.
        /// </summary>
        /// <param name="aParas">a paras.</param>
        public MC( IMailConfig aParas )
        {
            Paras = aParas;
        }
           

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
           DoSendMail(SetMessage(string.Format("自動維運系統作業通知 ({0}：[{1}])", SRNOTITL, SRNO), MailBody));
        }

        /// <summary>
        /// 產生一個 MailMessage實例，並將傳入設定到該實例中
        /// </summary>
        /// <param name="Subject">信件主旨</param>
        /// <param name="aBody">a body.</param>
        /// <returns>MailMessage 實例</returns>
        private MailMessage SetMessage( string Subject, string aBody )
        {
            MailMessage myMessage = new MailMessage()
            {
                Subject = Subject,
                SubjectEncoding = Encoding.UTF8,
                Body = aBody,  
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High,
                From = new MailAddress(Paras.MCMail, Paras.MCName, Encoding.UTF8)
            };
            foreach (var item in ToUsers)
            {
                myMessage.To.Add(new MailAddress(item.EMAIL, item.NAME, Encoding.UTF8));
            }

            if (this.ATTACHFLAG == true)
            {
                string strFilePath = this.ATTACHDIR;
                Attachment attachment1 = new Attachment(strFilePath)
                {
                    Name = System.IO.Path.GetFileName(strFilePath),
                    NameEncoding = Encoding.UTF8,
                    TransferEncoding = System.Net.Mime.TransferEncoding.Base64,
                };
                attachment1.ContentDisposition.Inline = true;
                attachment1.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;
                myMessage.Attachments.Add(attachment1);
            }
            return myMessage;
        }

        /// <summary>
        /// Does the send mail.
        /// </summary>
        /// <param name="ms">The ms.</param>
        private void DoSendMail( MailMessage ms )
        {
            SmtpClient smtp = new SmtpClient(Paras.SMTPIP);
            smtp.Port = Int32.Parse(Paras.SMTPPort);
            int tryNumber = Paras.tryNumber;
            bool failed = false;
            do
            {
                try
                {
                    Stopwatch sw = new Stopwatch();                   
                    sw.Reset();
                    sw.Start();
                    failed = false;
                    smtp.Send(ms);
                    sw.Stop();
                   
                }
                catch (Exception E)  //發信發生錯誤時
                {
                    failed = true;
                    tryNumber--;                   
                    Thread.Sleep(Int32.Parse(Paras.MCQTime)); // 等待間隔
                }
            } while (failed && tryNumber != 0);
        }       
    }
}