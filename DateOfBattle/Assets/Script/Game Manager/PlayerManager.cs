using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [Header("References for linking in the editor")]
    [Tooltip("The player mouse input component")]
    public PlayerBattleMouse playerMouseInput;
    [Tooltip("The display to the motivation stuff through dialogue system. Will be changed soon! This is just a placeholder")]
    public GameObject displayMotivationScreen;
    [Tooltip("The Button to end current player turn")]
    public GameObject m_endPlayerTurnScreen;

    [Header("Debugging References")]
    [Tooltip("The list of player unit gameobjects!")]
    public List<GameObject> m_playerGOList;
    [Tooltip("The list of player units that the player has yet to interact with!")]
    public List<GameObject> m_playerNotInteractGOList;
    [SerializeField, Tooltip("The flag to check whether the player has clicked on emptiness")]
    protected bool m_clickedFlag = false;
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
    }

    public IEnumerator updateOwnUnits()
    {
        // Clone the list!
        m_playerNotInteractGOList = new List<GameObject>(m_playerGOList);
        while (m_playerNotInteractGOList.Count > 0)
        {
            // Will wait every frame for the player to click on the tile which belongs to the player! Also making sure that the player has not interact with the tile before!
            while (playerMouseInput.playerClickedTile == null || playerMouseInput.playerClickedTile.tag != "Player" || (!m_playerNotInteractGOList.Contains(playerMouseInput.playerClickedTile.gameObject) && playerMouseInput.playerClickedTile.tag == "Player"))
            {
                yield return null;
            }
            TileScript firstTileClicked = playerMouseInput.playerClickedTile;
            // Set it to null and prevent player from pressing!
            playerMouseInput.playerClickedTile = null;
            playerMouseInput.enabled = false;

            // set the motivation to be true then wait for it to be inactive!
            displayMotivationScreen.SetActive(true);
            while (displayMotivationScreen.activeSelf)
                yield return null;

            // Then we wait till the next tile that the player clicked on or maybe there is none!
            UnitFSM playerFSM = firstTileClicked.GetComponent<UnitFSM>();
            CharacterScript playerUnitStat = firstTileClicked.GetComponent<CharacterScript>();
            playerMouseInput.enabled = true;
            // Wait for next frame
            yield return null;
            m_endPlayerTurnScreen.SetActive(true);
            // Wait till the player clicked on a tile and it turns out to be the enemy or player clicked on the background and nothing is selected forsure!
            m_clickedFlag = false;
#region UNIT_ACTION
            while (m_endPlayerTurnScreen.activeSelf)
            { 
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
                    // Player pressed nothing, so move towards there!
                    playerFSM.GetGenericState("MoveState").interactWithState(playerMouseInput.playerMouseLastClickedPos);
                    playerFSM.ChangeCurrentState("MoveState");
                    // Then wait for the FSM to be finished updating!
                    yield return playerFSM.updateStateCoroutine;
                }
                else
                {
                    // Reset the tile!
                    playerMouseInput.playerClickedTile = null;
                }
                m_clickedFlag = false;
                yield return null;
            }
            #endregion
            // Unblock the raycast!
            hasUIBlockRaycast = false;
            playerUnitStat.resetMoveSpeed();
            // Removed the already interacted gameobject!
            m_playerNotInteractGOList.Remove(firstTileClicked.gameObject);
        }
        // Set it to be false!
        GameManager.Instance.isItPlayerTurn = false;
        yield break;
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
        ObserverSystemScript.Instance.SubscribeEvent("PlayerClickedOnEmpty", playerMouseBattleClickedEmpty);
        ObserverSystemScript.Instance.SubscribeEvent("GameOver", stopUpdateCoroutine);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerClickedOnEmpty", playerMouseBattleClickedEmpty);
        ObserverSystemScript.Instance.UnsubscribeEvent("GameOver", stopUpdateCoroutine);
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

    /// <summary>
    /// Stopped the current coroutine update so as to prevent bugs!
    /// </summary>
    protected void stopUpdateCoroutine()
    {
        // Stop own update coroutine unless someone messed it up by starting the coroutine in other scripts!
        StopCoroutine("updateOwnUnits");
    }
}
