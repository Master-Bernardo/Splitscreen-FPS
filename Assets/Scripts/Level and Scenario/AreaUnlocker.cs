using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUnlocker : MonoBehaviour
{
    public GameObject[] objectsToUnlock;
    public GameObject[] objectsToHide;
    
    public void Unlock()
    {
        for (int i = 0; i < objectsToUnlock.Length; i++)
        {
            objectsToUnlock[i].SetActive(true);
        }

        for (int i = 0; i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(false);
        }

        Destroy(gameObject);
    }
}
