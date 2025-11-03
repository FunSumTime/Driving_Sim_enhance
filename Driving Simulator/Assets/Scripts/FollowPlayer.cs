using FishNet.Object;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public NetworkBehaviour player; // will get added when player spawns

    public Vector3 thirdPersonOffset = new Vector3(0f, 5f, -7f);
    public Vector3 hoodOffset = new Vector3(0f, 1.8f, 0.3f); // first person view
    public Vector3 backupOffset = new Vector3(0f, 3.5f, 7f); // in front of the car because we'll look backward

    [Range(0.01f, 0.5f)] public float positionSmoothTime = 0.12f;
    [Range(1f, 20f)] public float rotationLerpSpeed = 12f;

    public bool backupIsToggle = false; // false = hold key to back-up view, true = toggle

    private Vector3 _currentOffset = new Vector3(0f,5f,-7f); // third person
    private Vector3 _vel;
    private int _viewIndex = 0;         // 0 = third-person, 1 = hood
    private bool _backupToggledOn;  // used only if backupIsToggle = true

  

    void LateUpdate()
    {
        // varable to determine backup or not
        bool useBackup = false;
        if (!player) return;

        // Toggle main view (V)
        if (Input.GetKeyDown(KeyCode.V))
        {
            //switches between 0 and 1
            _viewIndex = 1 - _viewIndex;
            _currentOffset = (_viewIndex == 0) ? thirdPersonOffset : hoodOffset;
        }

        // Backup view handling
        bool backupHeld2 = (Input.GetKey(KeyCode.S));

        // checks if w is down if so do not do backup camera
        bool foward_down =  Input.GetKey(KeyCode.W);
        if (foward_down)
        {
            backupHeld2 = false;
        }
        // only if backup held is true will this be true
        useBackup = backupIsToggle ? _backupToggledOn : backupHeld2;



            // Choose offset and orientation
        Vector3 targetOffset = useBackup ? backupOffset : _currentOffset;
        // which vector to use
        Vector3 desiredPos = player.transform.TransformPoint(targetOffset);

        // Move
        // switch the postion of the camera to desired postion but as a transition
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref _vel,0f);

        // Rotate (look opposite when backing up)
        Quaternion desiredRot = useBackup
            ? Quaternion.LookRotation(-player.transform.forward, player.transform.up)
            : Quaternion.LookRotation(player.transform.forward, player.transform.up);
        // slerp is a smooth rotation from current to desired
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotationLerpSpeed*Time.deltaTime);
    }
}
