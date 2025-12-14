using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeData upgradeData;
    public float increase;
    public Image iconImage;
    public TextMeshProUGUI titleText, descText;
    public int rarity;

    private PlayerManager playerManager;

    [SerializeField] private float luckScaling = 0.01f;

    [SerializeField] private float[] baseWeights = new float[]
    {
        50f, // Common
        35f, // Uncommon
        10f, // Rare
        4f,  // Epic
        1f   // Legendary
    };

    [SerializeField] private float[] rarityLuckMultipliers = new float[]
    {
        -1.2f, // Common
        -0.3f, // Uncommon
        1f,    // Rare
        1.5f,  // Epic
        3f     // Legendary
    };

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerManager = player.GetComponent<PlayerManager>();
    }

    int GetRarityByLuck()
    {
        float luckFactor = playerManager != null ? playerManager.GetLuck : 0f;

        float total = 0f;
        float[] weights = new float[5];

        // Her rarity için ağırlık hesapla
        for (int i = 0; i < 5; i++)
        {
            // Exponential scaling ile luck etkisini uygula
            weights[i] = baseWeights[i] * Mathf.Exp(rarityLuckMultipliers[i] * luckFactor * luckScaling);
            total += weights[i];
        }

        // Weighted random selection
        float roll = Random.Range(0f, total);
        float current = 0f;

        for (int i = 0; i < 5; i++)
        {
            current += weights[i];
            if (roll <= current) return i;
        }

        return 0; // Ne olur ne olmaz kenks
    }

    public void EditButton()
    {
        SelectRarity();
        iconImage.sprite = upgradeData.upgradeIcon;
        titleText.text = upgradeData.upgradeName;
        descText.text = string.Format(upgradeData.upgradeDescription, (int)increase);
    }

    public void SelectRarity()
    {
        rarity = GetRarityByLuck();

        switch (rarity)
        {
            case 0: // Common
                increase = Random.Range(2, 5);
                GetComponent<Image>().color = new Color32(0, 77, 255, 87);
                break;
            case 1: // Uncommon
                increase = Random.Range(5, 9);
                GetComponent<Image>().color = new Color32(0, 200, 80, 120);
                break;
            case 2: // Rare
                increase = Random.Range(9, 14);
                GetComponent<Image>().color = new Color32(120, 80, 255, 140);
                break;
            case 3: // Epic
                increase = Random.Range(14, 20);
                GetComponent<Image>().color = new Color32(255, 0, 38, 160);
                break;
            case 4: // Legendary
                increase = Random.Range(20, 27);
                GetComponent<Image>().color = new Color32(255, 170, 0, 180);
                break;
        }
    }
}
