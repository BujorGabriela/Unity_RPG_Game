using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    public GameObject menu;

    [SerializeField] GameObject[] statsButtons;

    public static MenuManager instance;

    private PlayerStats[] playerStats;
    [SerializeField] TextMeshProUGUI[] nameText, hpText, manaText, currentXPText, xpText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] SpriteRenderer[] characterImage;
    [SerializeField] GameObject[] characterPanel;

    [SerializeField] TextMeshProUGUI statName, statHP, statMana, statDex, statDef, statEquipedWeapon, statEquipedArmor;
    [SerializeField] TextMeshProUGUI statWeaponPower, statArmorDefence;
    [SerializeField] Image characterSatImage;

    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;

    public TextMeshProUGUI itemName, itemDescription;

    public ItemsManager activeItem;

    [SerializeField] GameObject characterChoicePanel;
    [SerializeField] TextMeshProUGUI[] itemsCharacterChoiceNames;

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

        statEquipedWeapon.text = playerSelected.equippedWeaponName;
        statEquipedArmor.text = playerSelected.equippedArmorName;

        statWeaponPower.text = playerSelected.weaponPower.ToString();
        statArmorDefence.text = playerSelected.armorDefence.ToString();

    }

    public void UpdateItemsInventory()
    {
        //foreach (Transform itemSlot in itemSlotContainerParent)
        //{
            //Destroy(itemSlot.gameObject);
        //}


        foreach (ItemsManager item in Inventory.instance.GetItemsList())
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Items Image").GetComponent<Image>();
            itemImage.sprite = item.itemsImage;

            TextMeshProUGUI itemsAmountText = itemSlot.Find("Amount Text").GetComponent<TextMeshProUGUI>();

            if (item.amount > 1)
                itemsAmountText.text = item.amount.ToString();
            else
                itemsAmountText.text = "";

            itemSlot.GetComponent<ItemButton>().itemOnButton = item;
        }
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            Inventory.instance.RemoveItem(activeItem);
            UpdateItemsInventory();
        }
    }

    public void UseItem(int selectedCharacter)
    {
        if (activeItem != null)
        {
            activeItem.UseItem(selectedCharacter);
        }
        OpenCharacterChoicePanel();
        DiscardItem();
    }

    public void OpenCharacterChoicePanel()
    {
        characterChoicePanel.SetActive(true);

        if(activeItem)
        {
            for(int i = 0; i < playerStats.Length; i++)
                {
                    PlayerStats activePlayer = GameManager.instance.GetPlayerStats()[i];
                    itemsCharacterChoiceNames[i].text = activePlayer.playerName;

                    bool activePlayerAvailable = activePlayer.gameObject.activeInHierarchy;
                    itemsCharacterChoiceNames[i].transform.parent.gameObject.SetActive(activePlayerAvailable);
            }
        }
  
    }

    public void CloseCharacterChoicePanel()
    {
        characterChoicePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEngine.Debug.Log("We've quit the game");
    }

    public void FadeImage()
    {
        imageToFade.GetComponent<Animator>().SetTrigger("Start Fading");
    }
}
