using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;
    public GameObject dialoguePanel;

    // Where the Dialogue Appears
    private Image panelImage;
    private TextMeshProUGUI npcName;
    private TextMeshProUGUI dialogueBox;
    private TextMeshProUGUI spaceToContinue;

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
        panelImage = dialoguePanel.GetComponent<Image>();
        npcName = dialoguePanel.GetComponent<DialogueBox>().npcName;
        dialogueBox = dialoguePanel.GetComponent<DialogueBox>().dialogueBox;
        spaceToContinue = dialoguePanel.GetComponent<DialogueBox>().spaceToContinue;

        panelImage.enabled = false;
        npcName.text = "";
        dialogueBox.text = "";
        spaceToContinue.enabled = false;
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
            panelImage.enabled = true;
            npcName.text = curNpcName;
            dialogueBox.text = sentences.Dequeue();
            spaceToContinue.enabled = true;
        }
    }

    public void EndDialogue()
    {
        sentences.Clear();
        panelImage.enabled = false;
        npcName.text = "";
        dialogueBox.text = "";
        spaceToContinue.enabled = false;
    }
}
