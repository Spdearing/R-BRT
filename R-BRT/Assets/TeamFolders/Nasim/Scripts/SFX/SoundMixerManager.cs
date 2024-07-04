using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;

	public void SetMasterVolume(float level)
	{
		audioMixer.SetFloat("MasterVol", level);
	}

	public void SetSoundFXVolume(float level)
	{
		audioMixer.SetFloat("SFXVol", level);

	}

	public void SetMusicVolume(float level)
	{
		audioMixer.SetFloat("MusicVol", level);
	}
}
