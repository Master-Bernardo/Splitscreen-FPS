using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSTestScript : MonoBehaviour
{
    Vector2 mousePosition;
    Vector2 previousMousePosition;
    Vector2 deltaMouse;

    public float xSensitivity = 0.3f;
    public float ySensitivity = 0.3f;

    public Transform cam;

    public void OnLookAroundFP(InputValue value)
    {
        deltaMouse = value.Get<Vector2>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(deltaMouse);

        //float mouseX = Input.GetAxis("MouseX");
        //float mouseY = Input.GetAxis("MouseY");

       

        //body rotation: 
        transform.forward = Quaternion.AngleAxis(deltaMouse.x * xSensitivity, transform.up) * transform.forward;
        cam.forward = Quaternion.AngleAxis(-deltaMouse.y * ySensitivity, cam.right) * cam.forward;




        /*Vector2 currentMousePosition = Mouse.current.position.ReadValue();
        Debug.Log(currentMousePosition);
        deltaMouse = mousePosition - previousMousePosition;
        //Debug.Log("mouseDelta: " + deltaMouse);
        previousMousePosition = mousePosition;

        Vector3 currentLookVector = Quaternion.AngleAxis(deltaMouse.x, transform.up) * transform.forward;// Quaternion.Euler(-deltaMouse.y, -deltaMouse.x, 0) * transform.forward;
        currentLookVector = Quaternion.AngleAxis(deltaMouse.y, transform.right) * currentLookVector;


        Vector3 _rotation = new Vector3(deltaMouse.y, deltaMouse.x, 0f) * 1;
        transform.forward = currentLookVector;
        //Debug.Log("dorward: " + transform.forward);
        //Debug.Log("currentLookVector: " + currentLookVector);*/
    }
}
