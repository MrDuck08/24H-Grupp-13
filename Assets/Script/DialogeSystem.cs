using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogeSystem : MonoBehaviour
{

    [SerializeField] List<GameObject> dialogeBundle = new List<GameObject>();
    List<GameObject> dialogueInBundle = new List<GameObject>();
    [SerializeField] GameObject winDialogeBundle;

    [SerializeField] List<GameObject> choiceBundle = new List<GameObject>();

    int onWhatDialogueBundle = 0;
    int onWhatDialogue = 0;

    bool doingChoice = false;
    bool enableInput = true;

    PatienceBar patienceBar;

    [SerializeField] float correctChoiceAdd;
    [SerializeField] float wrongChoiceSubtract;

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
        patienceBar = FindFirstObjectByType<PatienceBar>();

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

            if (onWhatDialogueBundle == 1 && onWhatDialogue == 8)
            {
                Debug.Log("Drain");
                patienceBar.EnableDraining();
            }

            NextDialogue();

        }

    }

    void NextDialogue()
    {

        if (doingChoice == true)
        {
            return;
        }

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);
        dialogueInBundle[onWhatDialogue].SetActive(true);
        WhatTextToWrite(dialogueInBundle[onWhatDialogue].GetComponentInChildren<TextMeshProUGUI>());

        onWhatDialogue++;

        if (onWhatDialogue >= dialogueInBundle.Count)
        {
            doingChoice = true;
            NextChoice();

        }

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

        patienceBar.SubtractAmount(wrongChoiceSubtract);

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

        patienceBar.AddAmount(correctChoiceAdd);

        doingChoice = false;

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);
        choiceBundle[onWhatDialogueBundle].SetActive(false);
        dialogueInBundle.Clear();

        onWhatDialogue = 0;

        onWhatDialogueBundle++;

        if (onWhatDialogue < dialogeBundle.Count)
        {
            dialogeBundle[onWhatDialogueBundle].SetActive(true);
        }
        else
        {

            winDialogeBundle.SetActive(true);

        }

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
