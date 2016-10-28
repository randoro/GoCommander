using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    public float turnSpeed = 0.2f;
    public Transform player;

    private bool bDragging;
    private Vector3 oldPos;
    private Vector3 panOrigin;

    private Vector3 offset;
    private GameObject map;
    private GoogleMap gMap;

    void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + 15.0f, player.position.z + 10.0f);
        transform.position = player.position + offset;
        transform.LookAt(player.position);

        map = GameObject.FindGameObjectWithTag("Map");
        gMap = map.GetComponent<GoogleMap>();
    }

    void LateUpdate()
    {
        if (Input.touchCount > 0 && Input.touchCount < 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            offset = Quaternion.AngleAxis(touchDeltaPosition.x * turnSpeed, Vector3.up) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }

        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            Camera.main.transform.Translate(new Vector3(0, 0, -(deltaMagnitudeDiff * 0.05f)));
            offset = transform.position - player.position;
            Vector3 cameraRelative = transform.InverseTransformVector(transform.localPosition);
            cameraRelative = new Vector3(cameraRelative.x, cameraRelative.y, Mathf.Clamp(cameraRelative.z, -20.0f, -10.0f));
            
            transform.localPosition = transform.TransformVector(cameraRelative);
            
        }
        

        //For debugging purposes
#if UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }

#endif
    }
}
