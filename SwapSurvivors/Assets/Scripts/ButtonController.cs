using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;      
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public void QuitButton(){
        Application.Quit();
    }

    public void OpenUIButton(GameObject ui){
        ui.SetActive(true);
    }

    public void CloseUIButton(GameObject ui){
        ui.SetActive(false);
    }

    public void LoadScene(int id)
    {
        SceneData.sceneToLoad = id;
        SceneManager.LoadScene("LoadingScene");
    }

    public void RefreshAllUpgradeButtons()
    {
        Shop shop = FindFirstObjectByType<Shop>();
        PlayerManager playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        if (shop != null && playerManager.Gold >= shop.refreshCost)
        {
            shop.refreshCount++;
            shop.ChooseUpgradeRandomly();
            playerManager.SpendGold(shop.refreshCost);
        }
    }
}
