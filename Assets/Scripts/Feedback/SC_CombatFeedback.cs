using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Lightweight combat feedback using existing VFX and audio assets.
public class SC_CombatFeedback : MonoBehaviour
{
    public static SC_CombatFeedback Instance { get; private set; }

    [Header("VFX Prefabs")]
    [SerializeField] private GameObject enemyHitVfxPrefab;
    [SerializeField] private GameObject projectileImpactVfxPrefab;
    [SerializeField] private GameObject shieldBlockVfxPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip hurtClip;

    [Header("Tuning")]
    [SerializeField] private float enemyHitVfxScale = 0.35f;
    [SerializeField] private float impactVfxScale = 0.25f;
    [SerializeField] private float blockVfxScale = 0.3f;
    [SerializeField] private float vfxLifetime = 1.25f;
    [SerializeField] private float hitVolume = 0.45f;
    [SerializeField] private float hurtVolume = 0.55f;
    [SerializeField] private float blockVolume = 0.4f;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        ValidateVfxPrefab(enemyHitVfxPrefab, nameof(enemyHitVfxPrefab));
        ValidateVfxPrefab(projectileImpactVfxPrefab, nameof(projectileImpactVfxPrefab));
        ValidateVfxPrefab(shieldBlockVfxPrefab, nameof(shieldBlockVfxPrefab));
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        ValidateVfxPrefab(enemyHitVfxPrefab, nameof(enemyHitVfxPrefab));
        ValidateVfxPrefab(projectileImpactVfxPrefab, nameof(projectileImpactVfxPrefab));
        ValidateVfxPrefab(shieldBlockVfxPrefab, nameof(shieldBlockVfxPrefab));
    }
#endif

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void PlayEnemyHit(Vector3 worldPosition)
    {
        SpawnVfx(enemyHitVfxPrefab, worldPosition, enemyHitVfxScale);
        PlayOneShot(hitClip, hitVolume, UnityEngine.Random.Range(0.95f, 1.08f));
    }

    public void PlayProjectileImpact(Vector3 worldPosition)
    {
        GameObject prefab = projectileImpactVfxPrefab != null
            ? projectileImpactVfxPrefab
            : enemyHitVfxPrefab;

        SpawnVfx(prefab, worldPosition, impactVfxScale);
        PlayOneShot(hitClip, hitVolume * 0.65f, UnityEngine.Random.Range(0.9f, 1.05f));
    }

    public void PlayShieldBlock(Vector3 worldPosition)
    {
        GameObject prefab = shieldBlockVfxPrefab != null
            ? shieldBlockVfxPrefab
            : enemyHitVfxPrefab;

        SpawnVfx(prefab, worldPosition, blockVfxScale);
        PlayOneShot(hitClip, blockVolume, UnityEngine.Random.Range(0.85f, 1f));
    }

    public void PlayPlayerHurt()
    {
        PlayOneShot(hurtClip, hurtVolume, UnityEngine.Random.Range(0.82f, 0.95f));

        CameraFollow cameraFollow = Camera.main != null
            ? Camera.main.GetComponent<CameraFollow>()
            : null;

        cameraFollow?.PlayDamageShake();
    }

    private void SpawnVfx(GameObject prefab, Vector3 worldPosition, float scale)
    {
        if (prefab == null)
            return;

        GameObject instance;
        try
        {
            instance = Instantiate(prefab, worldPosition, Quaternion.identity);
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(
                $"[SC_CombatFeedback] Failed to instantiate VFX prefab '{prefab.name}': {ex.Message}",
                this);
#endif
            return;
        }

        if (instance == null)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(
                $"[SC_CombatFeedback] Instantiate returned null for VFX prefab '{prefab.name}'. Re-assign the prefab in the Inspector.",
                this);
#endif
            return;
        }

        instance.transform.localScale = Vector3.one * scale;
        Destroy(instance, vfxLifetime);
    }

    private void ValidateVfxPrefab(GameObject prefab, string fieldName)
    {
        if (prefab == null)
            return;

#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(prefab);
        if (string.IsNullOrEmpty(assetPath))
        {
            Debug.LogWarning(
                $"[SC_CombatFeedback] {fieldName} is not a valid project prefab ('{prefab.name}'). Drag a prefab from the Project window onto CombatFeedback.",
                this);
        }
#elif DEVELOPMENT_BUILD
        if (prefab.scene.IsValid())
        {
            Debug.LogWarning(
                $"[SC_CombatFeedback] {fieldName} references a scene object ('{prefab.name}') instead of a prefab asset. Assign a prefab from the Project window.",
                this);
        }
#endif
    }

    private void PlayOneShot(AudioClip clip, float volume, float pitch)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
        audioSource.pitch = 1f;
    }
}
