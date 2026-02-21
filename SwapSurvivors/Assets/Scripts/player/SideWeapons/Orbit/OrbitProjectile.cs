using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitProjectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private GameObject player;

    // Hangi düşmana en son ne zaman vurduk? (InstanceID, Zaman)
    private Dictionary<int, float> lastHitTimes = new Dictionary<int, float>();
    [SerializeField] private float hitDelay = 0.5f;

    public void Init(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    private void Awake() => player = GameObject.FindGameObjectWithTag("Player");

    private void Start()
    {
        StartCoroutine(Scale());
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (player == null) return;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out IEnemy enemyController))
            {
                if (enemyController.IsDead) return;

                int enemyID = collision.gameObject.GetInstanceID();
                // Eğer bu düşmana daha önce vurulmadıysa veya bekleme süresi dolduysa
                if (!lastHitTimes.ContainsKey(enemyID) || Time.time >= lastHitTimes[enemyID] + hitDelay)
                {
                    enemyController.TakeDamage(damage);

                    // Vuruş zamanını güncelle
                    lastHitTimes[enemyID] = Time.time;
                }
            }
        }
    }

    // Düşman mermiden çıkarsa listeden silinir
    private void OnTriggerExit2D(Collider2D collision)
    {
        int enemyID = collision.gameObject.GetInstanceID();
        if (lastHitTimes.ContainsKey(enemyID))
            lastHitTimes.Remove(enemyID);
    }
}
