using Microsoft.EntityFrameworkCore;
using Trips.Models;

using System.Net.Mail;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace Trips
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DbTripsContext>(options => options.UseSqlServer(
              builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(60); });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseSession();
            app.Run();
        }

        public static void SendInvoiceEmail(string toEmail, string subject, string body, int tripId, decimal tripPrice, DateTime paymentDate)
        {
            string fromMail = "lutfitala35@gmail.com";
            string fromPassword = "yjkj lggu fvrc cpse";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toEmail));
            message.Body = body;
            message.IsBodyHtml = true;

            byte[] pdfBytes = GenerateInvoicePDF(tripId, tripPrice, paymentDate);
            MemoryStream ms = new MemoryStream(pdfBytes);
            message.Attachments.Add(new Attachment(ms, "Invoice.pdf", "application/pdf"));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }

        public static void SendTripEmail(string toEmail, string subject, string body, byte[] tripPDF)
        {
            string fromMail = "lutfitala35@gmail.com";
            string fromPassword = "yjkj lggu fvrc cpse";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(toEmail));
            message.Body = body;
            message.IsBodyHtml = true;

            MemoryStream ms = new MemoryStream(tripPDF);
            message.Attachments.Add(new Attachment(ms, "TripDetails.pdf", "application/pdf"));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);
        }

        public static byte[] GenerateInvoicePDF(int tripId, decimal tripPrice, DateTime paymentDate)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            document.Add(new Paragraph("Invoice"));
            document.Add(new Paragraph("Trip ID: " + tripId));
            document.Add(new Paragraph("Amount Paid: $" + tripPrice));
            document.Add(new Paragraph("Payment Date: " + paymentDate.ToString()));

            document.Close();
            return ms.ToArray();
        }

        public static byte[] GenerateTripPDF(Trip trip)
        {
            MemoryStream ms = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, ms);
            document.Open();

            document.Add(new Paragraph("Trip Details"));
            document.Add(new Paragraph("Trip Name: " + trip.TripName));
            document.Add(new Paragraph("Destination: " + trip.Destination));
            document.Add(new Paragraph("Start Date: " + trip.StartDate));
            document.Add(new Paragraph("End Date: " + trip.EndDate));
            document.Add(new Paragraph("Description: " + trip.TDescription));

            document.Close();
            return ms.ToArray();
        }
    }
}
