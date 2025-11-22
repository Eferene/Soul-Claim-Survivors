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
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private float _damage;

    private float _health;

    // Properties
    public float PlayerHealth { get { return _health; } }

    public void Initialize()
    {
        _health = _maxHealth;
    }

    // Health management methods
    public void DecreaseHealth(float amount)
    {
        _health -= amount;
        if (_health < 0) _health = 0;
    }

    public void IncreaseHealth(float amount)
    {
        _health += amount;
        if (_health > _maxHealth) _health = _maxHealth;
    }

    // Damage management methods
    public void DecreaseDamage(float amount)
    {
        _damage -= amount;
        if (_damage < 0) _damage = 0;
    }

    public void IncreaseDamage(float amount)
    {
        _damage += amount;
    }
}
