using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using RevitPlugIn.Dtos;


namespace RevitPlugIn.Utils
{
    public class APIUtils
    {
        private static readonly HttpClient client;
        private static string BaseUrl = "https://frtask4.azurewebsites.net/";
        private static string SasToken = ConfigurationManager.AppSettings["SASKey"];
        static APIUtils()
        {
            var handler = new HttpClientHandler();
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            client = new HttpClient(handler);
        }

        public static async Task<string> GetAuthTokenAsync(string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var jsonRequest = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(BaseUrl + "login", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic responseData = JsonConvert.DeserializeObject(responseBody);
            string token = responseData.token;

            return token;
        }
        public static async Task<UserLoggedDto> GetUserInfoAsync(string token)
        {
            try
            {
                // API'den kullanıcı bilgilerini almak için istek yap
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(BaseUrl + "login/userinfo");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<UserLoggedDto>(responseBody);
                return userInfo;
            }
            catch (Exception ex)
            {
                // Hata durumunda işlemleri burada ele al
                Console.WriteLine("Error occurred: " + ex.Message);
                return null;
            }
        }
        public static async Task AddFamiliesFromBlobAsync()
        {
            try
            {
                // API endpoint'i
                string addFamiliesFromBlobUrl = BaseUrl + "family/addfamily";

                // HTTP isteğini yap
                var response = await client.PostAsync(addFamiliesFromBlobUrl, null);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Families added from Blob Storage successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while adding families from Blob Storage: " + ex.Message);
            }
        }

        public static async Task<List<FamilyDto>> GetAllFamiliesAsync()
        {
            try
            {
                var response = await client.GetAsync(BaseUrl + "family");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var families = JsonConvert.DeserializeObject<List<FamilyDto>>(responseBody);
                return families;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred: " + ex.Message);
                return null;
            }
        }

        public static async Task<bool> DeleteFamilyAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{BaseUrl}family/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public static async Task<FamilyDto> GetFamilyAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"{BaseUrl}family/{id}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var family = JsonConvert.DeserializeObject<FamilyDto>(responseBody);
                return family;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while getting family: " + ex.Message);
                return null;
            }
        }
        public static async Task UploadFamilyToBlobAsync(string fileName, byte[] fileContent)
        {
            try
            {
                // API endpoint'i
                string uploadUrl = BaseUrl + "family/upload";

                // Dosya içeriğini hazırla
                ByteArrayContent content = new ByteArrayContent(fileContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Dosya adını ve içeriğini ekleyerek HTTP isteği oluştur
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(content, "file", fileName);

                    // HTTP isteğini yap
                    var response = await client.PutAsync(uploadUrl, formData);
                    response.EnsureSuccessStatusCode();
                }

                Console.WriteLine("File uploaded successfully to Azure Blob Storage.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while uploading file to Azure Blob Storage: " + ex.Message);
            }
        }

        public static async Task<byte[]> DownloadFamilyFromBlobAsync(string fileUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                // URL'ye SAS belirtecini ekleyin
                var uriBuilder = new UriBuilder(fileUrl);
                var queryParams = uriBuilder.Query.TrimStart('?');
                var uriWithSasToken = $"{uriBuilder.Scheme}://{uriBuilder.Host}{uriBuilder.Path}?{queryParams}&{SasToken}";

                var response = await client.GetAsync(uriWithSasToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download family from blob. Status code: {response.StatusCode}");
                }

                return await response.Content.ReadAsByteArrayAsync();
            }
        }

    }

}
