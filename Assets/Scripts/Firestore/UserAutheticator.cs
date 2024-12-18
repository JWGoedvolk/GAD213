using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Auth;

namespace JW.GPG.Firestore
{
    /// <summary>
    /// This script will save and manage the users for the game by using Firebase Firestore
    /// </summary>
    public class UserAutheticator : MonoBehaviour
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        [Header("Inputs")]
        [SerializeField] private TMP_InputField emailField;
        [SerializeField] private TMP_InputField passwordField;

        [Header("Texts")]
        [SerializeField] private string email;
        [SerializeField] private string password;

        public void GetInputs()
        {
            email = emailField.text;
            password = passwordField.text;
        }

        public void AuthenticateUser()
        {
            GetInputs();

            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                // TODO: Close log in screen

                GameManager.UserAuthenticated = true;
            });
        }

        public void SignUpNewUser()
        {
            GetInputs();

            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                // TODO: Close log in screen

                GameManager.UserAuthenticated = true;
            });
        }

        public void SignOut()
        {
            auth.SignOut();
        }
    }
}
