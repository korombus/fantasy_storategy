using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ScenarioData {
    private string _fileName;
    private TextAsset _scenario;   //!< シナリオ本文
    public bool IsFirst;
    public bool IsRead;            //!< 既に読まれているかどうか

    public string FileName { get { return _fileName; } }
    public TextAsset Scenario   { get { return _scenario; } }

    public ScenarioData(string name, TextAsset i_scenario, bool i_first, bool i_read) {
        _fileName = name;
        _scenario = i_scenario;
        IsFirst = i_first;
        IsRead = i_read;
    }
}
/*
public class ScenarioDebugData
{
    private Stage _stage;          //!< 読まれるステージ
    private string _scenario;   //!< シナリオ本文
    public bool IsRead;            //!< 既に読まれているかどうか

    public Stage Stage { get { return _stage; } }
    public string Scenario { get { return _scenario; } }

    public ScenarioDebugData(Stage i_state, string i_scenario, bool i_read) {
        _stage      = i_state;
        _scenario   = i_scenario;
        IsRead      = i_read;
    }
}
*/