using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial 1" 
            || SceneManager.GetActiveScene().name == "Tutorial 2"
            || SceneManager.GetActiveScene().name == "The Finish Line"
            && gameObject.name == "TutorialDialogueTrigger")
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
