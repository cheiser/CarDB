using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using CarDB.Models;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.ComponentModel;

namespace CarDB
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            await sendConfirmationMail(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task sendConfirmationMail(IdentityMessage message)
        {
            try {
                System.Diagnostics.Debug.WriteLine("TRYING TO SEND MAIL");
                //MailMessage mail = new MailMessage("noreply@hotmail.com", message.Destination);
                MailMessage mail = new MailMessage(ConfigurationManager.AppSettings["mailAccount"],
                    message.Destination);
                SmtpClient client = new SmtpClient();
                //client.Port = 25;
                client.Port = 587;
                //client.Port = 465;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true; // True if server uses SSL
                client.UseDefaultCredentials = false;
                //client.Host = "smtp.google.com";
                //client.Host = "smtp.gmail.com";
                client.Host = "smtp-mail.outlook.com";
                var credentials = new NetworkCredential(
                 ConfigurationManager.AppSettings["mailAccount"],
                 ConfigurationManager.AppSettings["mailPassword"]
                 );
                client.Credentials = credentials;
                client.Timeout = 10000;
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mail);

                //client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                //client.SendAsync("tillinvite@gmail.com", message.Destination, mail.Subject, message.Body, "userState");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Got an exception while sending mail: " + e.ToString());
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
