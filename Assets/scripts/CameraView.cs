using UnityEngine;

[RequireComponent(typeof(UnityEngine.Camera))]
public class CameraView : MonoBehaviour
{
    [SerializeField] private float _delay = 0.15f;
    [SerializeField] private Transform _target;
    [SerializeField] private float _shakePower = 0.16f;

    private Vector3 velocity = Vector3.zero;
    private UnityEngine.Camera _camera;

    private Vector3 _originalPos;
    private float _shakeTimeoutTimestamp;

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
            Vector3 delta = _target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = _originalPos + delta;
            _originalPos = Vector3.SmoothDamp(_originalPos, destination, ref velocity, _delay);

            if (Time.time <= _shakeTimeoutTimestamp)
            {
                Vector3 shakeDestination = _originalPos + Random.insideUnitSphere * _shakePower;
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
    public void ShakeCamera(float duration, float shakePower)
    {
        _shakePower = shakePower;
        _shakeTimeoutTimestamp = Time.time + duration;
    }
}
