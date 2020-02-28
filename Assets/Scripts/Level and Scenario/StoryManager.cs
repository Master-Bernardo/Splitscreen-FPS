using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public float deparentPlayerDelay;
    public Transform playerTransform;
    public Transform vehicleTransform;
    bool parentIsNull = false;
    public GameObject units;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform.SetParent(vehicleTransform);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!parentIsNull)
        {
            if (Time.timeSinceLevelLoad > deparentPlayerDelay)
            {

                playerTransform.SetParent(null);
                parentIsNull = true;
            }
           
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            units.SetActive(true);
        }
       
    }
}
