using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Turret;

public class MapNode : MonoBehaviour
{
    [HideInInspector]
    public GameObject turretGo; //���浱ǰCube���ϵ���̨���� 
    private Color initColor;
 


    public bool isUpgraded = false;  //�ж�Cube���� ����̨������� 
    public int upgradeds = 2;
    public Turrets turrets;    //��̨����
    public Color color;
    public static MapNode Map;
    void Start()
    {
        Map = this;
        color = this.GetComponent<MeshRenderer>().material.color;
    }


   
   

    public void BuildTurret(GameObject GameObject)
    {//�����������̨����
        

        //������̨���壬���е�turretData��������̨��Ԥ����
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
        
        Debug.Log("����"+i);
        if (i == upgradeds)
        {
            isUpgraded = true;
        }
        //����������ж�����̨Ҫ���������� ����ֱ������
        if (isUpgraded == true) return;
        //��̨��������
        if (i<upgradeds) {  ++i; }
        // ��ȡ��ǰ��scale
        Vector3 currentScale = turretGo.transform.localScale;
        // �����µ�scale
        Vector3 newScale = new Vector3(currentScale.x+(currentScale.x*0.2f), currentScale.y + (currentScale.y * 0.2f), currentScale.z + (currentScale.z * 0.2f)); // ����Ϊ�Ŵ�����
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
        //������̨���ÿ�һЩ����
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
