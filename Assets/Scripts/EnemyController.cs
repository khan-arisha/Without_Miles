using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool isTopDown = false;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float idleDelay = 2.0f;
    [Space]
    [SerializeField] private Animator animator;
    private int patrolIndex = 0;
    private Camera cam;

    private Rigidbody2D rb;

    private float idleTimer = 0.0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }
    
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        if(patrolPoints.Length <= 0) return;

        int xMove = 0, yMove = 0;
        Vector2 vel = rb.velocity;
        
        if (Vector2.Distance(patrolPoints[patrolIndex].position, transform.position) > 2.0f && idleTimer <= 0.0f)
        {
            xMove = patrolPoints[patrolIndex].position.x > transform.position.x ? 1 : -1;
            if(isTopDown) yMove = patrolPoints[patrolIndex].position.y > transform.position.y ? 1 : -1;
            if(xMove != 0) transform.GetChild(0).localScale = new Vector2(-xMove*0.5f, 0.5f);
            if(animator != null) animator.SetBool("isWalking", true);
        }
        else if(idleTimer > 0.0f)
        {
            idleTimer -= Time.deltaTime;
        }
        else
        {
            idleTimer = idleDelay;
            patrolIndex++;
            if (patrolIndex >= patrolPoints.Length) patrolIndex = 0;
            if(animator != null) animator.SetBool("isWalking", false);
        }

        if(xMove < 0 && vel.x > 0) vel.x = 0.0f;
        else if(xMove > 0 && vel.x < 0) vel.x = 0.0f;
        
        vel.x += xMove * acceleration * Time.fixedDeltaTime;
        vel.y += yMove * acceleration * Time.fixedDeltaTime;

        if (vel.x > speed) vel.x = speed;
        else if (vel.x < -speed) vel.x = -speed;
        else if (xMove == 0) vel.x /= 1.2f;

        if (isTopDown)
        {
            if (vel.y > speed) vel.y = speed;
            else if (vel.y < -speed) vel.y = -speed;
            else if (yMove == 0) vel.y /= 1.2f;
        }

        rb.velocity = vel;
    }
}
