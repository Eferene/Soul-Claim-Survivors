using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image healthImage;
    [SerializeField] TextMeshProUGUI healthText;

    private PlayerManager playerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            GameObject playerobj = GameObject.FindWithTag("Player");
            playerManager = playerobj.GetComponent<PlayerManager>();
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
        scoreText.text = "Score: " + playerManager.Score;
    }

    public void UpdateHealthSlider()
    {
        healthText.text = playerManager.CurrentHealth + "/" + playerManager.MaxHealth;
        healthImage.fillAmount = playerManager.CurrentHealth / playerManager.MaxHealth;
    }
}