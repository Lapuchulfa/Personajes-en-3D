using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource AudioSource;
    public AudioClip footstepSound;
    public AudioClip jumpSound;


    public void AudioSaltar()
    {
        AudioSource.PlayOneShot(jumpSound);
    }

    public void AudioCorrer()
    {
        AudioSource.PlayOneShot(footstepSound);
    }   
}