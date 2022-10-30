using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageHighscore : MonoBehaviour
{
    [SerializeField] TMP_Text stageParText;
    [SerializeField] int stageNumber;
    void Start()
    {
        string stageParKey = "Stage" + stageNumber;
        if (PlayerPrefs.HasKey(stageParKey))
        {
            stageParText.text = "Best Par: " + PlayerPrefs.GetInt(stageParKey);
        }
        else
        {
            stageParText.text = "Best Par: -";
        }
    }
}
