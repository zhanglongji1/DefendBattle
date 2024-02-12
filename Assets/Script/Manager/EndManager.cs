using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public int hp=10;
    public static EndManager instance;

 
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
           hp=hp- col.GetComponent<EnemyController>().atk;

            BulletPoolManger.Instance.PushObject(col.gameObject);
            EnemyManager._instance.enemiesInRange.Remove(col.gameObject);
            Turrets.Instant.attackList.Remove(col.gameObject);
            //Destroy(col.gameObject);
        }
    }

}
