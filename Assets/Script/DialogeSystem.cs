using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogeSystem : MonoBehaviour
{

    [SerializeField] List<GameObject> dialogeBundle = new List<GameObject>();
    List<GameObject> dialogueInBundle = new List<GameObject>();

    [SerializeField] List<GameObject> choiceBundle = new List<GameObject>();

    int onWhatDialogueBundle = 0;
    int onWhatDialogue = 0;

    bool doingChoice = false;
    bool enableInput = true;

    #region Text

    TextMeshProUGUI text;
    string textToWrite;
    int characterIndex;
    [SerializeField] float timePerCharacter;
    float timer;

    bool writeText = false;

    GameObject currentChoiceText;

    #endregion

    private void Start()
    {
        onWhatDialogueBundle = 0;

        foreach (Transform child in dialogeBundle[onWhatDialogueBundle].transform)
        {

            dialogueInBundle.Add(child.gameObject);
            child.gameObject.SetActive(false);

        }

        dialogueInBundle[onWhatDialogue].SetActive(true); // Start Dialogue
        WhatTextToWrite(dialogueInBundle[onWhatDialogue].GetComponentInChildren<TextMeshProUGUI>());

        onWhatDialogue++;

    }

    private void Update()
    {

        if (writeText)
        {

            timer -= Time.deltaTime;

            if(timer <= 0)
            {
     
                timer += timePerCharacter;
                characterIndex++;
                text.text = textToWrite.Substring(0, characterIndex);

                if(characterIndex >= textToWrite.Length)
                {

                    writeText = false;

                }

            }

        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !doingChoice && enableInput)
        {
            if (onWhatDialogueBundle == 1 && onWhatDialogue == 6)
            {
                return;
            }

            NextDialogue();

        }

    }

    void NextDialogue()
    {

        if (onWhatDialogue >= dialogueInBundle.Count)
        {
            doingChoice = true;
            NextChoice();

            return;

        }

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);
        dialogueInBundle[onWhatDialogue].SetActive(true);
        WhatTextToWrite(dialogueInBundle[onWhatDialogue].GetComponentInChildren<TextMeshProUGUI>());

        onWhatDialogue++;

    }

    void NextChoice()
    {

        choiceBundle[onWhatDialogueBundle].SetActive(true);

    }

    #region Choices

    public void WrongChoice(GameObject WrongText)
    {
        if (enableInput == false)
        {
            return;
        }

        // - Patiance

        if (currentChoiceText != null)
        {

            currentChoiceText.GetComponentInChildren<TextMeshProUGUI>().text = textToWrite;
            currentChoiceText.SetActive(false);

        }

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);

        currentChoiceText = WrongText;

        currentChoiceText.SetActive(true);
        WhatTextToWrite(currentChoiceText.GetComponentInChildren<TextMeshProUGUI>());


    }

    public void RightChoice()
    {
        if (enableInput == false)
        {
            return;
        }

        doingChoice = false;

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);
        choiceBundle[onWhatDialogueBundle].SetActive(false);
        dialogueInBundle.Clear();

        onWhatDialogue = 0;

        onWhatDialogueBundle++;

        dialogeBundle[onWhatDialogueBundle].SetActive(true);

        foreach (Transform child in dialogeBundle[onWhatDialogueBundle].transform)
        {

            dialogueInBundle.Add(child.gameObject);
            child.gameObject.SetActive(false);

        }

        onWhatDialogue++;

        NextDialogue();

    }

    #endregion

    public void skipIfTutorial()
    {
        if (onWhatDialogueBundle == 1 && onWhatDialogue == 6)
        {
            NextDialogue();
        }
    }

    public void EnableInput()
    {
        enableInput = true;
    }

    public void DisableInput()
    {
        enableInput = false;
    }

    void WhatTextToWrite(TextMeshProUGUI textToChange)
    {

        text = textToChange;
        textToWrite = textToChange.text;
        textToChange.text = "";
        characterIndex = 0;

        writeText = true;

    }

}
