using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem instance;

    [Header("UI")]
    public GameObject upgradePanel;

    public Image[] optionIcons = new Image[3];
    public TextMeshProUGUI[] optionTitles = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] optionDescs = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] optionPercents = new TextMeshProUGUI[3];

    [Header("Upgrades (ScriptableObjects)")]
    public Upgrade[] allUpgrades;

    private Upgrade[] currentChoices = new Upgrade[3];

    PlayerMovement player;
    GunSystem1 gun;

    public bool IsMenuOpen => upgradePanel.activeSelf;


    // ----------------------
    //  RARITY COLOR LOOKUP
    // ----------------------
    public string GetRarityHex(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => "#FFFFFF",
            Rarity.Uncommon => "#4CFF4C",
            Rarity.Rare => "#4C7BFF",
            Rarity.Epic => "#C44CFF",
            Rarity.Legendary => "#FFA64C",
            _ => "#FFFFFF"
        };
    }


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        gun = FindObjectOfType<GunSystem1>();

        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }



    // ----------------------
    //       CORE LOGIC
    // ----------------------
    public void GiveUpgrades()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        for (int i = 0; i < 3; i++)
        {
            // FIX: Instantiate so ScriptableObject is NOT modified
            Upgrade baseUpgrade = allUpgrades[Random.Range(0, allUpgrades.Length)];
            Upgrade upgrade = Instantiate(baseUpgrade);

            // Roll rarity FIRST
            upgrade.rarity = Upgrade.RollRarity();

            // Roll a % value based on rarity
            upgrade.rolledValue = Mathf.Round(upgrade.GetRandomValueForRarity(upgrade.rarity));

            currentChoices[i] = upgrade;

            // ----------------------
            //      UI POPULATION
            // ----------------------
            string rarityHex = GetRarityHex(upgrade.rarity);
            string rarityText = upgrade.rarity.ToString();

            optionTitles[i].text =
                $"{upgrade.upgradeName} <color={rarityHex}>({rarityText})</color>";

            optionDescs[i].text = upgrade.description;

            if (upgrade.icon != null)
                optionIcons[i].sprite = upgrade.icon;
            else
            {
                optionIcons[i].sprite = null;
                optionIcons[i].color = Color.clear;
            }

            optionPercents[i].text =
                $"{GetUpgradeLabel(upgrade)}: {upgrade.rolledValue}%";
        }

        upgradePanel.SetActive(true);
    }



    private void Update()
    {
        if (upgradePanel == null || !upgradePanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectUpgrade(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectUpgrade(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectUpgrade(2);
    }



    public void SelectUpgrade(int index)
    {
        if (currentChoices[index] != null)
            currentChoices[index].Apply(player, gun);

        CloseMenu();
    }


    void CloseMenu()
    {
        upgradePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Automatically names the "% stat" for UI
    private string GetUpgradeLabel(Upgrade upgrade)
    {
        if (upgrade is DamageUpgrade) return "Damage Increase";
        if (upgrade is SpeedUpgrade) return "Speed Increase";
        if (upgrade is FireRateUpgrade) return "Fire Rate Increase";
        if (upgrade is HealthUpgrade) return "Max Health Increase";

        return "Increase";
    }
}







