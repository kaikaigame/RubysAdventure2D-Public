using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float displayTimeSpan = 4.0f;
    public GameObject dialogueBox;

    private float displayTimer;

    void Start()
    {
        
    }

    void Update()
    {
        if (displayTimer > 0)
        {
            displayTimer -= Time.deltaTime;

            if (displayTimer < 0)
            {
                dialogueBox.SetActive(false);
            }
        }
    }

    public void DisplayDialogue()
    {
        dialogueBox.SetActive(true);
        displayTimer = displayTimeSpan;//赋值计时
    }
}
