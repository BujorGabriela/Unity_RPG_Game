using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string newGameScene;
    [SerializeField] GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Player_Pos_X"))
            continueButton.SetActive(true);
        else
            continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitButton()
    {
        UnityEngine.Debug.Log("We just quit the game");
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
