using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currentRoom;

    public float cameraMovementSpeed;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }

        // camera to move from room center to the next room center
        Vector3 targetPosition = GetCameraTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * cameraMovementSpeed);
    }

    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            // camera stay in the same position
            return Vector3.zero;
        }

        Vector3 targetPosition = currentRoom.GetRoomCentre();
        targetPosition.z = transform.position.z;

        return targetPosition;
    }

    public bool isSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
