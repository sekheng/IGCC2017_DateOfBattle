using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class DialogController : MonoBehaviour {
    private VD m_videData = null;

    // Use this for initialization
    void Start () {
        //  VIDE生成
        //gameObject.AddComponent<VD>();
        m_videData = GetComponent<VD>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDisable()
    {
        //  VIDEの削除
        VD.EndDialogue();
    }

    //  テキストの表示
    void OnGUI()
    {
        if (VD.isActive)
        {
            var data = VD.nodeData;
            if (data.isPlayer)
            {
                for (int i = 0; i < data.comments.Length; i++)
                {
                    //  ボタンが押されたら
                    if (GUILayout.Button(data.comments[i]))
                    {
                        //  次に移動
                        data.commentIndex = i;
                        VD.Next();
                    }
                }
            }
            else
            {
                GUILayout.Label(data.comments[data.commentIndex]);

                if (GUILayout.Button("PUSH"))
                {
                    VD.Next();
                }
            }
            if (data.isEnd)
            {
                //  VIDEの削除
                VD.EndDialogue();
            }
        }
        else
        {
            if (GUILayout.Button("ReturnStart"))
            {
                //  VIDE_Assign読込み
                VD.BeginDialogue(GetComponent<VIDE_Assign>());
            }
        }
    }

    ////  初期化
    //void Initialize()
    //{
    //    m_npcText = GameObject.Find("NPCText").GetComponent<Text>();
    //    m_npcName = GameObject.Find("NPCLabel").GetComponent<Text>();
    //    m_playerText = GameObject.Find("playerContainer").GetComponent<Text>();
    //    m_itemPopUp = GameObject.Find("itemContainer");
    //    m_uiContainer = GameObject.Find("TextContainer");

    //    m_NPCSprite = GameObject.Find("NPCImage").GetComponent<Image>();
    //    m_PlayerSprite = GameObject.Find("PlayerImage").GetComponent<Image>();


    //    GameObject.Find("itemContainer").SetActive(false);
    //    GameObject.Find("playerContainer").SetActive(false);
    //    GameObject.Find("NPCText").transform.parent.gameObject.SetActive(false);
    //    GameObject.Find("TextContainer").SetActive(false);

    //}

}
