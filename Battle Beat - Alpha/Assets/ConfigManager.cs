﻿//==============================
// Created    by akiirohappa (木原)
// Customized by KAITO-I     (稲福)
//==============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//==============================
// Config操作／管理
//==============================
public class ConfigManager : MonoBehaviour
{
    [SerializeField] VolumeConfig SoundVolume;

    private void Start()
    {
        this.SoundVolume.Init();
    }

    //------------------------------
    // 音量値設定反映
    //------------------------------
    // [引数]
    // float value : スライダーの値
    //------------------------------
    public void SetMasterVol(float value)
    {
        this.SoundVolume.SoundManager.MasterVolume = value;
    }

    public void SetBGMVol(float value)
    {
        this.SoundVolume.SoundManager.BGM.Volume = value;
    }

    public void SetSEVol(float value)
    {
        this.SoundVolume.SoundManager.SE.Volume = value;
    }

    public void SetVoiceVol(float value)
    {
        this.SoundVolume.SoundManager.Voice.Volume = value;
    }

    //==============================
    // Volume値設定
    //==============================
    [System.Serializable]
    class VolumeConfig
    {
        public SoundManager SoundManager { get; private set; }
        [SerializeField] Slider MasterVolume;
        [SerializeField] Slider BGMVolume;
        [SerializeField] Slider SEVolume;
        [SerializeField] Slider VoiceVolume;

        //------------------------------
        // 初期化
        //------------------------------
        public void Init()
        {
            this.SoundManager = SoundManager.Instance;
            this.MasterVolume.value = this.SoundManager.MasterVolume;
            this.BGMVolume.value    = this.SoundManager.BGM.Volume;
            this.SEVolume.value     = this.SoundManager.SE.Volume;
            this.VoiceVolume.value  = this.SoundManager.Voice.Volume; 
        }
    }
}
