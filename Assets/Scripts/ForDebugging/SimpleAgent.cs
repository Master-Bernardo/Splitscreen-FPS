using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour
{

    public EC_Movement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement.SetUpComponent(null);
    }

    // Update is called once per frame
    void Update()
    {
        movement.UpdateComponent();
        if (Input.GetKeyDown(KeyCode.M))
        {
            movement.MoveTo(transform.position + transform.forward * 5);
        }
    }
}
