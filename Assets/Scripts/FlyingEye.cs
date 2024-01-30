using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    Damagable damageable;

    Transform nextWaypoint;
    int waypointIndex = 0;


    public float flightSpeed = 2f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;

    private bool _hasTarget = false;
    public bool CanMove
	{
		get { return animator.GetBool(AnimationStrings.canMove); }
	}
    public bool HasTarget {
		get {return _hasTarget;}
		private set
		{
			_hasTarget = value;
			animator.SetBool(AnimationStrings.hasTarget, value);
		}
	}
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damagable>();
    }

    void Start()
    {
        nextWaypoint = waypoints[waypointIndex];
    }

    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            } else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        // fly to next waypoint
        Vector2 direction = (nextWaypoint.position - transform.position).normalized;
    }
}
