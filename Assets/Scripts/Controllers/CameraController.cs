using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{ // -- FK -- 
    [SerializeField] public Transform _target; // target for the camera to Look At.

    [SerializeField] private Camera cam;

    [SerializeField] private Vector3 offset = new Vector3(); // Camera distance compared to the player
    [SerializeField] private float pitch = 2f; // Camera distance from player feet upwards
    
    // Updated 2023.02.12 1:30 -- FK --

    private void Start(){

        if(!IsOwner){ // -- FK -- 2023.02.12 1:30
            cam.enabled = false;
            Debug.Log("Camera of the Player succesfully disabled for other players.");
        }
        
        if(IsOwner){
            SetCameraToLocalTarget();
        }

        
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        transform.position = _target.position - offset;
        transform.LookAt(_target.position + Vector3.up *pitch);
    }

    // https://forum.unity.com/threads/getting-clients-to-find-host-on-other-computer-next-steps-for-netcode-and-connecting.1232337/
    // Innen vettem az If szerkezetet. OwnerClientId == NetworkManager.LocalCLientId
    // Először ellenőriznunk kell hogy a kliensünk tulajdonosa-e a karakternek, ha igen akkor a kamera a karakterre nézzen.
    public void SetCameraToLocalTarget(){
        if(OwnerClientId == NetworkManager.LocalClientId){
            _target = NetworkManager.LocalClient.PlayerObject.transform;
            cam = NetworkManager.LocalClient.PlayerObject.GetComponentInChildren<Camera>();
            Debug.Log("Camera set to: " + NetworkManager.LocalClient.ClientId); // -- FK -- 2023.02.12 1:30");
        }
    }
 
}
