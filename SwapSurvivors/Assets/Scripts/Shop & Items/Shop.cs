using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IPanel
{
    [SerializeField] private ShopData shopData;
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI refreshCostText;
    [SerializeField] private Button refreshButton;
    public bool playerInside = false;
    private PlayerManager playerManager;
    public int refreshCount = 0;
    private int firstRefreshCost = 5;
    public int refreshCost
    {
        get
        {
            return firstRefreshCost + (refreshCount * 15);
        }
    }

    private void OnEnable()
    {
        refreshCostText.text = refreshCost.ToString();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].shop = this;
            upgradeButtons[i].InitializeButton(playerManager);
        }
        ChooseUpgradeRandomly();

        playerManager.OnTokenChanged += UpdateUpgradeButtons;
        playerManager.OnGoldChanged += UpdateRefreshButton;
    }

    private void OnDisable()
    {
        if(playerManager != null)
        {
            playerManager.OnTokenChanged -= UpdateUpgradeButtons;
            playerManager.OnGoldChanged -= UpdateRefreshButton;
        }
    }

    public void ChooseUpgradeRandomly()
    {
        List<UpgradeData> upgrades = new List<UpgradeData>();
        for(int i = 0; i < shopData.upgrades.Length; i++)
        {
            upgrades.Add(shopData.upgrades[i]);
        }

        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = Random.Range(0, upgrades.Count);
            UpgradeData choosedUpgrade = upgrades[index];
            upgrades.RemoveAt(index);

            upgradeButtons[i].upgradeData = choosedUpgrade;
            upgradeButtons[i].EditButton();
        }
    }

    /*
    public void ChooseUpgradeRandomly(UpgradeButton button)
    {
        List<UpgradeData> upgrades = new List<UpgradeData>();
        for(int i = 0; i < shopData.upgrades.Length; i++)
        {
            upgrades.Add(shopData.upgrades[i]);
        }

        int index = Random.Range(0, upgrades.Count);
        UpgradeData choosedUpgrade = upgrades[index];

        button.upgradeData = choosedUpgrade;
        button.EditButton();
    }
    */

    public void OpenAndClosePanel()
    {
        if(!panel.activeInHierarchy)
        {
            panel.SetActive(true);
            foreach (var button in upgradeButtons)
            {
                button.ControlButton();
            }
        }
        else
        { 
            panel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            BaseCharacterController controller = collision.GetComponent<BaseCharacterController>();

            if(controller != null)
            {
                playerInside = true;
                controller.SetCurrentObject(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            BaseCharacterController controller = collision.GetComponent<BaseCharacterController>();

            if(controller != null)
            {
                playerInside = false;
                controller.ClearCurrentObject();
            }
        }
    }

    private void UpdateUpgradeButtons(int token)
    {
        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].ControlButton();
        }
    }

    private void UpdateRefreshButton(int gold)
    {
        if(refreshCostText == null || refreshButton == null || playerManager == null || !gameObject.activeInHierarchy)
        {
            Debug.LogWarning("refresh button güncellenemedi!");
            Debug.Log(refreshCostText);
            Debug.Log(refreshButton);
            Debug.Log(playerManager);
            Debug.Log(gameObject.activeInHierarchy);
            return;
        }

        refreshCostText.text = refreshCost.ToString();
        if(playerManager.Gold < refreshCost)
        {
            refreshButton.interactable = false;
        }
        else
        {
            refreshButton.interactable = true;
        }
    }
}
