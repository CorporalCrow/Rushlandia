using UnityEngine;
using UnityEngine.UI;

public class InformationInterface : MonoBehaviour
{    
    [SerializeField]
    private InventoryScriptableObject mainInventory;
    [SerializeField]
    private InventoryScriptableObject mainEquipment;
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
                        switch (inventorySlots[i].item.buffs[j].attribute)
                        {
                            case Attributes.Health:
                                _stats = string.Concat(_stats, "Health: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Defense:
                                _stats = string.Concat(_stats, "Defense: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Speed:
                                _stats = string.Concat(_stats, "Speed: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Damage:
                                _stats = string.Concat(_stats, "Damage: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Projectile‏‏‎Speed:
                                _stats = string.Concat(_stats, "Projectile Speed: ", Mathf.Round(inventorySlots[i].item.buffs[j].value * 10) / 10, "\n");
                                break;
                            case Attributes.FireRate:
                                _stats = string.Concat(_stats, "Fire Rate: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.MaxAmmo:
                                _stats = string.Concat(_stats, "Max Ammo: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.ReloadTime:
                                _stats = string.Concat(_stats, "Reload Time: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Spread:
                                _stats = string.Concat(_stats, "Spread: ", Mathf.Round(inventorySlots[i].item.buffs[j].value * 100) / 100, "\n");
                                break;
                            case Attributes.ProjectileCount:
                                _stats = string.Concat(_stats, "Projectile Count: ", Mathf.RoundToInt(inventorySlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.VolleyDelay:
                                _stats = string.Concat(_stats, "Volley Delay: ", Mathf.Round(inventorySlots[i].item.buffs[j].value * 10) / 10, "\n");
                                break;
                        }
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
                        switch (equipmentSlots[i].item.buffs[j].attribute)
                        {
                            case Attributes.Health:
                                _stats = string.Concat(_stats, "Health: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Defense:
                                _stats = string.Concat(_stats, "Defense: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Speed:
                                _stats = string.Concat(_stats, "Speed: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Damage:
                                _stats = string.Concat(_stats, "Damage: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Projectile‏‏‎Speed:
                                _stats = string.Concat(_stats, "Projectile Speed: ", Mathf.Round(equipmentSlots[i].item.buffs[j].value * 10) / 10, "\n");
                                break;
                            case Attributes.FireRate:
                                _stats = string.Concat(_stats, "Fire Rate: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.MaxAmmo:
                                _stats = string.Concat(_stats, "Max Ammo: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.ReloadTime:
                                _stats = string.Concat(_stats, "Reload Time: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.Spread:
                                _stats = string.Concat(_stats, "Spread: ", Mathf.Round(equipmentSlots[i].item.buffs[j].value * 100) / 100, "\n");
                                break;
                            case Attributes.ProjectileCount:
                                _stats = string.Concat(_stats, "Projectile Count: ", Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value), "\n");
                                break;
                            case Attributes.VolleyDelay:
                                _stats = string.Concat(_stats, "Volley Delay: ", Mathf.Round(equipmentSlots[i].item.buffs[j].value * 10) / 10, "\n");
                                break;
                        }
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