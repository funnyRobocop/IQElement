using UnityEngine;
using System.Collections;

//--------------------------------------
// Скрипт для управления аудио ресурсами
//--------------------------------------
public class Audio : MonoBehaviour {
	
	public AudioSource audioSource;
	public AudioSource audioSourceWrong;
	public AudioClip changeScreen;
	public AudioClip select;
	public AudioClip rotate;
	public AudioClip win;
	public AudioClip wrong;

	public static Audio instance { get; private set; }


	void Awake()
	{
		instance = this;
	}
	
	public void PlaySelect () {
		audioSource.PlayOneShot(select);
	}
	
	public void PlayRotate () {
		audioSource.PlayOneShot(rotate);
	}
	
	public void PlayWin () {
		//audioSource.PlayOneShot(win);
	}
	
	public void PlayWrong () {
		if (!audioSourceWrong.isPlaying)
			audioSourceWrong.PlayOneShot(wrong);
	}
	
	public void VolumeOff () {
		audioSource.volume = 0;
		audioSourceWrong.volume = 0;
	}
	
	public void VolumeOn () {
		audioSource.volume = 1;
		audioSourceWrong.volume = 1;
	}
}
