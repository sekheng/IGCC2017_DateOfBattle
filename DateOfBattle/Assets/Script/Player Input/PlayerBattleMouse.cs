using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// To be used at selection of unit during battle.
/// Only enabled during player's turn!
/// </summary>
public class PlayerBattleMouse : MonoBehaviour {
    [Header("Values and References needed for PlayerBattleMouse")]
    [Tooltip("The amount of time to wait for the temp click to disappear")]
    public float m_TempClickTimer = 0.5f;
    [Tooltip("The temporary click stuff of prefab")]
    public GameObject selectUI;
    [Tooltip("The prefab that has clear indication of the object being pressed")]
    public GameObject playerUnitSelectUI;

    [Header("Debugging References")]
    [Tooltip("This is to know which Tile the player has clicked on. Do not mess with it!")]
    public TileScript playerClickedTile;
    [Tooltip("The Position which the player clicked on!")]
    public Vector2 playerMouseLastClickedPos;
    [Tooltip("The actual gameobject of the playerUnitSelect because the gameobject tends to get destroyed")]
    public GameObject playerSelectGO;
    [Tooltip("The actual gameobject of temp select")]
    public GameObject tempSelectGO;

    Coroutine m_TimeCounterRoutine;

    // Update is called once per frame
    //   void Update () {
    //       // If player pressed the mouse button down during the 1 frame
    //       if (Input.GetMouseButtonDown(0))
    //       {
    //           // Get the mouse to world coordinate. so simper.
    //           Vector2 mouseWorldCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //           // Use overlap point since there is no raycast2D from MouseWorld Coordinate to collider
    //           RaycastHit2D hit2D = Physics2D.Linecast(mouseWorldCoord, mouseWorldCoord);
    //           if (!hit2D)
    //           { 
    //               if (playerClickedTile && playerClickedTile.GetComponent<UnitFSM>().GetGenericState("MoveState").interactWithState(mouseWorldCoord))
    //               {
    //                   playerClickedTile.GetComponent<UnitFSM>().ChangeCurrentState("MoveState");
    //                   playerClickedTile = null;
    //               }
    //               return;
    //           }
    //           TileScript theSelectedTile = hit2D.collider.GetComponent<TileScript>();
    //           if (theSelectedTile)
    //           {
    //               // TODO: this need more changes to it!
    //               // If player clicked tile happens to belong to the player, then can move the tile!
    //               if (playerClickedTile && playerClickedTile.tag == "Player" && theSelectedTile != playerClickedTile && theSelectedTile.tag != "Unwalkable")
    //               {
    //                   // If the interaction is successful, then change the state
    //                   if (playerClickedTile.GetComponent<UnitFSM>().GetGenericState("MoveState").interactWithState(theSelectedTile))
    //                   {
    //                       playerClickedTile.GetComponent<UnitFSM>().ChangeCurrentState("MoveState");
    //                       playerClickedTile = null;
    //                   }
    //               }
    //               else
    //               {
    //                   playerClickedTile = theSelectedTile;
    //                   ObserverSystemScript.Instance.TriggerEvent("PlayerClickedUnit");
    //               }
    //           }
    //       }
    //}

    private void Update()
    {
        // If player pressed the mouse button down during the 1 frame
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse to world coordinate. so simper.
            playerMouseLastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Use overlap point since there is no raycast2D from MouseWorld Coordinate to collider
            RaycastHit2D hit2D = Physics2D.Linecast(playerMouseLastClickedPos, playerMouseLastClickedPos);
            if (hit2D)
            {
                playerClickedTile = hit2D.collider.GetComponent<TileScript>();
                if (selectUI)
                    MakeSelectUI();
            }
            else
            {
                ObserverSystemScript.Instance.TriggerEvent("PlayerClickedOnEmpty");
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// To visualize what the player has clicked upon!
    /// </summary>
    private void OnDrawGizmos()
    {
        if (playerClickedTile)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(playerClickedTile.transform.position, 1);
        }
    }

    /// <summary>
    /// When clicked, the selection UI appears on that object.
    /// クリックすると、そのオブジェクトに選択UIを表示する
    /// </summary>
    private void MakeSelectUI()
    {
        Debug.Log("Select");
        //  生成するオブジェクトの座標設定
        //selectUI.transform.position = playerClickedTile.transform.position;
        //GameObject UIobj = Instantiate(selectUI);
        ////selectUI.SetActive(false);

        ////  指定された時間にオブジェクトを消す
        //Destroy(UIobj, 2.0f);
        if (!tempSelectGO)
            tempSelectGO = Instantiate(selectUI);
        tempSelectGO.SetActive(true);
        tempSelectGO.transform.position = playerClickedTile.transform.position;
        tempSelectGO.transform.SetParent(playerClickedTile.transform);
        if (m_TimeCounterRoutine != null)
            StopCoroutine(m_TimeCounterRoutine);
        m_TimeCounterRoutine = StartCoroutine(UpdateForTempClickDisappear());
    }

    IEnumerator UpdateForTempClickDisappear()
    {
        yield return new WaitForSeconds(m_TempClickTimer);
        tempSelectGO.transform.SetParent(null);
        tempSelectGO.SetActive(false);
        m_TimeCounterRoutine = null;
        yield break;
    }

    public void SetUnitIndicatorPermanent(Transform unitTransform)
    {
        if (unitTransform)
        {
            // If no unit select indicator, then instantiate it!
            if (!playerSelectGO)
                playerSelectGO = Instantiate(playerUnitSelectUI);
            playerSelectGO.SetActive(true);
            playerSelectGO.transform.position = unitTransform.position;
            playerSelectGO.transform.SetParent(unitTransform);
        }
        else
        {
            if (playerSelectGO)
            {
                // Which means set it to be inactive!
                playerSelectGO.transform.SetParent(null);
                playerSelectGO.SetActive(false);
            }
        }
    }

    //private void SelectTracking()
    //{
    //    selectUI.transform.position = playerClickedTile.transform.position;
    //}

#endif
}
