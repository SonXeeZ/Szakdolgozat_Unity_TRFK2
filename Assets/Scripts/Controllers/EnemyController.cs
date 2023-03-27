using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{
    public float lookRadius = 10f;
    public NetworkVariable<bool> isPlayerEnteredCollider = new NetworkVariable<bool>();
    Transform target;
    NavMeshAgent agent;

    void Update()
    {
        if(isPlayerEnteredCollider.Value){
            FollowPlayer();
        }
        
    }

    public override void OnNetworkSpawn()
    {
        agent = GetComponent<NavMeshAgent>();
        isPlayerEnteredCollider.Value = false;
        isPlayerEnteredCollider.OnValueChanged += PlayerEnteredColliderValueChanged;
    }

    private void PlayerEnteredColliderValueChanged(bool previousValue, bool newValue)
    {
        //Debug.Log("isPlayerEnteredCollider value changed from " + previousValue + " to " + newValue);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void FollowPlayer()
    {
        EnemyEnteredGizmoServerRPC(target.gameObject.GetComponent<NetworkObject>().OwnerClientId);  
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnemyEnteredGizmoServerRPC(ulong enemyPlayerId)
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            EnemySetMessageClientRpc(enemyPlayerId);
            SetEnemyDestinationClientRpc(target.position, enemyPlayerId);
        }
    }

    [ClientRpc]
    private void EnemySetMessageClientRpc(ulong enemyPlayerId)
    {
        Debug.Log("Enemy player set to id: " + enemyPlayerId);
    }

    [ClientRpc]
    private void SetEnemyDestinationClientRpc(Vector3 destination, ulong clientId)
    {
        Debug.Log("(SetEnemyDestinationClientRpc)Enemy player set to id: " + clientId);
        agent.SetDestination(destination);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetEnemyTransformServerRpc(ulong enemyPlayerId)
    {
        var enemyTransform = NetworkManager.Singleton.ConnectedClients[enemyPlayerId].PlayerObject.transform;
        target = enemyTransform;
        Debug.Log("SetEnemyTransformServerRpc() Enemy set to enemy player id: " + enemyPlayerId);
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player")){ // -- FK -- 2023.02.23 23:30
            if(target == null){
                isPlayerEnteredCollider.Value = true;
                SetEnemyTransformServerRpc(other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
                Debug.Log("Player entered gizmo id: " + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){ // -- FK -- 2023.02.23 23:30    
            target = null;
            isPlayerEnteredCollider.Value = false;
            Debug.Log("Player exited gizmo id: " + other.gameObject.GetComponent<NetworkObject>().OwnerClientId);
            
        }
    }
}