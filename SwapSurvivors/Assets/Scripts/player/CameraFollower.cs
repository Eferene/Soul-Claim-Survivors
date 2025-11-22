using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody2D playerRB;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        playerRB = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        Vector2 playerPos = playerRB.transform.position;
        mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, -10);
    }
}
