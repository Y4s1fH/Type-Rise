using UnityEngine;

public class SkillPanel : MonoBehaviour // Səhnədə həmişə AÇIQ olan panelin skripti
{
    // Bütün skill butonlarını bura massiv kimi atırsan
    public SkillButton[] allSkillButtons;

    private void Update()
    {
        foreach (var btn in allSkillButtons)
        {
            if (btn != null && btn.skilldata != null)
            {
                // Şərt ödənilibmi? (Məsələn, bağlı olduğu skill açılıbmı?)
                bool isUnlocked = btn.skilldata.IsRequirement();

                // Əgər butonun cari vəziyyəti ilə şərt üst-üstə düşmürsə, açırıq və ya bağlayırıq
                if (btn.gameObject.activeSelf != isUnlocked)
                {
                    btn.gameObject.SetActive(isUnlocked);
                }
            }
        }
    }
}