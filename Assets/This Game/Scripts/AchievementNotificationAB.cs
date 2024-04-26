using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AchievementNotificationAB : MonoBehaviour
{
    [SerializeField] private GameObject achievementBox;
    [SerializeField] private Text notificationTextT;
    [SerializeField] private int totalAchievements;

    private int _i = 0;

    private int _completedD = 0;

    // Update is called once per frame
    private void FixedUpdate()
    {
        _i++;
        if (_i > 4)
        {
            _i = 0;
            CheckUpgradeE();
        }
    }

    private void CheckUpgradeE()
    {
        _completedD = 0;
        for (int id = 0; id < totalAchievements; id++)
        {
            if (PlayerPrefs.GetInt("Achievement" + id, 0) > 0)
                _completedD++;
        }

        notificationTextT.text = _completedD + " / " + totalAchievements;
    }

    public void OpenAchievemntsS()
    {
        achievementBox.SetActive(true);
    }
}