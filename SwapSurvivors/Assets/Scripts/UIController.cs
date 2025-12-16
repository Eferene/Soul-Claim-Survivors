using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    private InputActions controls;

    [Header("In Game UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveDurationText;
    [SerializeField] private GameObject pauseMenu;

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
        damageText.text = "Damage: " + playerManager.CurrentDamage;
        rangeText.text = "Range: " + playerManager.CurrentRange;
        speedText.text = "Speed: " + playerManager.CurrentSpeed;
        attackSpeedText.text = "Attack Speed: " + playerManager.CurrentCooldown;

        maxHealthText.text = "Max Health: " + playerManager.MaxHealth;
        healthRegenText.text = "Health Regen: " + playerManager.RawRegenScore;
        lifeStealText.text = "Life Steal: " + playerManager.RawLifeStealScore;
        armorText.text = $"Armor: {playerManager.RawArmorScore}% ";

        criticalHitText.text = $"Critical Chance: {playerManager.CritChance * 100}%";
        criticalDamageText.text = $"Critical Damage: {playerManager.CritMultiplier}x";

        luckText.text = "Luck: " + playerManager.RawLuckScore;
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

    public void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    }

    public void ActivateESCMenu()
    {
        if(!pauseMenu.activeInHierarchy)
        {
            CloseAllPanels();
            OpenAndClosePanel(pauseMenu);
            return;
        }
        OpenAndClosePanel(pauseMenu);
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