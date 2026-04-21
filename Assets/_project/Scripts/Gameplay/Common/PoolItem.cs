using UnityEngine;
using UnityEngine.Pool;

public class PoolItem : MonoBehaviour
{
    private IObjectPool<GameObject> _myPool;
    private bool _autoReturn;
    private float _timer;
    private float _duration;

    public void Setup(IObjectPool<GameObject> pool, bool autoReturn, float duration)
    {
        _myPool = pool;
        _autoReturn = autoReturn;
        _duration = duration;
    }

    private void OnEnable()
    {
        if (_autoReturn) _timer = _duration;
    }

    private void Update()
    {
        if (!_autoReturn) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0) ReturnToPool();
    }

    public void ReturnToPool()
    {
        // Pastikan objek masih aktif sebelum dikembalikan untuk menghindari error
        if (gameObject.activeSelf)
        {
            _myPool.Release(gameObject);
        }
    }
}