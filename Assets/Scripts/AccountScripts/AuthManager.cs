using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public DatabaseReference userDatabaseRef;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    [SerializeField] private RegisterConfirmation myConfirmationWindow;

    private void Start()
    {
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("YOUR_DATABASE_URL_HERE");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        OpenConfirmationWindow("Sikeresen regisztráltál, menjen a játék!");
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        //userDatabaseRef = FirebaseDatabase.DefaultInstance.GetReference("users");
    }

    public void LoginButton()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    private IEnumerator Login(string email, string password)
    {
        var task = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogWarning($"Failed to sign in with {task.Exception}");
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1}) ({2})", user.DisplayName, user.Email, user.UserId);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            SceneManager.LoadScene("SampleScene");
        }
    }

    private IEnumerator Register(string email, string password, string username)
    {
        if (string.IsNullOrEmpty(username))
        {
        warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
        warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => task.IsCompleted);
            if (task.Exception != null)
            {
                Debug.LogWarning($"Failed to create user with {task.Exception}");
                FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register failed!";
                switch (errorCode)
                {
                    case AuthError.EmailAlreadyInUse:
                        message = "Email is already in use";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid email format";
                        break;
                    case AuthError.WeakPassword:
                        message = "Password is too weak";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                user = task.Result;
                myConfirmationWindow.confirmButton.onClick.AddListener(ConfirmButtonClicked);


                warningRegisterText.text = "";
                confirmLoginText.text = "Registration Successful";
                myConfirmationWindow.gameObject.SetActive(true);

                //SceneManager.LoadScene("SampleScene");
            }
        }
    }
    
    private void OpenConfirmationWindow(string message)
    {
        myConfirmationWindow.confirmButton.onClick.AddListener(ConfirmButtonClicked);
        myConfirmationWindow.confirmRegisterText.text = message;
    }

    private void ConfirmButtonClicked()
    {
         Debug.Log("Registration successful");
        // Save user data to Firebase Realtime Database
        //https://firebase.google.com/docs/database/unity/save-data
        userDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users");

            if (user != null)
            {
                // Create a new child under the "users" node with the UID as the key and the username as the value.
                userDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users");
                Dictionary<string, object> childUpdates = new Dictionary<string, object>();
                childUpdates["/" + user.UserId + "/username"] = usernameRegisterField.text;
                userDatabaseRef.UpdateChildrenAsync(childUpdates);
            }
            else
            {
                Debug.LogWarning("User is null, unable to save user data to database.");
            }
        Debug.Log("Confirm button clicked");
        myConfirmationWindow.gameObject.SetActive(false);
    }
}