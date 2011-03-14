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

        public SoundManager()
        {
            audioEngine = new AudioEngine(@"Content/Sounds/GameSounds.xgs");
            waveBank = new WaveBank(audioEngine, @"Content/Sounds/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content/Sounds/Sound Bank.xsb");            
        }

        public static void PlaySound(string name)
        {
            soundBank.GetCue(name).Play();          
        }
    }
}
