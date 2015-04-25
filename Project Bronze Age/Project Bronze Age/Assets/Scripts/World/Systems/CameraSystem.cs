using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{
    public Transform cameraTransform;

    // The target we are following
    [SerializeField]
    private Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;

    public void SetTarget(LiveActor actortarget)
    {
        if(actortarget == null)
        {
            target = null;
            return;
        }

        target = actortarget.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        var wantedRotationAngle = target.eulerAngles.y;
        var wantedHeight = target.position.y + height;

        var currentRotationAngle = cameraTransform.eulerAngles.y;
        var currentHeight = cameraTransform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        cameraTransform.position = target.position;
        cameraTransform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);

        // Always look at the target
        cameraTransform.LookAt(target);
    }
}

