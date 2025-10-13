using FishNet.Object;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public NetworkBehaviour player; // drag your car root transform here

    public Vector3 thirdPersonOffset = new Vector3(0f, 5f, -7f);
    public Vector3 hoodOffset = new Vector3(0f, 1.8f, 0.3f);
    public Vector3 backupOffset = new Vector3(0f, 3.5f, 7f); // in front of the car because we'll look backward

    [Range(0.01f, 0.5f)] public float positionSmoothTime = 0.12f;
    [Range(1f, 20f)] public float rotationLerpSpeed = 12f;

    public bool backupIsToggle = false; // false = hold key to back-up view, true = toggle

    private Vector3 _currentOffset = new Vector3(0f,5f,-7f);
    private Vector3 _vel;
    private int _viewIndex = 0;         // 0 = third-person, 1 = hood
    private bool _backupToggledOn;  // used only if backupIsToggle = true
    private string camaeraTag;

  

    void LateUpdate()
    {
        bool useBackup = false;
        if (!player) return;

        // Toggle main view (V)
        if (Input.GetKeyDown(KeyCode.V))
        {
            _viewIndex = 1 - _viewIndex;
            _currentOffset = (_viewIndex == 0) ? thirdPersonOffset : hoodOffset;
        }

        // Backup view handling
        bool backupHeld2 = (Input.GetKey(KeyCode.S));

        bool foward_down =  Input.GetKey(KeyCode.W);
        if (foward_down)
        {
            backupHeld2 = false;
        }
        useBackup = backupIsToggle ? _backupToggledOn : backupHeld2;



            // Choose offset and orientation
        Vector3 targetOffset = useBackup ? backupOffset : _currentOffset;
        Vector3 desiredPos = player.transform.TransformPoint(targetOffset);

        // Move
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref _vel,0f);

        // Rotate (look opposite when backing up)
        Quaternion desiredRot = useBackup
            ? Quaternion.LookRotation(-player.transform.forward, player.transform.up)
            : Quaternion.LookRotation(player.transform.forward, player.transform.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationLerpSpeed*Time.deltaTime);
    }
}
