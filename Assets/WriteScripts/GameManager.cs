using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Yeni Input System üçün lazım olan kitabxana

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject floatingTextPrefab;
    public GameObject SkillPanelObject;
    public GameObject achievmentpanelobject;
    public Transform canvasTransform;
    public TextMeshProUGUI paperText;
    public TextMeshProUGUI dolarText;
    public GameObject notebookObject;
    public GameObject ResetButton;
    public GameObject PressTextObject;
    public float resetmoneygain = 0f;
    public float resetmoneybonus = 1f;
    public float totalmoney = 0f;
    private string currentText = "";
    public float greencolorChance = 0f;      
    public float greencolorMultiplier = 1f;
    public float redcolorChance = 0f;
    public float redcolorMultiplier = 1f;
    private float lastTypeTime = 0f;
    public float typeCooldown = 0.15f;
    public TextMeshProUGUI frenzyButtonText;
    public TextMeshProUGUI frenzyTimerText;
    public int maxFrenzyClicks = 100;        
    public float frenzyDuration = 1f;        
    private int frenzyClickCount = 0;
    public int TotalKeystrokes = 0;
    public int TotalFrenzyActivations = 0;
    private bool isFrenzyReady = false;    
    public bool isFrenzyActive = false;
    public bool isFrenzyUnlocked = false;
    public bool isAutoResetUnlocked = false;
    public bool isPossessedQuillUnlocked = false;
    public bool isEncounterActive = false;
    private float frenzyTimer = 0f;
    public int totalcharsPerKeystroke;
    private float normalCooldown;
    public Button FrenzyButton;
    public RectTransform frenzyFillRect;
    [HideInInspector] public List<SkillData> registeredSkills = new List<SkillData>();
    [HideInInspector] public List<AchievmentData> allAchievements = new List<AchievmentData>();
    public List<LetterData> alphabetList = new List<LetterData>();
     void Awake()
      {
            Instance = this;
            if (alphabetList.Count == 0)
            {
                alphabetList.Add(new LetterData { key = Key.A, letterStr = "a", letterincome = 0.1f, canBeGreen = true, isUnlocked = true });
                alphabetList.Add(new LetterData { key = Key.B, letterStr = "b", letterincome = 100f, canBeGreen = false, isUnlocked = false });
                alphabetList.Add(new LetterData { key = Key.C, letterStr = "c", letterincome = 10000f, canBeGreen = false, isUnlocked = false });
            }
       }
    void Start()
    {
        if(ResetButton != null)
        {
            ResetButton.SetActive(false);
        }
        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(false); 
        }
        if(FrenzyButton != null)
        {
            FrenzyButton.interactable = false;
        }
        if(FrenzyButton!= null)
        {
            FrenzyButton.gameObject.SetActive(false);
        }
        UpdateFrenzyUI();
    }
    void Update()
    {
        if (notebookObject != null && !notebookObject.activeSelf)
        {
            return;
        }
        if (isFrenzyActive)
        {
            frenzyTimer -= Time.deltaTime;

            if (frenzyTimerText != null)
            {
                frenzyTimerText.text = $"FRENZY: 00:0{(int)frenzyTimer}";
            }

            if (frenzyTimer <= 0f)
            {
                EndFrenzy(); 
            }
        }
        LetterData activeLetter = null;
        if (Keyboard.current != null)
        {
            foreach (var letterData in alphabetList)
            {
                if (!letterData.isUnlocked) continue;

                bool isBtnPressed = isFrenzyActive ? Keyboard.current[letterData.key].isPressed : Keyboard.current[letterData.key].wasPressedThisFrame;

                if (isBtnPressed)
                {
                    activeLetter = letterData;
                    break; 
                }
            }
        }
        if (activeLetter !=null)
        {
            if (PressTextObject != null) PressTextObject.SetActive(false);
            if (Time.time - lastTypeTime >= typeCooldown)
            {
                lastTypeTime = Time.time;
                int charsToAdd = activeLetter.charsPerClick +totalcharsPerKeystroke;
                if (charsToAdd > 0)
                {
                    string testText = currentText;
                    for (int i = 0; i < charsToAdd; i++)
                    {
                        testText += activeLetter.letterStr;
                    }
                    paperText.text = testText;
                    paperText.ForceMeshUpdate();
                    if (paperText.textInfo.lineCount <= 11)
                    {
                        TotalKeystrokes++;
                        if (LevelManager.Instance != null)
                        {
                            LevelManager.Instance.AddXP(LevelManager.Instance.xpPerType*charsToAdd);
                        }
                        activeLetter.totalTimesPressed++;
                        StatsPanelManager.Instance.RefreshGlobalStats();
                        float actualGain = 0f;

                        for (int i = 0; i < charsToAdd; i++)
                        {
                            float gain = CalculateIncomeAndColor(activeLetter, 1f, out bool isGreen, out bool isRed);
                            if (isRed) currentText += $"<color=#FF0000>{activeLetter.letterStr}</color>";
                            else if (isGreen) currentText += $"<color=#00FF00>{activeLetter.letterStr}</color>";
                            else currentText += activeLetter.letterStr;

                            actualGain += gain;
                        }

                        // Gerçek (renkli) metni ekrana bas
                        paperText.text = currentText;

                        if (floatingTextPrefab != null && canvasTransform != null)
                        {
                            GameObject floatText = Instantiate(floatingTextPrefab, canvasTransform);
                            floatText.transform.SetAsFirstSibling();
                            TextMeshProUGUI tmp = floatText.GetComponent<TextMeshProUGUI>();
                            if (tmp != null) tmp.text = "+" + NumberFormatter.FormatValue(actualGain) + " $";
                        }

                        totalmoney += actualGain;
                        activeLetter.totalMoneyGenerated += actualGain;
                        UpdateMoneyText();

                        paperText.text = currentText + activeLetter.letterStr;
                        paperText.ForceMeshUpdate();
                        if (isFrenzyUnlocked && !isFrenzyReady && !isFrenzyActive)
                        {
                            // Oyunçu uğurla vurduğu üçün Frenzy artır
                            frenzyClickCount += charsToAdd;
                            UpdateFrenzyUI();

                            if (frenzyClickCount >= maxFrenzyClicks)
                            {
                                isFrenzyReady = true;
                                if (frenzyButtonText != null) frenzyButtonText.text = "READY!";
                                if (FrenzyButton != null) FrenzyButton.interactable = true;
                            }
                        }
                        if (paperText.textInfo.lineCount > 11 || paperText.isTextOverflowing)
                        {
                            if (paperText.textInfo.lineCount > 11 || paperText.isTextOverflowing)
                            {
                                if (isAutoResetUnlocked)
                                {
                                    ResetNotebook(); 
                                }
                                else
                                {
                                    if (ResetButton != null && !ResetButton.activeSelf) ResetButton.SetActive(true);
                                }
                            }
                        }

                        // Əsl mətni geri qaytarırıq (Çünki yuxarıda bir hərf əlavə edib gizlicə test etdik)
                        paperText.text = currentText;
                    }
                    else
                    {
                        // Səhifə zatən doludur, yazmağa qoyma və Reset düyməsi hər ehtimala qarşı yenə yansın
                        paperText.text = currentText;
                        if (isAutoResetUnlocked)
                        {
                            ResetNotebook(); // Səhifə doludursa və düyməyə basıbsa, dərhal avto-sıfırla
                        }
                        else
                        {
                            if (ResetButton != null && !ResetButton.activeSelf) ResetButton.SetActive(true);
                        }
                    }
                }
            }
        }

        if (paperText != null)
        {
            paperText.text = currentText;
        }
    }
    public void UnlockLetter(Key targetKey)
    {
        foreach (var letter in alphabetList)
        {
            if (letter.key == targetKey)
            {
                letter.isUnlocked = true;
                break;
            }
        }
    }
    public void UpdateMoneyText()
    {
        dolarText.text = $"<sprite name=\"dollar2\"> {NumberFormatter.FormatValue(totalmoney)}";
    }
    public void ResetNotebook()
    {
        currentText = "";
        if (floatingTextPrefab != null && canvasTransform != null)
        {
            
            GameObject floatText = Instantiate(floatingTextPrefab, canvasTransform);

           
            TextMeshProUGUI tmp = floatText.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                float actualresetmoneygain = resetmoneygain * resetmoneybonus;
                tmp.text = "+" + NumberFormatter.FormatValue(actualresetmoneygain) + " $";
            }
        }
        totalmoney += resetmoneygain;
        UpdateMoneyText() ;
        if (paperText != null)
            paperText.text = currentText;

       
        if (ResetButton != null)
            ResetButton.SetActive(false);
    }
    public void ToggleNotebook()
    {
        if (notebookObject != null)
        {
            bool isActive = notebookObject.activeSelf;
            notebookObject.SetActive(!isActive);
        }
    }
    public void ToggleSKillTree()
    {
        if(SkillPanelObject != null)
        {
            bool isActive = SkillPanelObject.activeSelf;
            SkillPanelObject.SetActive(!isActive);
            StatsPanelManager.Instance.statsPanelObject.SetActive(false);
            achievmentpanelobject.SetActive(false);
        }
    }
    public void ToggleAchievment()
    {
        if(achievmentpanelobject != null)
        {
            bool isActive = achievmentpanelobject.activeSelf;
            achievmentpanelobject.SetActive(!isActive);
            StatsPanelManager.Instance.statsPanelObject.SetActive(false);
            SkillPanelObject.SetActive(false);
        }
    }
    private void UpdateFrenzyUI()
    {
        if (frenzyButtonText != null && !isFrenzyReady)
        {
            frenzyButtonText.text = $"FRENZY {frenzyClickCount} / {maxFrenzyClicks}";
        }
        if (frenzyFillRect != null)
        {
            float percent = (float)frenzyClickCount / maxFrenzyClicks;
            frenzyFillRect.anchorMin = new Vector2(0f, 0f);
            frenzyFillRect.anchorMax = new Vector2(percent, 1f);
            frenzyFillRect.offsetMin = Vector2.zero;
            frenzyFillRect.offsetMax = Vector2.zero;
        }
    }
    public void UnlockFrenzy()
    {
        isFrenzyUnlocked = true;
        if (FrenzyButton != null)
        {
            FrenzyButton.gameObject.SetActive(true); 
            FrenzyButton.interactable = false;
        }
        UpdateFrenzyUI();
    }
    public void ActivateFrenzy()
    {
        if (isFrenzyReady && !isFrenzyActive)
        {
            isFrenzyReady = false;
            isFrenzyActive = true;
            frenzyTimer = frenzyDuration;
            TotalFrenzyActivations++;

            normalCooldown = typeCooldown;
            typeCooldown = 0f;            

            if (frenzyTimerText != null)
            {
                frenzyTimerText.gameObject.SetActive(true);
            }
            if (FrenzyButton != null)
            {
                FrenzyButton.interactable = false;
            }
            if (frenzyFillRect != null) { frenzyFillRect.anchorMax = new Vector2(1f, 1f); frenzyFillRect.offsetMax = Vector2.zero; }
            if (frenzyFillRect != null) { frenzyFillRect.gameObject.SetActive(false); }
        }
    }
    private void EndFrenzy()
    {
        isFrenzyActive = false;
        typeCooldown = normalCooldown; 
        frenzyClickCount = 0;         

        UpdateFrenzyUI(); 

        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(false);
        }
        if (frenzyFillRect != null) { frenzyFillRect.anchorMax = new Vector2(0f, 1f); frenzyFillRect.offsetMax = Vector2.zero; }
        if (frenzyFillRect != null) { frenzyFillRect.gameObject.SetActive(true); }
    }
    public float CalculateIncomeAndColor(LetterData letter, float baseMultiplier, out bool isGreen, out bool isRed)
    {
        isGreen = false;
        isRed = false;
        float finalGain = letter.letterincome * baseMultiplier;

        if (letter.canBeGreen)
        {
            float randomVal = Random.Range(0f, 100f);

            if (randomVal <= redcolorChance)
            {
                isRed = true;
                finalGain *= redcolorMultiplier;
            }
            else if (randomVal <= redcolorChance + greencolorChance)
            {
                isGreen = true;
                finalGain *= greencolorMultiplier;
            }
        }
        return finalGain;
    }
}