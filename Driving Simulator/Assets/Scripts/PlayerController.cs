using UnityEngine;
using FishNet.Object;

public class NewMonoBehaviourScript : NetworkBehaviour
{
    private float speed = 10.0f;
    private float turnSpeed = 25.0f;
    private float horizontalInput;
    private float forwardInput;
    private float drift = 1.0f;

    private enum Scheme { WASD, ARROWS }
    private Scheme scheme;


    public override void OnStartClient()
    {
        
        base.OnStartClient();
        // Assign unique tags based on owner ID
        if (IsOwner)
        {
            // Find the one and only MainCamera
            GameObject mainCam = GameObject.FindWithTag("MainCamera");
            // For this client, tell that MainCamera to point to and follow this client's player.
            // (All the other clients will have Main Camera point to their players)
            mainCam.GetComponent<FollowPlayer>().player = this;
        }
    }
    void Awake()
    {
        scheme = Scheme.WASD; // Default scheme

    }

    void Update()
    {
        if (!base.IsOwner) return;
        // Read inputs directly from keys (no Input Manager axes needed)
        if (scheme == Scheme.WASD)
        {
            forwardInput = Input.GetAxis("Vertical"); // W/S 
            horizontalInput = Input.GetAxis("Horizontal"); // A/D 
        }


        // Drift logic: handle both schemes
        bool forwardDown = Input.GetKey(KeyCode.W);
        bool backDown = Input.GetKey(KeyCode.S);

        if (forwardDown && backDown)
        {
            drift += 0.01f;
            speed = 0.0f;
        }
        else
        {
            drift = drift > 1.0f ? drift - 0.1f : 1.0f;
            speed = 10.0f;
        }

        // Move & turn (local space)
        bool isBackingUp = false;

        // Example check — use the same tag logic or a shared flag from your camera script
        if (!forwardDown)
            isBackingUp = Input.GetKey(KeyCode.S); // player 1 backs up when pressing S

        float direction = isBackingUp ? -1f : 1f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput * drift);
        if (forwardInput != 0) // only turn when moving forward or backward
            transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput  * direction);
    }
}
