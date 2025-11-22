using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    // Singleton instance
    private static PlayerStats _instance;
    public static PlayerStats Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<PlayerStats>("PlayerStats"); // resources klasöründen asseti yükler
                _instance.Initialize(); // Başlangıç değerlerini ayarlar
            }
            return _instance;
        }
    }

    // Fields
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _damage;

    private int _health;

    // Properties
    public int PlayerHealth { get { return _health; } }

    public void Initialize()
    {
        _health = _maxHealth;
    }

    // Health management methods
    public void DecreaseHealt(int amount)
    {
        _health -= amount;
        if (_health < 0) _health = 0;
    }

    public void IncreaseHealt(int amount)
    {
        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
    }

    // Damage management methods
    public void DecreaseDamage(int amount)
    {
        _damage -= amount;
        if (_damage < 0) _damage = 0;
    }

    public void IncreaseDamage(int amount)
    {
        _damage += amount;
    }
}
