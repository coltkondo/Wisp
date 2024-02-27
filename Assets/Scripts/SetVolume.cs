using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
    }
}
