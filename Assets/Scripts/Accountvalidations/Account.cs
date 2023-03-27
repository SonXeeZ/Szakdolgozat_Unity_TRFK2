using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account
{ // -- FK -- 2023.02.10
    public int id;
    public string accountUsername;
    public string accountPassword;

    public Character character;

    //TODO: Character list
    // public List<Character> characters;

    // TODO:
    // public string accountEmail;

    public Account(int id, string accountUsername, string accountPassword)
    {
        this.id = id;
        this.accountUsername = accountUsername;
        this.accountPassword = accountPassword;
    }

}
