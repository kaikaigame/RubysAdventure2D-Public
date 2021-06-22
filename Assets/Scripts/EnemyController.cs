using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float timeToChangeDirection = 2f;
    public bool horizontal;

    private float remainingTime;

    private Vector2 direction = Vector2.left;
    private Rigidbody2D rb;
    private Animator anim;

    private static readonly int lookX = Animator.StringToHash("lookX");
    private static readonly int lookY = Animator.StringToHash("lookY");

    private bool repaired;
    public GameObject smokeEffect;
    public ParticleSystem fixedEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        remainingTime = timeToChangeDirection;
        direction = horizontal ? Vector2.left : Vector2.down;
    }

    void Update()
    {
        if (repaired)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = timeToChangeDirection;
            direction *= -1;
        }

        anim.SetFloat(lookX, direction.x);
        anim.SetFloat(lookY, direction.y);

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (repaired)
        {
            return;
        }

        PlayerController controller = other.collider.GetComponent<PlayerController>();

        if (controller)
        {
            controller.ChangeHealth(-1);
        }
    }

    public void Fixed()
    {
        anim.SetTrigger("fixed");

        smokeEffect.SetActive(false);

        //产生爆炸效果
        Instantiate(fixedEffect, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        rb.simulated = false;

        repaired = true;
    }
}
