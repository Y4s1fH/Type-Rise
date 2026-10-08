using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDetailPanel : MonoBehaviour
{
    public static SkillDetailPanel Instance;
    public TextMeshProUGUI skillnametext;
    public TextMeshProUGUI skilldescriptiontext;
    public TextMeshProUGUI skillinfotext;
    public TextMeshProUGUI skillcosttext;
    public TextMeshProUGUI skillleveltext;
    public GameObject costobject;
    private SkillData currentskilldata;
    void Awake()
    {
        Instance = this;
    }
    public void SetupPanel(SkillData data)
    {
        currentskilldata = data;
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (currentskilldata == null) return;
        if(skillnametext != null)
        {
            skillnametext.text = currentskilldata.SkillName;
        }
        if(skillleveltext != null)
        {
            skillleveltext.text = $"Level: {NumberFormatter.FormatValue(currentskilldata.currentlevel)} / {NumberFormatter.FormatValue(currentskilldata.maxlevel)}";
        }
        if(skilldescriptiontext != null)
        {
            skilldescriptiontext.text = currentskilldata.description;
        }
        if(skillinfotext != null)
        {
            if (currentskilldata.currentlevel >= currentskilldata.maxlevel)
            {
                skillinfotext.text = currentskilldata.MaxGetFormattedInfo();
            }
            else
            {
                skillinfotext.text = currentskilldata.GetFormattedInfo();
            }
 
        }
        if(skillcosttext != null)
        {
            if (currentskilldata.currentlevel >= currentskilldata.maxlevel)
            {
                if (costobject != null)
                {
                    costobject.SetActive(false);
                }
            }
            else
            {
                if(costobject != null)
                {
                    costobject.SetActive(true);
                }
                skillcosttext.text = $"Price: {NumberFormatter.FormatValue(currentskilldata.basecost)} <sprite name=\"dollar\">";
            }
        }
    }
}
