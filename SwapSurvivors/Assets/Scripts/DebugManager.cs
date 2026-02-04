using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance;

    [SerializeField] private bool enableDebugMenu = false;
    private PlayerManager playerManager;

    [Header("Activated Features")]
    [SerializeField] private bool isMenuOpen = false;
    [SerializeField] private bool isGodMode = false;

    // --- GUI VARIABLES ---
    private float windowWidth = 250f;
    private Rect windowRect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        windowRect = new Rect(20, 20, windowWidth, 0);
    }

    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (enableDebugMenu)
        {
            enableDebugMenu = false;
            isMenuOpen = !isMenuOpen;
            
            if (isMenuOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void OnGUI()
    {
        if (!isMenuOpen) return;

        windowRect = GUILayout.Window(999, windowRect, DrawDebugWindow, "Soul Claim Debug");
    }

    private void DrawDebugWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Label($"<b>Zaman Skalası: {Time.timeScale:F1}x</b>");
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("||", GUILayout.Width(40))) SetTimeScale(0f);
        if (GUILayout.Button("0.5x", GUILayout.Width(40))) SetTimeScale(0.5f);
        if (GUILayout.Button("1x", GUILayout.Width(40))) SetTimeScale(1f);
        if (GUILayout.Button("2x", GUILayout.Width(40))) SetTimeScale(2f);
        if (GUILayout.Button("5x", GUILayout.Width(40))) SetTimeScale(5f);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // --- OYUNCU CHEATLERİ ---
        GUILayout.Label("<b>Oyuncu Hileleri</b>");
        
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Canı Doldur (Heal)"))
        {
            playerManager.HealCharacter(playerManager.MaxHealth);
            Debug.Log("Can dolduruldu!");
        }

        if (GUILayout.Button("+1 Level Atla"))
        {
            playerManager.LevelUp(1);
            Debug.Log("Level atlandı!");
        }

        if (GUILayout.Button("+100 Gold Ekle"))
        {
            playerManager.GainGold(100);
            Debug.Log("100 Gold eklendi!");
        }

        if (GUILayout.Button("+5 Token Ekle"))
        {
            playerManager.GainToken(5);
            Debug.Log("5 Token eklendi!");
        }

        GUILayout.Space(10);

        // --- SAHNE KONTROLLERİ ---
        GUILayout.Label("<b>Genel İşlemler</b>");

        if (GUILayout.Button("Tüm Düşmanları Yok Et (Kill All)"))
        {
            KillAllEnemies();
        }

        if (GUILayout.Button("Sahneyi Yenile (Restart)"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SetTimeScale(1f);
        }

        GUILayout.EndVertical();
        
        GUI.DragWindow();
    }

    // --- FONKSİYONLAR ---

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    private void KillAllEnemies()
    {
        EnemyPool.Instance.ReturnAllEnemiesToPool();
        Debug.Log("Tüm düşmanlar yok edildi!");
    }
}