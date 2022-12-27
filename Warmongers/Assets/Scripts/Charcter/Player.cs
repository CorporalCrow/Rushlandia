using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerController playerController;

    [HideInInspector] public ItemObject[] ItemObjects;

    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;

    [HideInInspector] public Transform mainHand;
    [HideInInspector] public Transform offHand;
    public Transform mainHandTransform;
    public Transform offHandTransform;

    [HideInInspector] public Transform mainHandAmmo;
    [HideInInspector] public Transform offHandAmmo;
    public Transform mainHandAmmoTransform;
    public Transform offHandAmmoTransform;

    public InventorySlot[] equipmentSlots { get { return equipment.Container.Slots; } }

    [HideInInspector] public bool fPressed = false;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();

        for (int i = 0; i < attributes.Length; i++)
        {
            float tempValue = attributes[i].value.BaseValue;
            attributes[i].SetParent(this);
            attributes[i].value.BaseValue = tempValue;
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
    }


    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.itemPrefab != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    if (mainHand != null)
                                    {
                                        Destroy(mainHand.gameObject);
                                    }
                                    break;
                                case ItemType.Ranged:
                                    if (mainHand != null)
                                    {
                                        Destroy(mainHand.gameObject);
                                        Destroy(mainHandAmmo.gameObject);
                                    }
                                    
                                    break;
                            }
                            break;
                        case ItemType.Ranged:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    if (offHand != null)
                                    {
                                        Destroy(offHand.gameObject);
                                    }
                                    break;
                                case ItemType.Ranged:
                                    if (offHand != null)
                                    {
                                        Destroy(offHand.gameObject);
                                        Destroy(offHandAmmo.gameObject);
                                    }
                                    
                                    break;
                            }
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public ModifiableInt value;

    public void OnAddItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.itemPrefab != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Weapon:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    mainHand = Instantiate(_slot.ItemObject.itemPrefab, mainHandTransform).transform;
                                    break;
                                case ItemType.Ranged:
                                    mainHand = Instantiate(_slot.ItemObject.itemPrefab, mainHandTransform).transform;

                                    mainHandAmmo = Instantiate(_slot.ItemObject.uiDisplay, mainHandAmmoTransform).transform;
                                    mainHandAmmo.GetComponent<AmmoDisplayText>().targetWeapon = mainHand.gameObject;

                                    for (int i = 0; i < equipment.Container.Slots.Length; i++)
                                    {
                                        if (equipmentSlots[i].item.Id == _slot.ItemObject.data.Id)
                                        {
                                            for (int j = 0; j < equipmentSlots[i].item.buffs.Length; j++)
                                            {
                                                switch (equipmentSlots[i].item.buffs[j].attribute)
                                                {
                                                    case Attributes.Damage:
                                                        mainHand.GetComponent<Ranged>().attackDamage = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ProjectileSpeed:
                                                        mainHand.GetComponent<Ranged>().bulletSpeed = Mathf.Round(equipmentSlots[i].item.buffs[j].value * 10) / 10;
                                                        break;
                                                    case Attributes.FireRate:
                                                        mainHand.GetComponent<Ranged>().fireRate = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.MaxAmmo:
                                                        mainHand.GetComponent<Ranged>().maxAmmo = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ReloadTime:
                                                        mainHand.GetComponent<Ranged>().reloadTime = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.Spread:
                                                        mainHand.GetComponent<Ranged>().spreadFactor = Mathf.Round(equipmentSlots[i].item.buffs[j].value * 100) / 100;
                                                        break;
                                                    case Attributes.ProjectileCount:
                                                        mainHand.GetComponent<Ranged>().projectilesPerVolley = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.VolleyDelay:
                                                        mainHand.GetComponent<Ranged>().timeBetweenVolley = Mathf.Round(equipmentSlots[i].item.buffs[j].value * 10) / 10;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ItemType.Ranged:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    offHand = Instantiate(_slot.ItemObject.itemPrefab, offHandTransform).transform;
                                    break;
                                case ItemType.Ranged:
                                    offHand = Instantiate(_slot.ItemObject.itemPrefab, offHandTransform).transform;
                                    
                                    offHandAmmo = Instantiate(_slot.ItemObject.uiDisplay, offHandAmmoTransform).transform;
                                    offHandAmmo.GetComponent<AmmoDisplayText>().targetWeapon = offHand.gameObject;

                                    for (int i = 0; i < equipment.Container.Slots.Length; i++)
                                    {
                                        if (equipmentSlots[i].item.Id == _slot.ItemObject.data.Id)
                                        {
                                            for (int j = 0; j < equipmentSlots[i].item.buffs.Length; j++)
                                            {
                                                switch (equipmentSlots[i].item.buffs[j].attribute)
                                                {
                                                    case Attributes.Damage:
                                                        offHand.GetComponent<Ranged>().attackDamage = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ProjectileSpeed:
                                                        offHand.GetComponent<Ranged>().bulletSpeed = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value * 10) / 10;
                                                        break;
                                                    case Attributes.FireRate:
                                                        offHand.GetComponent<Ranged>().fireRate = Mathf.Round(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.MaxAmmo:
                                                        offHand.GetComponent<Ranged>().maxAmmo = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ReloadTime:
                                                        offHand.GetComponent<Ranged>().reloadTime = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.Spread:
                                                        offHand.GetComponent<Ranged>().spreadFactor = Mathf.Round(equipmentSlots[i].item.buffs[j].value * 100) / 100;
                                                        break;
                                                    case Attributes.ProjectileCount:
                                                        offHand.GetComponent<Ranged>().projectilesPerVolley = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.VolleyDelay:
                                                        offHand.GetComponent<Ranged>().timeBetweenVolley = Mathf.Round(equipmentSlots[i].item.buffs[j].value * 10) / 10;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem && fPressed)
        {
            Item _item = new Item(groundItem.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void Update()
    {
        // Temp for testing saving and loading
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", Mathf.RoundToInt(attribute.value.ModifiedValue)));
        if (attribute.type == Attributes.Health) playerStats.setHealth(Mathf.RoundToInt(attribute.value.ModifiedValue));
        if (attribute.type == Attributes.Defense) playerStats.setDefense(Mathf.RoundToInt(attribute.value.ModifiedValue));
        if (attribute.type == Attributes.Speed) playerController.setSpeed(Mathf.RoundToInt(attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;
    
    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}