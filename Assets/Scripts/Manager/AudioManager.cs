using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Date;

public enum SFX
{
    Button = 0, Buy, Clear, Fail, Grow, PauseIn, PauseOut, Sell, Touch, Unlock
}

public class AudioManager : MonoBehaviour
{
    [SerializeField, Tooltip("배경음 효과 출력 소스")]
    private AudioSource bgmAudioSource;
    [SerializeField, Tooltip("이펙트 효과 출력 소스")]
    private AudioSource sfxAudioSource;
    [SerializeField, Tooltip("각종 이펙트 효과 클릭")]
    private AudioClip[] clips;
    [SerializeField, Tooltip("배경음 음향")]
    private float bgmVolume;
    [SerializeField, Tooltip("이펙트 음향")]
    private float sfxVolume;
    [SerializeField, Tooltip("배경음 볼륨 조절 슬라이더")]
    private Slider bgmSlider;
    [SerializeField, Tooltip("효과음 볼륨 조절 슬라이더")]
    private Slider sfxSlider;

    public static System.Action<SFX> PlaySFXAudioSource;

    private void Start()
    {
        // 저장되있던 데이터 저장
        bgmVolume = DateLoad.GetFloatDate(nameof(bgmVolume));
        sfxVolume = DateLoad.GetFloatDate(nameof(sfxVolume));

        bgmAudioSource.volume = bgmVolume;
        sfxAudioSource.volume = sfxVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        PlaySFXAudioSource = (sfx) => { PlaySFXAudio(sfx); };
    }

    private void PlaySFXAudio(SFX sfx)
    {
        sfxAudioSource.PlayOneShot(clips[(int)sfx]);

        if (sfx == SFX.PauseIn) bgmAudioSource.Stop();
        else bgmAudioSource.Play();
    }

    public void BGMVolumeChanage()
    {
        bgmVolume = bgmSlider.value;
        bgmAudioSource.volume = bgmVolume;

        DateSave.SetDate(nameof(bgmVolume), bgmVolume);
    }

    public void SFXVolumeChanage()
    {
        sfxVolume = sfxSlider.value;
        sfxAudioSource.volume = sfxVolume;

        DateSave.SetDate(nameof(sfxVolume), sfxVolume);
    }

}
