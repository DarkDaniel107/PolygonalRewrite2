using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraManager : NetworkBehaviour
{
    public GameObject Player = null;
    public gamemanager gm;
    public float mouseSensitivity;
    public float xrotat = 1f;
    public float yrotat = 1f;
    public float maxY = 80;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!gm.Gettable) return;
        if (Player == null) {
            Player = NetworkClient.connection.identity.gameObject;
        }
        if (!gm.Started) return;
        transform.position = Player.transform.position;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // looker.LookRotation(NetworkClient.connection.identity.gameObject.transform, transform);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            xrotat -= -mouseX;
            if (transform.rotation.y < maxY && transform.rotation.y > -maxY)
            {
                yrotat -= mouseY;
            }
            transform.localRotation = Quaternion.Euler(yrotat, xrotat, 0f);
            /*
            
            
            // xrotat = Mathf.Clamp(xrotat, -90f, 90f);

            transform.localRotation = Quaternion.Euler(yrotat, xrotat, 0f);
            // transform.Rotate(Vector3.up * mouseX);
            */
        }
    }
}
