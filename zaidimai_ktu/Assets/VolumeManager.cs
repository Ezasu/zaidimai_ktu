using UnityEngine.Audio;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMusic (float valueMusic)
    {
        mixer.SetFloat("SliderMusic", Mathf.Log10(valueMusic) * 20);
    }
    public void SetSFX(float valueSFX)
    {
        mixer.SetFloat("SliderSFX", Mathf.Log10(valueSFX) * 20);
    }
}
