using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct RankMilestone
{
    public int requiredLevel; 
    public string rankName;
    public Sprite rankIcon;
}

public class LevelManager : MonoBehaviour
{
    public GameObject levelUpFloatingTextPrefab; 
    public Transform canvasTransform;
    public static LevelManager Instance;
    public RectTransform xpFillRect;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rankTitleText;
    public TextMeshProUGUI xpText;
    public Image LevelIcon;
    public int currentLevel = 1;
    public float currentXP = 0f;
    public float requiredXP = 10f;
    public float xpMultiplier = 2f;
    public float xpPerType = 1f;
    private Dictionary<int, string> rankDictionary = new Dictionary<int, string>()
    {
        { 1,   "The Blank Page" }, 
        { 2,   "The Pen Holder" }, 
        { 4,   "Ink Specialist" }, 
        { 7,   "Typing Novices" }, 
        { 10,  "Speedy Fingers" }, 
        { 15,  "Keyboard Kings" }, 
        { 20,  "Swift Scrawler" }, 
        { 25,  "Word Architect" }, 
        { 35,  "Text Tactician" },
        { 45,  "Master of Inks" }, 
        { 55,  "Grand Scripter" }, 
        { 65,  "Letter Maestro" }, 
        { 75,  "Legendary Pens" }, 
        { 85,  "Typing Legends" },
        { 100, "Supreme Scribe" }  
    };
    public List<Sprite> rankIcons = new List<Sprite>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddXP(float amount)
    {
        currentXP += amount;

        if (currentXP >= requiredXP)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= requiredXP;
        currentLevel++;
        requiredXP = Mathf.Round(requiredXP * xpMultiplier);
        if (levelUpFloatingTextPrefab != null && canvasTransform != null)
        {
            GameObject floatText = Instantiate(levelUpFloatingTextPrefab, canvasTransform);
            TextMeshProUGUI tmp = floatText.GetComponent<TextMeshProUGUI>();
            tmp.transform.SetAsFirstSibling();
            if (tmp != null)
            {
                tmp.text ="LEVEL UP!";
            }
        }
    }

    private void UpdateUI()
    {
        if (levelText != null) levelText.text = NumberFormatter.FormatValue(currentLevel);
        if (xpText != null) xpText.text = $"{NumberFormatter.FormatValue(currentXP)} / {NumberFormatter.FormatValue(requiredXP)}";

        UpdateRankInfo();

        if (xpFillRect != null)
        {
            float percent = Mathf.Clamp01(currentXP / requiredXP);
            xpFillRect.anchorMin = new Vector2(0f, 0f);
            xpFillRect.anchorMax = new Vector2(percent, 1f);
            xpFillRect.offsetMin = Vector2.zero;
            xpFillRect.offsetMax = Vector2.zero;
        }
    }
    private void UpdateRankInfo()
    {
        string activeRank = "The Blank Page";
        int highestUnlockedLevel = 0;
        int rankIndex = 0;
        int currentIndex = 0;

        // Dictionary içindəki ən uyğun (cari levelimizdən kiçik/bərabər olan ən böyük) titulu və onun sırasını tapırıq
        foreach (var kvp in rankDictionary)
        {
            if (currentLevel >= kvp.Key && kvp.Key >= highestUnlockedLevel)
            {
                highestUnlockedLevel = kvp.Key;
                activeRank = kvp.Value;
                rankIndex = currentIndex; // Neçənci rütbə olduğunu yadda saxlayırıq
            }
            currentIndex++;
        }

        // 1. Mətni təyin edirik
        if (rankTitleText != null)
        {
            rankTitleText.text = activeRank;
        }

        // 2. Tapdığımız sıraya (rankIndex) əsasən İkonu təyin edirik
        if (LevelIcon != null && rankIndex < rankIcons.Count)
        {
            LevelIcon.sprite = rankIcons[rankIndex];
        }
    }
}