using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopData shopData;
    [SerializeField] private List<int> items = new List<int>();
    [SerializeField] private GameObject panel;
    public bool playerInside = false;

    public void OpenAndCloseShop()
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
