using UnityEngine;
public class SkillData : MonoBehaviour
{
    public string SkillID;
    public string SkillName;
    [TextArea(3, 5)]
    public string description;
    public string info;
    public float increaseinfo;
    public float currentincrease;
    public float nextincrease;
    public int currentlevel = 0;
    public int maxlevel;
    public float basecost=5f;
    public float initialcost=5f;
    public float costmultiplier = 1f;
    public bool hasRequiredSkill = false;
    public SkillData requiredSkill;
    public int requiredSkillLevel = 1;
    public Sprite icon;
    public string material = "dolar";
    public string infoword;
    void Awake()
    {
        if (GameManager.Instance != null && !GameManager.Instance.registeredSkills.Contains(this))
        {
            GameManager.Instance.registeredSkills.Add(this);
        }
    }
    public bool IsRequirement()
    {
        if (hasRequiredSkill)
        {
            if (requiredSkill == null || requiredSkill.currentlevel < requiredSkillLevel)
                return false;
        }

        return true;
    }
    public string GetFormattedInfo()
    {
        switch (infoword)
        {
            case "plus":
               
                return $"+{NumberFormatter.FormatValue(currentincrease)} -> +{NumberFormatter.FormatValue(nextincrease)}";
            case "percentage":
                return $"{NumberFormatter.FormatValue(currentincrease)}% -> {NumberFormatter.FormatValue(nextincrease)}%";
            case "mult":
                return $"x{NumberFormatter.FormatValue(currentincrease)} -> x{NumberFormatter.FormatValue(nextincrease)}";
            case "mult-plus":
                return $"+x{NumberFormatter.FormatValue(currentincrease)} -> +x{NumberFormatter.FormatValue(nextincrease)}";
        }

       
        return "";
    }
    public void ChangeIncreases()
    {
        currentincrease = nextincrease;
        nextincrease += increaseinfo;
    }
    public string MaxGetFormattedInfo()
    {
        switch (infoword) 
        {
            case "plus":
                return $"+{NumberFormatter.FormatValue(currentincrease)}";
            case "percentage":
                return $"{NumberFormatter.FormatValue(currentincrease)}%";
            case "mult":
                return $"x{NumberFormatter.FormatValue(currentincrease)}";
            case "mult-plus":
                return $"+x{NumberFormatter.FormatValue(currentincrease)}";
        }
        return "";
    }
}
