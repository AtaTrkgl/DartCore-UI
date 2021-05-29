using UnityEngine;
using UnityEngine.Audio;

namespace DartCore.UI
{
    public class UIAudioManager : MonoBehaviour
    {
        public static void PlayOneShotAudio(AudioClip clip, float volume, AudioMixerGroup mixerGroup)
        {
            if (clip)
            {
                var obj = new GameObject {name = "Temp. AudioSource"};
                var audioSource = obj.AddComponent<AudioSource>();

                audioSource.loop = false;
                audioSource.clip = clip;
                audioSource.volume = volume;
                if (mixerGroup)
                    audioSource.outputAudioMixerGroup = mixerGroup;

                audioSource.Play();
                Destroy(obj, clip.length);
            }
        }
    }
}