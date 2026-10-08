using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievmentData : MonoBehaviour
{
    public static AchievmentData Instance;
    public string AchievmentId;
    public string [] AchievmentName;
    [TextArea(3, 5)]
    public string [] AchievmentDescription;
    public Sprite[] tierIcons;
    public Sprite unlockedIcon;
    public string[] rewardTexts;
    public float[] targetAmounts;
    public int currentTier = 0;
    public float currentAmount;
    public string rewardText;
    public bool isUnlocked = false;
    public bool isClaimed=false;
    public Image gridIcon;
    public string claiminfo;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (GameManager.Instance != null && !GameManager.Instance.allAchievements.Contains(this))
        {
            GameManager.Instance.allAchievements.Add(this);
        }
    }
    void Update()
    {
        UpdateMyGridIcon();
    }

    public void UpdateMyGridIcon()
    {
        if (gridIcon != null)
        {
            if (isUnlocked || isClaimed)
            {
                gridIcon.sprite = GetCurrentIcon();
            }
                

        }
    }
    public float GetCurrentTarget()
    {
        if (targetAmounts == null || targetAmounts.Length == 0) return 1f;
        if (currentTier < targetAmounts.Length) return targetAmounts[currentTier];
        return targetAmounts[targetAmounts.Length - 1];
    }
    public string GetCurrentRewardText()
    {
        if (rewardTexts == null || rewardTexts.Length == 0) return "No Reward";
        if (currentTier < rewardTexts.Length) return rewardTexts[currentTier];
        return "MAX";
    }
    public string GetCurrentName()
    {
        if (AchievmentName== null || AchievmentName.Length == 0) return "Name Missing";
        if (currentTier < AchievmentName.Length) return AchievmentName[currentTier];
        return AchievmentName[AchievmentName.Length - 1];
    }
    public string GetCurrentDescription()
    {
        if (AchievmentDescription == null || AchievmentDescription.Length == 0) return "Desc Missing";
        if (currentTier < AchievmentDescription.Length) return AchievmentDescription[currentTier];
        return AchievmentDescription[AchievmentDescription.Length - 1];
    }
    public Sprite GetCurrentIcon()
    {
        if (tierIcons == null || tierIcons.Length == 0) return null;
        if (currentTier < tierIcons.Length) return tierIcons[currentTier];
        return tierIcons[tierIcons.Length - 1];
    }
}
