﻿//------------------------------------
//作成者：木原　プレイヤーステータス
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatas : MonoBehaviour
{
    [SerializeField] int pnum;//プレイヤー番号
    [SerializeField] int id;//キャラID
    [SerializeField] int hp;//現在HP
    [SerializeField] int sp;//現在SP


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetHP()
    {
        return hp;
    }
    //HP減少(回復するならvalueを-にして)
    public void SetHP(int value)
    {
        hp -= value;
    }
    public int GetSP()
    {
        return sp;
    }
    public void SetSP(int value)
    {
        sp += value;
    }
}
