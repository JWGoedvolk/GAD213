using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JW.GPG.Firestore
{
    /// <summary>
    /// This script will save and manage the users for the game by using Firebase Firestore
    /// </summary>
    public class UserAutheticator : MonoBehaviour
    {
        [SerializeField] private Firestore dbHelper;
        [SerializeField] private List<SaveDataScriptable> users = new List<SaveDataScriptable>();
        [SerializeField] private bool updateUserListOnAwake = false;

        [Header("User")]
        public bool UserVerified = false;
        public TMP_InputField UsernameField;
        public string Username;
        public TMP_InputField EmailField;
        public string Email;
        public TMP_InputField PasswordField;
        public string Password;

        private void Awake()
        {
            if (updateUserListOnAwake)
            {
                users = dbHelper.GetUserData();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GetUserEmail()
        {
            Email = EmailField.text.Trim();
        }

        public void GetPassword()
        {
            Password = PasswordField.text.Trim();
        }

        public void GetName() 
        {
            Username = UsernameField.text.Trim();
        }

        public void RegisterUser()
        {
            Debug.Log("[INFO][USER]|STARTER| Registering new user to Firestore and list of local users");

            GetUserEmail();
            GetPassword();
            GetName();

            dbHelper.Email = Email;
            dbHelper.Password = Password;
            dbHelper.Username = Username;

            dbHelper.SaveToCloud();

            users.Add(dbHelper.NewUser);
            Debug.Log("[INFO][USER]|COMPLETED| Succesfully registered a new user");
        }

        public void StartVerification()
        {
            StartCoroutine(VerifyUser());
        }

        public IEnumerator VerifyUser()
        {
            Debug.Log("[INFO][USER]|STARTED| Starting user verification");
            // Get User information
            GetUserEmail();
            GetPassword();
            dbHelper.Email = Email;

            // Chec againt list of local users first to save read calls to Firestore
            Debug.Log("[INFO][USER] Attempting to verify user locally");
            foreach (var item in users)
            {
                if (item.Email == Email) // Emails match, so check if passwords match
                {
                    if (item.Password == Password)
                    {
                        // User has entered the correct credentials so can do things
                        Debug.Log($"[INFO][USER]|COMPLETED| User {item.Name} has been sucesfully verified");
                        UserVerified = true;
                        break;
                    }
                }
            }

            if (UserVerified) // If the user has been verified from the list of stored users, then no need to get from the Firestore.
            {
                yield return null;
            }

            Debug.Log("[INFO][USER]|ONGOING| Attempting to verify user from Firestore database");
            // Get the user data from Firestore
            dbHelper.LoadFromCloud();
            while(dbHelper.PlayerRetrieved)
            {
                Debug.Log("[INFO][USER]|WAITING| Waiting to retrieve users from Firestore");
                yield return new WaitForEndOfFrame();
            }

            // Check if succesfull
            if (dbHelper.RetrieveSuccesful)
            {
                Debug.Log("[INFO][USER]|ONGOING| User data retrieved from Firestore. Proceeding with verification");
                if (Password == dbHelper.Password)
                {
                    Debug.Log("User verified");
                    UserVerified = true;
                }
                else
                {
                    Debug.Log("User not verified");
                    UserVerified = false;
                }

                yield return null;
            }
            else
            {
                Debug.LogError("Could not load user");
                yield return null;
            }
        }
    } 
}
