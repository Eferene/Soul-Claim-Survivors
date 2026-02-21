using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    private InputActions controls;

    [Header("In Game UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveDurationText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image expSlider;

    [Header("Stats Panel Elements")]
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
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI tokenText;

    [Header("Other References")]
    public List<GameObject> allPanels = new List<GameObject>();

    private PlayerManager playerManager;

    private void Awake()
    {
        controls = new InputActions();
        controls.UI.Pause.performed += ctx => ActivateESCMenu();

        GameObject playerobj = GameObject.FindWithTag("Player");
        playerManager = playerobj.GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        playerManager.OnHealthChanged += UpdateHealthSlider;
        playerManager.OnScoreChanged += UpdateScoreText;
        playerManager.OnStatsUpdated += UpdateUpgradeTexts;
        playerManager.OnXPGained += UpdateLevelSlider;
        playerManager.OnLevelUped += UpdateLevelText;
        playerManager.OnGoldChanged += UpdateGold;
        playerManager.OnTokenChanged += UpdateTokens;
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        playerManager.OnHealthChanged -= UpdateHealthSlider;
        playerManager.OnScoreChanged -= UpdateScoreText;
        playerManager.OnStatsUpdated -= UpdateUpgradeTexts;
        playerManager.OnXPGained -= UpdateLevelSlider;
        playerManager.OnLevelUped -= UpdateLevelText;
        playerManager.OnGoldChanged -= UpdateGold;
        playerManager.OnTokenChanged -= UpdateTokens;
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

        goldText.text = $"{playerManager.Gold}";
        tokenText.text = $"{playerManager.Token}";
    }

    private void UpdateGold(int gold) => goldText.text = $"{gold}";
    private void UpdateTokens(int token) => tokenText.text = $"{token}";

    private void UpdateScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHealthSlider(float currentHealth, float maxHealth, float d)
    {
        int uiCurrent = Mathf.CeilToInt(currentHealth);
        healthText.text = $"{uiCurrent} / {maxHealth}";
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    public void OpenAndClosePanel(GameObject panel)
    {
        if (panel.TryGetComponent(out IPanel p)) p.OpenAndClosePanel();
        else Debug.LogWarning("IPanel tanımlanmamış.");
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void ActivateESCMenu()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            CloseAllPanels();
            OpenAndClosePanel(pauseMenu);
            Time.timeScale = 0f;
            return;
        }
        OpenAndClosePanel(pauseMenu);
        Time.timeScale = 1f;
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

    private void UpdateLevelSlider(int currentXP, int neededXP)
    {
        Debug.Log(Mathf.Clamp01((float)currentXP / neededXP));
        expSlider.fillAmount = Mathf.Clamp01((float)currentXP / neededXP);
    }
    private void UpdateLevelText(int level) => levelText.text = $"Level: {level}";
}