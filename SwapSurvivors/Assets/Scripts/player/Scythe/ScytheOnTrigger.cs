using UnityEngine;

public class ScytheOnTrigger : MonoBehaviour
{
    PlayerManager playerManager;
    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }

    private void Start()
    {
        transform.localScale = new Vector3(playerManager.CurrentRange / 8, playerManager.CurrentRange / 4, 1f);
        transform.localPosition = new Vector3(0f, transform.localScale.y + 0.5f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyController enemyController))
            {
                float damage = playerManager.GiveDamageCharacter();
                enemyController.TakeDamage(damage);
                //Debug.Log($"{collision.name} gelen {damage} hasarı yedi.");
            }
        }
    }
}
