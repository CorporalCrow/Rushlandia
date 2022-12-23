using UnityEngine;
using UnityEngine.UI;

public enum ItemType 
{
    Food,
    Helmet,
    Weapon,
    Ranged,
    Leggings,
    Chest,
    Default
}

public enum Attributes
{
    Health,
    Defense,
    Speed,
    Damage,
    Projectile‏‏‎Speed,
    FireRate,
    MaxAmmo,
    ReloadTime,
    Spread,
    ProjectileCount,
    VolleyDelay
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/Item")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public GameObject itemPrefab;
    public Text ammoDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new Item(); 

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id = -1;
    [HideInInspector]
    public string Description;
    [HideInInspector]
    public Sprite uiSprite;
    public ItemBuff[] buffs;
    public Item()
    {
        Name = "";
        Id = -1;
    }
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.data.Id;
        Description = item.description;
        uiSprite = item.uiDisplay;
        buffs = new ItemBuff[item.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max)
            {
                attribute = item.data.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff : IModifier
{
    public Attributes attribute;
    public float value;
    public float min;
    public float max;
    public ItemBuff(float _min, float _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref float baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
