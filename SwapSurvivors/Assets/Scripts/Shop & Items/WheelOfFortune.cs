using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class WheelOfFortune : MonoBehaviour, IPanel
{
    PlayerManager playerManager;
    [Header("Wheel Settings")]
    public Transform wheelTransform;
    public int numberOfPieces = 8;
    public bool playerInside = false;
    private bool isSpinned = false;
    public int spinPrice = 100;

    [Header("Piece Settings")]
    public Transform pieceParent;
    public GameObject piecePrefab;
    public Color[] pieceColorsArray;
    [SerializeField] private int[] pieceCoinValues = new int[] // Çıkacak coin değerleri burada
    {
        200,
        150,
        100,
        0,
    };

    [SerializeField] private float[] pieceChance = new float[] // Toplamlarını yüze tamamlayacak şekilde ayarla!
    {
        10f,
        25f,
        35f,
        30f,
    };

    [Header("UI Elements")]
    [SerializeField] private Transform infoObjsPanel;
    [SerializeField] private GameObject infoObj;
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private ParticleSystem burstEffect;
    [SerializeField] private Button spinButton;
    
    private List<WheelPiece> wheelPieces = new List<WheelPiece>();
    private WheelPiece selectedPiece;

    public void OpenAndClosePanel()
    {
        if (!wheelTransform.gameObject.activeInHierarchy)
        {
            wheelTransform.parent.gameObject.SetActive(true);
            particleEffect.Play();
        }
        else
        {
            wheelTransform.parent.gameObject.SetActive(false);
            particleEffect.Stop();
        }
    }

    private void OnEnable()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        playerManager.OnGoldChanged += UpdateSpinButton;
    }

    private void OnDisable()
    {
        if(playerManager != null)
        {
            playerManager.OnGoldChanged -= UpdateSpinButton;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BaseCharacterController controller = collision.GetComponent<BaseCharacterController>();

            if (controller != null)
            {
                playerInside = true;
                controller.SetCurrentObject(transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BaseCharacterController controller = collision.GetComponent<BaseCharacterController>();

            if (controller != null)
            {
                playerInside = false;
                controller.ClearCurrentObject();
            }
        }
    }

    void Awake()
    {
        CreateWheelPieces();

        GameObject playerobj = GameObject.FindWithTag("Player");
        playerManager = playerobj.GetComponent<PlayerManager>();
    }

    private void CreateWheelPieces()
    {
        wheelPieces.Clear();
        List<Color> pieceColors = pieceColorsArray.ToList();
        numberOfPieces = pieceChance.Length;

        float currentAngle = 0f;

        for (int i = 0; i < numberOfPieces; i++)
        {
            float fillAmount = pieceChance[i] / 100f;
            float angleSize = fillAmount * 360f;

            GameObject newInfoObj = Instantiate(infoObj, infoObjsPanel);
            TMP_Text infoText = newInfoObj.GetComponentInChildren<TMP_Text>();
            infoText.text = pieceCoinValues[i].ToString();
            Image infoImg = newInfoObj.GetComponentInChildren<Image>();

            GameObject piece = Instantiate(piecePrefab, pieceParent);
            Image img = piece.GetComponent<Image>();

            img.fillAmount = fillAmount;
            int colorIndex = Random.Range(0, pieceColors.Count);
            img.color = pieceColors[colorIndex];
            infoImg.color = pieceColors[colorIndex];
            pieceColors.RemoveAt(colorIndex);

            piece.transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

            wheelPieces.Add(new WheelPiece
            {
                coinValue = pieceCoinValues[i],
                startAngle = currentAngle,
                endAngle = currentAngle + angleSize,
                index = i
            });

            currentAngle += angleSize;
        }
    }

    public void Spin()
    {
        if(playerManager.Gold < spinPrice || isSpinned) return;
        
        playerManager.SpendGold(spinPrice);
        StartCoroutine(SpinWheel());

        if(spinButton != null) spinButton.interactable = false;
    }

    private IEnumerator SpinWheel()
    {
        particleEffect.Stop();
        int randomChoice = Random.Range(0, 100); // pieceChance toplamı 100 olacak şekilde ayarlandığı için 0,100 arasında değer isteniyor.
        int cumulative = 0; // Kümülatif toplam
        for(int i = 0; i < pieceChance.Length; i++)
        {
            cumulative += (int)pieceChance[i];

            if(randomChoice < cumulative)
            {
                selectedPiece = wheelPieces[i];
                break;
            }
        }

        if (isSpinned) yield break;

        float calculatedAngle = -(selectedPiece.startAngle + ((selectedPiece.endAngle - selectedPiece.startAngle) / 2));
        int spinCount = 7; // Kaç tur döneceği
        float targetAngle = 360f * spinCount + calculatedAngle; // Gidilecek açı 
        
        float duration = 4f; // Spin süresi
        float elapsed = 0f;
        float initialAngle = wheelTransform.eulerAngles.z; // Başlangıç açısı
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float rawt = elapsed / duration;
            float t = Mathf.SmoothStep(0f, 1f, rawt); // Yavaş başla - Hızlan - Yavaşla

            float angle = Mathf.Lerp(initialAngle, targetAngle, t);
            wheelTransform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
        wheelTransform.eulerAngles = new Vector3(0, 0, targetAngle);

        playerManager.GainGold(selectedPiece.coinValue);
        if(selectedPiece.coinValue > spinPrice)
        {
            burstEffect.Play();
            CameraFX.Instance.PunchCamera(new Vector3(-0.5f, 0.5f, -0.5f), 1f, 10, 1f);
        }
        particleEffect.Play();
        isSpinned = true;
    }

    private void UpdateSpinButton(int gold)
    {
        if(playerManager.Gold >= spinPrice && !isSpinned)
        {
            spinButton.interactable = true;
        }
        else
        {
            spinButton.interactable = false;
        }
    }
}

[System.Serializable]
public class WheelPiece
{
    public int coinValue;
    public float startAngle;
    public float endAngle;
    public int index;
}