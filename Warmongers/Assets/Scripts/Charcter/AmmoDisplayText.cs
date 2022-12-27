using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayText : MonoBehaviour
{
    public GameObject targetWeapon;
    public Text uiDisplay;

    void Start()
    {
        uiDisplay = this.GetComponent<Text>();
    }

    void Update()
    {
        AmmoDisplay();
    }

    public void AmmoDisplay()
    {
        if (targetWeapon.GetComponent<Ranged>().currentAmmo > 0)
        {
            uiDisplay.text = targetWeapon.GetComponent<Ranged>().currentAmmo.ToString();
        }
        else
        {
            uiDisplay.text = "Reloading...";
        }
    }
}
