using Assets.Script.Manager;
using Assets.Script.Utilts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Turret;

public class Turrets : MonoBehaviour
{
    public int id;
    public float checkeDistance;//攻击检测距离
    //public RaycastHit[] enemiesInRange =new RaycastHit[10];
    public List<GameObject> attackList = new List<GameObject>();
    public LayerMask targetLayer;
    public TurretData TurretData;
    //攻速，炮台攻击冷却时间
    public float attackRatetime ;
    //定时器，用来计算冷却时间的流逝
    private float timer = 0;

    //炮弹预制体 
    public GameObject bulletPrefab;
    //炮弹生成位置，即开火位置
    public Transform firePosition;
    // Start is called before the first frame update

    private GameObject targetEnemy; // 当前炮塔锁定的敌人

    //线渲染器，渲染激光
    private LineRenderer laserRenderer;

    public float attackValue;
    public static Turrets Instant;
    public TurretData.states currentState ;

    //记录初始速度范围
    public bool isSpeed=false;
    public float Ratetime;

    public bool isRange = false;
    public float cRange;
    public List<GameObject> target;
    private void Awake()
    {
        Instant = this;
        TurretData = DataManager.Instance.Turrets[id];
        checkeDistance = TurretData.Range;
        attackRatetime = TurretData.Speed;
        attackValue = TurretData.Value;
        timer = attackRatetime;

    }
    private void Start()
    {
        
        laserRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 3 || id == 6)
        {
            target = TurretManager.instance.turretsInRange;
        }
        else 
        {
            target = EnemyManager._instance.enemiesInRange;
        }
            
        if (target != null)
            {
                foreach (GameObject go in target)
                {
                    if (checkeDistance >= (int)Vector3.Distance(go.transform.position, this.gameObject.transform.position))
                    {
                        if (attackList.Contains(go))
                            break;

                        attackList.Add(go);
                        //Debug.Log("添加" + Time.deltaTime);
                    }
                }
                Attack();

            }
        

    }

    private void Attack()
    {

        int count = attackList.Count;
        // int count = Physics.SphereCastNonAlloc(transform.position, checkeDistance, Vector3.forward, enemys, 0, targetLayer.value)
        if (count > 0 && currentState != TurretData.states.Upgrade)
        {

            currentState = TurretData.states.Rotate;
            Vector3 direction = attackList[0].transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 将目标旋转应用到子物体的旋转上
            if (attackList[0].gameObject.tag == "Enemy" & id != 4)
            {
                this.transform.GetChild(0).rotation = Quaternion.Slerp(
                    this.transform.GetChild(0).rotation,
                    targetRotation,
                    TurretData.Time * Time.deltaTime  // 这里的 rotationSpeed 应该是一个合适的旋转速度值
                );
            }
            float angleDifference = Quaternion.Angle(this.transform.GetChild(0).rotation, targetRotation);
            currentState = TurretData.states.Attack;
            if (id == 5 && angleDifference < 10f)
            {
                LaserAttack(count);

            }
            else if (id == 3 || id == 6)
            {
                BuffAttack();
            }
            else if (id == 4)
            {
                RangeAttack();
            }
            else if (id == 1 || id == 2 || id == 7)
            {
                AttackEmit(count);
            }

    }
        if (attackList.Count != 0)
        {
            if ((int)Vector3.Distance(attackList[0].transform.position, this.gameObject.transform.position) > checkeDistance)
                attackList.Remove(attackList[0]);
        }

        else
        {
            if (!currentState.Equals(TurretData.states.Upgrade))
            {
                currentState = TurretData.states.Idle;
            }
            if (laserRenderer != null)
            {
                laserRenderer.enabled = false;

            }
        }
           
    }
   
    private void RangeAttack()
    {
        if (id == 4)
        {
            foreach (var item in attackList )
            {
               
                if (item != null)
                {
                    EnemyController controller = item.GetComponent<EnemyController>();
                    if (controller.agent.speed == controller.enemyData.Speed)
                    {
                        controller.SlowAndRestore(id);
                    }
                    

                }
            }
        }
    }

    private void BuffAttack()
    {
        foreach (var item in attackList )
        {
            if (item != null&&item.gameObject.tag=="Turret")
            {
                Turrets turrets = item.GetComponent<Turrets>();
                if (id == 3)
                {
                    if (turrets.isSpeed == false)
                    {
                        turrets.Ratetime = turrets.attackRatetime;
                        turrets.attackRatetime -= this.attackRatetime;
                        turrets.isSpeed = true;
                    }
                }
                else if (id == 6)
                {

                    if (turrets.isRange == false)
                    {
                        turrets.cRange = turrets.checkeDistance;
                        turrets.checkeDistance += this.attackValue;
                        turrets.isRange = true;
                    }
                }



            }
        }

    }

    public void LaserAttack(int count)
    {
        
        if (laserRenderer.enabled == false)
            laserRenderer.enabled = true;
        if (count > 0)
        {//画线，即画激光的起点和终点
            laserRenderer.SetPositions(new Vector3[] { firePosition.position, attackList[0].transform.position });
            //敌人每秒受伤，调用Enemy类的扣血方法 
            attackList[0].gameObject.GetComponent<EnemyController>().HpDamage(TurretData.Value * Time.deltaTime);
        }
    }
    private void AttackEmit(int count)
    {
  
        timer += Time.deltaTime;
        if (count == 0)
            timer = attackRatetime;
        else if (count > 0 && timer >= attackRatetime)
        {
            //让定时器进入冷却状态
            timer= 0;
            //攻击
            if (count > 0)
            {//生成炮弹
                
                GameObject bullet = BulletPoolManger.Instance.GetObject(bulletPrefab);
                //调用炮弹目标设定
                bullet.transform.position = firePosition.position;
                bullet.GetComponent<Bullet>().SetTarget(attackList[0].gameObject, TurretData.Value, TurretData.BulletSpeed,id);
            }
            else //敌人死完了，重置定时器 
                timer = attackRatetime;
        }

    }

    private void OnDrawGizmosSelected()
    {
        //画检测范围
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
        Gizmos.DrawSphere(transform.position, checkeDistance);
    }
    }
