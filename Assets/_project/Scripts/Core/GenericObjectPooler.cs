using UnityEngine;
using UnityEngine.Pool;

public class GenericObjectPooler : MonoBehaviour
{
    [Header("Settings Gudang")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int defaultSize = 10;
    [SerializeField] private int maxSize = 20;

    [Header("Settings Barang")]
    [SerializeField] private bool useAutoReturn = true;
    [SerializeField] private float autoReturnTime = 2f;

    private IObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: CreateItem,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: true,
            defaultCapacity: defaultSize,
            maxSize: maxSize
        );
    }

    // --- LOGIKA INTERNAL POOL ---
    private GameObject CreateItem()
    {
        GameObject obj = Instantiate(prefab);
        
        // Beri "Label" agar objek tahu cara pulang ke gudang ini
        var itemComponent = obj.AddComponent<PoolItem>();
        itemComponent.Setup(_pool, useAutoReturn, autoReturnTime);
        
        return obj;
    }

    private void OnGet(GameObject obj) => obj.SetActive(true);
    private void OnRelease(GameObject obj) => obj.SetActive(false);
    private void OnDestroyItem(GameObject obj) => Destroy(obj);

    // --- FUNGSI AKSES PUBLIK ---
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _pool.Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }
}