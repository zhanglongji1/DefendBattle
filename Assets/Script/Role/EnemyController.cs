using Assets.Script.Data;
using Assets.Script.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform endPos;
    public EnemyData enemyData;
    public Image image;
    public float hp=0;
    public int atk;
    public int enemyMoney;
    public Color color;
    private float timer; // 计时器
    private float originalSpeed; // 敌人的原始速度
    Vector3 Vector;
    private void Start()
    {
        color = this.GetComponent<MeshRenderer>().material.color;
        Vector = this.transform.position;
    }
    void OnEnable()
    {
    
        enemyData = DataManager.Instance.Enemys[1];
        agent = GetComponent<NavMeshAgent>();
        endPos = GameObject.Find("Navigation").transform.GetChild(1);
        hp =enemyData.HP+(SceneManager.GetActiveScene().buildIndex - 1) * enemyData.HPAdd;
        image.fillAmount = (float)hp / enemyData.HP;
        atk = enemyData.AD;
        enemyMoney = enemyData.Money+ (SceneManager.GetActiveScene().buildIndex - 1) * enemyData.MoneyAdd;
        agent.speed = enemyData.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.SetDestination(endPos.position);
        image.fillAmount = (float)hp / enemyData.HP;
    }
    public void HpDamage(float damage)
    {
        //敌人血量<=0 ，直接退出调用，保险起见
        if (hp <= 0) return;
        hp -= damage;
        //改变血条UI的显示
        image.fillAmount = (float)hp / enemyData.HP;
        //死亡
        if (hp <= 0)
            Die();
    }
    public void SlowAndRestore(int id)
    {

         this.agent.speed -= DataManager.Instance.Turrets[id].Buff;
         this.GetComponent<MeshRenderer>().material.color = Color.blue;
        if (hp>0)
        {
            StartCoroutine(SpeedRest());
        }
        
       
       
        // 恢复敌人原始速度 
    }
    IEnumerator SpeedRest()
    {
       
        yield return new WaitForSeconds(4);
        agent.speed = enemyData.Speed;
        this.GetComponent<MeshRenderer>().material.color = color;
        Debug.Log("恢复速度");
        yield break;
    }

    void Die()
    {
        Turrets.Instant.attackList.Remove(this.gameObject);
        StopCoroutine(StartCoroutine(SpeedRest()));
        this.gameObject.transform.position=Vector;
        BulletPoolManger.Instance.PushObject(this.gameObject);
        EnemyManager._instance.enemiesInRange.Remove(this.gameObject);
        
        // Destroy(this.gameObject);
        TurretManager.Instance.ChangeMoney(enemyMoney);
        this.GetComponent<MeshRenderer>().material.color = color;
    }
}
