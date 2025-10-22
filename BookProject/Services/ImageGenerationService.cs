using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BookProject.Services
{
    public class ImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "http://127.0.0.1:7860/sdapi/v1/txt2img";

        public ImageGenerationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateImageAsync(string prompt, string outputFolder)
        {
            var payload = new
            {
                prompt = prompt,
                negative_prompt = "skin showing, not safe for work, naked, nsfw, lowres, bad anatomy, worst quality, low quality",
                steps = 20,
                width = 512,
                height = 512
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(_apiUrl, content);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to Stable Diffusion API. Is the WebUI running with --api?", ex);
            }

            var result = await response.Content.ReadAsStringAsync();

            // Try parse JSON safely
            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(result);
            }
            catch (Exception)
            {
                throw new Exception($"API response is not valid JSON: {result}");
            }

            // Check for 'images' property
            if (!doc.RootElement.TryGetProperty("images", out var imagesElement))
            {
                throw new Exception($"API response did not contain 'images': {result}");
            }

            var imageBase64 = imagesElement[0].GetString();
            if (string.IsNullOrWhiteSpace(imageBase64))
                throw new Exception($"API returned empty image data: {result}");

            // Decode base64 to PNG
            var imageBytes = Convert.FromBase64String(imageBase64);
            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(outputFolder, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            return fileName;
        }
    }
}