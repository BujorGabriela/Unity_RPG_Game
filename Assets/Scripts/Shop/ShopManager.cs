using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopMenu, buyPanel, sellPanel;

    [SerializeField] TextMeshProUGUI currentBitcoinText;

    public List<ItemsManager> itemsForSale;

    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotBuyContainerParent;
    [SerializeField] Transform itemSlotSellContainerParent;

    [SerializeField] ItemsManager selectedItem;
    [SerializeField] TextMeshProUGUI buyItemName, buyItemDescription, buyItemValue;
    [SerializeField] TextMeshProUGUI sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        GameManager.instance.shopOpened = true;

        currentBitcoinText.text = "BTC: " + GameManager.instance.currentBitcoins;
        buyPanel.SetActive(true);
    }

    public void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopOpened = false;
    }

    public void OpenBuyPanel()
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);

        UpdateItemsInShop(itemSlotBuyContainerParent, itemsForSale);
    }

    public void OpenSellPanel()
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);

        UpdateItemsInShop(itemSlotSellContainerParent, Inventory.instance.GetItemsList());
    }

    private void UpdateItemsInShop(Transform itemSlotContainerParent, List<ItemsManager> itemsToLookThrough)
    {
        //foreach(Transform itemSlot in itemSlotContainerParent)
        //{
            //Destroy(itemSlot.gameObject);
        //}

        foreach (ItemsManager item in itemsToLookThrough)
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

    public void SelectedBuyItem(ItemsManager itemToBuy)
    {
        selectedItem = itemToBuy;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.itemDescription;
        buyItemValue.text = "Value: " + selectedItem.valueInCoins;
    }
    public void SelectedSellItem(ItemsManager itemToSell)
    {
        selectedItem = itemToSell;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.itemDescription;
        sellItemValue.text = "Value: " + (int)(selectedItem.valueInCoins*0.75f);
    }

    public void BuyItem()
    {
        if(GameManager.instance.currentBitcoins >= selectedItem.valueInCoins)
        {
            GameManager.instance.currentBitcoins -= selectedItem.valueInCoins;
            Inventory.instance.AddItems(selectedItem);

            currentBitcoinText.text = "BTC: " + GameManager.instance.currentBitcoins;
        }
    }

    public void SellItem()
    {
        if(selectedItem)
        {
            GameManager.instance.currentBitcoins += (int)(selectedItem.valueInCoins * 0.75f);
            Inventory.instance.RemoveItem(selectedItem);

            currentBitcoinText.text = "BTC: " + GameManager.instance.currentBitcoins;
            selectedItem = null;

            OpenSellPanel();
        }
    }
}
