using UnityEngine;
using UnityEngine.UI;

public class InformationInterface : MonoBehaviour
{    
    [SerializeField]
    private InventoryObject mainInventory;
    [SerializeField]
    private InventoryObject mainEquipment;
    public InventorySlot[] inventorySlots { get { return mainInventory.Container.Slots; } }
    public InventorySlot[] equipmentSlots { get { return mainEquipment.Container.Slots; } }

    public Text itemName;
    public GameObject itemSprite;
    public Text itemDescription;
    public Text itemStats;

    [SerializeField]
    private Sprite transparent;

    public void GetInfo(int _itemID)
    {
        if(_itemID != -1)
            for (int i = 0; i < mainInventory.Container.Slots.Length; i++)
            {
                if (inventorySlots[i].item.Id == _itemID)
                {
                    itemName.text = inventorySlots[i].item.Name;
                    itemSprite.GetComponent<Image>().sprite = inventorySlots[i].item.uiSprite;
                    itemDescription.text = inventorySlots[i].item.Description;

                    string _stats = null;
                    for (int j = 0; j < inventorySlots[i].item.buffs.Length; j++)
                    {
                        _stats = string.Concat(_stats, inventorySlots[i].item.buffs[j].attribute, ": ", inventorySlots[i].item.buffs[j].value, "\n");
                    }
                    itemStats.text = _stats;
                }
            }
        else
        {
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
            itemSprite.GetComponent<Image>().sprite = transparent;
            itemStats.text = string.Empty;
        }

        if (_itemID != -1)
            for (int i = 0; i < mainEquipment.Container.Slots.Length; i++)
            {
                if (equipmentSlots[i].item.Id == _itemID)
                {
                    itemName.text = equipmentSlots[i].item.Name;
                    itemSprite.GetComponent<Image>().sprite = equipmentSlots[i].item.uiSprite;
                    itemDescription.text = equipmentSlots[i].item.Description;

                    string _stats = null;
                    for (int j = 0; j < equipmentSlots[i].item.buffs.Length; j++)
                    {
                        _stats = string.Concat(_stats, equipmentSlots[i].item.buffs[j].attribute, ": ", equipmentSlots[i].item.buffs[j].value, "\n");
                    }
                    itemStats.text = _stats;
                }
            }
        else
        {
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
            itemSprite.GetComponent<Image>().sprite = transparent;
            itemStats.text = string.Empty;
        }
    }
}