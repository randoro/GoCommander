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

            // If the camera is orthographic...
            //if (Camera.main.isOrthoGraphic)
            //{
            //    // ... change the orthographic size based on the change in distance between the touches.
            //    camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            //    // Make sure the orthographic size never drops below zero.
            //    camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            //}
            //else
            //{
            // Otherwise change the field of view based on the change in distance between the touches.

            //deltaMagnitudeDiff * 0.1f);
            //Camera.main.fieldOfView += deltaMagnitudeDiff * 0.1f;
            Camera.main.transform.Translate(new Vector3(0, 0, -(deltaMagnitudeDiff * 0.05f)));
            offset = transform.position - player.position;
            Vector3 cameraRelative = transform.InverseTransformVector(transform.localPosition);
            cameraRelative = new Vector3(cameraRelative.x, cameraRelative.y, Mathf.Clamp(cameraRelative.z, -20.0f, -10.0f));

            


            //float per = cameraRelative.z + 20.0f;
            //int thirds = (int)per/3;
            //switch (thirds)
            //{
            //    case 0:
            //        gMap.zoom = 13;
            //        break;
            //    case 1:
            //        gMap.zoom = 14;
            //        break;
            //    case 2:
            //        gMap.zoom = 15;
            //        break;
            //    default:
            //        break;
            //}

            transform.localPosition = transform.TransformVector(cameraRelative);
            // Clamp the field of view to make sure it's between 0 and 180.
            //Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Mathf.Clamp(Camera.main.transform.localPosition.z, -20.0f, 20.0f));
            //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Clamp(Camera.main.transform.position.z, -20.0f, 20.0f));
            //}
        }


        //Vector3 cameraRelative = transform.InverseTransformVector(transform.localPosition);
        //cameraRelative = new Vector3(cameraRelative.x, cameraRelative.y, Mathf.Clamp(cameraRelative.z, -20.0f, 20.0f));
        //cameraRelative = cameraRelative + (Vector3.forward*0.01f);
        //print(cameraRelative);
        //transform.localPosition = transform.TransformVector(cameraRelative);

        //Camera.main.transform.position = Camera.main.transform.TransformPoint(Camera.main.transform.position + (Vector3.forward * 0.1f));
        //new Vector3(Camera.main.transform.position.x,
        // Camera.main.transform.position.y, Camera.main.transform.position.z+0.6f);

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
