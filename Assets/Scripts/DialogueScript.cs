using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//textbox max is 120 characters
//need something to spawn the dialogue box

public class DialogueScript : MonoBehaviour
{
    GameObject m_dialogue_object;

    Text m_name_text;
    Text m_dialogue_text;
    Image m_dialogue_image;

    Sprite test;

    Queue<DialogueItem> m_dialogues;
    DialogueItem m_current_dialogue;

    bool m_can_listen;
    bool active;

    int t_count = 0;

    void Start()
    {
        m_dialogue_object = gameObject;
        m_name_text = gameObject.GetComponentsInChildren<Text>()[2];
        m_dialogue_text = gameObject.GetComponentsInChildren<Text>()[1];
        m_dialogue_image = gameObject.transform.Find("CharacterImage").GetComponent<Image>();

        m_dialogues = new Queue<DialogueItem>();

        //test = Resources.Load<Sprite>("Materials/UIMaterials/bottom_border");
        //m_dialogue_image.overrideSprite = test;

        m_dialogue_text.text = "balls";

        m_can_listen = true; //not sure when to use

        HideDialogueBox();

        StartCoroutine("HandleDialogues");
    }

    IEnumerator HandleDialogues()
    {
        while (true)
        {
            if(m_dialogues.Count > 0 && active == false)
            {
                LoadNextDialogue();
                ShowDialogueBox();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    Debug.Log("added");
        //    if (t_count == 0)
        //    {
        //        AddDialogue(new DialogueItem("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", "test1", null));
        //        t_count++;
        //    }
        //    else if (t_count == 1)
        //    {
        //        AddDialogue(new DialogueItem("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", "test2", null));
        //        t_count++;
        //    }
        //    else if (t_count == 2)
        //    {
        //        AddDialogue(new DialogueItem("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", "test3", null));
        //        t_count++;
        //    }
        //}
    }

    public void AddDialogue(DialogueItem dialogueItem)
    {
        m_dialogues.Enqueue(dialogueItem);
    }

    private void LoadNextDialogue()
    {
        m_current_dialogue = m_dialogues.Dequeue();
        m_name_text.text = m_current_dialogue.GetTalkerName();
        m_dialogue_image.overrideSprite = m_current_dialogue.GetTalkerImage();
        m_dialogue_text.text = m_current_dialogue.NextTextSegment();
    }

    public void GoToNextText()
    {
        if (m_current_dialogue.HasMoreText())
        {
            m_dialogue_text.text = m_current_dialogue.NextTextSegment();
        }
        else
        {
            if(m_dialogues.Count > 0)
            {
                LoadNextDialogue();
            } else
            {
                HideDialogueBox();
            }
        }
    }

    private void ShowDialogueBox()
    {
        m_dialogue_object.transform.localPosition = new Vector3(0f, -67f, 0f);
        active = true;
    }

    private void HideDialogueBox()
    {
        m_dialogue_object.transform.localPosition = new Vector3(0f, -419f, 0f);
        active = false;
    }

    public void SetPlayerCanListen(bool canListen)
    {
        m_can_listen = canListen;
    }

}

