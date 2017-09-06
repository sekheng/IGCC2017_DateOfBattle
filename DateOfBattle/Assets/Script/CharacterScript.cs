using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {
    public enum TYPE_ATTACK
    {
        NONE = -1,
        GUNSHOT,
        ASSULT,
        SNIPE,
        CANNON,
        HEAL
    }

    protected const int MAXIMUM_MORALE = 100;
    protected const int DEFAULT_MORALE = 50;

    [SerializeField]
    protected int maximumHp;
    public int m_MaximumHp
    {
        set
        {
            maximumHp = value;
        }
        get
        {
            return maximumHp;
        }
    }

    protected int hp;
    public int m_Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maximumHp);
        }
        get
        {
            return hp;
        }
    }


    protected int morale;
    public int m_Morale
    {
        set
        {
            morale = Mathf.Clamp(value, 0, MAXIMUM_MORALE);
        }
        get
        {
            return morale;
        }
    }

    [SerializeField]
    protected TYPE_ATTACK attackType;
    public TYPE_ATTACK m_AttackType
    {
        set
        {
            attackType = value;
        }
        get
        {
            return attackType;
        }
    }

    [SerializeField]
    protected int cooldown;
    public int m_Cooldown
    {
        set
        {
            cooldown = value;
        }
        get
        {
            return cooldown;
        }
    }

    [SerializeField]
    protected int moveSpeed;
    public int m_MoveSpeed
    {
        set
        {
            moveSpeed = value;
        }
        get
        {
            return moveSpeed;
        }
    }

    //personal

    [SerializeField]
    protected int warlike;  //like attack
    public int m_Warlike
    {
        set
        {
            warlike = value;
        }
        get
        {
            return warlike;
        }
    }

    [SerializeField]
    protected int emotion;  //like talk
    public int m_Emotion
    {
        set
        {
            emotion = value;
        }
        get
        {
            return emotion;
        }
    }


    [SerializeField]
    protected int warily;  //dislike advace
    public int m_Warily
    {
        set
        {
            warily = value;
        }
        get
        {
            return warily;
        }
    }


    // Use this for initialization
    void Start () {
        m_Morale = DEFAULT_MORALE;
        m_Hp = m_MaximumHp;
    }
	
    void Action()
    {
    }

    void Attack()
    {
        switch (attackType)
        {
            case TYPE_ATTACK.NONE:
                break;
            case TYPE_ATTACK.GUNSHOT:
                //GUNSHOT
                break;
            case TYPE_ATTACK.ASSULT:
                //ASSULT
                break;
            case TYPE_ATTACK.SNIPE:
                //SNIPE
                break;
            case TYPE_ATTACK.CANNON:
                //CANNON
                break;
            case TYPE_ATTACK.HEAL:
                //HEAL
                break;
        }
    }

    void Damage(int damage, TYPE_ATTACK damageType)
    {
        const int WEAKNESS_MULTIPLY = 2;

        //Damage set
        switch (attackType)
        {
            case TYPE_ATTACK.GUNSHOT:
                if(damageType == TYPE_ATTACK.SNIPE || damageType == TYPE_ATTACK.CANNON)
                {
                    damage *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.ASSULT:
                if (damageType == TYPE_ATTACK.GUNSHOT || damageType == TYPE_ATTACK.CANNON)
                {
                    damage *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.SNIPE:
                if (damageType == TYPE_ATTACK.ASSULT)
                {
                    damage *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.CANNON:
                if (damageType == TYPE_ATTACK.SNIPE)
                {
                    damage *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.HEAL:
                //HEAL
                break;
        }
        m_Hp -= damage;
    }

    void MoraleFluctuate()
    {
        m_Morale--;
    }
}
