using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour {

    public Image profileImage1, profileImage2, rankImage;
    public Text nameText1, nameText2, rankText1, rankText2;
    public Text rankingText, levelsCompletedText, mostKillsText, timePlayedText;
    public Text roundsPlayedText, totalKillsText, KDAText;
    [Space(10)]
    public InputField NameInput;
    public Sprite[] avatars,rankSprites;
    public Image[] avatarBorders;
    int selectedAvatar;
    string rankstring = "UnRanked";

    [Space(10)]
    public string playerName;
    public int rank, ranking, levelsCompleted, mostKills, timePlayed, roundsPlayed, tKills;
    float kda;

    [Space(10)]
    public GameObject StatsBox, ChangenameBox;

    
    // Use this for initialization
    void Start () {

        LoadStats();
        if (PlayerPrefs.GetInt("firstTime", 0) < 1)
        {
            PlayerPrefs.SetInt("firstTime", 2);
            PlayerPrefs.Save();
            ChangenameBox.SetActive(true);
        }

    }
	
	

    public void SelectAvatar(int id)
    {
        selectedAvatar = id;
        foreach (Image i in avatarBorders)
        {
            i.color = Color.white;
        }

        avatarBorders[selectedAvatar].color = Color.yellow;
        PlayerPrefs.SetInt("SelectedAvatar",selectedAvatar);
        PlayerPrefs.Save();
    }

    public void SaveName()
    {
        PlayerPrefs.SetString("PlayerName",NameInput.text);
        PlayerPrefs.Save();
        LoadStats();
    }

    public void LoadStats()
    {
        selectedAvatar = PlayerPrefs.GetInt("SelectedAvatar",0);
        SelectAvatar(selectedAvatar);
        profileImage1.sprite = avatars[selectedAvatar];
        profileImage2.sprite = avatars[selectedAvatar];


        playerName = PlayerPrefs.GetString("PlayerName");
        nameText1.text = playerName;
        nameText2.text = playerName;

        ranking = PlayerPrefs.GetInt("Rankings",0);
        CalculateRank();
        //rankText1.text = rankstring;
        rankText2.text = "Rank: " + rankstring;
        rankingText.text = "Rating: "+ranking;
        mostKills = PlayerPrefs.GetInt("MostKills",0);
        mostKillsText.text = "" + mostKills;

        levelsCompleted = PlayerPrefs.GetInt("ulevels",1)-1;
        levelsCompletedText.text = "" + levelsCompleted;

        tKills = PlayerPrefs.GetInt("TotalKills",0);
        totalKillsText.text = "" + tKills;

        roundsPlayed = PlayerPrefs.GetInt("RoundsPlayed",0);
        roundsPlayedText.text = "" + roundsPlayed;

        if (tKills > 0)
        {
            kda = tKills / roundsPlayed;
        }
        else
            kda = 0.00f;

        KDAText.text = "" + kda.ToString("F2");

        timePlayed = PlayerPrefs.GetInt("TotalTime",0);
        ShowTime();
    }

    void CalculateRank()
    {
        if (ranking < 1000)
        {
            rankstring = "Unranked";
            rankImage.sprite = rankSprites[0];
        }
        else if (ranking < 2000)
        {
            rankstring = "Bronze";
            rankImage.sprite = rankSprites[1];
        }
        else if (ranking < 3500)
        {
            rankstring = "Silver";
            rankImage.sprite = rankSprites[2];
        }
        else 
        {
            rankstring = "Gold";
            rankImage.sprite = rankSprites[3];
        }
    }

    void ShowTime()
    {
        int s = timePlayed;
        if (s <= 0)
            s = 10;

        string r = "";
        r += (s / 3600).ToString() + " : ";
        s -= ((int)s / 3600) * 3600;
        r += (s / 60).ToString("00") + " : ";
        r += (s % 60).ToString("00") + "";

        timePlayedText.text = r;
    }

    public void ChangeName()
    {
        NameInput.text = "" + playerName;
        ChangenameBox.SetActive(true);

    }

    

    

}
