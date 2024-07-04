using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
	public static SoundFXManager instance;

	[SerializeField] private AudioSource soundFXObject;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform)
	{
		//spawn in gameObject
		AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

		//assign the audioClip
		audioSource.clip = AudioClip;
		
		//assign volume
		audioSource.volume = volume;

		//play sound
		audioSource.Play();

		//get length of sound FX clip
		float clipLength = audioSource.clip.length;

		//destroy the clip after it is done playing
		Destroy(audioSource.gameObject, clipLength);
	}


		public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform)
	{
		//assigning a random index
		int rand = Random.Range(0, audioClip.Length);

		//spawn in gameObject
		AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

		//assign the audioClip
		audioSource.clip = AudioClip[rand];
		
		//assign volume
		audioSource.volume = volume;

		//play sound
		audioSource.Play();

		//get length of sound FX clip
		float clipLength = audioSource.clip.length;

		//destroy the clip after it is done playing
		Destroy(audioSource.gameObject, clipLength);
	}
}
