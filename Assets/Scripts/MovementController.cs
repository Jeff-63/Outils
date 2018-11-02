using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 0;                     //This must be public or SerializeField
    public Vector3 velocity = new Vector3();

    int currentWaypointIndex;


    public void Update()
    {
        /*if (waypoints.Length > 0)
        { 
            if (!waypoints[currentWaypointIndex] ||  Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < .1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                velocity = new Vector3();
            }
            if (waypoints[currentWaypointIndex])
            {
                //transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
                transform.position = Vector3.SmoothDamp(transform.position, waypoints[currentWaypointIndex].position, ref velocity, .7f);
                transform.rotation = Quaternion.LookRotation(waypoints[currentWaypointIndex].position - transform.position);
            }
        }*/

        transform.MoveForward(speed, Time.deltaTime);
    }
	
}
