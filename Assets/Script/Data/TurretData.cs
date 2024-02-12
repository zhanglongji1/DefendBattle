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
        //���� ���  �˺�          ��Χ
        public float Speed;//����
        public int Money;    //��̨�Ľ��컨��
        public int Value;//�˺�
        public int Time;//����ʱ��
        public int Chance;//��������
        public int Times;//��������
        public int Range; //��Χ
        public int BulletSpeed ;   //�ӵ������ٶ�
        public int Buff;
        public enum states
        {
            Idle, Rotate, Upgrade, Attack
        }
    }
}
