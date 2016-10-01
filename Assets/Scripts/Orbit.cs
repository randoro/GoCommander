using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    public float turnSpeed = 4.0f;
    public Transform player;

    private bool bDragging;
    private Vector3 oldPos;
    private Vector3 panOrigin;

    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(player.position.x, player.position.y + 15.0f, player.position.z + 10.0f);
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }

    void LateUpdate()
    {
        

        if (Input.GetMouseButton(0))
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
    }
}
