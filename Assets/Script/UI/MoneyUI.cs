using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public void DelayAction()
    {
        BulletPoolManger.Instance.PushObject(this.gameObject);
    }
}
