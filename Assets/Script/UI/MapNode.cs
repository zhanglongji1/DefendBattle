using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Turret;

public class MapNode : MonoBehaviour
{
    [HideInInspector]
    public GameObject turretGo; //保存当前Cube身上的炮台物体 
    private Color initColor;
 


    public bool isUpgraded = false;  //判断Cube身上 的炮台升级与否 
    public int upgradeds = 2;
    public Turrets turrets;    //炮台数据
    public Color color;
    public static MapNode Map;
    void Start()
    {
        Map = this;
        color = this.GetComponent<MeshRenderer>().material.color;
    }


   
   

    public void BuildTurret(GameObject GameObject)
    {//储存参数的炮台数据
        

        //生成炮台物体，其中的turretData保存了炮台的预制体
        turretGo = Instantiate(GameObject);
        turretGo.transform.position = new Vector3(transform.position.x, transform.position.y+2, transform.position.z);
        TurretManager.Instance.selectedTurretData = null;
        TurretManager.instance.turretsInRange.Add(turretGo);
         turrets = turretGo.GetComponent<Turrets>();
        this.GetComponent<MeshRenderer>().material.color =Color.red ;

        

    }
    int i = 1;
    public void UpgradeTurret()
    {
        
        Debug.Log("升级"+i);
        if (i == upgradeds)
        {
            isUpgraded = true;
        }
        //保险起见先判断下炮台要是升级过了 ，就直接跳出
        if (isUpgraded == true) return;
        //炮台升级过了
        if (i<upgradeds) {  ++i; }
        // 获取当前的scale
        Vector3 currentScale = turretGo.transform.localScale;
        // 设置新的scale
        Vector3 newScale = new Vector3(currentScale.x+(currentScale.x*0.2f), currentScale.y + (currentScale.y * 0.2f), currentScale.z + (currentScale.z * 0.2f)); // 设置为放大两倍
         turretGo.transform.localScale = newScale;
        turrets.attackValue = turrets.TurretData.Value+turrets.TurretData.Value*0.2f;
        turrets.checkeDistance= turrets.TurretData.Range+turrets.TurretData.Range*0.2f;
        turrets.attackRatetime = turrets.TurretData.Speed- turrets.TurretData.Speed * 0.2f;
        if (turrets.id == 3 || turrets.id == 6)
        {
            foreach (var item in turrets.attackList )
            {
                if (item != null && item.gameObject.tag == "Turret")
                {
                    Turrets speed = item.GetComponent<Turrets>();
                    if (turrets.id == 3)
                    {
                        speed.isSpeed = false;
                    }
                    else if (turrets.id == 6)
                    {
                        speed.isRange = false;
                    }
                }

            }
        }
    }
    public int RemoveTurret()
    {
        this.GetComponent<MeshRenderer>().material.color = color;
        addBuff();
        //销毁炮台并置空一些变量
        Destroy(turretGo);
        turretGo = null;
        isUpgraded = false;
        return i;
        
    }

    public void addBuff()
    {
        if (turrets.id == 3 || turrets.id == 6)
        {
            foreach (var item in turrets.attackList )
            {
                if (item != null && item.gameObject.tag == "Turret")
                {
                    Turrets speed = item.GetComponent<Turrets>();
                    if (turrets.id == 3)
                    {
                        speed.isSpeed = false;
                        speed.attackRatetime = speed.Ratetime;
                    }
                    else if (turrets.id == 6)
                    {
                        speed.isRange = false;
                        speed.checkeDistance = speed.cRange;
                    }
                }

            }
        }
    }
}
