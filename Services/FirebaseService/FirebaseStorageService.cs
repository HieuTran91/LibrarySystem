using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Web;

namespace LibraryProject.Services.FirebaseService
{
    public class FirebaseStorageService
    {
        private readonly string _apiKey;
        private readonly string _storageBucket;
        private readonly string _authEmail;
        private readonly string _authPassword;
        public FirebaseStorageService(IConfiguration configuration)
        {
            _storageBucket = configuration["Firebase:StorageBucket"];
            _apiKey = configuration["Firebase:ApiKey"];
            _authEmail = configuration["Firebase:AuthEmail"];
            _authPassword = configuration["Firebase:AuthPassword"];
        }
        
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_authEmail, _authPassword);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            var task = new FirebaseStorage(
                _storageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("books")
                .Child(fileName)
                .PutAsync(file.OpenReadStream());

            try
            {
                string link = await task;
                Console.WriteLine(link);
                return link;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
                return null;
            }
        }


        public async Task DeleteImageAsync(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return;

                var storage = StorageClient.Create();
                var bucketName = _storageBucket;

                var uri = new Uri(imageUrl);
                var queryParams = HttpUtility.ParseQueryString(uri.Query);
                string encodedFilePath = uri.AbsolutePath.Split(new[] { "/o/" }, StringSplitOptions.None)[1];
                string filePath = HttpUtility.UrlDecode(encodedFilePath); 

                await storage.DeleteObjectAsync(bucketName, filePath);

                Console.WriteLine($"Deleted file: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image from Firebase: {ex.Message}");
            }
        }
    }
}