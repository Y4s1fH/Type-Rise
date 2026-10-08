using UnityEngine;
using TMPro;

public class PassiveIncomeManager : MonoBehaviour
{
    public static PassiveIncomeManager Instance;

    [Header("UI Əlaqələri")]
    public RectTransform penTransform; 
    public Transform floatingTextSpawnPoint;
    public GameObject PenObject;
    public GameObject sticknoteobject;
    [Header("Passiv Qazanc Tənzimləmələri")]
    public bool isUnlocked = false; 
    public float writeInterval = 2.0f; 
    public float incomeMultiplier = 1.0f;
    public float bonusmultiplier = 10f;
    public int targetLetterIndex = 0; 

    [Header("Animasiya Tənzimləmələri")]
    public float writeSpeed = 25f; 
    public float writeAmount = 3f; 
    private Vector3 startPenPos;
    private float timer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (penTransform != null)
            startPenPos = penTransform.localPosition;
    }

    void Update()
    {
        if (!isUnlocked || GameManager.Instance == null || penTransform == null) return;
        penTransform.localPosition = startPenPos + new Vector3(
            Mathf.Sin(Time.time * writeSpeed) * writeAmount,
            Mathf.Cos(Time.time * (writeSpeed * 0.8f)) * (writeAmount * 0.8f),
            0);

        timer += Time.deltaTime;
        if (timer >= writeInterval)
        {
            timer = 0f;
            GeneratePassiveIncome();
        }
    }

    void GeneratePassiveIncome()
    {
        if (GameManager.Instance.alphabetList.Count <= targetLetterIndex) return;

        LetterData activeLetter = GameManager.Instance.alphabetList[targetLetterIndex];
        if (!activeLetter.isUnlocked) return;
        float currentMultiplier = incomeMultiplier;

        if (GameManager.Instance.isPossessedQuillUnlocked && GameManager.Instance.isFrenzyActive)
        {
            currentMultiplier *= bonusmultiplier; // Frenzy aktivdirsə, qələmin çarpanını 5 qat artırır!
        }
        float actualGain = GameManager.Instance.CalculateIncomeAndColor(activeLetter, currentMultiplier, out bool isGreen, out bool isRed);

        GameManager.Instance.totalmoney += actualGain;
        activeLetter.totalMoneyGenerated += actualGain;
        GameManager.Instance.UpdateMoneyText();
        SpawnFloatingText(activeLetter.letterStr, actualGain, isGreen, isRed);
    }

    // Funksiya artıq 'bool isRed' parametrini də qəbul edir
    void SpawnFloatingText(string letter, float amount, bool isGreen, bool isRed)
    {
        if (GameManager.Instance.floatingTextPrefab != null && floatingTextSpawnPoint != null)
        {
            GameObject floatText = Instantiate(GameManager.Instance.floatingTextPrefab, floatingTextSpawnPoint.position, Quaternion.identity, floatingTextSpawnPoint);
            TextMeshProUGUI tmp = floatText.GetComponent<TextMeshProUGUI>();

            if (tmp != null)
            {
                string colorCode = "<color=#FFFFFF>"; // Normal rəng (Ağ/Kağız)

                if (isRed)
                {
                    colorCode = "<color=#FF0000>"; // Qırmızı
                }
                else if (isGreen)
                {
                    colorCode = "<color=#00FF2C>"; // Yaşıl
                }

                tmp.text = $"{colorCode}{letter}</color><color=#FFFFFF> -></color> +{NumberFormatter.FormatValue(amount)}$";
            }
        }
    }

    public void UnlockPassiveIncome()
    {
        isUnlocked = true;
    }
}