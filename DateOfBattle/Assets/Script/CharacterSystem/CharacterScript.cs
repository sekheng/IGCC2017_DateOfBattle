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
    public const int MAXIMUM_PERSONALTY = 100;

    public TileScript myTile;
    public DamageTextControl damageText;
    public PerticleExplosion damageEffect;

    [SerializeField, Tooltip("The unit max health")]
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
    [SerializeField, Tooltip("The unit health")]
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

    [SerializeField, Tooltip("The unit attack type")]
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

    [SerializeField, Tooltip("The unit attack damage")]
    protected int attackDamage;
    public int m_AttackDamage
    {
        set
        {
            attackDamage = value;
        }
        get
        {
            return attackDamage;
        }
    }

    [SerializeField, Tooltip("The unit attack interval")]
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

    [SerializeField, Tooltip("The unit move speed")]
    protected int moveSpeed;
    public int m_MoveSpeed
    {
        set
        {
            moveSpeed = Mathf.Max(0, value);
        }
        get
        {
            return moveSpeed;
        }
    }
    [SerializeField, Tooltip("The unit leftover move speed. It will be set to the move speed variable at the start so there is no need to set the value at the editor")]
    protected int leftOverMoveSpeed;
    public int m_leftOverMoveSpeed
    {
        set
        {
            leftOverMoveSpeed = Mathf.Max(0, value);
        }
        get
        {
            return leftOverMoveSpeed;
        }
    }

    [SerializeField, Tooltip("The unit attack range")]
    protected int range;
    public int m_Range
    {
        set
        {
            range = value;
        }
        get
        {
            return range;
        }
    }

    //personal

    [SerializeField, Tooltip("personal:the character like battle(0-100)")]
    protected int warlike;  //like attack
    public int m_Warlike
    {
        set
        {
            warlike = Mathf.Clamp(value, 0, MAXIMUM_PERSONALTY);
        }
        get
        {
            return warlike;
        }
    }

    [SerializeField, Tooltip("personal:the character like communicate(0-100)")]
    protected int emotional;  //like talk
    public int m_Emotional
    {
        set
        {
            emotional = Mathf.Clamp(value, 0, MAXIMUM_PERSONALTY);
        }
        get
        {
            return emotional;
        }
    }


    [SerializeField, Tooltip("personal:the character dislike dynamic act(0-100)")]
    protected int wary;  //dislike advace
    public int m_Wary
    {
        set
        {
            wary = Mathf.Clamp(value, 0, MAXIMUM_PERSONALTY);
        }
        get
        {
            return wary;
        }
    }
    
    public enum CHARACTER_CHARACTERISTIC
    {
        WARLIKE,
        EMOTIONAL,
        WARY,
        TOTAL_CHARACTERISTIC,
    };
    [Tooltip("What kind of characteristics!")]
    public CHARACTER_CHARACTERISTIC m_characterCharis;

    [Tooltip("The sprite of the character")]
    public Fungus.Character  charSprite;

    [Tooltip("Health bar")]
    public GameObject m_healthBar;

    // Use this for initialization
    void Start () {
        m_Morale = DEFAULT_MORALE;
        m_leftOverMoveSpeed = m_MoveSpeed;
        if (!damageEffect)
        {
            // If linking does not occur, find the tag!
            damageEffect = GameObject.FindGameObjectWithTag("ExplodeParticle").GetComponent<PerticleExplosion>();
        }
        if (!damageText)
        {
            // If linking does not occur, find the tag!
            damageText = GameObject.FindGameObjectWithTag("DamageText").GetComponent<DamageTextControl>();
        }
        if (m_Warlike > m_Emotional && m_Warlike > m_Wary)
        {
            m_characterCharis = CHARACTER_CHARACTERISTIC.WARLIKE;
        }
        else if (m_Emotional > m_Warlike && m_Emotional > m_Wary)
        {
            m_characterCharis = CHARACTER_CHARACTERISTIC.EMOTIONAL;
        }
        else if (m_Wary > m_Warlike && m_Wary > m_Emotional)
        {
            m_characterCharis = CHARACTER_CHARACTERISTIC.WARY;
        }
    }

    public void Attack(CharacterScript targetChara)
    {
        Debug.Log("attacking");
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
        targetChara.Damage(m_AttackDamage, m_AttackType);
    }

    public void Damage(int damage, TYPE_ATTACK damageType)
    {
        const int WEAKNESS_MULTIPLY = 2;
        float damageRate = 1.0f;

        //Damage set
        switch (m_AttackType)
        {
            case TYPE_ATTACK.GUNSHOT:
                if(damageType == TYPE_ATTACK.SNIPE || damageType == TYPE_ATTACK.CANNON)
                {
                    damageRate *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.ASSULT:
                if (damageType == TYPE_ATTACK.GUNSHOT || damageType == TYPE_ATTACK.CANNON)
                {
                    damageRate *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.SNIPE:
                if (damageType == TYPE_ATTACK.ASSULT)
                {
                    damageRate *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.CANNON:
                if (damageType == TYPE_ATTACK.SNIPE)
                {
                    damageRate *= WEAKNESS_MULTIPLY;
                }
                break;
            case TYPE_ATTACK.HEAL:
                //HEAL
                break;
        }
        if (damageText)
            damageText.PrintDamageValue((int)(damage * damageRate), this.gameObject);
        if (damageEffect)
            damageEffect.Explosion(this.gameObject.transform.position);
        //        Debug.Log((int)(damage * damageRateMorale));
        m_Hp -= (int)(damage * damageRate);
        // If there is a healthbar gameobject! because some scenes need testing.
        if (m_healthBar)
            m_healthBar.transform.localScale = new Vector3((float)m_Hp / m_MaximumHp,0.1f, 1.0f);
    }

    //Change moralle value for act. 
    //input: this action is balle?(0-100)
    //input: this action is communicate?(0-100)
    //input: this action is dynamic?(0-100)
    void MoraleFluctuate(int battle, int communicate, int dynamic)
    {
        // I dont understand what are these stuff. from AhHeng
        int moraleIncrement = 0;
        battle = Mathf.Clamp(battle, 0, MAXIMUM_PERSONALTY);
        communicate = Mathf.Clamp(communicate, 0, MAXIMUM_PERSONALTY);
        dynamic = Mathf.Clamp(dynamic, 0, MAXIMUM_PERSONALTY);

        //what is likes charactars
        moraleIncrement -= Mathf.Abs(battle - m_Warlike) - (MAXIMUM_PERSONALTY / 2);
        moraleIncrement -= Mathf.Abs(communicate - m_Emotional) - (MAXIMUM_PERSONALTY / 2);
        moraleIncrement += Mathf.Abs(dynamic - m_Wary) - (MAXIMUM_PERSONALTY / 2);

        //
        moraleIncrement /= 10;

        m_Morale += moraleIncrement;
    }

    public bool IsDead()
    {
        return m_Hp <= 0;
    }

    public void resetMoveSpeed()
    {
        m_leftOverMoveSpeed = m_MoveSpeed;
    }
}
