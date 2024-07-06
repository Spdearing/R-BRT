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


	[Header("----Audio Source ----")]
	public AudioClip walk;
	public AudioClip caught;
	public AudioClip invisibleActive;


}
