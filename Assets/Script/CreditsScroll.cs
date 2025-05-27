using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] float startPositionY;
    [SerializeField] GameObject title;
    [SerializeField] List<GameObject> textList;
    [SerializeField] float speed;

    void Start()
    {
        StartCredits();
    }

    void Update()
    {
        foreach (GameObject obj in textList)
        {
            obj.transform.position = obj.transform.position + Vector3.up * speed * Time.deltaTime;
        }
    }

    public void StartCredits()
    {
        Vector3 titlePos = title.transform.position;

        foreach (GameObject obj in textList)
        {
            Debug.Log(obj);
            obj.transform.position = obj.transform.position - Vector3.Scale(titlePos, Vector3.up);
            //obj.transform.position = Vector3.zero;
        }
    }
}
