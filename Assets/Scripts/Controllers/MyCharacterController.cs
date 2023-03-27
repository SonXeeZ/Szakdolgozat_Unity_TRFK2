
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System;
using Unity.Netcode;
using Unity.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class MyCharacterController : NetworkBehaviour
{ // -- FK -- 
    [SerializeField] private InputAction movement = new InputAction();
    [SerializeField] private LayerMask layerMask = new LayerMask();
    private NavMeshAgent agent = null;
    [SerializeField] private Camera cam = null;



    private void Start()
    {
        // https://forum.unity.com/threads/getting-clients-to-find-host-on-other-computer-next-steps-for-netcode-and-connecting.1232337/
        

        if(IsLocalPlayer){
            NetworkManager.OnClientConnectedCallback += (id) =>
        {
            cam = NetworkManager.LocalClient.PlayerObject.GetComponentInChildren<Camera>();

            Debug.Log($"ID: [ {id} ] player connected to the server...");
            Debug.Log("Camera for Input set to: " + NetworkManager.LocalClient.ClientId); // -- FK -- 2023.02.12 1:30
        };
        }
        

        // Properties of the Player character
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 5.5f;
        agent.acceleration = 999.9f;
        agent.angularSpeed = 900.9f;
    }
    private void OnEnable()
    {
        movement.Enable();
        //Debug.Log("Movement's enabled.");
    }
    private void OnDisable()
    {
        movement.Disable();
        //Debug.Log("Movement's disabled.");
    }
    private void Update()
    {
        if (!IsOwner) return;
        
        HandleInput();
    }



    private void HandleInput()
    {
        if (movement.ReadValue<float>() == 1)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            //Debug.Log("Mouse position's read.");
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000,layerMask))
            {
                //Debug.Log("Trying to move player to destination if under MaxDistance.");
                PlayerMove(hit.point);
                //Debug.Log("Moving to destination.");
            }
        }
    }

    private void PlayerMove(Vector3 locationToGoWithPlayer)
    {
        agent.SetDestination(locationToGoWithPlayer);
    }
}
