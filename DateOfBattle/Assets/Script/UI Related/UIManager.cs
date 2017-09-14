using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text nameString;
    public Text status;
    public Slider slider;
    public Text hpString;
    // Use this for initialization
    void Start () {
		
	}

    public void DisplayUI(string name, int speed, int morale, int range, int damage, int maxHp, int hp)
    {
        this.enabled = true;
        nameString.text = name;
        status.text = "Speed:" + speed.ToString() + " Moral:" + morale.ToString() + "\nRange;" + range.ToString() + " AttackDamage:" + damage.ToString();
        slider.value = hp / maxHp;
        hpString.text = hp.ToString() + "/" + maxHp.ToString();
    }

    public void HideUI()
    {
        this.enabled = false;
    }

}
