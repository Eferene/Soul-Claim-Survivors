using Unity.VisualScripting;
using UnityEngine;

public class CharacterTransformManager : MonoBehaviour
{
    [SerializeField] private GameObject scytheCharacter;
    [SerializeField] private GameObject shotgunCharacter;
    private PlayerManager playerManager;

    private GameObject currentCharacter;
    private bool isT = true;

    private void Start()
    {
        currentCharacter = GameObject.FindWithTag("Player");
        playerManager = currentCharacter.GetComponent<PlayerManager>();
    }

    public void TransformCharacter()
    {
        Vector2 spawnPos;
        if (isT)
        {
            spawnPos = currentCharacter.transform.position;
            Destroy(currentCharacter);
            currentCharacter = Instantiate(shotgunCharacter, spawnPos, Quaternion.identity);
            isT = false;
        }
        else
        {
            spawnPos = currentCharacter.transform.position;
            Destroy(currentCharacter);
            currentCharacter = Instantiate(scytheCharacter, spawnPos, Quaternion.identity);
            isT = true;
        }
    }

    // deneme amaçlı
    public void IncLevel() => playerManager.IncreaseCharacterLevel(1);
    public void DecLevel() => playerManager.IncreaseCharacterLevel(-1);
}
