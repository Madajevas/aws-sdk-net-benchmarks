// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using MimeKit;

BenchmarkRunner.Run<RawMessageBenchmark>();


[MemoryDiagnoser]
public class RawMessageBenchmark
{
    private MimeMessage mimeMessage;

    [GlobalSetup]
    public void SetUp()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Memory Test", "memory@test"));
        message.To.Add(new MailboxAddress("To #1", "to.1@test"));
        message.To.Add(new MailboxAddress("To #2", "to.2@test"));
        message.To.Add(new MailboxAddress("To #3", "to.3@test"));
        message.Subject = "Comparing memory consumption";
        message.Headers.Add(new Header("X-CUSTOM-HEADER", Guid.NewGuid().ToString()));
        message.Headers.Add(new Header("X-MAYBE-ANOTHer", Guid.NewGuid().ToString()));

        var emailBody = new BodyBuilder
        {
            TextBody = "Lorem ipsum odor amet, consectetuer adipiscing elit. Eleifend efficitur magnis; ad tellus mus aliquam turpis non porttitor. Odio elit gravida viverra integer et ullamcorper dui. Nec morbi felis facilisi et vulputate nunc; justo primis nisl? Laoreet posuere diam pretium ridiculus semper, ligula vehicula at. Netus facilisis scelerisque hac tempor fusce quis pretium vivamus. Felis quis blandit eu; dictumst parturient molestie. Laoreet convallis interdum iaculis eros massa blandit primis inceptos eros. Himenaeos placerat sollicitudin lorem cursus dapibus quisque laoreet.\r\n\r\nMontes ad quisque nostra convallis nostra ridiculus tortor. Egestas praesent dictumst nulla ut senectus tortor porttitor fringilla porttitor. Lacinia senectus habitant ligula senectus lacus etiam nullam consectetur. Fermentum congue cursus morbi feugiat class odio. Euismod conubia natoque pretium natoque suspendisse erat. Mattis nibh et imperdiet bibendum porttitor urna amet? Nec iaculis eleifend aenean tristique maximus malesuada ac iaculis. Tellus enim mattis tempus ornare habitasse etiam tristique. Cras penatibus proin felis; fames blandit aptent auctor consectetur. Per sem suspendisse felis magnis luctus mattis ridiculus aptent.\r\n\r\nImperdiet faucibus justo sed accumsan netus metus ut phasellus. Faucibus quam ad auctor eleifend mi justo. Natoque velit elit, nec nascetur augue suspendisse ante. Faucibus dui ac vitae dis ligula viverra dolor. Porttitor lobortis posuere nibh dictum himenaeos nullam quisque dis. Orci nullam consectetur ligula ante aliquet sapien natoque nam.\r\n\r\nAc egestas condimentum efficitur quisque urna viverra. Proin etiam himenaeos sagittis id ac etiam vitae dapibus. Phasellus natoque ultrices nec vehicula duis hac. Condimentum congue risus efficitur ut hac montes tortor. Varius parturient netus fermentum egestas suspendisse orci tortor. Netus nisl id dignissim in nostra pellentesque pharetra tellus. Bibendum vulputate nostra malesuada fermentum pretium risus ex fames. Vehicula donec donec viverra in commodo urna condimentum. Turpis quam laoreet varius varius, posuere molestie.\r\n\r\nIn tortor morbi dapibus vivamus per, finibus at molestie. Pellentesque sociosqu commodo at dapibus curabitur montes. Habitasse cursus in efficitur leo pellentesque vel tempor. Odio dapibus accumsan congue curabitur condimentum; erat luctus litora. In nam consequat molestie euismod laoreet augue volutpat ridiculus? Odio habitasse laoreet aliquet hac curae aptent cras blandit. Fames senectus congue taciti maecenas lacinia eleifend lorem quisque? Aptent ridiculus nostra ornare bibendum sem mi ligula.",
            HtmlBody = """<div class="lg:text-[28px] lg:leading-tight sm:text-xl sm:leading-tight text-lg leading-tight text-neutral-600 font-serif sm:*:mb-6 *:mb-4" id="text"><p>Lorem ipsum odor amet, consectetuer adipiscing elit. Eleifend efficitur magnis; ad tellus mus aliquam turpis non porttitor. Odio elit gravida viverra integer et ullamcorper dui. Nec morbi felis facilisi et vulputate nunc; justo primis nisl? Laoreet posuere diam pretium ridiculus semper, ligula vehicula at. Netus facilisis scelerisque hac tempor fusce quis pretium vivamus. Felis quis blandit eu; dictumst parturient molestie. Laoreet convallis interdum iaculis eros massa blandit primis inceptos eros. Himenaeos placerat sollicitudin lorem cursus dapibus quisque laoreet.</p><p>Montes ad quisque nostra convallis nostra ridiculus tortor. Egestas praesent dictumst nulla ut senectus tortor porttitor fringilla porttitor. Lacinia senectus habitant ligula senectus lacus etiam nullam consectetur. Fermentum congue cursus morbi feugiat class odio. Euismod conubia natoque pretium natoque suspendisse erat. Mattis nibh et imperdiet bibendum porttitor urna amet? Nec iaculis eleifend aenean tristique maximus malesuada ac iaculis. Tellus enim mattis tempus ornare habitasse etiam tristique. Cras penatibus proin felis; fames blandit aptent auctor consectetur. Per sem suspendisse felis magnis luctus mattis ridiculus aptent.</p><p>Imperdiet faucibus justo sed accumsan netus metus ut phasellus. Faucibus quam ad auctor eleifend mi justo. Natoque velit elit, nec nascetur augue suspendisse ante. Faucibus dui ac vitae dis ligula viverra dolor. Porttitor lobortis posuere nibh dictum himenaeos nullam quisque dis. Orci nullam consectetur ligula ante aliquet sapien natoque nam.</p><p>Ac egestas condimentum efficitur quisque urna viverra. Proin etiam himenaeos sagittis id ac etiam vitae dapibus. Phasellus natoque ultrices nec vehicula duis hac. Condimentum congue risus efficitur ut hac montes tortor. Varius parturient netus fermentum egestas suspendisse orci tortor. Netus nisl id dignissim in nostra pellentesque pharetra tellus. Bibendum vulputate nostra malesuada fermentum pretium risus ex fames. Vehicula donec donec viverra in commodo urna condimentum. Turpis quam laoreet varius varius, posuere molestie.</p><p>In tortor morbi dapibus vivamus per, finibus at molestie. Pellentesque sociosqu commodo at dapibus curabitur montes. Habitasse cursus in efficitur leo pellentesque vel tempor. Odio dapibus accumsan congue curabitur condimentum; erat luctus litora. In nam consequat molestie euismod laoreet augue volutpat ridiculus? Odio habitasse laoreet aliquet hac curae aptent cras blandit. Fames senectus congue taciti maecenas lacinia eleifend lorem quisque? Aptent ridiculus nostra ornare bibendum sem mi ligula.</p></div>"""
        };
        emailBody.Attachments.Add("1.png", File.ReadAllBytes("./1.png"));
        emailBody.Attachments.Add("2.jpg", File.ReadAllBytes("./2.jpg"));
        emailBody.Attachments.Add("3.jpg", File.ReadAllBytes("./3.jpg"));
        emailBody.Attachments.Add("4.jpg", File.ReadAllBytes("./4.jpg"));
        message.Body = emailBody.ToMessageBody();

        this.mimeMessage = message;
    }
}
