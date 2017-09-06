using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class DialogController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        //  VIDE生成
        gameObject.AddComponent<VD>();
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

}
