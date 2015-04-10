using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {

    public Transform cameraTransform, targetTransform;
    public float movementspeed = 10.0f;

    void Update()
    {
        cameraTransform.Translate(targetTransform.position.normalized * movementspeed * Time.deltaTime, Space.World);

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) // zoom away from target
            cameraTransform.position = new Vector3(cameraTransform.position.x - 1f, cameraTransform.position.y + 1f, cameraTransform.position.z);
        else if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) // zoom to target
            cameraTransform.position = new Vector3(cameraTransform.position.x + 1f, cameraTransform.position.y - 1f, cameraTransform.position.z);
    }
}
