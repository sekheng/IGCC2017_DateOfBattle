using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class DialogueParser : MonoBehaviour {

    //  表示させる情報
    struct DialogueLine
    {
        public string m_name;
        public string m_content;
        public int m_pose;
        public string m_position;
        public string[] m_options;

        public DialogueLine(string name,string content,int pose,string position)
        {
            m_name = name;
            m_content = content;
            m_pose = pose;
            m_position = position;
            m_options = new string[0];
        }
    }

    List<DialogueLine> lines;

	// Use this for initialization
	void Start () {
        //  ファイル名
        string file = "Assets/Script/DIalog System/dialog_test";
        string sceneNum = SceneManager.GetActiveScene().name;
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".txt";

        lines = new List<DialogueLine>();

        //  ファイル読込み
        LoadDialogue(file);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //  読込み
    void LoadDialogue(string fileName)
    {
        string line;
        StreamReader r = new StreamReader(fileName);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] lineData = line.Split(';');
                    if (lineData[0] == "Player")
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], "", 0, "");
                        lineEntry.m_options = new string[lineData.Length - 1];

                        for (int i = 1; i < lineData.Length; i++)
                        {
                            lineEntry.m_options[i - 1] = lineData[i];
                        }
                        lines.Add(lineEntry);
                    }
                    else
                    {
                        DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3]);
                        lines.Add(lineEntry);
                    }
                }
            }
            while (line != null);
            r.Close();
        }
    }

    //  位置取得
    public string GetPosition(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].m_position;
        }
        return "";
    }

    //  名前取得
    public string GetName(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].m_name;
        }
        return "";
    }

    //  文取得
    public string GetContent(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].m_content;
        }
        return "";
    }

    //  キャラ姿勢取得
    public int GetPose(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].m_pose;
        }
        return 0;
    }

    //  オプション取得
    public string[] GetOptions(int lineNumber)
    {
        if (lineNumber < lines.Count)
        {
            return lines[lineNumber].m_options;
        }
        return new string[0];
    }

}
