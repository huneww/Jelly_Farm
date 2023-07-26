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
    [SerializeField, Tooltip("����� ȿ�� ��� �ҽ�")]
    private AudioSource bgmAudioSource;
    [SerializeField, Tooltip("����Ʈ ȿ�� ��� �ҽ�")]
    private AudioSource sfxAudioSource;
    [SerializeField, Tooltip("���� ����Ʈ ȿ�� Ŭ��")]
    private AudioClip[] clips;
    [SerializeField, Tooltip("����� ����")]
    private float bgmVolume;
    [SerializeField, Tooltip("����Ʈ ����")]
    private float sfxVolume;
    [SerializeField, Tooltip("����� ���� ���� �����̴�")]
    private Slider bgmSlider;
    [SerializeField, Tooltip("ȿ���� ���� ���� �����̴�")]
    private Slider sfxSlider;

    // ȿ���� ��� Action �Լ�
    public static System.Action<SFX> PlaySFXAudioSource;

    private void Start()
    {
        // ������ִ� ������ ����
        bgmVolume = DateLoad.GetFloatDate(nameof(bgmVolume));
        sfxVolume = DateLoad.GetFloatDate(nameof(sfxVolume));

        // �ҷ��� �����͸� ����, �����̴� ������ ����
        bgmAudioSource.volume = bgmVolume;
        sfxAudioSource.volume = sfxVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;

        // Action�Լ��� ����
        PlaySFXAudioSource = (sfx) => { PlaySFXAudio(sfx); };
    }
    
    /// <summary>
    /// ȿ���� ��� �޼���
    /// </summary>
    /// <param name="sfx">����� ȿ����</param>
    private void PlaySFXAudio(SFX sfx)
    {
        // ȿ���� ���
        sfxAudioSource.PlayOneShot(clips[(int)sfx]);
    }

    /// <summary>
    /// ����� ���� ���� �޼���
    /// </summary>
    public void BGMVolumeChanage()
    {
        // �����̴� ������ bgmVolume�� ����
        bgmVolume = bgmSlider.value;
        // ����� ���� ����
        bgmAudioSource.volume = bgmVolume;
        // ����� ������ ����
        DateSave.SetDate(nameof(bgmVolume), bgmVolume);
    }

    /// <summary>
    /// ȿ���� ���� ���� �޼���
    /// </summary>
    public void SFXVolumeChanage()
    {
        // �����̴� ������ sfxVolume�� ����
        sfxVolume = sfxSlider.value;
        // ����� ���� ����
        sfxAudioSource.volume = sfxVolume;
        // ����� ������ ����
        DateSave.SetDate(nameof(sfxVolume), sfxVolume);
    }

}
