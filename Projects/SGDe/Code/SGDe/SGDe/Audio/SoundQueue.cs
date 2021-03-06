﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SGDE.Audio
{
    public class SoundQueue
	{
		private List<SoundObject> m_sounds;
		private int m_index;
		private bool m_musicIsPlaying;
		private Song m_currentSong;
		private int m_queueSize;
		private Song m_nextSongID;
		private double m_loopLocation; //Millseconds into song to loop to
		//private Function m_loopFunction; //Callback function for loops
		private double m_nextSongLoopLocation;


        /*
         
         * Implementation
         * SoundQueue mySoundQueue = new SoundQueue();
           mySoundQueue.playMusic(Content.Load<Song>("musicAsset"), 0);
         * mySoundQueue.playSoundEffect(Content.Load<SoundEffect>("soundAsset"));
         
         */

        public SoundQueue()
		{
			m_queueSize = 30;
			m_sounds = new List<SoundObject>(m_queueSize);
			m_index = 0;
            for (int i = 0; i < m_queueSize; i++)
            {
                m_sounds.Add(null);
            }
			m_musicIsPlaying = false;
			m_currentSong = null;
			m_nextSongID = null;
			m_nextSongLoopLocation = 0;
			m_loopLocation = 0;
			//m_loopFunction = loopMusic;
		}

        //Public properties
		public double LoopLocation
		{
            get
            {
			    return m_loopLocation;
            }
		}
		public double NextSongLoopLocation
		{
            get
            {
			    return m_loopLocation;
            }
		}
		public Song CurrentSongID
		{
            get
            {
			    return m_currentSong;
            }
		}

        //Will set an event up to play the next song based on what the nextSongID is set to
        /*public void NextMusic(event:Event)
        {
            //var tempSound:Sound = m_audioMC.getSound(m_nextSongID);
            if (tempSound != null)
            {
                //m_currentSong.removeEventListener(Event.SOUND_COMPLETE, nextMusic);
                
                m_currentSong = tempSound.play(m_nextSongLoopLocation, 0, new SoundTransform(SaveData.BGVolumeLevel));
                if (m_currentSong != null)
                {
                    //m_currentSong.addEventListener(Event.SOUND_COMPLETE, m_loopFunction);
                    m_musicIsPlaying = true;
                }
            }
        }*/

        //Prepare next song to play
        public void SetNextMusic(Song id, double loopLoc)
        {
            m_nextSongID = id;
            m_nextSongLoopLocation = loopLoc;
        }

        //Set function to run once the music finishes (usually can just be nextMusic() function)
		/*public void setLoopFunction(Function func)
		{
			m_loopFunction = func;
		}*/
		
        //Get sound from index (will be null if nothing is there)
		public SoundObject GetSoundObject(int index)
		{
			return (index >= 0 && index < m_sounds.Capacity && m_sounds[index] != null) ? m_sounds[index] : null;
		}
		
        //Start music
		public void PlayMusic(Song sound, double loopLoc)
		{
			m_loopLocation = loopLoc;
			if (m_musicIsPlaying)
			{
				StopMusic();
			}
			if (sound != null)
			{
                m_currentSong = sound;
                MediaPlayer.Volume = 1;
                MediaPlayer.Play(m_currentSong);
                m_loopLocation = loopLoc;
                m_musicIsPlaying = true;
			}
		}

        //Loop music. You can have the loop function be this if you want to play the same song
		/*public void LoopMusic(Event event)
		{
			if (m_currentSongID != null)
			{
				//m_currentSong.removeEventListener(Event.SOUND_COMPLETE, loopMusic);
                MediaPlayer.Volume = 1;
                MediaPlayer.Play(m_currentSongID);
				//m_currentSong.addEventListener(Event.SOUND_COMPLETE, m_loopFunction);
				m_musicIsPlaying = true;
			}
		}*/

		public void SetMusicVolume(float num)
		{
			MediaPlayer.Volume = num;
		}
		
		public void StopMusic()
		{
			if (m_musicIsPlaying)
			{
				//m_currentSong.removeEventListener(Event.SOUND_COMPLETE, m_loopFunction);
				MediaPlayer.Stop();
				m_currentSong = null;
				m_musicIsPlaying = false;
			}
		}
		
        //Next two functions are seperated in case you want independent volumes (assuming you have some sort of static Save Data class to keep track of audio options)
		public int PlaySoundEffect(SoundEffect sound)
		{
			if (sound != null)
			{
				if (m_sounds[m_index] != null)
				{
					m_sounds[m_index].Stop();
				}
				if (m_sounds[m_index] != null)
				{
					if (!m_sounds[m_index].IsFinished)
					{
						m_sounds[m_index].Stop();
					}
					m_sounds[m_index] = null;
				}
				SoundObject sc = new SoundObject();
				sc.Play(sound, 1.0f, null);
				if (!sc.IsError)
				{
					m_sounds[m_index] = sc;
					int oldIndex = m_index;
					m_index++;
					if (m_index > m_queueSize-1)
					{
						m_index = 0;
					}
					return oldIndex;
				} else
				{
					return -1;
				}
			} else
			{
				return -1;
			}
		}
		public int PlayVoiceEffect(SoundEffect sound)
		{
			if (sound != null)
			{
				if (m_sounds[m_index] != null)
				{
					m_sounds[m_index].Stop();
				}
				if (m_sounds[m_index] != null)
				{
					if (!m_sounds[m_index].IsFinished)
					{
						m_sounds[m_index].Stop();
					}
					m_sounds[m_index] = null;
				}
				SoundObject sc = new SoundObject();
				sc.Play(sound, 1.0f, null);
				if (!sc.IsError)
				{
					m_sounds[m_index] = sc;
					int oldIndex = m_index;
					m_index++;
					if (m_index > m_queueSize-1)
					{
						m_index = 0;
					}
					return oldIndex;
				} else
				{
					return -1;
				}
			} else
			{
				return -1;
			}
		}

		public void StopSound(int loc)
		{
			if (loc >= 0 && m_sounds[loc] != null)
			{
				m_sounds[loc].Stop();
			}
		}
		public void StopAllSounds()
		{
			for (int i = 0; i < m_sounds.Capacity; i++)
			{
				if (m_sounds[i] != null)
				{
					m_sounds[i].Stop();
					m_sounds[i] = null;
				}
			}
		}
		public void PauseAllSounds()
		{
            for (int i = 0; i < m_sounds.Capacity; i++)
			{
				if (m_sounds[i] != null && !m_sounds[i].IsFinished)
				{
					m_sounds[i].Pause();
				}
			}
		}
		public void UnpauseAllSounds()
		{
            for (int i = 0; i < m_sounds.Capacity; i++)
			{
				if (m_sounds[i] != null && !m_sounds[i].IsFinished)
				{
					m_sounds[i].Unpause();
				}
			}
		}
	}
}
