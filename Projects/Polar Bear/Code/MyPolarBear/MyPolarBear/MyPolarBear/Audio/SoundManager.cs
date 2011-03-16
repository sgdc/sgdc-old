using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MyPolarBear.Audio
{
    class SoundManager
    {
        private static AudioEngine audioEngine;
        private static WaveBank waveBank;
        private static SoundBank soundBank; 
              
        private static Dictionary<string, SoundEffectInstance> Songs;

        public SoundManager()
        {
            audioEngine = new AudioEngine(@"Content/Sounds/GameSounds.xgs");
            waveBank = new WaveBank(audioEngine, @"Content/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content/Sounds/Sound Bank.xsb");

            Songs = new Dictionary<string, SoundEffectInstance>();
        }

        public static void AddMusic(string name, SoundEffect song, bool isLooped, float volume = 0.1f)
        {
            Songs.Add(name, song.CreateInstance());
            Songs[name].IsLooped = isLooped;
            Songs[name].Volume = volume;
        }

        public static SoundEffectInstance GetSound(string name)
        {
            return Songs[name];
        }

        public static void PlayMusic(string name)
        {
            if (Songs[name].State == SoundState.Stopped)
            {               
                Songs[name].Play();               
            }
            else if (Songs[name].State == SoundState.Paused)
            {
                Songs[name].Resume();
            }
        }

        public static void StopMusic(string name)
        {
            if (Songs[name].State == SoundState.Playing)
                Songs[name].Stop();
        }

        public static void StopAllMusic()
        {
            foreach (var song in Songs)
            {
                song.Value.Stop();
            }            
        }

        public static void PauseMusic(string name)
        {
            if (Songs[name].State == SoundState.Playing)
                Songs[name].Pause();
        }

        public static void PauseAllMusic()
        {
            foreach (var song in Songs)
            {
                song.Value.Pause();
            }
        }        

        public static void PlaySound(string name)
        {
            soundBank.GetCue(name).Play();
        }
    }
}
