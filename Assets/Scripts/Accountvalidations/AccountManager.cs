using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{ // -- FK -- 2023.02.10
    private List<Account> accounts;
    private Account currentlyRegisteredAccount;

    void Start(){
        accounts = new List<Account>();
    }

    public Account SetAccount(Account account, int accountId, string accountUsername, string accountPassword){
        account.id = accountId;
        account.accountUsername = accountUsername;
        account.accountPassword = accountPassword;
        return account;
        
    }

    public void AddAccount(Account account){
        if(!IsCurrentAccountAlreadyExists(account)){
            accounts.Add(account);
        }
        else{
            Debug.Log("Account already exists so it wasn't added to the Accounts list !");
        }
        
    }

    public void RemoveAccount(Account account){
        accounts.Remove(account);
    }

    public Account GetAccount(string username){
        foreach(Account account in accounts){
            if(account.accountUsername == username){
                SetCurrentAccount(account);
                return account;
            }
        }

        LogAccountsCount();

        return null;
    }

    public void SetCurrentAccount(Account account){
        currentlyRegisteredAccount = account;
    }

    public void LogAccountsCount(){
        Debug.Log("Accounts count: " + accounts.Count);
    }

    public bool IsCurrentAccountAlreadyExists(Account account){
        foreach(Account acc in accounts){
            if(acc.accountUsername == account.accountUsername){
                return true;
            }
        }
        return false;
    }


}
