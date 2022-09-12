using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //public Slider CamSensitivity;
    public float mouseSensitivity;


    //new
    [HideInInspector]
    public LayerMask collisionLayer;
    [HideInInspector]
    public Vector3[] adjustedCameraClipPoints;
    [HideInInspector]
    public Vector3[] adesiredCameraClipPoints;
   




    private Transform parent;
    float xRotation = 0f;
    private void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;

       
    }

    private void Update()
    {
        //mouseSensitivity = CamSensitivity.value;
        
        Rotate();

       
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        parent.Rotate(Vector3.up, mouseX);  
    }

    

}
