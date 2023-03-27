using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void DealDamage(int damage);
    void DealDamageServerRpc(int damage);
    void Die();
}
