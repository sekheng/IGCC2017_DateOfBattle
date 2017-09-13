using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will handle the entire game flow.
/// Unfortunately, no time to code proper game rules. LOL.
/// </summary>
public class GameManager : MonoBehaviour {
    [Header("The References to call and link in Unity!")]
    [Tooltip("Need to stop the player input in order to prevent bugs!")]
    public PlayerBattleMouse m_playerMouseInput;
    [Tooltip("Display who turn is it screen")]
    public Text displayTurnText;
    [Tooltip("The Flowchart to call")]
    public Fungus.Flowchart m_dialogueFlowchart;

    [Header("Debugging References and they will be automatically linked inside codes. Do not touch!")]
    [SerializeField,Tooltip("The flag to show whose turn is it!")]
    protected bool m_isItPlayerTurn = true;
    /// <summary>
    /// To be set by the Unity inspector to toggle whose turn to move!
    /// </summary>
    public bool isItPlayerTurn
    {
        set
        {
            // Need to make sure the value is not the same!
            m_isItPlayerTurn = value;
            // Begin updating of the manger depending on the flag!
            switch(m_isItPlayerTurn)
            {
                case false:
                    if (displayTurnText)
                        displayTurnText.text = "Enemy turn";
                    EnemyAIManager.Instance.StartCoroutine(EnemyAIManager.Instance.updateOwnUnits());
                    // Disable the inputs!
                    m_playerMouseInput.enabled = false;
                    break;
                default:
                    if (displayTurnText)
                        displayTurnText.text = "Player turn";
                    // Enable the inputs!
                    m_playerMouseInput.enabled = true;
                    PlayerManager.Instance.StartCoroutine(PlayerManager.Instance.updateOwnUnits());
                    break;
            }
        }
        get
        {
            return m_isItPlayerTurn;
        }
    }

    // The GameManager Singleton!
    public static GameManager Instance
    {
        get; private set;
    }

    /// <summary>
    /// Need to set up the singleton for GameManager.
    /// </summary>
    private void Awake()
    {
        // If this instance has already exists, just destroy it!
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start () {
        // Since the intro will play, just set it to be active regardless what!
        //m_introDisplayGO.SetActive(true);
        if (!m_dialogueFlowchart)
            m_dialogueFlowchart = FindObjectOfType<Fungus.Flowchart>();
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.SubscribeEvent("EnemyFortressLost", enemyLostFortress);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerFortressLost", playerLostFortress);
        ObserverSystemScript.Instance.UnsubscribeEvent("EnemyFortressLost", enemyLostFortress);
    }


    /// <summary>
    /// Display the losing screen and restart everything if necessary!
    /// There is no need to know if there are any fortress there since other scripts will be calling it!
    /// </summary>
    protected void playerLostFortress()
    {
        playerLostDisplayAnimation();
    }

    /// <summary>
    /// Display the winning screen and proceed to the next level if there is any!
    /// There is no need to know if there are any fortress there since other scripts will be calling it!
    /// </summary>
    protected void enemyLostFortress()
    {
        playerWonDisplayAnimation();
    }

    /// <summary>
    /// Display flashy effects if player won.
    /// Hopefully got time to implement.
    /// </summary>
    public void playerWonDisplayAnimation()
    {
        m_dialogueFlowchart.SendFungusMessage("WIN");
        // Need to ensure that the mouse input from the player is disabled to prevent further bugs
        m_playerMouseInput.enabled = false;
        // Trigger game over event since it is over!
        ObserverSystemScript.Instance.TriggerEvent("GameOver");
    }

    /// <summary>
    /// Display flashy effects if player lost.
    /// Hopefully got time to implement.
    /// </summary>
    public void playerLostDisplayAnimation()
    {
        m_dialogueFlowchart.SendFungusMessage("LOSE");
        // Need to ensure that the mouse input from the player is disabled to prevent further bugs
        m_playerMouseInput.enabled = false;
        // Trigger game over event since it is over!
        //ObserverSystemScript.Instance.TriggerEvent("GameOver");
        PlayerManager.Instance.enabled = false;
        EnemyAIManager.Instance.enabled = false;
    }

    /// <summary>
    /// A simple function to be called when beginning the battle!
    /// </summary>
    void BeginBattle()
    {
        // Have to hardcode this part as there is no time to do other parts.
        isItPlayerTurn = false;
    }
}
