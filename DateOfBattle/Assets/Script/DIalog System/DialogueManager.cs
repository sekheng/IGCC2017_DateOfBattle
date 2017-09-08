﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    DialogueParser m_parser;

    //  文、キャラ名
    public string m_dialogue, m_characterName;
    //  行番号
    public int m_lineName;
    //  姿勢
    int m_pose;
    //  位置
    string m_position;

    string[] m_options;
    public bool m_playerTalking;
    List<Button> m_buttons = new List<Button>();

    public Text m_dialogueBox;
    public Text m_nemeBox;
    public GameObject m_choiceBox;

	// Use this for initialization
	void Start () {
        //  初期化
        m_dialogue = "";
        m_characterName = "";
        m_pose = 0;
        m_position = "L";
        m_playerTalking = false;
        m_parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        m_lineName = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && m_playerTalking == false)
        {
            ShowDialogue();
            m_lineName++;
        }
        UpdateUI();
	}

    //  ダイアログの表示
    public void ShowDialogue()
    {
        ResetImages();
        ParseLine();
    }

    //  キャラのリセット
    void ResetImages()
    {
        //  キャラ名が無かったら
        if (m_characterName != "")
        {
            //  取得
            GameObject character = GameObject.Find(m_characterName);
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = null;
        }
    }

    //  文の表示
    void ParseLine()
    {
        //  プレイヤー以外
        if (m_parser.GetName(m_lineName) != "Player")
        {
            m_playerTalking = false;
            m_characterName = m_parser.GetName(m_lineName);
            m_dialogue = m_parser.GetContent(m_lineName);
            m_pose = m_parser.GetPose(m_lineName);
            m_position = m_parser.GetPosition(m_lineName);
            DisplayImages();
        }
        //  プレイヤー
        else
        {
            m_playerTalking = true;
            m_characterName = "";
            m_dialogue = "";
            m_pose = 0;
            m_position = "";
            m_options = m_parser.GetOptions(m_lineName);
            CreateButtons();
        }
    }

    //  文字を表示
    void DisplayImages()
    {
        if (m_characterName != "")
        {
            //  キャラオブジェクの取得
            GameObject character = GameObject.Find(m_characterName);
            SetSpritePositions(character);

            //  キャラスプライトの取得
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = character.GetComponent<Character>().characterPoses[m_pose];
        }
    }

    //  スプライトの向きをセット
    void SetSpritePositions(GameObject spriteObj)
    {
        if (m_position == "L")
        {
            spriteObj.transform.position = new Vector3(-6, 0);
        }
        else if (m_position == "R")
        {
            spriteObj.transform.position = new Vector3(6, 0);
        }
        spriteObj.transform.position = new Vector3(spriteObj.transform.position.x, spriteObj.transform.position.y, 0);
    }

    //  選択ボタン作成
    void CreateButtons()
    {
        for (int i = 0; i < m_options.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(m_choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            cb.SetText(m_options[i].Split(':')[0]);
            cb.m_option = m_options[i].Split(':')[1];
            cb.m_box = this;
            b.transform.SetParent(this.transform);
            b.transform.localPosition = new Vector3(0, -25 + (i * 50));
            b.transform.localScale = new Vector3(1, 1, 1);
            m_buttons.Add(b);
        }
    }

    //  UIの更新
    void UpdateUI()
    {
        if(!m_playerTalking)
        {
            ClearButtons();
        }
        m_dialogueBox.text = m_dialogue;
        m_nemeBox.text = m_characterName;
    }

    //  ボタンの削除
    void ClearButtons()
    {
        for (int i = 0; i < m_buttons.Count; i++)
        {
            print("Clearing buttons");
            Button b = m_buttons[i];
            m_buttons.Remove(b);
            Destroy(b.gameObject);
        }
    }

}