using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace SGDE.Audio
{
    public class SoundObject
	{
		private string m_name;
		private SoundEffect m_sound;
		private SoundEffectInstance m_soundInstance;
		private float m_volume;
		private bool m_isPlaying;
		private bool m_isFinished;
		private double m_currentPosition;
		private bool m_error;
		
		public SoundObject() 
		{
			m_name = null;
			m_soundInstance = null;
			m_sound = null;
			m_isPlaying = false;
			m_isFinished = false;
			m_currentPosition = 0;
			m_error = false;
		}
		
		public SoundEffectInstance Instance
		{
			get
            {
                return m_soundInstance;
            }
		}
		public double Volume
		{
			get
            {
                return m_volume;
            }
		}
		public bool IsPlaying
		{
			get
            {
                return m_isPlaying;
            }
		}
		public bool IsFinished
		{
			get
            {
                return m_isFinished;
            }
		}
		public double CurrentPosition
		{
			get
            {
                return m_currentPosition;
            }
		}
		public bool IsError
		{
			get
            {
                return m_error;
            }
		}
		public string Name
		{
			get
            {
                return m_name;
            }
		}
		
		public void Play(SoundEffect sound, float volume, string name)
		{
			if (sound != null && volume >= 0 && volume <= 1)
			{
				m_sound = sound;
				m_volume = volume;
				m_name = name;
				m_soundInstance = m_sound.CreateInstance();
                m_soundInstance.Volume = m_volume;
                m_soundInstance.Play();
				//m_channel.addEventListener(Event.SOUND_COMPLETE, finish);
				m_isPlaying = true;
			}
		}
		public void Pause()
		{
			if (m_isPlaying)
			{
				//m_soundInstance.removeEventListener(Event.SOUND_COMPLETE, finish);
				m_isPlaying = false;
				//m_currentPosition = m_soundInstance.position;
				m_soundInstance.Pause();
			}
		}
		public void Unpause()
		{
			if (!m_isFinished)
			{
				//m_soundInstance = m_sound.play(m_currentPosition, 0, new SoundTransform(m_volume));
				//m_soundInstance.addEventListener(Event.SOUND_COMPLETE, finish);
                m_soundInstance.Resume();
				m_isPlaying = true;
			}
		}
		public void Stop()
		{
			if (m_isPlaying)
			{
				//m_channel.removeEventListener(Event.SOUND_COMPLETE, finish);
				m_soundInstance.Stop();
				m_isPlaying = false;
				m_isFinished = true;
			}
		}
		/*private void Finish(e:Event)
		{
			//m_channel.removeEventListener(Event.SOUND_COMPLETE, finish);
			m_isFinished = true;
			m_isPlaying = false;
		}*/
		
	}
}
