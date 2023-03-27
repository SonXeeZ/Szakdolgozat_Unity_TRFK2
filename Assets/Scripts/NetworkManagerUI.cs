using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{ // -- TR -- 
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    [SerializeField] private TextMeshProUGUI playersIngameText; // -- FK -- 2023.02.11

    private void Awake()
    {
        Cursor.visible = true; // -- FK -- 2023.02.11

        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
    // https://www.youtube.com/watch?v=rFCFMkzFaog&list=PLQMQNmwN3FvyyeI1-bDcBPmZiSaDMbFTi&index=2 -- FK -- 2023.02.11

    private void Update() // PlayersManager Update Kapcsolódik hozzá. -- FK -- 2023.02.11 https://www.youtube.com/watch?v=HWPKlpeZUjM&t=895s
    {
        playersIngameText.text = $"Players in game: {PlayersManager.Instance.GetPlayersInGame().ToString()}";
    }

}
