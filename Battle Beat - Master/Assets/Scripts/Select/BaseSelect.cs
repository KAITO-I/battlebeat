﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSelect: MonoBehaviour
{
    protected SelectCountroll _controll;
    protected SoundManager _soundManager;
    protected ControllerManager.Controller _controller;

    [SerializeField]
    protected SpriteRenderer _playerPicture, _playerDescrition;
    [SerializeField]
    protected Image _playerNameImg;
    [SerializeField]
    protected Sprite[] _charaName;
    [SerializeField]
    protected List<DiscritionClass> discritions = new List<DiscritionClass>();
    [SerializeField]
    protected GameObject Teap;
    protected Vector3 Gole, Gole2;


    protected int length;//操作キャラ数
    protected int _charactorID;//選択されているキャラ
    protected int _charactorDecritionID;//説明の枚数
    protected bool _playerOK, _playerDecritionOK;//選択されているか、説明を出すか
    protected float _teapMoveTime;

   protected float[] _xSize =
    {
        0.7f,
        0.4f,
        0.3f,
        0.35f
    };
    protected float[] _ySize =
    {
        0.7f,
        0.4f,
        0.3f,
        0.35f
    };

    public virtual void Inctance(SelectCountroll c, SoundManager s)
    {
        _controll = c;
        _soundManager = s;
        _charactorID = 0;
        _playerOK = false;
        _playerDecritionOK = false;
        _playerNameImg.sprite = _charaName[_charactorID];
        _playerPicture.sprite = _controll.CharaObj[_charactorID].GetCharaSprite;
        //Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
        _playerDescrition.sprite = discritions[_charactorID]._discritionSprites[_charactorDecritionID];
        _playerDescrition.enabled = _playerDecritionOK;
        length = _charaName.Length;
        _teapMoveTime = 0f;
    }
    public abstract void playerUpdate();
    //===============十字キーの処理==================
    protected void InputProcess(int _ID)
    {
        int old = _charactorID;
        if (!_playerOK&&!_playerDecritionOK)//選択されていないとき,説明画面が表示されていないとき
        {
            if (_controller.GetAxisUp(ControllerManager.Axis.DpadY) < 0)//下入力
            {
                _controll.CharaObj[_charactorID].charaSelect(_ID, false);
                _charactorID = (_charactorID + 1) % 2;
                //左->0:中央->2右->4足す
                if (old >= 2 && old <= 3) _charactorID += 2;
                else if (old >= 4 && old <= 5) _charactorID += 4;
                _controll.CharaObj[_charactorID].charaSelect(_ID, true);
                //Player_Obj.transform.localScale = new Vector3(_xSize[_Player], _ySize[_Player], 1);
                _soundManager.PlaySE(SEID.General_Controller_Select);
            }
            else if (_controller.GetAxisUp(ControllerManager.Axis.DpadY) > 0)//上入力
            {
                _controll.CharaObj[_charactorID].charaSelect(_ID, false);
                _charactorID = (_charactorID - 1 + 2) % 2;
                //左->0:中央->2右->4足す
                if (old >= 2 && old <= 3) _charactorID += 2;
                else if (old >= 4 && old <= 5) _charactorID += 4;
                _controll.CharaObj[_charactorID].charaSelect(_ID, true);
                //Player_Obj.transform.localScale = new Vector3(_xSize[_Player], _ySize[_Player], 1);
                _soundManager.PlaySE(SEID.General_Controller_Select);
            }
        }

        if (_controller.GetAxisUp(ControllerManager.Axis.DpadX) < 0)//左入力
        {
            if (_playerDecritionOK)//キャラ説明が表示されている
            {
                _charactorDecritionID = (_charactorDecritionID - 1 + discritions[_charactorID]._discritionSprites.Count) % discritions[_charactorID]._discritionSprites.Count;

            }
            else//普通の選択移動
            {
                _charactorDecritionID = 0;//最初から見るように初期化
                if (!_playerOK)
                {
                    _controll.CharaObj[_charactorID].charaSelect(_ID, false);
                    _charactorID = (_charactorID - 2 + 6) % 6;
                    _controll.CharaObj[_charactorID].charaSelect(_ID, true);
                    _soundManager.PlaySE(SEID.General_Controller_Select);
                }
            }

        }
        else if (_controller.GetAxisUp(ControllerManager.Axis.DpadX) > 0)//右入力
        {
            if (_playerDecritionOK)
            {//キャラ説明が表示されている
                _charactorDecritionID = (_charactorDecritionID + 1) % discritions[_charactorID]._discritionSprites.Count;
            }
            else
            {
                _charactorDecritionID = 0;//最初から見るように初期化
                if (!_playerOK)
                {
                    _controll.CharaObj[_charactorID].charaSelect(_ID, false);
                    _charactorID = (_charactorID + 2) % 6;
                    _controll.CharaObj[_charactorID].charaSelect(_ID, true);
                    _soundManager.PlaySE(SEID.General_Controller_Select);
                }
            }
        }
    }
    //=============ボタン操作関数================
    protected void ButtonInput()
    {
        //決定ボタンの処理
        if (_controller.GetButtonDown(ControllerManager.Button.A))
        {
            //キャラがないところを決定した際の処理
            if (_charactorID == 4 && _charactorID == 5)
            {
                _playerOK = false;
            }
            else
            {
                _playerOK = true;
                _teapMoveTime = 0f;
            }
        }
        //キャンセルボタンの処理
        if (_controller.GetButtonDown(ControllerManager.Button.B))
        {
            //キャラ選択時は選択を外す
            //戻る画面をホップアップで表示
            if (!_playerOK)
            {
                //if (true) SceneLoader.Instance.LoadScene(SceneLoader.Scenes.MainMenu);
            }
            else _playerOK = false;
        }
        //説明画面
        if (_controller.GetButtonDown(ControllerManager.Button.X))
        {
            if (_playerDecritionOK) _playerDecritionOK = false;
            else _playerDecritionOK = true;
        }
        _playerDescrition.enabled = _playerDecritionOK;

    }
    //==============テープを動かす処理============
    protected float ReadyBerMove(bool _Chack, float _MoveTime)
    {
        if (_MoveTime <= 1)
        {
            _MoveTime += 0.1f;
        }
        Transform rect = Teap.GetComponent<Transform>();
        if (!_Chack) Teap.GetComponent<Transform>().position = Vector3.Lerp(rect.position, Gole2, _MoveTime);
        else if (_Chack) Teap.GetComponent<Transform>().position = Vector3.Lerp(rect.position, Gole, _MoveTime);
        return _MoveTime;
    }
    //=============指定されたものを返す関数==========
    public T GetItem<T>(string item=null)//引数で同じ型の変数だが、別の変数を判定している
    {
        if (typeof(T) == typeof(SpriteRenderer))
        {
            if (item == "player")
            {
                return (T)(object)_playerPicture;
            }
            return (T)(object)_playerDescrition;
        }
        else if (typeof(T) == typeof(int))
        {
            return (T)(object)_charactorID;
        }
        else if (typeof(T) == typeof(bool))
        {
            if (item == "player")
            {
                return (T)(object)_playerOK;
            }
            return (T)(object)_playerDecritionOK;
        }
        else if (typeof(T) == typeof(Image))
        {
            return (T)(object)_playerNameImg;
        }

        return default(T);
    }
}
//二次元配列用のクラス
[System.Serializable]
public class DiscritionClass
{
    public string Name;
    public List<Sprite> _discritionSprites = new List<Sprite>();
    public DiscritionClass(List<Sprite> list)
    {
        _discritionSprites = list;
    }
}