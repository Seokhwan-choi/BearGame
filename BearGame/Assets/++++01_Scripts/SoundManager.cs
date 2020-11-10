using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bear
{
    [System.Serializable]
    public class Sound
    {
        public string[] Name;
    }

    public class SoundManager : MonoBehaviour
    {
        const int SECount = 10;
        const float DefaultBgmDelay = 0.5f;

        Dictionary<string, AudioClip> mAudioSources;
        AudioSource Bgm;
        AudioSource[] SoundEffect;
        
        public void Init()
        {
            mAudioSources = new Dictionary<string, AudioClip>();
            var audioSource = Resources.LoadAll<AudioClip>("Sounds");

            for(int i=0; i<audioSource.Length;++i)
            {
                mAudioSources.Add(audioSource[i].name, audioSource[i]);
            }

            //오디오소스 컴포넌트 호출
            Bgm = gameObject.AddComponent<AudioSource>();

            SoundEffect = new AudioSource[SECount];
            for(int i = 0; i < SoundEffect.Length; ++i)
            {
                SoundEffect[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlayBg(string name, float delay = DefaultBgmDelay)
        {
            if (mAudioSources.TryGetValue(name, out var clip))
            {
                Bgm.clip = clip;
            }

            Bgm.volume = 0.5f;
            Bgm.loop = true;
            Bgm.PlayDelayed(delay);
        }

        public void PauseBG()
        {
            if (Bgm.isPlaying)
            {
                Bgm.Pause();
            }
        }

        public void UnPauseBG()
        {
            if (!Bgm.isPlaying)
            {
                Bgm.UnPause();
            }
        }


        public void PlaySE(string name)
        {
            for (int i = 0; i < SoundEffect.Length; ++i)
            {
                if (SoundEffect[i].isPlaying) continue;
                if (mAudioSources.TryGetValue(name, out var clip))
                {   
                    SoundEffect[i].clip = clip;
                    SoundEffect[i].Play();
                    return;
                }                
            }
        }

        public void PauseSEAll()
        {
            for (int i = 0; i < SoundEffect.Length; ++i)
            {
                if (SoundEffect[i].isPlaying)
                {
                    SoundEffect[i].Pause();
                }                
            }
        }


    }
}

