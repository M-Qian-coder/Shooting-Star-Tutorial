using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>
public class MissileDisplay : MonoBehaviour
{
    static Text amountText;
    static Image cooldonwImage;
    void Awake()
    {
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        cooldonwImage = transform.Find("Cooldown Image").GetComponent<Image>();
    }
    public static void UpdateAmountText(int amount)=>amountText.text = amount.ToString();
    public static void updateCooldownIamge(float fillAmount) => cooldonwImage.fillAmount = fillAmount;
}
