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

    [Tooltip("The different smiling face based on morale! There can only be 3 as it is hardcoded!")]
    public Sprite[] smilingFaceMorale;
    [Tooltip("Where the iconFace is")]
    public Image faceIcon;

    public void DisplayUI(string name, int speed, int morale, int range, int damage, int maxHp, int hp)
    {
        this.enabled = true;
        nameString.text = name;
        status.text = "Speed:" + speed.ToString() + " Motivation:" + morale.ToString() + "\nRange;" + range.ToString() + " AttackDamage:" + damage.ToString();
        slider.value = (float)hp / (float)maxHp;
        hpString.text = hp.ToString() + "/" + maxHp.ToString();
    }

    public void DisplayUI(CharacterScript charStat)
    {
        string charName;
        switch (charStat.m_AttackType)
        {
            case CharacterScript.TYPE_ATTACK.GUNSHOT:
                charName = "Soldier";
                break;
            case CharacterScript.TYPE_ATTACK.CANNON:
                charName = "Tank";
                break;
            default:
                charName = "WhoCares";
                break;
        }
        DisplayUI(charName, charStat.m_MoveSpeed, charStat.m_Motivation, charStat.m_Range, charStat.m_AttackDamage, charStat.m_MaximumHp, charStat.m_Hp);
    }

    public void HideUI()
    {
        this.enabled = false;
    }

}
