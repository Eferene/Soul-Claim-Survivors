using UnityEngine;
using DG.Tweening;

public class CameraFX : MonoBehaviour
{
    public static CameraFX Instance;
    private Transform mainCamera;
    private Vector3 firstLocalPos;
    private Tweener currentShakeTween;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main.transform;

        firstLocalPos = mainCamera.localPosition;
    }

    private void ControlShakeTween()
    {
        if(currentShakeTween != null && currentShakeTween.IsActive())
        {
            currentShakeTween.Complete();
        }
    }

    public void ShakeCamera(float duration, float strength, int vibrato)
    {
        ControlShakeTween();
        currentShakeTween = mainCamera.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
        {
            mainCamera.localPosition = firstLocalPos;
        });
    }

    public void PunchCamera(Vector3 punch, float duration, int vibrato, float elasticity)
    {
        ControlShakeTween();
        currentShakeTween = mainCamera.DOPunchPosition(punch, duration, vibrato, elasticity).OnComplete(() =>
        {
            mainCamera.localPosition = firstLocalPos;
        });
    }

    public void DamagePunch(float maxHealth, float damageAmount)
    {
        float damagePercent = damageAmount / maxHealth;
        float punchAmount = Mathf.Clamp(damagePercent * 0.5f, 0.1f, 0.5f);
        PunchCamera(Vector3.one * punchAmount, 0.2f, 10, 1f);
    }
}
