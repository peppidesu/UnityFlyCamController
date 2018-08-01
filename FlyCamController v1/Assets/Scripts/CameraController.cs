using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float rotationSensitivity;
    public float minRotationY;
    public float maxRotationY;
    public float movementSpeed;
    public Vector3 MinBounds;
    public Vector3 MaxBounds;
    public float minZoom;
    public float maxZoom;
    public float movementLerpTime;
    public float rotationLerpTime;

    private float zoomAmount = 0;
    private float scroll;
    private float hAxis = 0;
    private float vAxis;
    RaycastHit hit;

    Vector3 oldZoomVector = new Vector3(0,0,0);
    private void Move()
    {
        Vector3 zoomVector;
        float rotY = transform.rotation.eulerAngles.y;
        
        zoomAmount += 100 * scroll;
        if (zoomAmount >= minZoom && zoomAmount <= maxZoom)
        zoomVector = 100*scroll * transform.forward;
        else if (zoomAmount < minZoom)
        {
            zoomVector = (100 * scroll - (zoomAmount - minZoom)) * transform.forward;
            zoomAmount = minZoom;
        }            
        else
        {
            zoomVector = (100 * scroll - (zoomAmount - maxZoom)) * transform.forward;
            zoomAmount = maxZoom;
        }
        zoomVector = Vector3.Lerp(oldZoomVector, zoomVector, movementLerpTime);
        oldZoomVector = zoomVector;

        float moveX = movementSpeed * (hAxis * Mathf.Cos((Mathf.PI / 180) * rotY) + vAxis * Mathf.Sin((Mathf.PI / 180) * rotY))+zoomVector.x;
        float moveZ = movementSpeed * (vAxis * Mathf.Cos((Mathf.PI / 180) * rotY) - hAxis * Mathf.Sin((Mathf.PI / 180) * rotY))+zoomVector.z;
        float moveY = zoomVector.y;
        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        Vector3 newPos = transform.position + movement;
        transform.position = Vector3.Lerp(transform.position, newPos, movementLerpTime);
    }
    private void Rotate()
    {
        
        
        
        float rotX = Input.GetAxis("Mouse X");
        float rotY = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(1))
        {
            float newYrotation = -rotY * rotationSensitivity + transform.localRotation.eulerAngles.x;
            
            if ( newYrotation < maxRotationY && newYrotation > minRotationY )
                transform.RotateAround(hit.point, transform.right, -rotY * rotationSensitivity);
            transform.RotateAround(hit.point, Vector3.up, rotX * rotationSensitivity);
            
                
            
        }
        else
        {
            Physics.Raycast(transform.position, transform.forward, out hit);
        }
    }
    private void Start()
    {
        Physics.Raycast(transform.position, transform.forward, out hit);
    }
    private void Update()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        scroll = Input.GetAxis("Mouse ScrollWheel");
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }
}
