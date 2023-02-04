using UnityEngine;

public class Monster : MonoBehaviour
{
    [HideInInspector] public int currentHealth;
    public int maxHealth;
    public int damage;

    public GameObject spritePrefab;
    public GameObject teleportPrefab;

    private GameObject _teleport;
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        Invoke(nameof(Teleport), 1.5f);
    }

    private void Teleport()
    {
        _spriteRenderer.enabled = false;
        _teleport = Instantiate(teleportPrefab, transform.position, Quaternion.identity, transform);
        Destroy(_teleport, 1f);
    }
}