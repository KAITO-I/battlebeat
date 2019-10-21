﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniAnimation : BasePlayerAnimation
{
    public enum UniState
    {
        Start,
        Wait,
        Attack,
        Back
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    //トラップの時はこの関数を呼んでもらう
    public void UniAnim(UniState state,GameObject player=null, Vector3? Gole=null)
    {
        Vector3 _Gole = Gole ?? Vector3.zero;//デフォルトでいらないため
        switch (state)
        {
            case UniState.Start:
                anim.SetTrigger("Start");
                Vector3 vec = _Gole;
                if (_playerClass.PlayerID == 1)//デバッグできないため未確認
                {
                    vec += new Vector3(1, 0, -1);
                }
                else vec += new Vector3(-1, 0, -1);
                StartCoroutine(enumerator(player,_Gole));//移動
                break;
            case UniState.Wait:
                anim.SetTrigger("Wait");
                break;
            case UniState.Attack:
                anim.SetTrigger("Attack");
                break;
            case UniState.Back:
                anim.SetTrigger("Back");
                StartCoroutine(enumerator(player, _Gole));//移動/戻るときはゾーンのいる位置に行く
                break;
        }
    }

}
