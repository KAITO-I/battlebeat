﻿//スキル仕様変更により修正 by　金川 2019-07-07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Hp;
    public float HpMax;
    public float Sp;
    public float SpMax;
    public Vector2Int Pos;
    public virtual void TakeDamage(float Damage) { Hp -= Damage; Debug.Log(gameObject.name + "が" + Damage.ToString() + "ダメージを受けた。"); }
    public virtual float GetSpecialParameter(string str) { return 0; }

    public enum MoveComand
    {
        None,
        Left,
        Right,
        Up,
        Down,
        Attack_1,
        Attack_2,
        Attack_3,
        Attack_4
    }

    private MoveComand input = MoveComand.None;
    private bool canInput = true;

    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode UpKey;
    public KeyCode DownKey;
    public KeyCode Attack_1Key;
    public KeyCode Attack_2Key;
    public KeyCode Attack_3Key;
    public KeyCode Attack_4Key;

    //指定した場所に出現させるため
    

    [SerializeField]
    public int PlayerID;
    [SerializeField]
    public int[] CoolDownCount = new int[4];

    void Start()
    {
        transform.position = BoardManager._instance.ToWorldPos(Pos);
        //Playerの位置が同じになってしまうので少し上げる
        transform.position += new Vector3(0, 1f, 0);
        Hp = HpMax;
        Sp = 0;
        IStart();

    }
    protected virtual void IStart()
    {

    } 
    // Update is called once per frame
    void Update()
    {
        //Test();
        if (canInput)
        {
            if (Input.GetKeyDown(LeftKey)) input = MoveComand.Left;
            else if (Input.GetKeyDown(RightKey)) input = MoveComand.Right;
            else if (Input.GetKeyDown(UpKey)) input = MoveComand.Up;
            else if (Input.GetKeyDown(DownKey)) input = MoveComand.Down;
            else if (Input.GetKeyDown(Attack_1Key)) input = MoveComand.Attack_1;
            else if (Input.GetKeyDown(Attack_2Key)) input = MoveComand.Attack_2;
            else if (Input.GetKeyDown(Attack_3Key)) input = MoveComand.Attack_3;
            else if (Input.GetKeyDown(Attack_4Key)) input = MoveComand.Attack_4;
            if (input != MoveComand.None) canInput = false;
        }
    }

    public virtual void Turn()
    {
        PlayerMove(input);
        input = MoveComand.None;
        canInput = true;
    }

    //Playerが動けるタイミングになったら活動できる
    void  PlayerMove(MoveComand move)
    {
        int TempY = Pos.y;
        int TempX = Pos.x;

        //ジャンプのタイミングを一定間隔::どの位置に飛ぶのかチェック
        //行動を操作する
        switch (move)
        {
            //左移動
            case MoveComand.Left:
                TempX -= 1;
                if (!BoardManager._instance.Is_In_Stage(TempX, TempY, PlayerID))
                {
                    TempX = Pos.x;
                }
                break;
            //右移動
            case MoveComand.Right:
                TempX += 1;
                if (!BoardManager._instance.Is_In_Stage(TempX, TempY, PlayerID))
                {
                    TempX = Pos.x;
                }
                break;
            //上移動
            case MoveComand.Up:
                TempY -= 1;
                if (!BoardManager._instance.Is_In_Stage(TempX, TempY, PlayerID))
                {
                    TempY = Pos.y;
                }
                break;
            //下移動
            case MoveComand.Down:
                TempY += 1;
                if (!BoardManager._instance.Is_In_Stage(TempX, TempY, PlayerID))
                {
                    TempY = Pos.y;
                }
                break;
            //攻撃移動
            case MoveComand.Attack_1:
                Attack_1();
                break;
            case MoveComand.Attack_2:
                Attack_2();
                break;
            case MoveComand.Attack_3:
                Attack_3();
                break;
            case MoveComand.Attack_4:
                Attack_4();
                break;
            case MoveComand.None:
                break;
            default:
                break;
        }

        if (move >= MoveComand.Left && move <= MoveComand.Down)
        {
            //ここでポジションを変更してる
            transform.position = BoardManager._instance.ToWorldPos(new Vector2Int(TempX, TempY));
            //Playerの位置が同じになってしまうので少し上げる
            transform.position += new Vector3(0, 1f, 0);
            //現在のポジションを入れる
            Pos.x = TempX;
            Pos.y = TempY;
        }

    }

    protected virtual void Attack_1()
    {
    }
    protected virtual void Attack_2()
    {
    }
    protected virtual void Attack_3()
    {
    }
    protected virtual void Attack_4()
    {
    }
    protected virtual float GetBuff() { return 0; }
}
