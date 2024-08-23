using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Dialog : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float textSpeed = 0.05f;

    public string[] lines;
    private int index;

    private Action onDialogFinished;

    private void Awake()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false); // Ensure dialog is hidden initially
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialog(string[] dialogLines, Action onFinish)
    {
        lines = dialogLines;
        onDialogFinished = onFinish;    // Set the callback
        index = 0;
        gameObject.SetActive(true);
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine (TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            onDialogFinished?.Invoke();
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
