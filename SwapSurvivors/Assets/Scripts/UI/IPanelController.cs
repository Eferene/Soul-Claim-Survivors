using UnityEngine;

public class IPanelController : MonoBehaviour, IPanel
{
    public void OpenAndClosePanel()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}