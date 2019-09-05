﻿//==============================
// Created by KAITO-I
//==============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================
// サウンド管理
//==============================
// BGMで使う音源のリスト
public enum BGMID {
    None = -1,
    Title,
    MainMenu,
    CharacterSelect,
    InGame,
    Result
}

// SEで使う音源のリスト
public enum SEID {
    Controller_Select,
    Controller_Decision,
    Controller_Back,
    Shutter_Down,
    Shutter_Close,
    Shutter_Up,
    CharacterSelect_Ready,
    CharacterSelect_Go,
    Game_Countdown,
    Game_Ready,
    Game_Character_General_Slash,
    Game_Character_General_Smash,
    Game_Character_General_Change,
    Game_Character_General_Finish,
    Game_Character_General_Move,
    Game_Character_Homie_Guitar,
    Game_Character_Homie_GuitarSwing,
    Game_Character_Homie_GuitarSP,
    Game_Character_Homie_Buff,
    Game_Character_Kagura_Sword,
    Game_Character_Kagura_SwordSP,
    Game_Character_Kagura_Chain
}

public class SoundManager : MonoBehaviour {
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) Debug.Log("SoundManagerが読み込まれていません");
            return instance;
        }
    }

    //==============================
    // クラス
    //==============================
    // BGM
    [Header("BGM")]
    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private AudioClip[] bgmClips;
    private BGMID playingBGMID;

    // SE
    [Header("SE")]
    [SerializeField]
    private GameObject seObjectParent;
    [SerializeField]
    private GameObject seObject;
    [SerializeField]
    private AudioClip[] seClips;

    [SerializeField]
    private int canPlaySECount;
    private List<SoundEffect> usedSEGameObject;
    private List<SoundEffect> unusedSEGameObject;

    //------------------------------
    // Startより前に初期化
    //------------------------------
    private void Awake() {
        if (instance != null) return;
        instance = this;

        this.usedSEGameObject      = new List<SoundEffect>();
        this.unusedSEGameObject    = new List<SoundEffect>();
        for (int i = 0; i < this.canPlaySECount; i++) {
            GameObject obj = Instantiate(this.seObject);
            obj.transform.SetParent(this.seObjectParent.transform);
            this.unusedSEGameObject.Add(obj.GetComponent<SoundEffect>());
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //------------------------------
    // BGM再生
    //------------------------------
    // [引数]
    // BGMID bgmID : 再生するBGMのID
    //------------------------------
    public void PlayBGM(BGMID bgmID) {
        // 同じ曲なら処理しない
        if (this.playingBGMID == bgmID) return;

        // 曲が流れていれば停止
        if (this.playingBGMID != BGMID.None) StopBGM();

        // BGMIDがNoneなら実行しない
        if (bgmID == BGMID.None) return;

        // 再生
        this.playingBGMID = bgmID;
        this.bgm.clip = this.bgmClips[(int)bgmID];
        this.bgm.Play();
    }

    //------------------------------
    // BGM停止
    //------------------------------
    public void StopBGM() {
        this.playingBGMID = BGMID.None;
        this.bgm.Stop();
    }

    //------------------------------
    // SE再生
    //------------------------------
    // [引数]
    // SEID seID : 再生するSEのID
    //------------------------------
    public void PlaySE(SEID seID) {
        // 0個じゃなければ実行
        if (this.unusedSEGameObject.Count > 0) {
            SoundEffect se = this.unusedSEGameObject[0];
            se.Play(this.seClips[(int)seID]);
            this.unusedSEGameObject.RemoveAt(0);
            this.usedSEGameObject.Add(se);
        }
    }

    //------------------------------
    // SE自身が終わったことを報告
    //------------------------------
    public void SEEndCallBack(SoundEffect se) {
        this.usedSEGameObject.Remove(se);
        this.unusedSEGameObject.Add(se);
    }
}