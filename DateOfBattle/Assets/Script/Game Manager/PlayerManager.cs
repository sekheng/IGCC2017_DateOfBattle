using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PlayerManager : MonoBehaviour {
    [Header("References for linking in the editor")]
    [Tooltip("The player mouse input component")]
    public PlayerBattleMouse playerMouseInput;
    [Tooltip("The Button to end current player turn")]
    public GameObject m_endPlayerTurnScreen;
    [Tooltip("The flowchart of Fungus")]
    public Flowchart theConversationChart;
    [Tooltip("The Color for unit that has used up 1 of its turn")]
    public Color colorOfUsedUnit = new Color(1, 1, 1, 0.5f);
    [Tooltip("The message to send to start motivate unit dialogue")]
    public string m_MotivateDialogueStr = "Motivate";
    [Tooltip("The variable key for motivation value")]
    public string m_MotivationVarStr = "Motivation";
    [Tooltip("The variable key for Attack Type")]
    public string m_AttckTypeVarStr = "AttackType";

    [Header("Debugging References")]
    [Tooltip("The list of player unit gameobjects!")]
    public List<GameObject> m_playerGOList;
    [Tooltip("The list of player units that the player has yet to interact with!")]
    public List<GameObject> m_playerNotInteractGOList;
    [SerializeField, Tooltip("The flag to check whether the player has clicked on emptiness")]
    protected bool m_clickedFlag = false;
    [SerializeField, Tooltip("To check whether the player has finished conversing with the unit")]
    protected bool m_hasFinishedConversing;
    [SerializeField, Tooltip("The last player unit that has taken action!")]
    protected TileScript m_lastActionUnitTile;
    [SerializeField, Tooltip("What characteristic player has chosen")]
    protected CharacterScript.CHARACTER_CHARACTERISTIC m_PlayerChoseChar;

    /// <summary>
    /// So as to ensure it will jump out of the loop at the update of the unit's action!
    /// </summary>
    public bool ClickedFlagTrigger
    {
        set
        {
            m_clickedFlag = value;
        }
    }
    [Tooltip("To check whether the UI is blocking the raycast!")]
    public bool hasUIBlockRaycast = false;

    // The singleton for PlayerManager!
    public static PlayerManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        // Need to make sure only 1 PlayerManager exists!
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        m_playerGOList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        // When linking does not appear
        if (!theConversationChart)
            theConversationChart = FindObjectOfType<Flowchart>();
    }

    public IEnumerator updateOwnUnits()
    {
        // Clone the list!
        m_playerNotInteractGOList = new List<GameObject>(m_playerGOList);
        yield return new WaitForSeconds(1.0f);
        // If there is a previous unit, move the camera towards there!
        if (m_lastActionUnitTile)
            CameraMovement.Instance.MoveTowardsPosition(m_lastActionUnitTile.transform.position);
        else if (m_playerGOList.Count > 0)
        {
            // Randomly move the camera to any of the objects!
            CameraMovement.Instance.MoveTowardsPosition(m_playerGOList[0].transform.position);
        }
        while (m_playerNotInteractGOList.Count > 0)
        {
            CameraMovement.Instance.BeginCamFreeMovement();
            playerMouseInput.enabled = true;
            // Will wait every frame for the player to click on the tile which belongs to the player! Also making sure that the player has not interact with the tile before!
            while (playerMouseInput.playerClickedTile == null || playerMouseInput.playerClickedTile.tag != "Player" || (!m_playerNotInteractGOList.Contains(playerMouseInput.playerClickedTile.gameObject) && playerMouseInput.playerClickedTile.tag == "Player"))
            {
                yield return null;
            }
            CameraMovement.Instance.StopCamUpdateMovement();
            TileScript firstTileClicked = playerMouseInput.playerClickedTile;
            // Set it to null and prevent player from pressing!
            playerMouseInput.playerClickedTile = null;
            playerMouseInput.enabled = false;
            CharacterScript playerUnitStat = firstTileClicked.GetComponent<CharacterScript>();
            m_hasFinishedConversing = false;
            // set the motivation to be true then wait for it to be inactive!
            //displayMotivationScreen.SetActive(true);
            //while (displayMotivationScreen.activeSelf)
            //    yield return null;
            //string talkToWhatChar = playerUnitStat.m_AttackType.ToString() + "|" + playerUnitStat.m_characterCharis.ToString();
            // Try to talk to the character
            //theConversationChart.SendFungusMessage(talkToWhatChar);
            theConversationChart.SetIntegerVariable(m_MotivationVarStr, playerUnitStat.m_Motivation);
            theConversationChart.SetStringVariable(m_AttckTypeVarStr, playerUnitStat.m_AttackType.ToString());
            theConversationChart.SendFungusMessage(m_MotivateDialogueStr);
            while (!m_hasFinishedConversing)
            {
                yield return null;
            }
            // then get
            // Then we wait till the next tile that the player clicked on or maybe there is none!
            UnitFSM playerFSM = firstTileClicked.GetComponent<UnitFSM>();
            yield return null;
            playerUnitStat.m_Motivation = theConversationChart.GetIntegerVariable(m_MotivationVarStr);

            // Wait till the player clicked on a tile and it turns out to be the enemy or player clicked on the background and nothing is selected forsure!
            m_clickedFlag = false;
            //if (playerFSM.GetGenericState("DemoralizeState").interactWithState(m_PlayerChoseChar))
            if (playerFSM.GetGenericState("DemoralizeState").interactWithState(null))
            {
                // Only Successful interaction will mean being able to move the unit!
                m_endPlayerTurnScreen.SetActive(true);
            }
            playerMouseInput.enabled = true;
            // Wait for next frame
            yield return null;
            #region UNIT_ACTION
            while (m_endPlayerTurnScreen.activeSelf)
            {
                CameraMovement.Instance.BeginCamFreeMovement();
                // Wait for PlayerBattleMouse to send the event trigger!
                while (!m_clickedFlag && m_endPlayerTurnScreen.activeSelf)
                {
                    if (playerMouseInput.playerClickedTile && (playerMouseInput.playerClickedTile.tag == "Enemy" || playerMouseInput.playerClickedTile.tag == "EnemyFortress"))
                    {
                        // check whether the player is close to the enemy!
                        if (playerUnitStat.m_Range >= Vector3.Distance(playerUnitStat.transform.position, playerMouseInput.playerClickedTile.transform.position))
                        {
                            CharacterScript otherCharStat = playerMouseInput.playerClickedTile.GetComponent<CharacterScript>();
                            playerFSM.GetGenericState("AttackState").interactWithState(otherCharStat);
                            playerFSM.ChangeCurrentState("AttackState");
                            // If Player attack, this means the unit turn has ended
                            m_endPlayerTurnScreen.SetActive(false);
                            break;
                        }
                        else
                        {
                            playerMouseInput.playerClickedTile = null;
                        }
                    }
                    yield return null;
                }
                // Making sure the player clicked on an empty tile and their available movement tiles are more than 0.
                if (!hasUIBlockRaycast && !playerMouseInput.playerClickedTile && playerUnitStat.m_leftOverMoveSpeed > 0)
                {
                    // If successful interaction, then will move!
                    // Player pressed nothing, so move towards there!
                    if (playerFSM.GetGenericState("MoveState").interactWithState(playerMouseInput.playerMouseLastClickedPos))
                    {
                        playerFSM.ChangeCurrentState("MoveState");
                        // Then wait for the FSM to be finished updating!
                        yield return playerFSM.updateStateCoroutine;
                    }
                }
                //else
                {
                    // Reset the tile!
                    playerMouseInput.playerClickedTile = null;
                }
                CameraMovement.Instance.StopCamUpdateMovement();
                m_clickedFlag = false;
                yield return null;
            }
            #endregion
            // Unblock the raycast!
            hasUIBlockRaycast = false;
            if (firstTileClicked)
            {
                playerUnitStat.resetMoveSpeed();
                // Removed the already interacted gameobject!
                m_playerNotInteractGOList.Remove(firstTileClicked.gameObject);
                m_lastActionUnitTile = firstTileClicked;
                firstTileClicked.GetComponent<SpriteRenderer>().color = colorOfUsedUnit;
            }
            else
            {
                RemoveDestroyedUnit();
            }
        }
        CameraMovement.Instance.StopCamUpdateMovement();
        // Set it to be false!
        GameManager.Instance.isItPlayerTurn = false;
        // Then change all of the unit color back to normal
        foreach (GameObject unitGO in m_playerGOList)
        {
            SpriteRenderer unitSprRender = unitGO.GetComponent<SpriteRenderer>();
            unitSprRender.color = Color.white;
        }
        yield break;
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
        ObserverSystemScript.Instance.SubscribeEvent("PlayerClickedOnEmpty", playerMouseBattleClickedEmpty);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerClickedOnEmpty", playerMouseBattleClickedEmpty);
        StopCoroutine("updateOwnUnits");
    }

    /// <summary>
    /// The function to be called when a player unit died!
    /// </summary>
    protected void playerManagerUnitDied()
    {
        m_playerGOList.Remove(ObserverSystemScript.Instance.GetStoredEventVariable<GameObject>("PlayerUnitDied"));
        ObserverSystemScript.Instance.removeTheEventVariableNextFrame("PlayerUnitDied");
        if (m_playerGOList.Count <= 0)
            GameManager.Instance.playerLostDisplayAnimation();
    }

    /// <summary>
    /// To be guided by PlayerBattleMouse input that the player has pressed on emptiness
    /// </summary>
    protected void playerMouseBattleClickedEmpty()
    {
        m_clickedFlag = true;
    }

    public void MessageForFinishConverse()
    {
        m_hasFinishedConversing = true;
    }

    public void MakeItWarlike()
    {
        m_PlayerChoseChar = CharacterScript.CHARACTER_CHARACTERISTIC.WARLIKE;
    }

    public void MakeItEmotional()
    {
        m_PlayerChoseChar = CharacterScript.CHARACTER_CHARACTERISTIC.EMOTIONAL;
    }

    public void MakeItWary()
    {
        m_PlayerChoseChar = CharacterScript.CHARACTER_CHARACTERISTIC.WARY;
    }

    public void RemoveDestroyedUnit()
    {
        foreach (GameObject go in m_playerNotInteractGOList)
        {
            if (go == null)
            {
                m_playerNotInteractGOList.Remove(go);
                break;
            }
        }
    }
}
