using UnityEngine;
using DG.Tweening;

public class CameraFX : MonoBehaviour
{
    public static CameraFX Instance;
    private Camera mainCamera;
    private CameraFollower cameraFollower;

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
        mainCamera = Camera.main;
        cameraFollower = mainCamera.GetComponent<CameraFollower>();
    }

    public void ShakeCamera(float duration, float strength, int vibrato)
    {
        mainCamera.transform.DOShakePosition(duration, strength, vibrato);
    }

    public void PunchCamera(Vector3 punch, float duration, int vibrato, float elasticity)
    {
        mainCamera.transform.DOPunchPosition(punch, duration, vibrato, elasticity);
    }

    public void ZoomCamera(float targetSize, float duration)
    {
        mainCamera.DOOrthoSize(targetSize, duration);
    }

    public void DamagePunch(float damageAmount)
    {
        float invLerp = Mathf.InverseLerp(0, 100, damageAmount);
        float punchAmount = Mathf.Lerp(0.1f, 0.5f, invLerp);
        float duration = Mathf.Lerp(0.1f, 0.5f, invLerp);
        float vibrato = Mathf.Lerp(5, 20, invLerp);
        float elasticity = Mathf.Lerp(0.5f, 1.5f, invLerp);
        PunchCamera(Vector3.one * punchAmount, duration, (int)vibrato, elasticity);
    }
}
