using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Agent agent;
    public float speed = 3.0f;
    public Transform target;
    public Rigidbody rbody;

    //step 1 calculate  distance between target and current location
    

    
    void Update()
    {
        //step 1 calculate  distance between target and current location
        Vector3 distance = new Vector3((target.position.x - rbody.position.x),
                                        0.0f,
                                        (target.position.z - rbody.position.z));

        //step 2 nroamlize the difference
        distance = Vector3.ClampMagnitude(distance, 1);
        agent.velocity = distance * speed;
        agent.UpdateMovement();
    }

    
    
}
