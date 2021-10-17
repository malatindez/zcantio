using UnityEngine;

[RequireComponent(typeof(UnityEngine.Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField] private float _delay = 0.15f;
    public Transform _target;
    [SerializeField] private float _shakePower = 0.16f;
    [SerializeField] private float _verticalShakePower = 0.16f;
    [SerializeField] private float _verticalShakeSpeed = 8f;

    private Vector3 velocity = Vector3.zero;
    private UnityEngine.Camera _camera;

    private Vector3 _originalPos;
    private float _shakeTimeoutTimestamp;
    private float _smoothTimeoutTimestamp;

    private void Start()
    {
        _camera = GetComponent<UnityEngine.Camera>();
    }

    private void Awake()
    {
        _originalPos = transform.position;
    }

    private void LateUpdate()
    {
        if (_target)
        {
            Vector3 point = _camera.WorldToViewportPoint(_target.position);
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = _originalPos + delta;
            _originalPos = Vector3.SmoothDamp(_originalPos, destination, ref velocity, _delay);

            if (Time.time <= _shakeTimeoutTimestamp)
            {
                Vector3 shakeDestination = _originalPos + Random.insideUnitSphere * _shakePower;
                shakeDestination.z = _originalPos.z;

                transform.position = shakeDestination;
            } else if (Time.time <= _smoothTimeoutTimestamp)
            {
                Vector3 shakeDestination = _originalPos + new Vector3(0,Mathf.Sin(Time.realtimeSinceStartup*_verticalShakeSpeed)) * _verticalShakePower;
                shakeDestination.z = _originalPos.z;

                transform.position = shakeDestination;
            }
            else
            {
                transform.position = _originalPos;
            }
        }

    }

    /// <summary>
    /// Shakes camera during specified duration.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    public void ShakeCamera(float duration)
    {
        _shakeTimeoutTimestamp = Time.time + duration;
    }

    /// <summary>
    /// Shakes camera during specified duration with specified power.
    /// </summary>
    /// <param name="duration">Duration in seconds.</param>
    /// <param name="shakeAmount">Shaked power.</param>
    public void ShakeCameraRoughly(float duration, float shakePower)
    {
        _shakePower = shakePower;
        _shakeTimeoutTimestamp = Time.time + duration;
    }

    public void ShakeCameraSmoothly(float duration)
    {
        _smoothTimeoutTimestamp = Time.time + duration;
    }
#if DEBUG
    [ContextMenu("Shake For 5sec")]
    private void TestShake()
    {
        if (Application.isPlaying)
            ShakeCamera(5);
    }
#endif
}
