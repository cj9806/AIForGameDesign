using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSystem : MonoBehaviour
{
    public int boidCount = 100;
    public float boidSpawnRadius = 20;
    public float neighborhoodRadius = 5;
    public GameObject[] boidPrefabs;
    public float mass = 1.0f;
    public float maxForce = 5.0f;
    public float maxSpeed = 10.0f;

    public Transform boidTarget;
    //array of references to the  transform components of each boid
    private Transform[] boidTransform;
    //array of vector3's that will be used to overide position after each update
    private Vector3[] boidPosition;
    //array of vec3s  that will be used to calculate the new position of each boid after update

    private Vector3[] boidVelocity;

    private int[] GetFlockmates(int currentBoid)
    {
        List<int> nearbyIndeces = new List<int>();
        for(int i = 0; i < boidCount; i++)
        {
            if (i == currentBoid) { continue; }

            if ((boidPosition[i] - boidPosition[currentBoid]).magnitude < neighborhoodRadius)
            {
                nearbyIndeces.Add(i);
            }
        }

        return nearbyIndeces.ToArray();

    }
    void Start()
    {
        boidTransform = new Transform[boidCount];
        boidPosition = new Vector3[boidCount];
        boidVelocity = new Vector3[boidCount];

        for(int i = 0; i < boidCount; i++)
        {
            GameObject prefab = boidPrefabs[Random.Range(0, boidPrefabs.Length)];
            GameObject newBoid = Instantiate(prefab,
                                                transform.position + Random.insideUnitSphere * boidSpawnRadius,
                                                transform.rotation);
            boidTransform[i] = newBoid.transform;
            boidTransform[i].localScale = prefab.transform.localScale;
            boidPosition[i] = boidTransform[i].position;
            boidVelocity[i] = boidTransform[i].forward;
        }
    }

    void Update()
    {
        for(int i = 0; i < boidCount; i++)
        {
            Vector3 sumForces = Vector3.zero;
            int[] flockmateindeces = GetFlockmates(i);
            //calculate and add forces together
            if (flockmateindeces.Length > 0)
            {

                //seperation
                Vector3 seperationForce = Vector3.zero;
                for (int j = 0; j < flockmateindeces.Length; j++)
                {
                    Vector3 fleePosition = boidPosition[flockmateindeces[j]];
                    Vector3 currPosition = boidPosition[i];

                    seperationForce += currPosition - fleePosition;
                }
                sumForces += seperationForce;

                //alignment
                Vector3 sumOfNeighboringVelocities = Vector3.zero;
                for (int j = 0; j < flockmateindeces.Length; j++)
                {
                    sumOfNeighboringVelocities += boidVelocity[flockmateindeces[j]];
                }
                sumOfNeighboringVelocities /= flockmateindeces.Length;
                sumForces += (sumOfNeighboringVelocities - boidPosition[i]);

                //coheason
                Vector3 sumofneighboringpositions = Vector3.zero;
                for (int j = 0; j < flockmateindeces.Length; j++)
                {
                    sumofneighboringpositions += boidPosition[flockmateindeces[j]];
                }
                sumofneighboringpositions /= flockmateindeces.Length;
                sumForces += sumofneighboringpositions - boidPosition[i];


            }
            //integrate forces
            //move towards targt at all times
            sumForces += (boidTarget.position - boidPosition[i]).normalized * maxSpeed;
            //consider mass - 'heavier' objects will take more force to move
            sumForces = Vector3.ClampMagnitude(sumForces, maxForce);
            sumForces /= mass;

            boidVelocity[i] = boidVelocity[i] + sumForces * Time.deltaTime;
            boidVelocity[i] = Vector3.ClampMagnitude(boidVelocity[i], maxSpeed);
            

        }

        //integrate
        for(int i = 0; i < boidCount; i++)
        {
            //euler integration
            boidPosition[i] += boidVelocity[i] * Time.deltaTime;

            //update the boid game objects
            boidTransform[i].position = boidPosition[i];
            boidTransform[i].forward = boidVelocity[i];
        }
    }
}
