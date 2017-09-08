using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour {

    public string m_option;
    public DialogueManager m_box;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //  ボタンテキスのセット
    public void SetText(string newText)
    {
        this.GetComponent<Text>().text = newText;
    }

    //  オプションのセット
    public void SetOption(string newOption)
    {
        this.m_option = newOption;
    }

    //  オプションの分解
    public void ParseOption()
    {
        string command = m_option.Split(',')[0];
        string commandModifier = m_option.Split(',')[1];
        m_box.m_playerTalking = false;

        if(command=="line")
        {
            m_box.m_lineName = int.Parse(commandModifier);
            m_box.ShowDialogue();
        }
        else if(command=="scene")
        {
            Application.LoadLevel("scene" + commandModifier);
        }
    }

}
