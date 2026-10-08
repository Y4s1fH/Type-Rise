using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUIManager : MonoBehaviour
{
    private AchievmentData achievementdata;
    public static AchievementUIManager Instance;

    [Header("Detail Panel Elements")]
    public Image detailIcon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public RectTransform progressBarFillRect;
    public TextMeshProUGUI progressText;
    public Button claimButton;
    public TextMeshProUGUI claimButtonText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.allAchievements.Count > 0)
        {
            UpdateDetailPanel(GameManager.Instance.allAchievements[0]);
        }
    }

    private void Update()
    {
        if (achievementdata == null || achievementdata.isUnlocked || achievementdata.isClaimed) return;

        // Mərkəzi skriptdən real dəyəri alırıq
        float realTimeValue = AchievementManager.Instance.GetAchievementProgress(achievementdata.AchievmentId);

        // Hazırkı mərhələnin hədəfini (Tier Target) alırıq
        float target = achievementdata.GetCurrentTarget();

        if (achievementdata.currentAmount != realTimeValue)
        {
            achievementdata.currentAmount = realTimeValue;

            // Əgər hazırkı mərhələnin hədəfinə çatdısa, kilidi aç
            if (achievementdata.currentAmount >= target)
            {
                achievementdata.currentAmount = target;
                achievementdata.isUnlocked = true;
            }

            RefreshProgressBar();
        }
    }

    public void UpdateDetailPanel(AchievmentData data)
    {
        achievementdata = data;

        titleText.text = data.GetCurrentName();
        descriptionText.text = data.GetCurrentDescription();
        if (data.isUnlocked)
        {
            detailIcon.sprite =  data.unlockedIcon;
        }

        RefreshProgressBar();
    }

    private void RefreshProgressBar()
    {
        // Hazırkı mərhələnin hədəfini alırıq
        float target = achievementdata.GetCurrentTarget();
        float progressPercentage = 0f;

        if (target > 0)
        {
            progressPercentage = Mathf.Clamp01(achievementdata.currentAmount / target);
        }

        if (progressBarFillRect != null)
        {
            progressBarFillRect.anchorMin = new Vector2(0f, 0f);
            progressBarFillRect.anchorMax = new Vector2(progressPercentage, 1f);

            // UI DAŞMA XƏTASININ HƏLLİ
            progressBarFillRect.offsetMin = Vector2.zero;
            progressBarFillRect.offsetMax = Vector2.zero;
        }

        progressText.text = $"{achievementdata.currentAmount}/{target}";

        if (achievementdata.isUnlocked && !achievementdata.isClaimed)
        {
            claimButton.interactable = true;
            // Dinamik olaraq hazırkı mərhələnin mükafat yazısını göstəririk
            claimButtonText.text = $"+{achievementdata.GetCurrentRewardText()} {achievementdata.claiminfo}";
            detailIcon.sprite = achievementdata.unlockedIcon;
        }
        else
        {
            claimButton.interactable = false;
            // Tam bitibsə "CLAIMED", bitməyibsə növbəti mərhələnin yazısını göstər
            claimButtonText.text = achievementdata.isClaimed ? "CLAIMED" : $"+{achievementdata.GetCurrentRewardText()} {achievementdata.claiminfo}";
        }
        if (achievementdata.isUnlocked && !achievementdata.isClaimed)
        {
            claimButton.interactable = true;
            claimButtonText.text = $"+{achievementdata.GetCurrentRewardText()} {achievementdata.claiminfo}";
            // Kilid açılanda mərhələnin rəngli/yeni çərçivəli ikonunu göstər
            detailIcon.sprite = achievementdata.GetCurrentIcon();
        }
        else
        {
            claimButton.interactable = false;
            claimButtonText.text = achievementdata.isClaimed ? "CLAIMED" : $"+{achievementdata.GetCurrentRewardText()} {achievementdata.claiminfo}";
        }
    }

    public void ButtonClicked()
    {
        if (achievementdata != null && achievementdata.isUnlocked && !achievementdata.isClaimed)
        {
            // Mərkəzi skriptə xəbər veririk: Mükafatı ver və Mərhələni (Tier) artır!
            AchievementManager.Instance.ApplyRewardAndAdvanceTier(achievementdata);

            // UI-ı tam yeniləyirik (çünki ola bilsin hədəf 100-dən 500-ə qalxdı və bar yenidən boşaldı)
            UpdateDetailPanel(achievementdata);
        }
    }
}