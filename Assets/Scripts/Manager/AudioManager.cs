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

    // 효과음 출력 Action 함수
    public static System.Action<SFX> PlaySFXAudioSource;

    private void Start()
    {
        // 저장되있던 데이터 저장
        bgmVolume = DateLoad.GetFloatDate(nameof(bgmVolume));
        sfxVolume = DateLoad.GetFloatDate(nameof(sfxVolume));

        // 불러온 데이터를 볼륨, 슬라이더 벨류에 저장
        bgmAudioSource.volume = bgmVolume;
        sfxAudioSource.volume = sfxVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        // Action함수에 연결
        PlaySFXAudioSource = (sfx) => { PlaySFXAudio(sfx); };
    }
    
    /// <summary>
    /// 효과음 출력 메서드
    /// </summary>
    /// <param name="sfx">출력할 효과음</param>
    private void PlaySFXAudio(SFX sfx)
    {
        // 효과음 출력
        sfxAudioSource.PlayOneShot(clips[(int)sfx]);
    }

    /// <summary>
    /// 배경음 볼륨 변경 메서드
    /// </summary>
    public void BGMVolumeChanage()
    {
        // 슬라이더 벨류를 bgmVolume에 저장
        bgmVolume = bgmSlider.value;
        // 오디오 볼륨 조절
        bgmAudioSource.volume = bgmVolume;
        // 변경된 데이터 저장
        DateSave.SetDate(nameof(bgmVolume), bgmVolume);
    }

    /// <summary>
    /// 효과음 볼륨 변경 메서드
    /// </summary>
    public void SFXVolumeChanage()
    {
        // 슬라이더 벨류를 sfxVolume에 저장
        sfxVolume = sfxSlider.value;
        // 오디오 볼륨 조절
        sfxAudioSource.volume = sfxVolume;
        // 변경된 데이터 저장
        DateSave.SetDate(nameof(sfxVolume), sfxVolume);
    }

}
