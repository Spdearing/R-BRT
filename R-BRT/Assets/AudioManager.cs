using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Header("----Audio Source ----")]
	[SerializeField] AudioSource masterSource;
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource sfxSource;
	[SerializeField] AudioSource ambienceSource;


	[Header("----Player Audio Clips ----")]
	public AudioClip walk;
	public AudioClip jump;
	public AudioClip rockHold;
	public AudioClip rockThrow;
	public AudioClip rockLand;

	[Header("----Abilities----")]
	public AudioClip batteryPickup;
	public AudioClip jetpackStart;
	public AudioClip jetpackAirborne;
	public AudioClip jetpackLand;
	public AudioClip invisibleActive;
	public AudioClip buttonPressed;

	[Header("----Enemies----")]
	public AudioClip botSuspicious;
	public AudioClip botCaught;
	public AudioClip flyingBotIdle;
	public AudioClip groundBot;
	public AudioClip spiderBot;

	[Header("Ambience")]
	public AudioClip clock;
	public AudioClip electricity;
	public AudioClip windDown;

	[Header("Music")]
	public AudioClip menuMusic;
	public AudioClip levelMusic;
	public AudioClip choiceMuic;
	public AudioClip endingMusic;


	public void PlaySFX(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip);
	}


}
