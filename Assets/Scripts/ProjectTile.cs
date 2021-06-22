using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemyController = other.collider.GetComponent<EnemyController>();

        if (enemyController)
        {
            enemyController.Fixed();
        }

        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
