using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText, nameText;
    [SerializeField] GameObject dialogBox, nameBox;

    [SerializeField] string[] dialogSentences;
    [SerializeField] int currentSentence;

    public static DialogController instance;

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;
        dialogText.text = dialogSentences[currentSentence];
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                currentSentence++;

                if(currentSentence >= dialogSentences.Length)
                {
                    dialogBox.SetActive(false);
                }
                else
                {
                    dialogText.text = dialogSentences[currentSentence];
                }            
            }
        }
    }

    public void ActivateDialog(string[] newSentencesToUse)
    {
        dialogSentences = newSentencesToUse;
        currentSentence = 0;

        dialogText.text = dialogSentences[currentSentence];
        dialogBox.SetActive(true);

    }
}
