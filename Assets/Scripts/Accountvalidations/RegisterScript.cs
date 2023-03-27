using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RegisterScript : MonoBehaviour
{ // -- FK -- 2023.02.08 - 02.10
    public TMP_InputField m_registeredUsername;
    public TMP_InputField m_registeredPassword;

    [SerializeField] public AccountManager accountManager;



    public void Start(){
        m_registeredUsername.text = "";
        m_registeredPassword.text = "";
    }

    public void RegisterAccount(){
        string username = m_registeredUsername.text;
        string password = m_registeredPassword.text;
        Account account = new Account(0, username, password);

        if(username != null && password != null && username != "" && password != ""){
            
            if(!accountManager.IsCurrentAccountAlreadyExists(account)){
                Debug.Log("Sikeres regisztráció!");
                accountManager.AddAccount(account);
            } else{
                Debug.Log("A felhasználónév már foglalt!");
            }
            
        }else{
            Debug.Log("A regisztrációhoz töltsd ki minden adatot!");
        }
        //TODO: Check if user already exists in database

        // TODO: Add user to database

        
    }
}
