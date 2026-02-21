using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region --- Configuration ---
    [Header("Base Stats")]
    [SerializeField] private PlayerStats baseStats;

    // Logaritmik Soft Cap Ayarları (Sabitler)
    private const float MAX_LUCK = 100f;        // Luck max bu değere yaklaşır
    private const float MAX_LIFESTEAL = 0.1f;   // Max %10 can çalma
    private const float MAX_ARMOR_RED = 0.85f;  // Max %85 hasar azaltma
    private const float MAX_HP_REGEN_PERC = 0.04f; // Yarım saniyede MaxHP'nin en fazla %4'u yenilenir
    private const float MAX_PICKUP = 300f; // Pickup range max %300 olur

    // Soft Cap eğim hızı (B değeri). Bu değer ne kadar büyükse cap'e ulaşmak o kadar zorlaşır.
    private const float SCALE_STANDARD = 333f; // Oyuncuya gözüken değer yaklaşık 500 civarı iken sınıra ulaşır
    #endregion

    #region --- Runtime Modifiers ---

    // Bu değerler Bonustur. 0.1f = %10 artış demektir.
    private float _damageMod = 0f;
    private float _rangeMod = 0f;
    private float _speedMod = 0f;
    private float _attackSpeedMod = 0f;

    private float _flatMaxHpBonus = 0f;

    // Bu statlar birikir, hesaplanırken logaritmik cap uygulanır
    private float _hpRegenStat = 0f;
    private float _lifeStealStat = 0f;
    private float _armorStat = 0f;
    private float _luckStat = 0f;
    private float _pickupStat = 0f;

    private float _critChanceMod = 0f; // 0.05 = %5
    private float _critDamageMod = 0f; // çarpan

    private float _regenTimer = 0f;
    #endregion

    #region --- Public Properties ---
    // ---- UI için değerler ---
    public float UIDamageScore => 1 + (_damageMod / 100);
    public float UIRangeScore => _rangeMod;
    public float UISpeedScore => _speedMod;
    public float UIAttackSpeedScore => _attackSpeedMod;
    public float UIRegenScore => _hpRegenStat;
    public float UILifeStealScore => _lifeStealStat;
    public float UIArmorScore => _armorStat;
    public float UILuckScore => _luckStat;
    public float UICritChanceScore => _critChanceMod;
    public float UICritDamageScore => 1 + (_critDamageMod / 100);
    public float UIPickupScore => _pickupStat;

    // --- Doğrudan Çarpan & Bonus Stats ---
    public float MaxHealth => baseStats.MaxHealth + _flatMaxHpBonus;
    public float CurrentDamage => baseStats.AttackDamage * (1 + (_damageMod / 100));
    public float CurrentRange => baseStats.AttackRange * (1 + (_rangeMod / 100));
    public float CurrentSpeed => baseStats.MovementSpeed * (1 + (_speedMod / 100));
    public float CurrentCooldown => baseStats.AttackCooldown / (1 + (_attackSpeedMod / 100));

    // --- Logaritmik & Cap Stats ---
    public float RegenPercentPerSec => ApplySoftCap(_hpRegenStat, MAX_HP_REGEN_PERC, SCALE_STANDARD); // MaxHP'nin yüzde kaçı yenilenecek? Max %10
    public float LifeStealRate => ApplySoftCap(_lifeStealStat, MAX_LIFESTEAL, SCALE_STANDARD); // Vurulan hasarın yüzde kaçı dönecek? Max %10
    public float DamageReduction => ApplySoftCap(_armorStat, MAX_ARMOR_RED, SCALE_STANDARD); // Alınan hasarın yüzde kaçı engellenecek? Max %85
    public float CurrentLuck => ApplySoftCap(_luckStat, MAX_LUCK, SCALE_STANDARD); // Max 100
    public float CritChance => Mathf.Clamp01(_critChanceMod / 100); // 0-1 arası clamp
    public float CritMultiplier => 1 + (_critDamageMod / 100); // Base + bonus
    public float PickUpRange => ApplySoftCap(_pickupStat, MAX_PICKUP, SCALE_STANDARD);
    #endregion

    #region --- State Variables ---
    public float CurrentHealth { get; private set; }
    public int CharacterLevel { get; private set; } = 1;
    public int CurrentExperience { get; private set; } = 0;
    public int Level { get; private set; } = 0;
    public int Score { get; private set; } = 0;
    public int Token { get; private set; } = 50;
    public int Gold { get; set; } = 0;
    #endregion

    #region --- Events ---
    public event Action<float, float, float> OnHealthChanged; // Current, Max, ChangeAmount
    public event Action<int> OnScoreChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnCharacterLevelChanged;
    public event Action OnPlayerDied;
    public event Action<bool> OnDamageHitOccurred; // IsCrit?
    public event Action OnStatsUpdated; // UI update için
    public event Action<int, int> OnXPGained; // XP miktarı ve gerekli XP
    public event Action<int> OnLevelUped; // Yeni seviye
    public event Action<int> OnTokenChanged; // Yeni token miktarı
    public event Action<int> OnGoldChanged; // Yeni altın miktarı
    #endregion

    #region --- Unity Lifecycle Methods ---

    private void Awake() => InitializeStats();

    private void Update() => HandleRegeneration();

    private void InitializeStats()
    {
        // Base upgrade stats
        _damageMod = baseStats.StartingDamageBonus;
        _rangeMod = baseStats.StartingRangeBonus;
        _speedMod = baseStats.StartingSpeedBonus;
        _attackSpeedMod = baseStats.StartingAttackSpeedBonus;

        _flatMaxHpBonus = 0f;

        _hpRegenStat = baseStats.BaseHealthRegenPercent;
        _lifeStealStat = baseStats.BaseLifeStealPercent;
        _armorStat = baseStats.BaseArmorPercent;
        _luckStat = baseStats.BaseLuck;

        _critChanceMod = baseStats.BaseCritChance;
        _critDamageMod = baseStats.BaseCritMultiplier;

        _pickupStat = baseStats.BasePickUpRange;

        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth, 0f);

        // --- Stat Analiz Logu ---
        Debug.Log($"<color=green>=== Başlangıç Stat Analizi (Puan -> Gerçek Değer) ===</color>\n" +
                  $"Luck: {_luckStat} Puan -> <color=yellow>{CurrentLuck:F2}</color>\n" +
                  $"Lifesteal: {_lifeStealStat} Puan -> <color=yellow>%{LifeStealRate * 100f:F2}</color>\n" +
                  $"Armor: {_armorStat} Puan -> <color=yellow>%{DamageReduction * 100f:F2}</color>\n" +
                  $"Regen: {_hpRegenStat} Puan -> <color=yellow>%{RegenPercentPerSec * 100f:F2}</color>\n" +
                  $"Pickup: {_pickupStat} Puan -> <color=yellow>%{PickUpRange:F2}</color>");
    }

    #endregion

    #region --- Health Management Methods ---
    public void TakeDamageCharacter(float rawDamage)
    {
        float takenDamage = rawDamage * (1f - DamageReduction);

        CurrentHealth -= takenDamage;

        // CameraFX.Instance.DamagePunch(MaxHealth, takenDamage);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Debug.Log("Wasted");
            OnPlayerDied?.Invoke();
        }

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth, takenDamage);
    }

    public void HealCharacter(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth, amount);
    }

    private void HandleRegeneration()
    {
        if (CurrentHealth >= MaxHealth || CurrentHealth <= 0)
        {
            _regenTimer = 0f;
            return;
        }

        _regenTimer += Time.deltaTime;

        if (_regenTimer >= 0.5f)
        {
            float regenPercent = RegenPercentPerSec;
            if (regenPercent > 0)
            {
                // Yarım saniyede MaxHP'nin %X'i kadar iyileş
                float amount = MaxHealth * regenPercent;
                HealCharacter(amount);
            }
            _regenTimer = 0f;
        }
    }
    #endregion

    #region --- Damage Management Methods ---
    public float CalculateDamage()
    {
        // Damage hesabı
        float damage = CurrentDamage;

        // Randomize
        float variance = baseStats.DamageVariance;
        damage *= UnityEngine.Random.Range(1f - variance, 1f + variance);

        // Crit hesabı
        bool isCriticalHit = UnityEngine.Random.value <= CritChance;
        if (isCriticalHit)
            damage *= CritMultiplier;

        OnDamageHitOccurred?.Invoke(isCriticalHit);

        return Mathf.RoundToInt(damage);
    }

    public void ApplyLifeSteal(float damageDealt)
    {
        float rate = LifeStealRate;
        if (rate > 0)
        {
            float healAmount = damageDealt * rate;
            HealCharacter(healAmount);
            Debug.Log($"Lifesteal çalıştı: {healAmount} can geldi.");
        }
    }
    #endregion

    #region --- Upgrade Management Methods ---

    /// <param name="amount"> 0.2 = 20% artış </param>
    public void UpgradeDamage(float amount)
    {
        _damageMod += amount;
        UIUpdate();
    }

    /// <param name="amount"> 0.2 = 20% artış </param>
    public void UpgradeRange(float amount)
    {
        _rangeMod += amount;
        UIUpdate();
    }

    /// <param name="amount"> 0.2 = 20% artış </param>
    public void UpgradeSpeed(float amount)
    {
        _speedMod += amount;
        UIUpdate();
    }

    /// <param name="amount"> 0.2 = 20% artış </param>
    public void UpgradeAttackSpeed(float amount)
    {
        _attackSpeedMod += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan HP artışı </param>
    public void UpgradeMaxHP(float amount)
    {
        _flatMaxHpBonus += amount;
        CurrentHealth += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan HP yenileme artışı </param>
    public void UpgradeHpRegenStat(float amount)
    {
        _hpRegenStat += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan yüzde artışı alır. +20 = 20% </param>
    public void UpgradeLifeStealStat(float amount)
    {
        _lifeStealStat += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan yüzde artışı alır. +5 = 5% </param>
    public void UpgradeArmorStat(float amount)
    {
        _armorStat += amount;
    }

    /// <param name="amount"> Doğrudan yüzde artışı alır. +20 = 20% </param>
    public void UpgradeLuckStat(float amount)
    {
        _luckStat += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan yüzde artışı alır. +5 = 5% </param>
    public void UpgradeCritChance(float amount)
    {
        _critChanceMod += amount;
        UIUpdate();
    }

    /// <param name="amount"> Doğrudan çarpan artışı alır. +0.5 = x0.5 artış </param>
    public void UpgradeCritDamage(float amount)
    {
        _critDamageMod += amount;
        UIUpdate();
    }

    private void UIUpdate() => OnStatsUpdated?.Invoke();
    #endregion

    #region --- Score and Level Management Methods ---
    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void AddExperience(int amount)
    {
        int neededExp;
        CurrentExperience += amount;

        while (true)
        {
            neededExp = (int)(100 * Math.Pow(1.1, Level));

            if (CurrentExperience < neededExp)
                break;

            LevelUp(neededExp);
        }
        OnXPGained?.Invoke(CurrentExperience, neededExp);
    }

    public void LevelUp(int neededExp)
    {
        Level += 1;
        CurrentExperience -= neededExp;

        Token += Level / 10 + 1; // Her 10 seviyede +1 token kazancı

        OnLevelChanged?.Invoke(Level);
        OnLevelUped?.Invoke(Level);
        OnTokenChanged?.Invoke(Token);
    }

    public void IncreaseCharacterLevel(int amount)
    {
        if (CharacterLevel < 3)
            CharacterLevel += amount;
        OnLevelChanged?.Invoke(CharacterLevel);
    }

    public void GainGold(int amount)
    {
        if (amount <= 0) return;
        Gold += amount;
        OnGoldChanged?.Invoke(Gold);
    }

    public void SpendGold(int amount)
    {
        if (amount <= 0) return;
        Gold -= amount;
        if (Gold < 0) Gold = 0;
        OnGoldChanged?.Invoke(Gold);
    }

    public void GainToken(int amount)
    {
        if (amount <= 0) return;
        Token += amount;
        OnTokenChanged?.Invoke(Token);
    }

    public void SpendToken(int amount)
    {
        if (amount <= 0) return;
        Token -= amount;
        if (Token < 0) Token = 0;
        OnTokenChanged?.Invoke(Token);
    }
    #endregion

    #region --- Math Helpers ---
    /// <summary>
    /// Logaritmik Soft Cap Formülü
    /// </summary>
    /// <param name="stat"> Biriken puan </param>
    /// <param name="maxValue"> Ulaşılabilecek teorik maksimum sınır </param>
    /// <param name="scale"> Eğrinin yumuşaklığı. Ne kadar yüksek ise o kadar yavaş artar </param>
    /// <returns></returns>
    private static float ApplySoftCap(float stat, float maxValue, float scale)
    {
        // Stat = scale ise limitin %63ü
        // Stat = 2scale ise % 86
        // Stat = 3scale ise % 95
        if (stat <= 0) return 0;
        return maxValue * (1f - Mathf.Exp(-stat / scale));
    }
    #endregion
}