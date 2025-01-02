using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.Model;
using ClinicManagementSystem.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ClinicManagementSystem.Service
{
	/// <summary>
	/// Lớp EmailService chứa các phương thức gửi email
	/// </summary>
	public class EmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService()
		{
			var emailSettings = new EmailSettings();
			ConfigurationManager.AppSetting
				.GetSection("EmailSettings")
				.Bind(emailSettings);
			_emailSettings = emailSettings;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string body)
		{
			try
			{
				var email = new MimeMessage();
				email.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
				email.To.Add(MailboxAddress.Parse(toEmail));
				email.Subject = subject;

				var builder = new BodyBuilder();
				builder.HtmlBody = body;
				email.Body = builder.ToMessageBody();

				using (var smtp = new SmtpClient())
				{
					await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
					await smtp.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
					await smtp.SendAsync(email);
					await smtp.DisconnectAsync(true);
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Failed to send email: {ex.Message}");
			}
		}
	}
}
