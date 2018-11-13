using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Transform objectToFollow;

    private void Update()
    {
        if (objectToFollow.transform.position.x > transform.position.x)
        {
            transform.position = new Vector3(objectToFollow.transform.position.x,transform.position.y,transform.position.z);
        }
    }
}
