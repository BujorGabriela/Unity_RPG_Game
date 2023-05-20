using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    [SerializeField] GameObject menu;

    [SerializeField] GameObject[] statsButtons;

    public static MenuManager instance;

    private PlayerStats[] playerStats;
    [SerializeField] TextMeshProUGUI[] nameText, hpText, manaText, currentXPText, xpText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] SpriteRenderer[] characterImage;
    [SerializeField] GameObject[] characterPanel;

    [SerializeField] TextMeshProUGUI statName, statHP, statMana, statDex, statDef;
    [SerializeField] Image characterSatImage;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(menu.activeInHierarchy)
            {
                menu.SetActive(false);
                GameManager.instance.gameMenuOpened = false;
            }
            else
            {
                UpdateStats();
                menu.SetActive(true);
                GameManager.instance.gameMenuOpened = true;
            }
        }
    }

    public void UpdateStats()
    {
        playerStats = GameManager.instance.GetPlayerStats();

        for(int i = 0; i < playerStats.Length; i++)
        {
            characterPanel[i].SetActive(true);
            nameText[i].text = playerStats[i].playerName;
            hpText[i].text = "HP: "+ playerStats[i].currentHP + "/" + playerStats[i].maxHP;
            manaText[i].text = "Mana: " + playerStats[i].currentMana + "/" + playerStats[i].maxMana;
            currentXPText[i].text = "Current XP:" + playerStats[i].currentXP;

            characterImage[i].sprite = playerStats[i].characterImage;

            xpText[i].text = playerStats[i].currentXP.ToString() + "/" + playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].maxValue = playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].value = playerStats[i].currentXP;
        }
    }

    public void StatsMenu()
    {
        for(int i = 0; i < playerStats.Length; i++)
        {
            statsButtons[i].SetActive(true);

            statsButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerStats[i].playerName;
        }
        StatsMenuUpdate(1);
    }

    public void StatsMenuUpdate(int playerSelectedNumber)
    {
        PlayerStats playerSelected = playerStats[playerSelectedNumber];
        statName.text = playerSelected.playerName;
        statHP.text = playerSelected.currentHP.ToString() + "/" + playerSelected.maxHP;
        statMana.text = playerSelected.currentMana.ToString() + "/" + playerSelected.maxMana;
        statDex.text = playerSelected.dexterity.ToString();
        statDef.text = playerSelected.defence.ToString();
        characterSatImage.sprite = playerSelected.characterImage;

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("We've quit the game");
    }

    public void FadeImage()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("Start Fading");
    }
}
