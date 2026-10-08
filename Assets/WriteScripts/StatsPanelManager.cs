using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class StatsPanelManager : MonoBehaviour
{
    public static StatsPanelManager Instance;
    [Header("Totals:")]
    public TextMeshProUGUI totalEarningsText;
    public TextMeshProUGUI totalKeystrokesText;
    public TextMeshProUGUI totalResetGainText;
    public TextMeshProUGUI totalResetBonusText;
    public TextMeshProUGUI totalFrenzyText;

    [Header("Letters:")]
    public TextMeshProUGUI letterNameText;
    public TextMeshProUGUI letterIncomeText;
    public TextMeshProUGUI letterPressedText;
    public TextMeshProUGUI letterGeneratedText;
    [Header("Colors:")]
    public TextMeshProUGUI greecolorchancetext;
    public TextMeshProUGUI greencolormultipliertext;
    public TextMeshProUGUI redcolorchancetext;
    public TextMeshProUGUI redcolormultipliertext;
    [Header("Frenzy")]
    public TextMeshProUGUI maxfrenzyclicktext;
    public TextMeshProUGUI frenzydurationtext;

    [Header("PanelObjects:")]
    public GameObject statsPanelObject;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if (statsPanelObject != null && statsPanelObject.activeSelf)
        {
            if (Keyboard.current != null)
            {
                foreach (var letter in GameManager.Instance.alphabetList)
                {
                    if (letter.isUnlocked && Keyboard.current[letter.key].wasPressedThisFrame)
                    {
                        UpdateLetterStats(letter); 
                        break;
                    }
                }
            }
        }
    }

    public void ToggleStatsPanel()
    {
        if (statsPanelObject != null)
        {
            bool isActive = statsPanelObject.activeSelf;
            statsPanelObject.SetActive(!isActive);
            GameManager.Instance.SkillPanelObject.SetActive(false);
            GameManager.Instance.achievmentpanelobject.SetActive(false);
            if (!isActive)
            {
                RefreshGlobalStats();
                ShowDefaultLetter(); 
            }
        }
    }

    public void RefreshGlobalStats()
    {
        if (totalEarningsText != null)
            totalEarningsText.text = "Total Earnings: <color=#00FF00>$</color> " + NumberFormatter.FormatValue(GameManager.Instance.totalmoney);
        if (totalKeystrokesText != null)
            totalKeystrokesText.text = "Total Keystrokes: " + NumberFormatter.FormatValue(GameManager.Instance.TotalKeystrokes);
        if (totalResetGainText != null)
            totalResetGainText.text = "Total Reset Money Gain: +" + NumberFormatter.FormatValue(GameManager.Instance.resetmoneygain);
        if (totalResetBonusText != null)
        {
            float bonusPercentage = (GameManager.Instance.resetmoneybonus - 1f) * 100f;
            totalResetBonusText.text = "Total Reset Money Bonus: +" + NumberFormatter.FormatValue(bonusPercentage) + "%";
        }
        if (totalFrenzyText != null)
            totalFrenzyText.text = "Total Frenzy Activations: " + NumberFormatter.FormatValue(GameManager.Instance.TotalFrenzyActivations);
        if (greecolorchancetext != null)
            greecolorchancetext.text = "Green Color Chance: " + NumberFormatter.FormatValue(GameManager.Instance.greencolorChance)+"%";
        if (redcolorchancetext != null)
            redcolorchancetext.text = "RedColor Chance: " + NumberFormatter.FormatValue(GameManager.Instance.redcolorChance)+"%";
        if(greencolormultipliertext != null)
            greencolormultipliertext.text = "Green Color Multiplier: "+NumberFormatter.FormatValue(GameManager.Instance.greencolorMultiplier)+"x";
        if(redcolormultipliertext != null)
            redcolormultipliertext.text = "Red Color Multiplier: "+NumberFormatter.FormatValue(GameManager.Instance.redcolorMultiplier)+"x";
        if (maxfrenzyclicktext != null)
            maxfrenzyclicktext.text = "Max Keystrokes For Frenzy: " + NumberFormatter.FormatValue(GameManager.Instance.maxFrenzyClicks);
        if (frenzydurationtext != null)
            frenzydurationtext.text = "Frenzy Duration: " + NumberFormatter.FormatValue(GameManager.Instance.frenzyDuration-1);
    }

    private void ShowDefaultLetter()
    {
        foreach (var letter in GameManager.Instance.alphabetList)
        {
            if (letter.isUnlocked)
            {
                UpdateLetterStats(letter);
                return;
            }
        }
    }

    private void UpdateLetterStats(LetterData letter)
    {

        if (letterNameText != null)
            letterNameText.text = "Letter: " + letter.letterStr.ToLower()+ "(Press any unlocked letter to view stats)";

        if (letterIncomeText != null)
            letterIncomeText.text = "Letter Income: +" + NumberFormatter.FormatValue(letter.letterincome) + "<color=#00FF00>$</color>";

        if (letterPressedText != null)
            letterPressedText.text = "Letter Pressed: " + letter.totalTimesPressed;

        if (letterGeneratedText != null)
            letterGeneratedText.text = "Generated: " + NumberFormatter.FormatValue(letter.totalMoneyGenerated) + "<color=#00FF00>$</color>";
    }
}

