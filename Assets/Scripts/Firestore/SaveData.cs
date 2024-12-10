using Firebase.Firestore;

namespace JW.GPG.Firestore
{
    [FirestoreData]
    public class SaveData
    {
        private string username;
        private string password;
        private string data;

        [FirestoreProperty]
        public string Username
        {
            get => username; 
            set => username = value;
        }

        [FirestoreProperty]
        public string Password
        {
            get => password;
            set => password = value;
        }

        [FirestoreProperty]
        public string Data
        {
            get => data;
            set => data = value;
        }
    } 
}
