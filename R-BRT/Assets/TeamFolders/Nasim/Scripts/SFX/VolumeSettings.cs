using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
	[SerializeField] private AudioMixer myMixer;
	[SerializeField] private Slider masterSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;

	private void Start()
	{

		if (PlayerPrefs.HasKey("masterVolume"))
		{
			LoadVolume();
		}
		else
		{
			SetMasterVolume();
			SetMusicVolume();
			SetSFXVolume();
		}

		masterSlider.value = masterSlider.maxValue;
		musicSlider.value = musicSlider.maxValue;
		sfxSlider.value = sfxSlider.maxValue;

	}

	public void SetMasterVolume()
	{
		float volume = masterSlider.value;
		myMixer.SetFloat("master", Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("masterVolume", volume);
	}

	public void SetMusicVolume()
	{
		float volume = musicSlider.value;
		myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("musicVolume", volume);
	}

	public void SetSFXVolume()
	{
		float volume = sfxSlider.value;
		myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
		PlayerPrefs.SetFloat("sfxVolume", volume);
	}

	private void LoadVolume()
	{
		masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
		musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
		sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

		SetMasterVolume();
		SetMusicVolume();
		SetSFXVolume();
	}
}
