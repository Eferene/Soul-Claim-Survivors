using UnityEngine;

public class WeaponShop : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject panel;
    public bool playerInside = false;

    public void OpenAndClosePanel()
    {
        if(!panel.activeInHierarchy)
        {
            panel.SetActive(true);
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
}
