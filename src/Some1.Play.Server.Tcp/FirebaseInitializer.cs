using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Some1.Play.Server.Tcp
{
    public static class FirebaseInitializer
    {
        public static async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var credential = await GoogleCredential.FromFileAsync(@".\some1-476d0-firebase-adminsdk-9ns3b-a72d5c388e.json", cancellationToken);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
                ProjectId = "some1-476d0"
            });
        }
    }
}
