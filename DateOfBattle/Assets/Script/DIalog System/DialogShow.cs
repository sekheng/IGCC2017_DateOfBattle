using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogShow : MonoBehaviour {

    //  ダイアログのプレファブ
    public Canvas canvasDialogPrefab = null;

    public bool endLine = false;

    // Use this for initialization
    void Start () {
        endLine = false;
	}
	
	// Update is called once per frame
	void Update () {
        endLine = GameObject.Find("Panel").GetComponent<DialogueManager>().IsEndLine();

        //  会話が終了したら
        if (endLine) 
        {
            Debug.Log("End Line");
            //DestroyImmediate(canvasDialogPrefab,true);
            DestroyImmediate(GameObject.Find("Image"));
            DestroyImmediate(GameObject.Find("Panel"));
        }
	}

    //  ボタンが押された
    public void OnClick()
    {
        Debug.Log("OnClick!!");
        //  ダイアログの表示
        Instantiate(canvasDialogPrefab);
    }
    
}
