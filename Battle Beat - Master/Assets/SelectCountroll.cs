﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectCountroll : MonoBehaviour
{
    [SerializeField]
    GameObject Player01_Obj;
    [SerializeField]
    GameObject Player02_Obj;
    [SerializeField]
    Image _player1Text, _player2Text;

    ControllerManager.Controller _1Pcontroller = ControllerManager.Instance.Player1;
    ControllerManager.Controller _2Pcontroller = ControllerManager.Instance.Player2;

    //キャラ立ち絵
    SpriteRenderer Player01;
    SpriteRenderer Player02;
    //キャラクターID
    int _Player1;
    int _Player2;
    //キャラクター選択
    bool Player1_OK;
    bool Player2_OK;
    //戻る時間
    float Player1_Time;
    float Player2_Time;
    //戻る画面移動時間
    float ReturnTime;
    float ReturnTimeValumes;

    [SerializeField]
    Slider ReturnSlider;

    [SerializeField]
    GameObject Text01;
    [SerializeField]
    GameObject Text02;

    [SerializeField]
    GameObject FlameObj;
    List<CharaSelectObj> CharaObj;
    int length;

    [SerializeField]
    GameObject Ready;
    [SerializeField]
    Sprite[] _ChataText;

    string[] charaname =
    {
        "Chara1",
        "Chara2",
        "Chara3",
        "Chara4"
    };

    float[] _xSize =
    {
        0.4f,
        0.7f,
        0.25f,
        0.35f
    };
    float[] _ySize =
    {
        0.4f,
        0.7f,
        0.25f,
        0.35f
    };

    void Start()
    {
        Player01 = Player01_Obj.GetComponent<SpriteRenderer>();
        Player02 = Player02_Obj . GetComponent<SpriteRenderer>();

        Ready.SetActive(false);

        Text01.SetActive(false);
        Text02.SetActive(false);
        CharaObj = new List<CharaSelectObj>();
        foreach (Transform v in FlameObj.transform)
        {
            var CObj = v.GetComponent<CharaSelectObj>();
            CharaObj.Add(CObj);
        }
        length = FlameObj.transform.childCount;

        foreach (var c in CharaObj)
        {
            c.Init();
        }

        _Player1 = 0;
        _Player2 = 0;
        Player1_OK = false;
        Player2_OK = false;

        CharaObj[_Player1].charaSelect(1, true);
        CharaObj[_Player2].charaSelect(2, true);
        _player1Text.sprite = _ChataText[_Player1];
        _player2Text.sprite = _ChataText[_Player2];

        Player01.sprite = CharaObj[_Player1].GetCharaSprite;
        Player02.sprite = CharaObj[_Player2].GetCharaSprite;

        Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
        Player02_Obj.transform.localScale = new Vector3(_xSize[_Player2], _ySize[_Player2], 1);

        //戻り時間
        ReturnTime = 5;
        ReturnSlider.maxValue = ReturnTime;
    }

    // Update is called once per frame
    void Update()
    {        
        SelectMove();
        //SelectMoveDebug();
        //Charaが二人とも選択されたとき
        if (Player1_OK&& Player2_OK)
        {
            //決定ボタンを入力したら
            if (ControllerManager.Instance.GetButtonDown(ControllerManager.Button.Start))
            {
                Setting.p1c = (Setting.Chara)_Player1;
                Setting.p2c = (Setting.Chara)_Player2;
                SceneLoader.Instance.LoadScene(SceneLoader.Scenes.MainGame);
                Debug.Log("battleSceneへ");
            }
            Ready.SetActive(true);
            return;
        }
        Ready.SetActive(false);
    }

    void SelectMove()
    {
        //1P処理
        if (_1Pcontroller.GetButtonDown(ControllerManager.Button.Y)&&!Player1_OK)
        {
            if (_1Pcontroller.GetAxis(ControllerManager.Axis.DpadY) > 0)//上入力
            {
                CharaObj[_Player1].charaSelect(1, false);
                _Player1++;
                _Player1 = _Player1 % length;
                CharaObj[_Player1].charaSelect(1, true);

                Player01.sprite = CharaObj[_Player1].GetCharaSprite;
                _player1Text.sprite = _ChataText[_Player1];

                Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
            }
            else//下入力
            {
                CharaObj[_Player1].charaSelect(1, false);
                _Player1--;
                _Player1 = _Player1 % length;
                if (_Player1 < 0) _Player1 = 3;
                CharaObj[_Player1].charaSelect(1, true);

                Player01.sprite = CharaObj[_Player1].GetCharaSprite;
                _player1Text.sprite = _ChataText[_Player1];

                Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
            }
        }
        //2P処理
        if (_2Pcontroller.GetButtonDown(ControllerManager.Button.Y) && !Player2_OK)
        {
            if (_2Pcontroller.GetAxis(ControllerManager.Axis.DpadY) > 0)//上入力
            {
                CharaObj[_Player2].charaSelect(2, false);
                _Player2++;
                _Player2 = _Player2 % length;
                CharaObj[_Player2].charaSelect(2, true);

                Player02.sprite = CharaObj[_Player2].GetCharaSprite;
                _player2Text.sprite = _ChataText[_Player2];
                Player02_Obj.transform.localScale = new Vector3(_xSize[_Player2], _ySize[_Player2], 1);
            }
            else
            {
                CharaObj[_Player2].charaSelect(2, false);
                _Player2--;
                _Player2 = _Player2 % length;
                if (_Player2 < 0) _Player2 = 3;
                CharaObj[_Player2].charaSelect(2, true);

                Player02.sprite = CharaObj[_Player2].GetCharaSprite;
                _player2Text.sprite = _ChataText[_Player2];

                Player02_Obj.transform.localScale = new Vector3(_xSize[_Player2], _ySize[_Player2], 1);
            }
        }
        //選択時//渡す値を決定する
        if (_1Pcontroller.GetButtonDown(ControllerManager.Button.A))
        {
            Player1_OK = true;
        }
        if (_2Pcontroller.GetButtonDown(ControllerManager.Button.A))
        {
            Player2_OK = true;
        }
        //×ボタンの処理
        //長押しで画面移動処理
        if (_1Pcontroller.GetButton(ControllerManager.Button.B))
        {
            //キャラ選択時は選択を外す
            if (_1Pcontroller.GetButtonDown(ControllerManager.Button.B))
            {
                Player1_OK = false;
            }
            float difference = Time.time - Player1_Time;
            SetSilder(difference);
            if (difference > ReturnTime)
            {
                Debug.Log("一つ前の画面へ");
            }
        }
        if (_2Pcontroller.GetButton(ControllerManager.Button.B))
        {
            //キャラ選択時は選択を外す
            if (_2Pcontroller.GetButtonDown(ControllerManager.Button.B))
            {
                Player2_OK = false;
            }
            float difference = Time.time - Player2_Time;
            SetSilder(difference);
            if (difference > ReturnTime)
            {
                Debug.Log("一つ前の画面へ");
            }
        }
        //両方入力されていない
        if(!_1Pcontroller.GetButton(ControllerManager.Button.B) && !_2Pcontroller.GetButton(ControllerManager.Button.B))
        {
            Player1_Time = Time.time;
            Player2_Time = Time.time;
            //片方が入力しているとそのまま継続
            ReturnSlider.value = 0f;
        }
        Text01.SetActive(Player1_OK);
        Text02.SetActive(Player2_OK);
    }

    void SelectMoveDebug()
    {
        //1P処理
        if (Input.GetKeyDown(KeyCode.S) && !Player1_OK)
        {
            CharaObj[_Player1].charaSelect(1, false);
            _Player1++;
            _Player1 = _Player1 % length;
            CharaObj[_Player1].charaSelect(1, true);

            Player01.sprite = CharaObj[_Player1].GetCharaSprite;
            _player1Text.sprite = _ChataText[_Player1];

            Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
        }
        else if (Input.GetKeyDown(KeyCode.W) && !Player1_OK)
        {
            CharaObj[_Player1].charaSelect(1, false);
            _Player1--;
            _Player1 = _Player1 % length;
            if (_Player1 < 0) _Player1 = 3;
            CharaObj[_Player1].charaSelect(1, true);

            Player01.sprite = CharaObj[_Player1].GetCharaSprite;
            _player1Text.sprite = _ChataText[_Player1];

            Player01_Obj.transform.localScale = new Vector3(_xSize[_Player1], _ySize[_Player1], 1);
        }
        //2P処理
        if (Input.GetKeyDown(KeyCode.DownArrow) && !Player2_OK)
        {
            CharaObj[_Player2].charaSelect(2, false);
            _Player2++;
            _Player2 = _Player2 % length;
            CharaObj[_Player2].charaSelect(2, true);

            Player02.sprite = CharaObj[_Player2].GetCharaSprite;
            _player2Text.sprite = _ChataText[_Player2];
            Player02_Obj.transform.localScale = new Vector3(_xSize[_Player2], _ySize[_Player2], 1);

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !Player2_OK)
        {
            CharaObj[_Player2].charaSelect(2, false);
            _Player2--;
            _Player2 = _Player2 % length;
            if (_Player2 < 0) _Player2 = 3;
            CharaObj[_Player2].charaSelect(2, true);

            Player02.sprite = CharaObj[_Player2].GetCharaSprite;
            _player2Text.sprite = _ChataText[_Player2];

            Player02_Obj.transform.localScale = new Vector3(_xSize[_Player2], _ySize[_Player2], 1);
        }

        //選択時//渡す値を決定する
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Player1_OK = true;
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Player2_OK = true;
        }

        //×ボタンの処理
        //長押しで画面移動処理
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //キャラ選択時は選択を外す
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Player1_OK = false;
            }
            float difference = Time.time - Player1_Time;
            SetSilder(difference);
            if (difference > ReturnTime)
            {
                Debug.Log("一つ前の画面へ");
            }
        }
        if (Input.GetKey(KeyCode.RightControl))
        {
            //キャラ選択時は選択を外す
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Player2_OK = false;
            }
            float difference = Time.time - Player2_Time;
            SetSilder(difference);
            if (difference > ReturnTime)
            {
                Debug.Log("一つ前の画面へ");
            }
        }
        //両方入力されていない
        if (!Input.GetKey(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl))
        {
            Player1_Time = Time.time;
            Player2_Time = Time.time;
            //片方が入力しているとそのまま継続
            ReturnSlider.value = 0f;
        }
        Text01.SetActive(Player1_OK);
        Text02.SetActive(Player2_OK);
    }


    void SetSilder(float t)
    {
        if (t < ReturnSlider.value) return;
        ReturnSlider.value = t;
    }
}