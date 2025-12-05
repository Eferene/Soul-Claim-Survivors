using UnityEngine;

public class ScytheAnimationEvents : MonoBehaviour
{
    [SerializeField] private ScyhteCharacter scytheCharacter;

    public void DoAttack()
    {
        scytheCharacter.DoScytheHit();
    }

    // Level 1 ve 2 saldırı animasyonları bittikten sonra çağrılır
    public void CheckLevelTwoCombo()
    {
        if (!scytheCharacter.CheckCombo())
            gameObject.SetActive(false);
    }
}