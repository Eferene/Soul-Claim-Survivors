using UnityEngine;

public class Exp : MonoBehaviour
{
    public int expAmount = 1;
    public float speed = 0f;
    bool isLocked = false;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, player.transform.position) > 0.1f && Vector2.Distance(transform.position, player.transform.position) < 10f)
        {
            isLocked = true;
        }
        else if(Vector2.Distance(transform.position, player.transform.position) <= 0.1f)
        {
            isLocked = false;
            player.GetComponent<PlayerManager>().AddExperience(expAmount);
            Destroy(gameObject);
        }

        if(isLocked)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
        }
    }
}
