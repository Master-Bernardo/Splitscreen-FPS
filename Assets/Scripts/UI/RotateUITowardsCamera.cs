using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUITowardsCamera : MonoBehaviour
{

    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        transform.forward = cam.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = cam.forward;
    }
}
