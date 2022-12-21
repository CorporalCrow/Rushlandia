using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [HideInInspector] public ItemObject[] ItemObjects;

    [HideInInspector] public float maxHealth;
    [ReadOnly] public float currentHealth;
    
    [ReadOnly] public float defensePercentage;

    public HealthBar healthBar;

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

    private Gun gun;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

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
                                    Destroy(mainHand.gameObject);
                                    break;
                                case ItemType.Gun:
                                    Destroy(mainHand.gameObject);
                                    Destroy(mainHandAmmo.gameObject);
                                    break;
                            }
                            break;
                        case ItemType.Gun:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    Destroy(offHand.gameObject);
                                    break;
                                case ItemType.Gun:
                                    Destroy(offHand.gameObject);
                                    Destroy(offHandAmmo.gameObject);
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
                                case ItemType.Gun:
                                    mainHand = Instantiate(_slot.ItemObject.itemPrefab, mainHandTransform).transform;

                                    mainHandAmmo = Instantiate(_slot.ItemObject.ammoDisplay, mainHandAmmoTransform).transform;
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
                                                        mainHand.GetComponent<Gun>().attackDamage = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ProjectileSpeed:
                                                        mainHand.GetComponent<Gun>().bulletSpeed = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.Firerate:
                                                        mainHand.GetComponent<Gun>().fireRate = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.MaxAmmo:
                                                        mainHand.GetComponent<Gun>().maxAmmo = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ReloadTime:
                                                        mainHand.GetComponent<Gun>().reloadTime = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.Spread:
                                                        mainHand.GetComponent<Gun>().spreadFactor = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.ProjectileCount:
                                                        mainHand.GetComponent<Gun>().projectilesPerVolley = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.VolleyDelay:
                                                        mainHand.GetComponent<Gun>().timeBetweenVolley = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ItemType.Gun:
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    offHand = Instantiate(_slot.ItemObject.itemPrefab, offHandTransform).transform;
                                    break;
                                case ItemType.Gun:
                                    offHand = Instantiate(_slot.ItemObject.itemPrefab, offHandTransform).transform;
                                    
                                    offHandAmmo = Instantiate(_slot.ItemObject.ammoDisplay, offHandAmmoTransform).transform;
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
                                                        offHand.GetComponent<Gun>().attackDamage = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ProjectileSpeed:
                                                        offHand.GetComponent<Gun>().bulletSpeed = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.Firerate:
                                                        offHand.GetComponent<Gun>().fireRate = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.MaxAmmo:
                                                        offHand.GetComponent<Gun>().maxAmmo = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.ReloadTime:
                                                        offHand.GetComponent<Gun>().reloadTime = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.Spread:
                                                        offHand.GetComponent<Gun>().spreadFactor = equipmentSlots[i].item.buffs[j].value;
                                                        break;
                                                    case Attributes.ProjectileCount:
                                                        offHand.GetComponent<Gun>().projectilesPerVolley = Mathf.RoundToInt(equipmentSlots[i].item.buffs[j].value);
                                                        break;
                                                    case Attributes.VolleyDelay:
                                                        offHand.GetComponent<Gun>().timeBetweenVolley = equipmentSlots[i].item.buffs[j].value;
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

    private bool fPressed = false;

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
        if (Input.GetKeyDown(KeyCode.F))
            fPressed = true;
        else
            fPressed = false;
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
        if (attribute.type == Attributes.Health) setHealth(Mathf.Round(attribute.value.ModifiedValue * 100) / 100);
        if (attribute.type == Attributes.Defense) setDefense(Mathf.Round(attribute.value.ModifiedValue * 100) / 100);
        if (attribute.type == Attributes.Speed) GetComponent<PlayerController>().setSpeed(Mathf.Round(attribute.value.ModifiedValue * 100) / 100);
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth - (maxHealth - currentHealth);
        maxHealth = newHealth;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            gameObject.SetActive(false);
        }
    }

    public void setDefense(float newDefense) => defensePercentage = 1 - (newDefense / (newDefense + 100));

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage * defensePercentage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
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