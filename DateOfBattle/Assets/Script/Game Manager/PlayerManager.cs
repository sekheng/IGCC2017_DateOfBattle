using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    [Header("References for linking in the editor")]
    [Tooltip("The player mouse input component")]
    public PlayerBattleMouse playerMouseInput;
    [Tooltip("The display to the motivation stuff through dialogue system. Will be changed soon! This is just a placeholder")]
    public GameObject displayMotivationScreen;

    [Header("Debugging References")]
    [Tooltip("The list of player unit gameobjects!")]
    public List<GameObject> m_playerGOList;
    [Tooltip("The list of player units that the player has yet to interact with!")]
    public List<GameObject> m_playerNotInteractGOList;

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
            // Will wait every frame for the player to click on the tile which belongs to the player!
            while (playerMouseInput.playerClickedTile == null || playerMouseInput.playerClickedTile.tag != "Player")
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
            CharacterScript playerUnitState = firstTileClicked.GetComponent<CharacterScript>();
            playerMouseInput.enabled = true;
            // Wait till the player clicked on a tile and it turns out to be the enemy or player clicked on the background and nothing is selected for sure!
            while ((playerMouseInput.playerClickedTile && playerMouseInput.playerClickedTile.tag == "Enemy") || (playerMouseInput.playerClickedTile == null && Input.GetMouseButtonDown(0)))
            {
                if (playerMouseInput.playerClickedTile && playerMouseInput.playerClickedTile.tag == "Enemy")
                {
                    // jump out of loop and check whether the player is close to the enemy!

                    break;
                }
                yield return null;
            }

            if (!playerMouseInput.playerClickedTile)
            {
                // Player pressed nothing, so move towards there!
                UnitFSM playerFSM = firstTileClicked.GetComponent<UnitFSM>();
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
            // Removed the already interacted gameobject!
            m_playerNotInteractGOList.Remove(firstTileClicked.gameObject);
            yield return null;
        }
        // Set it to be false!
        GameManager.Instance.isItPlayerTurn = false;
        yield break;
    }

    private void OnEnable()
    {
        ObserverSystemScript.Instance.SubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
    }

    private void OnDisable()
    {
        ObserverSystemScript.Instance.UnsubscribeEvent("PlayerUnitDied", playerManagerUnitDied);
    }

    /// <summary>
    /// The function to be called when a player unit died!
    /// </summary>
    protected void playerManagerUnitDied()
    {
        // there is no time to code a perfect system so looping it is fine!
        foreach (GameObject go in m_playerGOList)
        {
            if (go == null)
            {
                m_playerGOList.Remove(go);
                break;
            }
        }
        if (m_playerGOList.Count <= 0)
            GameManager.Instance.playerLostDisplayAnimation();
    }
}
