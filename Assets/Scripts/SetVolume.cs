using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject player;
    public PlayerAudio playerAudio;
    public GameObject[] enemies;
    public EnemyHealth[] enemyHealth;
    public Boss_Projectile[] bossProjAudio;
    public EnemyFollowPlayer[] bossSound;
    public float volumeValue;

    void Start()
    {
        volumeValue = PlayerPrefs.GetFloat("VideoVolume"); 

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        player = GameObject.FindGameObjectWithTag("Player");
        playerAudio = player.GetComponent<PlayerAudio>();

        playerAudio.VolumeLevel = volumeValue;

        for (int i = 0; i < enemies.Length;)
        {
            if (enemies[i] != null)
            {
                enemyHealth = new EnemyHealth[enemies.Length];
                bossProjAudio = new Boss_Projectile[enemies.Length];
                bossSound = new EnemyFollowPlayer[enemies.Length];
                
                if (enemies[i].GetComponent<Boss_Projectile>() != null)
                {
                    bossProjAudio[i] = enemies[i].GetComponent<Boss_Projectile>();
                    bossProjAudio[i].VolumeLevel = volumeValue;
                }

                if (enemies[i].GetComponent<EnemyFollowPlayer>() != null)
                {
                    bossSound[i] = enemies[i].GetComponent<EnemyFollowPlayer>();
                    bossSound[i].VolumeLevel = volumeValue;
                }

                enemyHealth[i] = enemies[i].GetComponent<EnemyHealth>();
                enemyHealth[i].VolumeLevel = volumeValue;
                i++;
            }
            else
            {
                i++;
            }
        }

        mixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
        Debug.Log("VOLUME LEVEL LOADED AS: " + PlayerPrefs.GetFloat("Volume") + "");
    }
    public void SetLevel(float sliderValue)
    {
        float value;
        value = Mathf.Log10(sliderValue) * 20;
        //mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        mixer.SetFloat("Volume", value);

        playerAudio.VolumeLevel = volumeValue;

        for (int i = 0; i < enemies.Length;)
        {
            if (enemies[i] != null)
            {
                enemyHealth = new EnemyHealth[enemies.Length];
                bossProjAudio = new Boss_Projectile[enemies.Length];
                bossSound = new EnemyFollowPlayer[enemies.Length];

                if (enemies[i].GetComponent<Boss_Projectile>() != null)
                {
                    bossProjAudio[i] = enemies[i].GetComponent<Boss_Projectile>();
                    bossProjAudio[i].VolumeLevel = volumeValue;
                }

                if (enemies[i].GetComponent<EnemyFollowPlayer>() != null)
                {
                    bossSound[i] = enemies[i].GetComponent<EnemyFollowPlayer>();
                    bossSound[i].VolumeLevel = volumeValue;
                }

                enemyHealth[i] = enemies[i].GetComponent<EnemyHealth>();
                enemyHealth[i].VolumeLevel = volumeValue;
                i++;
            }
            else
            {
                i++;
            }
        }

        PlayerPrefs.SetFloat("VideoVolume", sliderValue);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        Debug.Log("VOLUME LEVEL SET TO: " + value + "");
    }
}
