using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, IPanel
{
    [SerializeField] private ShopData shopData;
    [SerializeField] private UpgradeButton[] upgradeButtons;
    [SerializeField] private GameObject panel;
    public bool playerInside = false;

    private void OnEnable()
    {
        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].shop = this;
            upgradeButtons[i].InitializeButton(GameObject.FindWithTag("Player").GetComponent<PlayerManager>());
        }
        ChooseUpgradeRandomly();
        for(int i = 0; i < upgradeButtons.Length; i++) upgradeButtons[i].EditButton();
    }

    private void ChooseUpgradeRandomly()
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
        }
    }

    public void OpenAndClosePanel()
    {
        if(!panel.activeInHierarchy)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        { 
            panel.SetActive(false);
            Time.timeScale = 1;
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
}
