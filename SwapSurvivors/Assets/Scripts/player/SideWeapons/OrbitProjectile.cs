using System.Collections;
using UnityEngine;

public class OrbitProjectile : MonoBehaviour
{
    private float speed;
    private GameObject player;
    private PlayerManager playerManager;

    public void Init(float speed) => this.speed = speed;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
    }
    private void Start() => transform.localScale = Vector3.zero;
    private void Update()
    {
        StartCoroutine(Scale());
        transform.RotateAround(player.transform.position, Vector3.forward, speed * Time.deltaTime);
    }

    private IEnumerator Scale()
    {
        float scale = 0;
        while (scale < 1f)
        {
            scale = Mathf.MoveTowards(scale, 1f, 1f * Time.deltaTime);
            transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out IEnemy enemyController))
            {
                if (enemyController.IsDead) return;

                float damage = playerManager.CalculateDamage();
                enemyController.TakeDamage(damage);
                playerManager.ApplyLifeSteal(damage);
            }
        }
    }
}
