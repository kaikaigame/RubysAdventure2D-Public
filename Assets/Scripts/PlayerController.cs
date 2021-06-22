using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 5;
    public GameObject ProjectTilePrefab;
   
    private int currentHealth;
    public int health => currentHealth;

    public float invincibleTotalFrozenTime = 2f;
    private float invincibleTimer;
    private bool isInvincible = false;

    public Transform respawnPosition;

    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 lookDirection = Vector2.down;
    private Vector2 currentInput;

    private float xPos;
    private float yPos;

    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        xPos = Input.GetAxis("Horizontal");
        yPos = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(xPos, yPos);

        if (!Mathf.Approximately(movement.x,0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            lookDirection.Set(movement.x, movement.y);
            lookDirection.Normalize();
        }

        anim.SetFloat("lookX", lookDirection.x);
        anim.SetFloat("lookY", lookDirection.y);
        anim.SetFloat("speed", movement.magnitude);

        currentInput = movement;

        if (Input.GetKeyDown(KeyCode.C))
        {
            LaunnchProjectTile();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, 
                lookDirection, 2f, LayerMask.GetMask("NPC"));

            if (hit)
            {
                NPCController npcController = hit.collider.GetComponent<NPCController>();

                if (npcController)
                {
                    npcController.DisplayDialogue();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position += currentInput * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            invincibleTimer = invincibleTotalFrozenTime;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        print("Current health:" + currentHealth);

        if (currentHealth == 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        rb.position = respawnPosition.position;
    }

    private void LaunnchProjectTile()
    {
        GameObject projectTileGameObject = null;

        if (lookDirection == Vector2.down)
        {
            projectTileGameObject = Instantiate(ProjectTilePrefab, rb.position + Vector2.down, 
                Quaternion.identity);
        }
        else
        {
            projectTileGameObject = Instantiate(ProjectTilePrefab, rb.position + Vector2.up * 0.5f, 
                Quaternion.identity);
        }

        ProjectTile projectTile = projectTileGameObject.GetComponent<ProjectTile>();

        projectTile.Launch(lookDirection, 300);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
