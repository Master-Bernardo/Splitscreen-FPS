using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRTS : MonoBehaviour
{
    public float movementSpeed;

    public void UpdateMovement(Vector3 movementVector)
    {
        transform.position += movementVector * movementSpeed;
    }

}

