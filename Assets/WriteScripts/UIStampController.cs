using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStampController : MonoBehaviour
{
    [Header("Animasiya Ayarları")]
    public float jumpHeight = 50f;    
    public float animationSpeed = 8f;
    public float incomeMultiplier = 1.0f;
    public GameObject floatingTextPrefab;
    [Header("Möhür İzi (UI Prefab)")]
    public GameObject imprintPrefab; 
    public RectTransform paperTarget; 
    public Transform canvasParent;   

    private RectTransform stampRect;
    private Vector2 originalPos;
    private bool isStamping = false;

    void Start()
    {
        stampRect = GetComponent<RectTransform>();
        originalPos = stampRect.anchoredPosition; 
    }
    public void TriggerStamp()
    {
        if (!isStamping)
        {
            StartCoroutine(PerformStamp());
        }
    }

    IEnumerator PerformStamp()
    {
        isStamping = true;

        Vector2 peakPos = originalPos + new Vector2(0, jumpHeight);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
            stampRect.anchoredPosition = Vector2.Lerp(originalPos, peakPos, t);
            yield return null;
        }
        t = 0;
        Vector2 hitPos = paperTarget.anchoredPosition;
        while (t < 1)
        {
            t += Time.deltaTime * (animationSpeed * 2); 
            stampRect.anchoredPosition = Vector2.Lerp(peakPos, hitPos, t);
            yield return null;
        }
        if (imprintPrefab != null && canvasParent != null)
        {
            GameObject newImprint = Instantiate(imprintPrefab, canvasParent);
            newImprint.transform.SetAsFirstSibling();
            RectTransform imprintRect = newImprint.GetComponent<RectTransform>();
            imprintRect.anchoredPosition = hitPos; 
            imprintRect.localRotation = Quaternion.Euler(0, 0, Random.Range(-10f, 10f));
            if (newImprint.GetComponent<FadeOutImprintUI>() == null)
            {
                newImprint.AddComponent<FadeOutImprintUI>();
            }
        }
        if (GameManager.Instance != null && GameManager.Instance.alphabetList.Count > 1)
        {
            LetterData activeLetter = GameManager.Instance.alphabetList[1]; 
            float actualGain = GameManager.Instance.CalculateIncomeAndColor(activeLetter, incomeMultiplier, out bool isGreen, out bool isRed);
            GameManager.Instance.totalmoney += actualGain;
            activeLetter.totalMoneyGenerated += actualGain; 
            GameManager.Instance.UpdateMoneyText();
            if (floatingTextPrefab != null && canvasParent != null)
            {
                GameObject fText = Instantiate(floatingTextPrefab, canvasParent);
                fText.transform.SetAsFirstSibling();
                RectTransform fTextRect = fText.GetComponent<RectTransform>();

                fTextRect.anchoredPosition = hitPos + new Vector2(0, 50f);

                TextMeshProUGUI tmp = fText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    string colorCode = "<color=#FFFFFF>"; 

                    if (isRed)
                    {
                        colorCode = "<color=#FF0000>"; 
                    }
                    else if (isGreen)
                    {
                        colorCode = "<color=#00FF2C>"; 
                    }
                    tmp.text = $"{colorCode}+{NumberFormatter.FormatValue(actualGain)}$</color>";
                }
            }
        }
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
            stampRect.anchoredPosition = Vector2.Lerp(hitPos, originalPos, t);
            yield return null;
        }

        stampRect.anchoredPosition = originalPos;
        isStamping = false;
    }
}