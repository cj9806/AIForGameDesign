using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //reffers to rigidbody that will be driven
    //assign this in editor
    [SerializeField]
    protected Rigidbody rbody;

    //defines the change in players position over time
    public virtual Vector3 velocity { get; set; }

    //itegrate movement and update player position
    public virtual void UpdateMovement()
    {
        //move rigidbodt to new postition
        //
        //the new position is equal to newPos = pos+vel*dT
        rbody.MovePosition(rbody.position + velocity * Time.deltaTime);
    }
}
