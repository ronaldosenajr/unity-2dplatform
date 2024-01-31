using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    Damagable damageable;

    Transform nextWaypoint;
    int waypointIndex = 0;


    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
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

    void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    void OnDisable()
    {
        damageable.damageableDeath.RemoveListener(OnDeath);
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
                UpdateDirection();
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

        //check if we are close enough to the waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = direction * flightSpeed;

        // see if we need to switch waypoint
        if (distance <= waypointReachedDistance)
        {
            waypointIndex += 1;

            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }

            nextWaypoint = waypoints[waypointIndex];
        }

    }

    private void UpdateDirection()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDeath()
    {
        Debug.Log("FlyingEye: OnDeath");
        deathCollider.enabled = true;
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
