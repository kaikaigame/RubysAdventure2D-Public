﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemHealth : MonoBehaviour
{
    public AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller)
        {
            controller.ChangeHealth(1);
            Destroy(gameObject);

            controller.PlaySound(collectedClip);
        }
    }
}
