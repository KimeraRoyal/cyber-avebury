using TMPro;
using UnityEngine;

namespace CyberAvebury
{
    public class GPSDebug : MonoBehaviour
    {
        private PlayerController m_player;

        private TMP_Text m_text;
        
        [TextArea(3, 5)] [SerializeField] private string m_format = "{0}\n{1}\n{2}\n{3}\n{4}\n{5}";
        
        private bool m_dirty;

        private int m_updateCount;
        private double m_timestamp;

        private LatLng m_playerCoordinates;
        private Vector3 m_playerPosition;

        private LatLng m_lastCoordinates;
        private Vector3 m_lastPosition;
        
        private void Awake()
        {
            if (!Debug.isDebugBuild)
            {
                gameObject.SetActive(false);
                return;
            }

            m_player = FindAnyObjectByType<PlayerController>();
            m_player.OnLocationInfoUpdated.AddListener(OnPlayerLocationInfoUpdated);
            m_player.OnLocationUpdated.AddListener(OnPlayerLocationUpdated);
            m_player.OnWorldPositionUpdated.AddListener(OnPlayerWorldPositionUpdated);

            m_text = GetComponentInChildren<TMP_Text>();
        }

        private void OnPlayerLocationInfoUpdated(LocationInfo _gpsInfo)
        {
            m_updateCount++;
            m_timestamp = _gpsInfo.timestamp;
            m_dirty = true;
        }

        private void Update()
        {
            if(!m_dirty) { return; }

            m_text.text = string.Format(m_format, m_updateCount, m_timestamp, m_playerCoordinates, m_playerPosition, m_lastCoordinates, m_lastPosition);         
            m_dirty = false;
        }

        private void OnPlayerLocationUpdated(LatLng _coordinates)
        {
            m_lastCoordinates = m_playerCoordinates;
            m_playerCoordinates = _coordinates;
            m_dirty = true;
        }

        private void OnPlayerWorldPositionUpdated(Vector3 _position)
        {
            m_lastPosition = m_playerPosition;
            m_playerPosition = _position;
            m_dirty = true;
        }
    }
}