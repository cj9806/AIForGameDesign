using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISteeringController : MonoBehaviour
{
    public Agent agent;

    [Header("Steering Settings")]
    public float maxSpeed = 3.0f;
    public float maxForce = 5.0f;

    protected List<SteeringBehaivor> steerings = new List<SteeringBehaivor>();

    protected void CalculateSteeringForce()
    {
        Vector3 steeringForce = Vector3.zero;
        for (int i = 0; i<steerings.Count; i++)
        {
            steeringForce += steerings[i].Steer(this);
        }
    }
}

public class SteeringBehaivor
{
    public virtual Vector3 Steer(AISteeringController controller)
    {
        return Vector3.zero;
    }
}

public class SeekSteering : SteeringBehaivor
{
    public Transform target;
    public override Vector3 Steer(AISteeringController controller)
    {
        return (target.position - controller.transform.position).normalized * controller.maxSpeed;
    }
    //I wonder if when you make a pointer to a void return type if you can use it a a varible that can parse as anything
}