using System.Collections.Generic;
using UnityEngine;

public class TimeBeam : BaseWeaponController
{
    [Header("Settings")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Combat Stats")]
    [SerializeField] private float cooldown;
    [SerializeField] private float freezeDuration;

    [Header("Beam Shape")]
    [SerializeField] private float beamRange;
    [SerializeField] private float beamWidth;
    [SerializeField] private int projectileCount;


    private List<RaycastHit2D> hitBuffer = new List<RaycastHit2D>();
    private ContactFilter2D filter = new ContactFilter2D();

    private Vector2 lastDirection;

    protected override void Awake()
    {
        base.Awake();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;
    }

    protected override float GetCooldown() => cooldown;

    protected override void Attack()
    {
        for (int i = 0; i < projectileCount; i++)
            FireBeam();
    }

    private void FireBeam()
    {
        lastDirection = Random.insideUnitCircle.normalized; // Rastgele bir yön
        Vector2 origin = transform.position;

        int hitCount = Physics2D.BoxCast(origin, new Vector2(0.1f, beamWidth), 0f, lastDirection, filter, hitBuffer, beamRange);


        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            if (hit.collider != null && hit.collider.TryGetComponent(out IEnemy enemy))
            {
                if (!enemy.IsDead)
                    enemy.ApplyFreeze(freezeDuration);
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
