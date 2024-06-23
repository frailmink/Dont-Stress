using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // The player
    public Vector3 offset; // Offset from the target
    public float scrollSpeed = 5f; // Speed of scrolling
    public float zoomSpeed = 2f; // Speed of zooming
    public float minZoom = 2f; // Minimum zoom level
    public float maxZoom = 50f; // Maximum zoom level

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target with offset
            transform.position = target.position + offset;

            // Scroll input
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                // Zoom based on scroll input
                float zoomAmount = cam.orthographicSize - scrollInput * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(zoomAmount, minZoom, maxZoom);
            }
        }
    }
}
