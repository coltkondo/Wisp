using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    void Start()
    {
        mixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
        Debug.Log("VOLUME LEVEL LOADED AS: " + PlayerPrefs.GetFloat("Volume") + "");
    }
        public void SetLevel(float sliderValue)
    {
        float value;
        value = Mathf.Log10(sliderValue) * 20;
        //mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        mixer.SetFloat("Volume", value);
        PlayerPrefs.SetFloat("VideoVolume", sliderValue);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        Debug.Log("VOLUME LEVEL SET TO: " + value + "");
    }
}
