using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Core
{
    /// <summary>
    /// 邮件发送帮助类（MailKit）  //gyrksuiipwurdbcd
    /// </summary>
    public static class EmailSendHelper
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromName">发件人姓名</param>
        /// <param name="fromEmail">发件人邮箱</param>
        /// <param name="toEmail">收件人邮箱</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="password">授权码</param>
        /// <returns></returns>
        public static string Send(string fromName,string fromEmail,string  toEmail,string subject,string content,string  password)
        {
            var message = new MimeMessage();  //初始化了一个右键发送程序
            message.From.Add(new MailboxAddress(fromName, fromEmail));   //“发件人姓名”，“发件人邮箱”
            message.To.Add(new MailboxAddress("test", toEmail));   //“收件人姓名(不生效)” ，“收件人邮箱”
            message.Subject = subject;                                //“邮件标题”
                                                                     //html or plain
            var bodyBuilder = new BodyBuilder();                      //初始化一个消息正文
            bodyBuilder.HtmlBody = content;   //“消息内容”
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //smtp服务器，端口，是否开启ssl
                    client.Connect("smtp.qq.com", 465, true);
                    client.Authenticate("3393597524@qq.com", password);  //“自己的邮箱”，“授权码”
                    client.Send(message);
                    client.Disconnect(true);
                }
                return "邮件发送成功";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 发送登录提醒
        /// </summary>
        /// <param name="toEmail">发给谁</param>
        /// <param name="password">邮箱授权码</param>
        /// <returns></returns>
        public static string SendLoginEmail(string toEmail, string password)
        {
            var res = Send("BossTeam团队","3393597524@qq.com", toEmail,"登陆提醒",$"亲爱的员工，您于{DateTime.Now}登录了！如非本人操作请修改密码！",password);
            return res;
        }

    }






}
