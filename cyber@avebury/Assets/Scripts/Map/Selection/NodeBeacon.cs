using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace CyberAvebury
{
    public class NodeBeacon : MonoBehaviour
    {
        private NodeSelection m_selection;
        private Player m_player;

        private Node m_node;

        private ParticleSystem m_particles;

        [SerializeField] private Color m_farColor;
        [SerializeField] private Color m_nearColor;

        private bool m_near;
        private bool m_dirty;
        
        private void Awake()
        {
            m_selection = FindAnyObjectByType<NodeSelection>();
            m_player = FindAnyObjectByType<Player>();

            m_node = GetComponentInParent<Node>();

            m_particles = GetComponent<ParticleSystem>();
            
            m_node.OnStateChanged.AddListener(OnNodeStateChanged);
        }

        private void Start()
        {
            CalculateDistance();
            m_dirty = true;
            UpdateColor();
        }

        private void Update()
        {
            CalculateDistance();
            UpdateColor();
        }

        private void CalculateDistance()
        {
            var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), m_player.GetFlatPosition());

            var near = distance <= m_selection.MaxDistance;
            if(m_near == near) { return; }
            m_near = near;

            m_dirty = true;
        }

        private void UpdateColor()
        {
            if(!m_dirty) { return; }

            var mainModule = m_particles.main;
            mainModule.startColor = m_near ? m_nearColor : m_farColor;
        }

        private void OnNodeStateChanged(NodeState _state)
        {
            var emission = m_particles.emission;
            switch (_state)
            {
                case NodeState.Locked:
                    emission.enabled = false;
                    m_particles.Stop();
                    break;
                case NodeState.Unlocked:
                    emission.enabled = true;
                    m_particles.Play();
                    break;
                case NodeState.Completed:
                    emission.enabled = false;

                    var main = m_particles.main;
                    StartCoroutine(StopParticles(main.duration + 1.0f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
            m_dirty = true;
        }

        private IEnumerator StopParticles(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            m_particles.Stop();
        }
    }
}
