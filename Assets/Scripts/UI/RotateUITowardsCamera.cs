using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUITowardsCamera : MonoBehaviour
{
    public int playerID;
    Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main.transform;
        //transform.forward = cam.forward;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController controller = HordeModeManager.Instance.playerControllers[playerID];
        transform.forward =  transform.position - controller.GetActiveCamera().transform.position;
        //Debug.Log("rotatating to: " + controller.GetActiveCamera().gameObject);
    }
}
