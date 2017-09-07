using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogShow : MonoBehaviour {

    //  ダイアログのプレファブ
    public Canvas canvasDialogPrefab = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //  ボタンが押された
    public void OnClick()
    {
        Debug.Log("OnClick!!");
        //  ダイアログの表示
        Instantiate(canvasDialogPrefab);
    }
    
}
