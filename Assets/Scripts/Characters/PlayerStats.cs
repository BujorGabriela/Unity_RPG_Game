using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats instance;

    public string playerName;

    public Sprite characterImage;

    [SerializeField] int maxLevel = 50;
    public int playerLevel = 1;
    public int currentXP;
    public int[] xpForNextLevel;
    [SerializeField] int baseLevelXP = 100;

    public int maxHP = 100;
    public int currentHP;

    public int maxMana = 30;
    public int currentMana;

    public int dexterity;
    public int defence;

    public string equippedWeaponName;
    public string equippedArmorName;

    public int weaponPower;
    public int armorDefence;

    public ItemsManager equipedWeapon, equipedArmor;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        xpForNextLevel = new int[maxLevel];
        xpForNextLevel[1] = baseLevelXP;

        for(int i = 2; i < xpForNextLevel.Length; i++)
        {
            xpForNextLevel[i] = (int)(0.02f * i * i * i + 3.06f * i * i + 105.6f * i); //from the dark souls remastered wiki
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            AddXP(100);
        }
    }

    public void AddXP(int amountOfXp)
    {
        currentXP += amountOfXp; // currentXP = currentXP + amountOfXp
        if(currentXP > xpForNextLevel[playerLevel])
        {
            currentXP -= xpForNextLevel[playerLevel];
            playerLevel++;

            if (playerLevel % 2 == 0)
            {
                dexterity++;
            }
            else
            {
                defence++;
            }

            maxHP = Mathf.FloorToInt(maxHP * 1.06f);
            currentHP = maxHP;

            maxMana = Mathf.FloorToInt(maxMana * 1.06f);
            currentMana = maxMana;
        }
    }

    public void AddHP(int amountHPToAdd)
    {
        currentHP += amountHPToAdd;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    
    public void AddMana(int amountManaToAdd)
    {
        currentMana += amountManaToAdd;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    public void EquipWeapon(ItemsManager weaponToEquip)
    {
        equipedWeapon = weaponToEquip;
        equippedWeaponName = equipedWeapon.itemName;
        weaponPower = equipedWeapon.weaponDexterity;
    }
    
    public void EquipArmor(ItemsManager armorToEquip)
    {
        equipedArmor = armorToEquip;
        equippedArmorName = equipedArmor.itemName;
        armorDefence = equipedArmor.armorDefence;
    }
}
