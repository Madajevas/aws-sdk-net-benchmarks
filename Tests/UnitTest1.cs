extern alias Forked;
extern alias Original;

using MimeKit;
using System.IO;

namespace Tests
{
    public class Tests
    {
        private Original::Amazon.Runtime.Internal.IRequest originalRequest;
        private MemoryStream memoryStream;
        private FileStream fileStream;

        [OneTimeSetUp]
        public void Setup()
        {
            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Memory Test", "memory@test"));
            message.To.Add(new MailboxAddress("To #1", "to.1@test"));
            message.To.Add(new MailboxAddress("To #2", "to.2@test"));
            message.To.Add(new MailboxAddress("To #3", "to.3@test"));
            message.Subject = "Comparing memory consumption";
            message.Headers.Add(new Header("X-CUSTOM-HEADER", Guid.NewGuid().ToString()));
            message.Headers.Add(new Header("X-MAYBE-ANOTHer", Guid.NewGuid().ToString()));

            var emailBody = new BodyBuilder
            {
                TextBody = "Lorem ipsum odor amet, consectetuer adipiscing elit. Eleifend efficitur magnis; ad tellus mus aliquam turpis non porttitor. Odio elit gravida viverra integer et ullamcorper dui. Nec morbi felis facilisi et vulputate nunc; justo primis nisl? Laoreet posuere diam pretium ridiculus semper, ligula vehicula at. Netus facilisis scelerisque hac tempor fusce quis pretium vivamus. Felis quis blandit eu; dictumst parturient molestie. Laoreet convallis interdum iaculis eros massa blandit primis inceptos eros. Himenaeos placerat sollicitudin lorem cursus dapibus quisque laoreet.",
                HtmlBody = """<div class="lg:text-[28px] lg:leading-tight sm:text-xl sm:leading-tight text-lg leading-tight text-neutral-600 font-serif sm:*:mb-6 *:mb-4" id="text"><p>Lorem ipsum odor amet, consectetuer adipiscing elit. Eleifend efficitur magnis; ad tellus mus aliquam turpis non porttitor. Odio elit gravida viverra integer et ullamcorper dui. Nec morbi felis facilisi et vulputate nunc; justo primis nisl? Laoreet posuere diam pretium ridiculus semper, ligula vehicula at. Netus facilisis scelerisque hac tempor fusce quis pretium vivamus. Felis quis blandit eu; dictumst parturient molestie. Laoreet convallis interdum iaculis eros massa blandit primis inceptos eros. Himenaeos placerat sollicitudin lorem cursus dapibus quisque laoreet.</p>"""
            };
            var first = new byte[1024 * 1024];
            Random.Shared.NextBytes(first);
            var second = new byte[1024 * 1024];
            Random.Shared.NextBytes(second);
            emailBody.Attachments.Add("1.png", first);
            emailBody.Attachments.Add("2.jpg", second);
            message.Body = emailBody.ToMessageBody();

            memoryStream = new MemoryStream();
            message.WriteTo(memoryStream);
            memoryStream.Position = 0;

            fileStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1024, FileOptions.DeleteOnClose);
            message.WriteTo(fileStream);
            fileStream.Position = 0;

            var sendRequest = new Original::Amazon.SimpleEmail.Model.SendRawEmailRequest
            {
                RawMessage = new Original::Amazon.SimpleEmail.Model.RawMessage(memoryStream)
            };
            var marshaller = new Original::Amazon.SimpleEmail.Model.Internal.MarshallTransformations.SendRawEmailRequestMarshaller();
            originalRequest = marshaller.Marshall(sendRequest);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            memoryStream.Dispose();
            fileStream.Dispose();
        }

        [Test]
        public void ConvertingToBase64_FromMemoryStream_ProducesSameResult()
        {
            memoryStream.Position = 0;
            var sendRequest = new Forked::Amazon.SimpleEmail.Model.SendRawEmailRequest
            {
                RawMessage = new Forked::Amazon.SimpleEmail.Model.RawMessage(memoryStream)
            };
            var marshaller = new Forked::Amazon.SimpleEmail.Model.Internal.MarshallTransformations.SendRawEmailRequestMarshaller();

            var request = marshaller.Marshall(sendRequest);

            Assert.That(
                request.Parameters["RawMessage.Data"],
                Is.EqualTo(originalRequest.Parameters["RawMessage.Data"]));
        }

        [Test]
        public void ConvertingToBase64_FromFileStream_ProducesSameResult()
        {
            fileStream.Position = 0;
            var sendRequest = new Forked::Amazon.SimpleEmail.Model.SendRawEmailRequest
            {
                RawMessage = new Forked::Amazon.SimpleEmail.Model.RawMessage(fileStream)
            };
            var marshaller = new Forked::Amazon.SimpleEmail.Model.Internal.MarshallTransformations.SendRawEmailRequestMarshaller();

            var request = marshaller.Marshall(sendRequest);

            Assert.That(
                request.Parameters["RawMessage.Data"],
                Is.EqualTo(originalRequest.Parameters["RawMessage.Data"]));
        }
    }
}