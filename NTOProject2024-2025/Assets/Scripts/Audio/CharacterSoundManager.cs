using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource audioSource;

    [Header("Footstep Sounds")]
    public AudioClip footstepSounds; // Массив звуков шагов
    
    // Метод для воспроизведения звука шага
    public void PlayFootstepSound()
    {
        AudioClip clip = footstepSounds;
        audioSource.PlayOneShot(clip);
    }

}
