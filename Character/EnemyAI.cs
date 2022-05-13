using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform enemyGraphics;
    public CharacterController2D controller;
    public bool typeFlying = false;
    public float flyingSpeed = 200;

    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            return;
        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        if(typeFlying)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * flyingSpeed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            
            if(distance < nextWaypointDistance)
                currentWaypoint++;

            if(force.x >= 0.01)
            {
                enemyGraphics.localScale = new Vector3(1f, 1f, 1f);
            }
            else if(force.x <= -0.01)
            {
                enemyGraphics.localScale = new Vector3(-1f, 1f, 1f);
            }
        } else {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            controller.Move(direction.x * Time.fixedDeltaTime, false, false);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            
            if(distance < nextWaypointDistance)
                currentWaypoint++;
        }
        
    }
}