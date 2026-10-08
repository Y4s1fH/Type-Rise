using System;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    private bool hasNotified;
    void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        
        if (AchievmentData.Instance.isUnlocked == true && hasNotified == false)
        {
            hasNotified = true; // Qıfılı bağlayırıq ki, 2-ci dəfə bura girməsin!
            NotificationManager.Instance.ShowNotification(AchievmentData.Instance.GetCurrentName(), AchievmentData.Instance.GetCurrentIcon());
        }

        // Əgər tier artarsa (Claim edilərsə), qıfılı yenidən açırıq ki, növbəti mərhələdə yenə işləsin
        if (AchievmentData.Instance.isUnlocked == false)
        {
            hasNotified = false;
        }
    }
    public float GetAchievementProgress(string id)
    {
        if (GameManager.Instance == null) return 0f;

        switch (id)
        {
            case "1":
                return GameManager.Instance.TotalKeystrokes;
            case "2":
                return GameManager.Instance.totalmoney;
            default:
                return 0f;
        }
    }
    public void ApplyRewardAndAdvanceTier(AchievmentData data)
    {
        if (GameManager.Instance == null) return;
        float currentRewardValue = 0f;
        if (data.rewardTexts != null && data.currentTier < data.rewardTexts.Length)
        {
            currentRewardValue = Convert.ToInt32(data.rewardTexts[data.currentTier]);
        }

        switch (data.AchievmentId)
        {
            case "1":
                GameManager.Instance.totalcharsPerKeystroke += (int)currentRewardValue;
                break;

            case "2":
                GameManager.Instance.totalmoney += currentRewardValue;
                break;
        }

        data.currentTier++;
        if (data.currentTier < data.targetAmounts.Length)
        {
            data.isUnlocked = false;
        }
        else
        {
            data.isClaimed = true;
        }
    }
}