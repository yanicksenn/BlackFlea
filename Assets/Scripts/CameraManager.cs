using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float shakeInitialAmplitude;

    public static Optional<CameraManager> Instance { get; private set; } = Optional<CameraManager>.OfEmpty();

    private void Awake()
    {
        if (Instance.IsPresent && Instance.Get() != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = Optional<CameraManager>.Of(this);
        }
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cameraNoise =
                vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cameraNoise.m_AmplitudeGain =
                Mathf.Lerp(shakeInitialAmplitude, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    private void OnDestroy() {
        if (Instance.Get() == this) {
            Instance = Optional<CameraManager>.OfEmpty();
        }
    }

    public void ShakeCamera(float amplitude, float time)
    {
        CinemachineBasicMultiChannelPerlin cameraNoise =
                vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraNoise.m_AmplitudeGain = amplitude;
        shakeTimer = time;
        shakeTimerTotal = time;
        shakeInitialAmplitude = amplitude;
    }
}