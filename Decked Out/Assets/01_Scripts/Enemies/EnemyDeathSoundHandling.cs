using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyDeathSoundHandling : MonoBehaviour
{
    public AudioClip enemyDeathSound;
    [SerializeField] private AudioMixerGroup mixerGroup;

    public void PlayDeathSound()
    {
        GameObject temp = new GameObject("Death Sound Player");
        AudioSource audioSource = temp.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip = enemyDeathSound;
        audioSource.Play();
        Destroy(temp, enemyDeathSound.length);
    }
}
