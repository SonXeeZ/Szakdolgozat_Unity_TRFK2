using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class EditPlayerName : MonoBehaviour {


    public static EditPlayerName Instance { get; private set; }


    public event EventHandler OnNameChanged;


    [SerializeField] private TextMeshProUGUI playerNameText;


    //private string playerName = "Code Monkey";

    public static string email;
    public static string playerName;

    private void Awake() {
        Instance = this;

    }

    private void Start() {
        //OnNameChanged += EditPlayerName_OnNameChanged;
        if (FirebaseAuth.DefaultInstance.CurrentUser != null) {
            playerName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
            playerNameText.text = playerName;
        }
    }

    private void EditPlayerName_OnNameChanged(object sender, EventArgs e) {
        LobbyManager.Instance.UpdatePlayerName(GetPlayerNameFromFirebase());
    }

    public string GetPlayerName() {
        return email;
    }

    public string GetPlayerNameFromFirebase(){
        return FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
    }

}