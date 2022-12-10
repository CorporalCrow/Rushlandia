using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayText : MonoBehaviour
{
    public GameObject targetWeapon;
    public Text ammoDisplay;

    void Start()
    {
        ammoDisplay = this.GetComponent<Text>();
    }

    void Update()
    {
        AmmoDisplay();
    }

    public void AmmoDisplay()
    {
        if (targetWeapon.GetComponent<Gun>().currentAmmo > 0)
        {
            ammoDisplay.text = targetWeapon.GetComponent<Gun>().currentAmmo.ToString();
        }
        else
        {
            ammoDisplay.text = "Reloading...";
        }
    }
}
