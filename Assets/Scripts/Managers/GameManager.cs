using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] PlayerStats[] playerStats;

    public bool gameMenuOpened, dialogBoxOpened, shopOpened;

    public int currentBitcoins;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        playerStats = FindObjectsOfType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UnityEngine.Debug.Log("Data has been saved");
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            UnityEngine.Debug.Log("Data has been loaded");
            LoadData();
        }

        if (gameMenuOpened || dialogBoxOpened || shopOpened)
        {
            Player.instance.deactivateMovement = true;
        }
        else
        {
            Player.instance.deactivateMovement = false;
        }
    }
    public PlayerStats[] GetPlayerStats()
    {
        return playerStats;
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("Player_Pos_X", Player.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Pos_Y", Player.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Pos_Z", Player.instance.transform.position.z);

        for(int i = 0; i < playerStats.Length; i++)
        {
            if(playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentXP", playerStats[i].currentXP);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentHP", playerStats[i].currentHP);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_MaxMana", playerStats[i].maxMana);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_CurrentMana", playerStats[i].currentMana);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Dexterity", playerStats[i].dexterity);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_Defence", playerStats[i].defence);

            PlayerPrefs.SetString("Player_" + playerStats[i].playerName + "_EquipedWeapon", playerStats[i].equippedWeaponName);
            PlayerPrefs.SetString("Player_" + playerStats[i].playerName + "_EquipedArmor", playerStats[i].equippedArmorName);

            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_WeaponPower", playerStats[i].weaponPower);
            PlayerPrefs.SetInt("Player_" + playerStats[i].playerName + "_ArmorDefence", playerStats[i].armorDefence);
        }

        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);

        PlayerPrefs.SetInt("Number_Of_Items", Inventory.instance.GetItemsList().Count);
        for (int i = 0; i < Inventory.instance.GetItemsList().Count; i++)
        {
            ItemsManager itemInInventory = Inventory.instance.GetItemsList()[i];
            PlayerPrefs.SetString("Item_" + i + "_Name", itemInInventory.itemName);

            if (itemInInventory.isStackable)
            {
                PlayerPrefs.SetInt("Items_" + i + "_Amount", itemInInventory.amount);
            }
        }
    }

    public void LoadData()
    {
        Player.instance.transform.position = new Vector3(
            PlayerPrefs.GetFloat("Player_Pos_X"),
            PlayerPrefs.GetFloat("Player_Pos_Y"),
            PlayerPrefs.GetFloat("Player_Pos_Z")
            );

        for( int i = 0; i < playerStats.Length; i++ )
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_active") ==0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Level");
            playerStats[i].currentXP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentXP");

            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxHP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentHP");

            playerStats[i].maxMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_MaxMana");
            playerStats[i].currentMana = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_CurrentMana");

            playerStats[i].dexterity = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Dexterity");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_Defence");

            playerStats[i].equippedWeaponName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_EquipedWeapon");
            playerStats[i].equippedArmorName = PlayerPrefs.GetString("Player_" + playerStats[i].playerName + "_EquipedArmor");

            playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_WeaponPower");
            playerStats[i].armorDefence = PlayerPrefs.GetInt("Player_" + playerStats[i].playerName + "_ArmorDefence");
        }

        for(int i = 0; i < PlayerPrefs.GetInt("Number_Of_Items"); i++)
        {
            string itemName = PlayerPrefs.GetString("Item_" + i + "_Name");
            ItemsManager itemToAdd = ItemsAssets.instance.GetItemAsset(itemName);

            int itemAmount = 0;
            if(PlayerPrefs.HasKey("Items_" + i + "_Amount"))
            {
                itemAmount = PlayerPrefs.GetInt("Items_" + i + "_Amount");
            }

            Inventory.instance.AddItems(itemToAdd);
            if(itemToAdd.isStackable && itemAmount > 1)
            {
                itemToAdd.amount = itemAmount;
            }
        }
    }
}
