using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{ // -- FK -- 2023.02.08 - 02.10
    public TMP_InputField m_username;
    public TMP_InputField m_password;

    [SerializeField] public AccountManager accountManager;

    public void Start(){
        m_username.text = "";
        m_password.text = "";
    }

    public void LoginToAccount(){
        string username = m_username.text;
        string password = m_password.text;
        Account account = accountManager.GetAccount(username);

        


        if(username != null && password != null && username != "" && password != ""){
            if(account != null){
                if(account.accountUsername == username && account.accountPassword == password){
                    Debug.Log("Sikeres belépés!");
                    SceneManager.LoadScene("SampleScene");
                }else{
                    Debug.Log("Hibás felhasználónév vagy jelszó!");
                }
                
            }
            else{
                Debug.Log("Ilyen felhasználó nem létezik---- JAVITANI!");
            }
        }else{
            Debug.Log("A belépéshez töltsd ki az adatokat.");
        }
    } 
    
}
