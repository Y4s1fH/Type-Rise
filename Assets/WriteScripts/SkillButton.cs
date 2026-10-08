using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class SkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillData skilldata;
    public GameObject skilldetailpanelobject;
    public Transform panelspawnpoint;
    public Outline outlinecomponent;
    public Color readyColor = Color.green;      
    public Color noMoneyColor = Color.red;      
    public Color lockedColor = Color.gray;      
    public Color maxLevelColor = new Color32(168, 85, 247, 255); 
    public GameObject connectedLine;
    private void Update()
    {
        if (skilldata == null || outlinecomponent == null) return;
        bool isUnlocked = skilldata.IsRequirement();
        if (!isUnlocked)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            if (connectedLine != null && connectedLine.activeSelf) connectedLine.SetActive(false);
            return; // Kilid ödənməyibsə aşağıdakı rəng kodlarını oxumağa ehtiyac yoxdur
        }
        else
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            if (connectedLine != null && !connectedLine.activeSelf) connectedLine.SetActive(true);
        }
        // 1. Max Level yoxlanışı
        if (skilldata.currentlevel >= skilldata.maxlevel)
        {
            outlinecomponent.effectColor = maxLevelColor;
        }
        // 2. Kilid yoxlanışı
        else if (!skilldata.IsRequirement())
        {
            outlinecomponent.effectColor = lockedColor;
        }
        // 3. Token yoxlanışı
        else
        {
            float money = Mathf.Round(GameManager.Instance.totalmoney * 10f) / 10f;
            if (money >= skilldata.basecost)
            {
                outlinecomponent.effectColor = readyColor;
            }
            else
            {
                outlinecomponent.effectColor = noMoneyColor;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skilldata == null || skilldetailpanelobject== null) return;
        SkillDetailPanel detailScript = skilldetailpanelobject.GetComponent<SkillDetailPanel>();
        if (detailScript != null)
        {
            detailScript.SetupPanel(skilldata);
        }
        RectTransform panelRect = skilldetailpanelobject.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            Transform targetTransform = panelspawnpoint != null ? panelspawnpoint : transform;
            Vector3 spawnPosition = targetTransform.position;
            spawnPosition.y += 150f; 
            panelRect.position = spawnPosition;
        }
        skilldetailpanelobject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (skilldetailpanelobject != null)
        {
            skilldetailpanelobject.SetActive(false);
        }
    }
    private bool isProcessingClick = false; 

    public void OnClickUpgrade()
    {
        if (skilldata == null) return;
        if (isProcessingClick) return;
        if (skilldata.currentlevel >= skilldata.maxlevel) return;
        bool canUpgrade = skilldata.IsRequirement();
        if (skilldata.material == "dolar")
        {
            float money = Mathf.Round(GameManager.Instance.totalmoney * 10f) / 10f;
            if (money >= skilldata.basecost && canUpgrade)
            {
                isProcessingClick = true; 
                GameManager.Instance.totalmoney -= skilldata.basecost;
                skilldata.currentlevel++;
                skilldata.ChangeIncreases();

                if (SkillEffectManager.Instance != null)
                {
                    SkillEffectManager.Instance.ApplySkillEffect(skilldata.SkillID);
                }
                skilldata.basecost = Mathf.Round(skilldata.basecost * skilldata.costmultiplier);
                GameManager.Instance.UpdateMoneyText();

                if(SkillDetailPanel.Instance != null)
                {
                    SkillDetailPanel.Instance.UpdateUI();
                }
                StartCoroutine(ResetClickLock());
            }
        }
    }
    private IEnumerator ResetClickLock()
    {
        yield return null;
        isProcessingClick = false;
    }

}
