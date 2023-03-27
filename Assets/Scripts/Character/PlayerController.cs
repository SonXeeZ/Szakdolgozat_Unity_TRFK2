using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField] 
    private Camera cam;

    [SerializeField] 
    private InputAction movement = new InputAction();

    [SerializeField] 
    private LayerMask layerMaskWalkable = new LayerMask();

    [SerializeField] 
    private Vector3 movePosition;

    private Vector3 spawnPosition = new Vector3(0.8142774f, 1f, -7.049958f);

    [SerializeField] private float speed; // character

    private float rotationSpeed = 10f;

    [SerializeField] 
    private Rigidbody rb;

    [SerializeField]
    private Character character;

    [SerializeField] 
    private NetworkVariable<Vector3> networkTransformPosition = new NetworkVariable<Vector3>();


    private void Start(){
         
        playerTransform = GetComponent<Transform>();
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        speed = character.Speed.Value;
        transform.position = spawnPosition;
                
        
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer){
            playerTransform = GetComponent<Transform>();
            cam = GetComponentInChildren<Camera>();
            rb = GetComponent<Rigidbody>();
            character = GetComponent<Character>();
            speed = character.Speed.Value;
            transform.position = spawnPosition;
        }
    }

    public void MoveCharacter(){

        if(Input.GetKey(KeyCode.Mouse0)){

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            RaycastHit hit;

            //Debug.Log("Raycast hit.");

            if(Physics.Raycast(ray, out hit, 1000, layerMaskWalkable)){
                
                //Debug.Log("Moving character to: " + hit.point);

                movePosition = hit.point;

                // [1] https://www.youtube.com/watch?v=zZDiC0aOXDY&t=1239s
                Vector3 destination = Vector3.MoveTowards(playerTransform.position, movePosition, character.Speed.Value * Time.deltaTime);
                Vector3 direction = (movePosition - playerTransform.position).normalized;
                
                if(Vector3.Distance(transform.position, hit.point) > 0.2f){
                    rb.velocity = direction * character.Speed.Value;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                
                }
                
            }
        }
        else{
            rb.velocity = Vector3.zero;
        }

    }

    private void Update(){

        if(IsOwner && IsLocalPlayer)
        {
            MoveCharacter();
        }
        
    }
    private void OnEnable(){
        movement.Enable();
    }

    private void OnDisable(){
        movement.Disable();
    }     
    
}


/*

using UnityEngine;
using Unity.Netcode;

public class MyPlayer : NetworkBehaviour
{
    // The speed of the player
    public float speed = 5f;

    // The target position of the player
    private Vector3 targetPosition;

    // The NetworkTransform component
    private NetworkTransform networkTransform;

    void Start()
    {
        // Get the NetworkTransform component
        networkTransform = GetComponent<NetworkTransform>();
    }

    void Update()
    {
        // Check if this is the local player
        if (IsLocalPlayer)
        {
            // Check if the left mouse button is pressed and held
            if (Input.GetMouseButton(0))
            {
                // Get the mouse position in world space
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Set the target position to be on the same plane as the player
                targetPosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);

                // Send a request to move to the server via RPC
                RequestMoveServerRpc(targetPosition);
            }
        }
    }

    // A ServerRpc attribute indicates that this method can only be invoked by a client and will always be executed on the server/host.
    [ServerRpc]
    void RequestMoveServerRpc(Vector3 requestedTargetPosition)
    {
        // Check if this request is valid (for example, not too far from current position)
        if (IsValidRequest(requestedTargetPosition))
        {
            // Grant access for moving and send back a response via RPC
            GrantMoveClientRpc(requestedTargetPosition);
        }
        else
        {
            // Deny access for moving and send back a response via RPC
            DenyMoveClientRpc();
        }
        
    }

    // A ClientRpc attribute indicates that this method can only be invoked by a server/host and will always be executed on all clients.
    [ClientRpc]
    void GrantMoveClientRpc(Vector3 grantedTargetPosition)
    {
        // Check if this is the local player who sent the request
        if (IsLocalPlayer)
        {
            // Move towards the granted target position at a constant speed
            transform.position = Vector3.MoveTowards(transform.position, grantedTargetPosition, speed * Time.deltaTime);

            // Synchronize the position with NetworkTransform
            networkTransform.NetworkWorldPosition = transform.position;
            
            Debug.Log("Access granted for moving.");
            
        }

*/