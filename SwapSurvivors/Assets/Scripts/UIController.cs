using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    private InputActions controls;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveDurationText;
    [SerializeField] private GameObject pauseMenu;

    // Upgrade Texts
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    [SerializeField] private TextMeshProUGUI healthRegenText;
    [SerializeField] private TextMeshProUGUI lifeStealText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI criticalHitText;
    [SerializeField] private TextMeshProUGUI criticalDamageText;
    [SerializeField] private TextMeshProUGUI luckText;



    private PlayerManager playerManager;

    private void Awake()
    {
        controls = new InputActions();
        controls.UI.Pause.performed += ctx => OpenAndClosePanel(pauseMenu);

        GameObject playerobj = GameObject.FindWithTag("Player");
        playerManager = playerobj.GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        playerManager.OnHealthChanged += UpdateHealthSlider;
        playerManager.OnScoreChanged += UpdateScoreText;
        playerManager.OnStatsUpdated += UpdateUpgradeTexts;
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        playerManager.OnHealthChanged -= UpdateHealthSlider;
        playerManager.OnScoreChanged -= UpdateScoreText;
        playerManager.OnStatsUpdated -= UpdateUpgradeTexts;
        controls.UI.Disable();
    }

    private void Start()
    {
        UpdateScoreText(playerManager.Score);
        UpdateHealthSlider(playerManager.CurrentHealth, playerManager.MaxHealth, 1f);
        UpdateUpgradeTexts();
    }

    private void UpdateUpgradeTexts()
    {
        damageText.text = $"Damage: {playerManager.UIDamageScore}x";
        rangeText.text = $"Range: {playerManager.UIRangeScore}%";
        speedText.text = $"Speed: {playerManager.UISpeedScore}%";
        attackSpeedText.text = $"Attack Speed: {playerManager.UIAttackSpeedScore}%";

        maxHealthText.text = "Max Health: " + playerManager.MaxHealth;
        healthRegenText.text = "Health Regen: " + playerManager.UIRegenScore;
        lifeStealText.text = "Life Steal: " + playerManager.UILifeStealScore;
        armorText.text = "Armor: " + playerManager.UIArmorScore;
        luckText.text = "Luck: " + playerManager.UILuckScore;

        criticalHitText.text = $"Critical Chance: {playerManager.UICritChanceScore}%";
        criticalDamageText.text = $"Critical Damage: {playerManager.UICritDamageScore}x";

    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHealthSlider(float maxHealt, float currentHealt, float d)
    {
        healthText.text = playerManager.CurrentHealth + "/" + playerManager.MaxHealth;
        healthImage.fillAmount = playerManager.CurrentHealth / playerManager.MaxHealth;
    }

    public void OpenAndClosePanel(GameObject panel)
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public IEnumerator StartWaveTimer(int duration)
    {
        int minutes, seconds;
        while (duration > 0)
        {
            minutes = Mathf.FloorToInt(duration / 60);
            seconds = Mathf.FloorToInt(duration % 60);
            waveDurationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            duration -= 1;
            yield return new WaitForSeconds(1);
        }
        minutes = Mathf.FloorToInt(duration / 60);
        seconds = Mathf.FloorToInt(duration % 60);
        waveDurationText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}