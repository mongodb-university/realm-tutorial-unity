using System;
using UnityEngine;
using UnityEngine.UIElements;

public class AuthenticationManager : MonoBehaviour
{

    private static VisualElement root;
    private static VisualElement authWrapper;
    private static Label subtitle;
    private static Button startButton;
    private static Button logoutButton;
    private static string loggedInUser;
    private static TextField userInput;
    // (Part 2 Sync): isInRegistrationMode is used to toggle between
    // authentication modes
    private static bool isInRegistrationMode = false;
    // (Part 2 Sync): passInput represents the password input
    private static TextField passInput;

    // (Part 2 Sync): toggleLoginOrRegisterUIButton is the button to toggle
    // between login or registration modes
    private static Button toggleLoginOrRegisterUIButton;

    #region PrivateMethods
    private static void HideAuthenticationUI()
    {
        authWrapper.AddToClassList("hide");
        logoutButton.AddToClassList("show");
    }

    // OnPressLogin() passes the username to the RealmController,
    // ScoreCardManager, and LeaderboardManager
    private static void OnPressLogin()
    {
        try
        {
            HideAuthenticationUI();
            loggedInUser = userInput.value;
            RealmController.SetLoggedInUser(loggedInUser);
            ScoreCardManager.SetLoggedInUser(loggedInUser);
            LeaderboardManager.Instance.SetLoggedInUser(loggedInUser);
        }
        catch (Exception ex)
        {
            Debug.Log("an exception was thrown:" + ex.Message);
        }
    }



    #endregion
    #region UnityLifecycleMethods
    // Start() is inherited from MonoBehavior and is called on the frame when a
    // script is enabled Start() defines AuthenticationScreen UI elements, and
    // sets click event handlers for them
    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        authWrapper = root.Q<VisualElement>("auth-wrapper");
        subtitle = root.Q<Label>("subtitle");
        startButton = root.Q<Button>("start-button");
        logoutButton = root.Q<Button>("logout-button");
        userInput = root.Q<TextField>("username-input");
        logoutButton.clicked += RealmController.LogOut;
        startButton.clicked += () =>
        {
            OnPressLogin();
        };

    }
    #endregion
}

