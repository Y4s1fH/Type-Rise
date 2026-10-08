using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.InputSystem;

public class TypingTestManager : MonoBehaviour
{
    // Gələcəkdə başqa skriptlərdən (məs. SkillTree) asanlıqla çatmaq üçün Instance yaradırıq
    public static TypingTestManager Instance; 

    [Header("Typing UI References")]
    public TextMeshProUGUI wordDisplay;

    [Header("Timer & Results UI")]
    public TextMeshProUGUI timerText;
    public GameObject resultsPanel;
    public GameObject typingpanel;
    public TextMeshProUGUI wpmText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI inkEarnedText;
    public TextMeshProUGUI inkText;
    public GameObject lockedicon;

    [Header("Unlock System")]
    public bool isTypingUnlocked = false; // Kilid şüşəsi (Skill Tree-dən true olacaq)
    public GameObject lockedWarningPopup; // Ekranda çıxacaq "Kilidlidir" xəbərdarlıq yazısı

    [Header("Settings")]
    public int wordsToDisplay = 30;
    public float testDuration = 30f;
    public string correctColorHex = "#FFD700";
    public string incorrectColorHex = "#FF0000";
    public string defaultColorHex = "#646669";

    private string[] wordPool = {
        "time", "year", "people", "way", "day", "man", "thing", "life", "child",
        "world", "school", "state", "family", "student", "group", "country", "problem",
        "hand", "part", "place", "case", "week", "company", "system", "program",
        "work", "number", "night", "point", "home", "water", "room", "mother", "area",
        "money", "story", "fact", "month", "lot", "right", "study", "book", "eye", "job",
        "word", "business", "issue", "side", "kind", "head", "house", "service", "friend"
    };

    private string targetText = "";
    private int currentIndex = 0;
    private int[] charStates;

    // Statistika və Vəziyyət
    private bool isTesting = false;
    private float currentTime;
    private int totalKeystrokes = 0;
    private int correctKeystrokes = 0;

    // İqtisadiyyat dəyişənləri
    private int pendingInkReward = 0;
    private int totalInk = 0; 

    void Awake()
    {
        Instance = this; // Başqa skriptlər bura asan çatsın deyə
    }

    void Start()
    {
        UpdateGlobalInkUI();
        
        // Oyun başlayanda xəbərdarlıq yazısını ehtiyata qarşı gizlədirik
        if (lockedWarningPopup != null) lockedWarningPopup.SetActive(false);
    }

    void OnEnable()
    {
        if (Keyboard.current != null) Keyboard.current.onTextInput += ProcessInputChar;
    }

    void OnDisable()
    {
        if (Keyboard.current != null) Keyboard.current.onTextInput -= ProcessInputChar;
    }

    void Update()
    {
        if (isTesting && typingpanel.activeSelf) 
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                EndTest();
            }

            UpdateTimerUI();
        }

        if (Keyboard.current != null && Keyboard.current.backspaceKey.wasPressedThisFrame && isTesting)
        {
            HandleBackspace();
        }
    }

    public void StartNewTest()
    {
        resultsPanel.SetActive(false);
        wordDisplay.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        isTesting = true;
        currentTime = testDuration;
        totalKeystrokes = 0;
        correctKeystrokes = 0;
        pendingInkReward = 0;

        GenerateNewWords();
    }

    public void GenerateNewWords()
    {
        targetText = "";
        for (int i = 0; i < wordsToDisplay; i++)
        {
            targetText += wordPool[Random.Range(0, wordPool.Length)];
            if (i < wordsToDisplay - 1) targetText += " ";
        }

        charStates = new int[targetText.Length];
        currentIndex = 0;

        UpdateDisplay();
    }

    private void ProcessInputChar(char c)
    {
        if (!isTesting) return;
        if (c == '\n' || c == '\r' || c == '\b') return;
        if (currentIndex >= targetText.Length) return;

        totalKeystrokes++;

        if (c == targetText[currentIndex])
        {
            charStates[currentIndex] = 1;
            correctKeystrokes++;
        }
        else
        {
            charStates[currentIndex] = 2;
        }

        currentIndex++;
        UpdateDisplay();
        CheckScroll();
    }

    private void HandleBackspace()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            if (charStates[currentIndex] == 1) correctKeystrokes--;
            charStates[currentIndex] = 0;
            UpdateDisplay();
        }
    }

    private void CheckScroll()
    {
        wordDisplay.ForceMeshUpdate();
        if (currentIndex == 0 || wordDisplay.textInfo.characterCount == 0) return;

        int lastTypedIndex = currentIndex - 1;
        if (lastTypedIndex >= wordDisplay.textInfo.characterCount) return;

        int currentLine = wordDisplay.textInfo.characterInfo[lastTypedIndex].lineNumber;

        if (currentLine >= 2)
        {
            int charsToRemove = wordDisplay.textInfo.lineInfo[1].firstCharacterIndex;
            targetText = targetText.Substring(charsToRemove);

            string newWords = "";
            for (int i = 0; i < 10; i++)
            {
                newWords += " " + wordPool[Random.Range(0, wordPool.Length)];
            }
            targetText += newWords;

            int[] newCharStates = new int[targetText.Length];
            for (int i = 0; i < targetText.Length - newWords.Length; i++)
            {
                newCharStates[i] = charStates[i + charsToRemove];
            }
            charStates = newCharStates;
            currentIndex -= charsToRemove;

            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        StringBuilder sb = new StringBuilder();
        int lastState = -1;

        for (int i = 0; i < targetText.Length; i++)
        {
            if (charStates[i] != lastState)
            {
                if (lastState != -1) sb.Append("</color>");

                if (charStates[i] == 0) sb.Append($"<color={defaultColorHex}>");
                else if (charStates[i] == 1) sb.Append($"<color={correctColorHex}>");
                else if (charStates[i] == 2) sb.Append($"<color={incorrectColorHex}>");

                lastState = charStates[i];
            }

            char displayChar = (charStates[i] == 2 && targetText[i] == ' ') ? '_' : targetText[i];
            sb.Append(displayChar);
        }

        if (lastState != -1) sb.Append("</color>");
        wordDisplay.text = sb.ToString();
    }

    private void UpdateTimerUI()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = $"00:{seconds.ToString("00")}";
    }

    private void EndTest()
    {
        isTesting = false;
        wordDisplay.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        resultsPanel.SetActive(true);

        float minutes = testDuration / 60f;
        int wpm = Mathf.RoundToInt((correctKeystrokes / 5f) / minutes);

        float accuracy = 0f;
        if (totalKeystrokes > 0)
        {
            accuracy = ((float)correctKeystrokes / totalKeystrokes) * 100f;
        }

        pendingInkReward = Mathf.RoundToInt(wpm * (accuracy / 100f));

        wpmText.text = $"WPM: {wpm}";
        accuracyText.text = $"Accuracy: {Mathf.RoundToInt(accuracy)}%";
        inkEarnedText.text = $"REWARD: +{pendingInkReward} <sprite name=\"ink2\">";
    }

    private void UpdateGlobalInkUI()
    {
        if (inkText != null)
        {
            // İnk panelini yalnız kilid açılanda göstərmək istəyirsənsə:
            inkText.gameObject.SetActive(isTypingUnlocked);
            
            if (isTypingUnlocked)
            {
                inkText.text = $" <sprite name=\"ink2\"> {NumberFormatter.FormatValue(totalInk)}";
            }
        }
    }

    public void CollectAndExit()
    {
        totalInk += pendingInkReward;
        UpdateGlobalInkUI();

        isTesting = false;
        resultsPanel.SetActive(false);
        wordDisplay.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        typingpanel.SetActive(false);
    }

    public void CollectAndRetry()
    {
        totalInk += pendingInkReward;
        UpdateGlobalInkUI();
        StartNewTest();
    }

    // YENİ VƏ ŞƏRTLİ TOGGLE MƏNTİQİ
    public void ToggleTypingPanel()
    {
        if (!isTypingUnlocked)
        {
            // Kilid qırılmayıbsa, panel açılmır və xəbərdarlıq çıxır
            if (lockedWarningPopup != null)
            {
                lockedWarningPopup.SetActive(true);
                // 2 saniyə sonra xəbərdarlığı gizlədən funksiyanı çağırırıq
                Invoke(nameof(HideWarning), 2f); 
            }
            return; // Kodun davamına keçməsinə icazə vermirik
        }

        // Əgər kilit açıqdırsa (isTypingUnlocked == true), köhnə məntiq işləyir
        if (typingpanel != null)
        {
            bool isActive = typingpanel.activeSelf;
            typingpanel.SetActive(!isActive);

            if (!isActive)
            {
                StartNewTest(); 
            }
            else
            {
                isTesting = false; 
            }
        }
    }

    // Xəbərdarlıq yazısını gizlətmək üçün funksiya
    private void HideWarning()
    {
        if (lockedWarningPopup != null)
        {
            lockedWarningPopup.SetActive(false);
        }
    }
}