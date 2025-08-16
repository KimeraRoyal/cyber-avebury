using System.Collections;
using UnityEngine;
using UnityEngine.Android;

namespace CyberAvebury
{
    public class PlayerController : MonoBehaviour
    {
        private GPS m_gps;

        [SerializeField] private float m_accuracy = 10.0f;
        [SerializeField] private float m_updateDistance = 10.0f;

        private double m_lastUpdateTime;

#if DEBUG
        [SerializeField] private LatLng m_debugPosition;

        [SerializeField] private float m_movementSpeed = 1.0f;
        [SerializeField] private float m_rotationSpeed = 1.0f;
#endif

        private void Awake()
        {
            m_gps = FindAnyObjectByType<GPS>();
        }

        private void Start()
        {
#if DEBUG
            transform.position = m_gps.GetScenePosition(m_debugPosition);
#endif
            StartCoroutine(GpsMovement());
        }

        private void Update()
        {
            KeyboardMovement();
        }

        private IEnumerator GpsMovement()
        {
            if (Application.isEditor) { yield break; }

            yield return AuthorizePermission(Permission.FineLocation);
            if (!Input.location.isEnabledByUser)
            {
                Debug.LogError("User has location services disabled.");
                yield break;
            }
            
            Input.location.Start(m_accuracy, m_updateDistance);
            var maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            
            if (maxWait < 1)
            {
                Debug.LogError("Location services timed out.");
                yield break;
            }
            
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("Location services failed to initialise.");
                yield break;
            }
            
            while (isActiveAndEnabled)
            {
                var gpsInfo = Input.location.lastData;
                if (gpsInfo.timestamp > m_lastUpdateTime)
                {
                    m_lastUpdateTime = gpsInfo.timestamp;
                    var location = new LatLng(gpsInfo.latitude, gpsInfo.longitude);
                    MoveToCoordinates(location);
                }

                yield return null;
            }
            Input.location.Stop();
        }

        private void MoveToCoordinates(LatLng _coordinates)
        {
            var position = m_gps.GetScenePosition(_coordinates);
            transform.position = position;
        }

        private void KeyboardMovement()
        {
#if DEBUG
            var movement = transform.forward * Input.GetAxis("Vertical");
            var rotation = Vector3.up * Input.GetAxis("Horizontal");
            transform.position += movement * (m_movementSpeed * Time.deltaTime);
            transform.eulerAngles += rotation * (m_rotationSpeed * Time.deltaTime);
#endif
        }

        private IEnumerator AuthorizePermission(string _permission)
        {
#if UNITY_ANDROID
            if (Permission.HasUserAuthorizedPermission(_permission)) { yield break; }
            
            Permission.RequestUserPermission(_permission);
            while (!Permission.HasUserAuthorizedPermission(_permission))
            {
                yield return new WaitForSeconds(1.0f);
            }
#endif
            yield return null;
        }
    }
}
