using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public class TurretData
    {
        //Speed	Money	value	Time	Chance	times

        public int Id;
        public string Name;
        //攻速 造价  伤害          范围
        public float Speed;//攻速
        public int Money;    //炮台的建造花费
        public int Value;//伤害
        public int Time;//缓动时间
        public int Chance;//暴击概率
        public int Times;//暴击倍率
        public int Range; //范围
        public int BulletSpeed ;   //子弹飞行速度
        public int Buff;
        public enum states
        {
            Idle, Rotate, Upgrade, Attack
        }
    }
}
