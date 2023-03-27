using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayersManager : Singleton<PlayersManager> // -- FK -- 2023.02.11
{ // -- FK -- 2023.02.11 [22:26]
    private NetworkVariable<int> _playersInGame = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone); // int változó ami tárolja a játékosok számát

    public int GetPlayersInGame()
    {
        return _playersInGame.Value;
    }
    
    private void Start(){
        // https://www.youtube.com/watch?v=rFCFMkzFaog&list=PLQMQNmwN3FvyyeI1-bDcBPmZiSaDMbFTi&index=2 -- FK -- 2023.02.11.-> Lambdás kifejezést innen vettem. + Singleton
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Debug.Log($"ID: [ {id} ] player connected to the server...");
        };
        
    }

    void Update(){ // https://www.youtube.com/watch?v=HWPKlpeZUjM&t=895s

        if(!IsServer) return;
        _playersInGame.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
    }
}
