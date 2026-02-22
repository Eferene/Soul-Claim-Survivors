using System.Collections.Generic;
using UnityEngine;

public class TimeBeam : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float duration;
    [SerializeField] private float beamWidth;
    [SerializeField] private float beamRange;
    [SerializeField] private float projectileCount;
    [SerializeField] private float cooldown;
    [SerializeField] private LayerMask enemyLayer;


    private List<RaycastHit2D> hitBuffer = new List<RaycastHit2D>();
    private ContactFilter2D filter = new ContactFilter2D();
    private float timer;

    private Vector2 lastDirection;

    private void Awake()
    {
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            for (int i = 0; i < projectileCount; i++)
                FireBeam();
            timer = 0;
        }
    }

    private void FireBeam()
    {

        lastDirection = Random.insideUnitCircle.normalized; // Rastgele bir yön
        Vector2 origin = transform.position;

        int hitCount = Physics2D.BoxCast(origin, new Vector2(0.1f, beamWidth), 0f, lastDirection, filter, hitBuffer, beamRange);


        for (int j = 0; j < hitCount; j++)
        {
            var hit = hitBuffer[j];
            if (hit.collider != null && hit.collider.TryGetComponent(out IEnemy enemy))
            {
                if (!enemy.IsDead)
                {
                    enemy.ApplyFreeze(duration);
                    Debug.Log($"Hit enemy: {enemy}");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Eğer daha önce ateş edilmediyse varsayılan olarak sağa baksın
        if (lastDirection == Vector2.zero) lastDirection = transform.right;

        Gizmos.color = Color.cyan;

        // Kutunun boyutu (Uzunluk x Genişlik)
        Vector3 boxSize = new Vector3(beamRange, beamWidth, 0f);

        // Kutunun merkezi: Başlangıç noktasından, yönün yarısı kadar ileride
        Vector3 center = transform.position + (Vector3)lastDirection * (beamRange / 2f);

        // Kutuyu yönün açısına göre döndürmemiz lazım
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);

        // Gizmos'u bu matrise göre çizdiriyoruz
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, boxSize); // Artık merkez sıfır çünkü matrisle taşıdık

        Gizmos.matrix = oldMatrix; // Matrisi geri yükle ki diğer gizmoslar sapıtmasın
    }
}
