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
    [SerializeField] GameObject loseDialogueBundle;

    [SerializeField] List<GameObject> choiceBundle = new List<GameObject>();

    int onWhatDialogueBundle = 0;
    int onWhatDialogue = 0;

    bool doingChoice = false;
    bool enableInput = true;
    bool loseDueToPatiance = false;

    PatienceBar patienceBar;

    [SerializeField] float correctChoiceAdd;
    [SerializeField] float nextDialogueAdd;
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

    #region Jump

    [Header("Jump")]

    [SerializeField] GameObject president;
    [SerializeField] GameObject jumpPos;

    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float downAcceration = 5;

    float timeForJumpUp;
    float originalJumpPos;
    float jumpAcceration = 1f;
    float distanseToJump;
    float xHalfWayPos;

    bool startJumpUp = false;
    bool startJumpingDown = false;
    bool jumping = false;

    #endregion

    SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
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

        originalJumpPos = timeForJumpUp;

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

        #region Jump

        if (startJumpUp)
        {

            timeForJumpUp -= Time.deltaTime;

            jumpSpeed += jumpAcceration * Time.deltaTime;

            president.transform.position = Vector2.MoveTowards(president.transform.position, new Vector2(jumpPos.transform.position.x - xHalfWayPos, xHalfWayPos * 2), jumpSpeed * Time.deltaTime);

            if(timeForJumpUp <= 0)
            {

                startJumpUp = false;
                startJumpingDown = true;

                distanseToJump = Vector2.Distance(jumpPos.transform.position, president.transform.position);

                timeForJumpUp = originalJumpPos;

                timeForJumpUp = distanseToJump * 2 / jumpSpeed;

                jumpAcceration = downAcceration;

                jumpSpeed = 0;

            }

        }

        if (startJumpingDown)
        {

            timeForJumpUp -= Time.deltaTime;

            jumpSpeed += jumpAcceration * Time.deltaTime;

            president.transform.position = Vector2.MoveTowards(president.transform.position, jumpPos.transform.position, jumpSpeed * Time.deltaTime);

            if (president.transform.position == jumpPos.transform.position)
            {

                startJumpingDown = false;

                sceneLoader.LoadSceneByName("LoseScene");

            }

        }

        #endregion

    }

    void NextDialogue()
    {

        if (doingChoice == true)
        {
            return;
        }

        if (!loseDialogueBundle)
        {
            patienceBar.AddAmount(nextDialogueAdd);
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

        if (loseDueToPatiance)
        {

            xHalfWayPos = jumpPos.transform.position.x - president.transform.position.x;

            xHalfWayPos /= 2;

            distanseToJump = Vector2.Distance(new Vector2(jumpPos.transform.position.x - xHalfWayPos, xHalfWayPos * 2), president.transform.position);

            timeForJumpUp = distanseToJump * 2 / jumpSpeed;

            jumpAcceration = -jumpSpeed / timeForJumpUp;

            jumping = true;
            startJumpUp = true;

            return;

        }

        if (choiceBundle.Count > onWhatDialogueBundle)
        {

            choiceBundle[onWhatDialogueBundle].SetActive(true);

        }
        else
        {

            sceneLoader.LoadSceneByName("WinScene");

        }

    }

    #region Choices

    public void WrongChoice(GameObject WrongText)
    {
        if (enableInput == false)
        {
            return;
        }

        patienceBar.EnableDraining();

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

        patienceBar.EnableDraining();
        patienceBar.AddAmount(correctChoiceAdd);

        doingChoice = false;

        dialogueInBundle[onWhatDialogue - 1].SetActive(false);
        choiceBundle[onWhatDialogueBundle].SetActive(false);
        dialogueInBundle.Clear();

        onWhatDialogue = 0;

        onWhatDialogueBundle++;

        if (onWhatDialogueBundle < dialogeBundle.Count)
        {
            dialogeBundle[onWhatDialogueBundle].SetActive(true);

            foreach (Transform child in dialogeBundle[onWhatDialogueBundle].transform)
            {

                dialogueInBundle.Add(child.gameObject);
                child.gameObject.SetActive(false);

            }

        }
        else
        {

            winDialogeBundle.SetActive(true);

            patienceBar.DisableDraining();

            foreach (Transform child in winDialogeBundle.transform)
            {

                dialogueInBundle.Add(child.gameObject);
                child.gameObject.SetActive(false);

            }

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

    public void Lose()
    {

        if(loseDueToPatiance) { return ; }

        loseDueToPatiance = true;

        dialogeBundle[onWhatDialogueBundle].SetActive(false);
        choiceBundle[onWhatDialogueBundle].SetActive(false);

        winDialogeBundle.SetActive(true);

        foreach (Transform child in loseDialogueBundle.transform)
        {

            dialogueInBundle.Add(child.gameObject);
            child.gameObject.SetActive(false);

        }

        doingChoice = false;

        NextDialogue();

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
