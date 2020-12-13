using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    float zoomSpeed;


    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3 (0, 0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed);
    }
}
