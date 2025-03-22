using UnityEngine;
using UnityEngine.Pool;

namespace CyberAvebury
{
    [RequireComponent(typeof(RectTransform))]
    public class DummyPool : MonoBehaviour
    {
        private IObjectPool<DummyWord> m_pool;

        [SerializeField] private DummyWord m_prefab;
        
        private void Awake()
        {
            m_pool = new ObjectPool<DummyWord>(Create, Take, Return, Destroy, true, 10, 100);
        }

        public DummyWord Get()
            => m_pool.Get();

        public void Release(DummyWord _particles)
            => m_pool.Release(_particles);

        private DummyWord Create()
        {
            var dummy = Instantiate(m_prefab, transform);
            return dummy;
        }

        private void Destroy(DummyWord _dummy)
        {
            Destroy(_dummy.gameObject);
        }

        private void Take(DummyWord _dummy)
        {
            _dummy.gameObject.SetActive(true);
        }

        private void Return(DummyWord _dummy)
        {
            _dummy.gameObject.SetActive(false);
        }
    }
}
