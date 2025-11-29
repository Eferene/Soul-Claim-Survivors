using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            UpdateScoreText();
            UpdateHealthSlider();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + PlayerStats.Instance.PlayerScore;
    }

    public void UpdateHealthSlider()
    {
        healthText.text = PlayerStats.Instance.PlayerHealth + "/" + PlayerStats.Instance.PlayerMaxHealth;
        healthSlider.value = PlayerStats.Instance.PlayerHealth / PlayerStats.Instance.PlayerMaxHealth;
    }
}