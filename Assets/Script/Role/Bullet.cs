using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //炮弹伤害值和速度
    private float damage ;
    private float speed ;
    private int id;
    //炮弹射击目标 
    private GameObject target;
    private bool isEnemySlowed = false; // 敌人是否已经被减速
    private float originalSpeed; // 敌人的原始速度
    private Vector3 vector;
    public GameObject atkChance;
    Vector3 startPosition;
    float fireTime;
    float hp;
    //设定炮弹将击中的目标
    public void SetTarget(GameObject target, float damage, float speed ,int id)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.id = id;
         startPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //当目标为空，炮弹 自动爆炸 并退出函数
        if (target == null)
        {
            Die();
            return;
        }
        //向敌人移动
        transform.LookAt(target.transform.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // 获取当前物体和目标物体的位置

        if (target.GetComponent<EnemyController>().hp <=0f)
        {
            Die();
        }
    }

    //伤害
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag=="Enemy")
        {
            
        
            if (id==7)//暴击
            {
                if (Random.Range(1, 101)<=DataManager.Instance.Turrets[id].Chance)
                {
                    col.GetComponent<EnemyController>().HpDamage(damage* DataManager.Instance.Turrets[id].Times);
                    atkChance.GetComponent<Test>().ChanceUp(damage * DataManager.Instance.Turrets[id].Times);
                    vector = new Vector3(col.transform.position.x + 2, col.transform.position.y + 2, col.transform.position.z + 3);
                    Instantiate(atkChance,vector,atkChance.transform.rotation);
                    Die();

                }
                else
                    col.GetComponent<EnemyController>().HpDamage(damage);
                Die();

            }
            else if (id==2)
            {
                col.GetComponent<EnemyController>().HpDamage(damage);
                Invoke("Die",0.3f);
            }
            else//普通
            {
                col.GetComponent<EnemyController>().HpDamage(damage);
                Die();
            }
           
        }
        else if (col==null)
        {
            Die();
        }
    }
   
    //炮弹爆炸函数
    void Die()
    {
        BulletPoolManger.Instance.PushObject(this.gameObject);
        //Destroy(this.gameObject);
       
    }


}
