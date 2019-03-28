using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;

    DialogueManager dm;

    private void Start()
    {
        dm = DialogueManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TriggerDialogue();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CloseDialogue();
        }
    }

    public void TriggerDialogue()
    {
        dm.StartDialogue(dialogue);
    }
    public void CloseDialogue()
    {
        dm.EndDialogue();
    }
}
