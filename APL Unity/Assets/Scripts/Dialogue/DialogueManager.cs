using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;
    public GameObject dialoguePanel;

    private Animator animator;


    // Where the Dialogue Appears
    private TextMeshProUGUI npcName;
    private TextMeshProUGUI dialogueBox;

    private Queue<string> sentences;
    private string curNpcName = "";


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one dialogueManager in scene!");
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        sentences = new Queue<string>();

        // I am so sorry for these names... I can't think of better ones.
        npcName = dialoguePanel.GetComponent<DialogueBox>().npcName;
        dialogueBox= dialoguePanel.GetComponent<DialogueBox>().dialogueBox;

        animator = dialoguePanel.GetComponent<Animator>();
    }
    

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        curNpcName = dialogue.name;
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        else
        {
            animator.SetBool("isOpen", true);
            npcName.text = curNpcName;
            dialogueBox.text = sentences.Dequeue();
        }
    }

    public void EndDialogue()
    {
        sentences.Clear();
        animator.SetBool("isOpen", false);
    }
}
