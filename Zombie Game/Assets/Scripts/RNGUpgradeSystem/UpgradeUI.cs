using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;

    public void ShowChoices(Upgrade a, Upgrade b, Upgrade c)
    {
        panel.SetActive(true);

        option1Text.text = $"1) {a.upgradeName}\n{a.description}";
        option2Text.text = $"2) {b.upgradeName}\n{b.description}";
        option3Text.text = $"3) {c.upgradeName}\n{c.description}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}

