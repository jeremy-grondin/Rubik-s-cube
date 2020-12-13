using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField]
    float speed = 0;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(1))
        //    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed, Space.World);
        

        if (Input.GetMouseButton(1))
            transform.rotation = Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * speed) 
                                 * transform.rotation ;
        
    }

}
