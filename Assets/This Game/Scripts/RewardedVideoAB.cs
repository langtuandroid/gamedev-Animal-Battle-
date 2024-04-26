using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Serialization;


public class RewardedVideoAB : MonoBehaviour
{
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject reward;
    [SerializeField] private GameObject skipped;
    [SerializeField] private GameObject failed;
    [SerializeField] private bool showDialogues;

    [HideInInspector] [SerializeField] private int rewardedAdState = 0;

    private void Start()
    {
    }

    public void AskPermissionN()
    {
        areYouSure.SetActive(true);
    }


    public void ShowRewardedVideoO()
    {
    }


    private void HandleAdResultT()
    {
    }

    private void GiveRewardD()
    {
        reward.SetActive(true);
    }
}