using UnityEngine;

public class ScytheAnimationEvents : MonoBehaviour
{
    [SerializeField] private ScyhteCharacter scytheCharacter;

    // Level 1 ve 2 saldırı animasyonları bittikten sonra çağrılır
    public void OnAnimationFinish()
    {
        if (!scytheCharacter.CheckCombo())
            gameObject.SetActive(false);
    }
}