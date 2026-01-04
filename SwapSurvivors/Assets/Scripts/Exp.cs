using UnityEngine;

public class Exp : MonoBehaviour
{
    public int expAmount = 1;
    public float speed = 3f;
    [SerializeField] private bool isLocked = false;
    GameObject player;
    Rigidbody2D rb;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, player.transform.position) > 0.25f && Vector2.Distance(transform.position, player.transform.position) < player.GetComponent<PlayerManager>().PickUpRange)
        {
            isLocked = true;
        }

        if(isLocked)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().AddExperience(expAmount);
            Destroy(gameObject);
        }
    }
}
