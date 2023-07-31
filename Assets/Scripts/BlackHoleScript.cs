using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballRB;
    public float gravity;
    private float gravityRadius;
    private Transform ballT;
    private float orbitTimer;
    
    private void Start()
    {
        gravityRadius = GetComponent<CircleCollider2D>().radius;
        ballT = ballRB.transform;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (orbitTimer < 7)
        {
            orbitTimer += Time.deltaTime;
            Vector2 dir = transform.position - ballRB.transform.position;
            float force = gravityRadius - Vector2.Distance(transform.position, ballT.position);
            ballRB.AddForce(force * gravity * Time.deltaTime * dir);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        orbitTimer = 0;
    }

}
