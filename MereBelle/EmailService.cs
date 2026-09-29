using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MereBelle
{
    public static class EmailService
    {
        private static readonly HttpClient Http = new();

        public static async Task SendConfirmationAsync(string name, string email, string code)
        {
            string apiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY")
                ?? throw new InvalidOperationException("BREVO_API_KEY is not set.");
            string sender = Environment.GetEnvironmentVariable("BREVO_SENDER_EMAIL")
                ?? throw new InvalidOperationException("BREVO_SENDER_EMAIL is not set.");

            var payload = new
            {
                sender = new { name = "Mère Belle", email = sender },
                to = new[] { new { email, name } },
                subject = "Confirm your Mère Belle account",
                htmlContent = $@"
                    <div style='font-family:Georgia,serif;background:#FFF0F5;padding:30px;text-align:center'>
                      <h1 style='color:#B0245A;font-style:italic'>Mère Belle</h1>
                      <p style='color:#E75480'>beauty, lovingly nurtured</p>
                      <p style='color:#5A2A3C'>Hello {System.Net.WebUtility.HtmlEncode(name)}, welcome!<br>
                      Your confirmation code is:</p>
                      <h2 style='color:#D63C6E;letter-spacing:6px'>{code}</h2>
                    </div>"
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email")
            {
                Content = JsonContent.Create(payload)
            };
            request.Headers.Add("api-key", apiKey);

            using var response = await Http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Email failed ({(int)response.StatusCode}): {body}");
            }
        }
    }
}