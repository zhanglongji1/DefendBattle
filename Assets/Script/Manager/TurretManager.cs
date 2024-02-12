using Assets.Script.Manager;
using Assets.Script.Utilts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Turret;

public class TurretManager : MonoSingleton<TurretManager>
{
    public GameObject[] GameObject;
    public GameObject selectedTurretData;
    public GameObject background;
  
    private int cost;//����
    private int upgradeCost;//����
    public Text gold;
    public int money;

    private MapNode selectedMapCube;
    public GameObject optionCanvas;
    public Button upgradeBtn;
    public GameObject costGameobject;
    GameObject obj;
    public static TurretManager instance;
    public List<GameObject> turretsInRange = new List<GameObject>();//�����б�
    MapNode mapCube;
    void Start()
    {   
        instance = this;
        money= DataManager.Instance.GenerateTimeData[SceneManager.GetActiveScene().buildIndex].gold;
        gold.text = "��" +money;

    }



    void Update()
    {

#if UNITY_EDITOR
        HandleMouseInput();
#else

            HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        // ����Ƿ��д�������
        if (Input.GetMouseButtonDown(0))
        {

            // ��鴥�����Ƿ���UIԪ����   IsPointerOverGame0bject
            if (Input.GetTouch(0).phase == TouchPhase.Began&&!(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))) 
            {
                // �������UI�ϣ�ִ�д�������
                CreateTurret();
            }
        }
    }

    private void HandleMouseInput()
    {
        // ����������Ƿ���
        if (Input.GetMouseButtonDown(0))
        {
            // �������Ƿ���UIԪ����
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // �������UI�ϣ�ִ��������
                CreateTurret();
               
            }
        }
    }
    public void SelecTurret(int i)
    {
        selectedTurretData = GameObject[i - 1];
        cost = DataManager.Instance.Turrets[i].Money;
        upgradeCost = cost + (int)Math.Round(cost * 0.3f);
        if (mapCube.turretGo == null && selectedTurretData != null)
        {

            //���Դ����������жϽ�Ǯ��Ŀ
            if (money >= cost)
            {

                //���ٽ�Ǯ��Ŀ�����ý��췽��
                ChangeMoney(-cost);
                mapCube.BuildTurret(selectedTurretData);
                background.SetActive(false);
                selectedTurretData = null;
            }
            else
            {
                obj = BulletPoolManger.Instance.GetObject(costGameobject);

                //StartCoroutine(DelayAction(() => BulletPoolManger.Instance.PushObject(obj), 1f));
                Debug.Log("����");
            }
        }
        }
    public void CreateTurret()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
         
        if (isCollider)
        {
            //��ȡMapCube����� 
            mapCube = hit.collider.GetComponent<MapNode>();
            if (mapCube.turretGo == null)
            {
                background.SetActive(true);
              
            }
            if (mapCube.turretGo != null)
            {
                Turrets.Instant.currentState = TurretData.states.Upgrade;
                mapCube.addBuff();
                //���ٵ��һ����̨������������̨��UI��δ���� 
                //�������ʾUI ��������ʾλ�ã������Ƿ� �������ж������ڿ���������ť������
                    ShowOptionUI(mapCube.transform.position, mapCube.turrets.checkeDistance, mapCube.isUpgraded);
                
                selectedMapCube = mapCube;
            }
        }
        if (!isCollider||(mapCube != selectedMapCube && optionCanvas.activeInHierarchy))
        {
            //����UI
            Invoke("HideOptionUI", 0.3f);
        }
    }


    //��ʾ��������UI�ĺ�������һ����������ʾλ�� ���ڶ�����������ť�Ƿ�����
    void ShowOptionUI(Vector3 pos, float checkeDistance ,bool isDisable = false)
    {
        
        //�ֶ�����
        optionCanvas.SetActive(false);
        AttackRangeDisplay.a_Instant.Off();
        //�����û���
        optionCanvas.SetActive(true);

        //�� UI��ʾ����̨λ��
        optionCanvas.transform.position = new Vector3(pos.x,pos.y+15,pos.z) ;

        if (isDisable) upgradeBtn.GetComponentInChildren<Text>().text = "Max";
        else upgradeBtn.GetComponentInChildren<Text>().text = "����";
        AttackRangeDisplay.a_Instant.Open(pos, checkeDistance);
    }

    public void HideOptionUI()
    {
       
        //����UI
        AttackRangeDisplay.a_Instant.Off();
        optionCanvas.SetActive(false);
        if (optionCanvas.activeSelf)
        {
            Turrets.Instant.currentState = TurretData.states.Idle;
        }
        
    }
 

    //��̨������ť�¼�
    public void OnUpgradeBtnDown()
    {
        if (upgradeBtn.GetComponentInChildren<Text>().text == "Max")
        {
            Invoke("HideOptionUI", 0.3f);
            return;
        }
        //���Դ����������жϽ�Ǯ��Ŀ
        if (money >= upgradeCost)
        {
            
            //���ٽ�Ǯ��Ŀ�����ý��췽��
            ChangeMoney(-upgradeCost);
            selectedMapCube.UpgradeTurret();
            upgradeCost +=(int)(upgradeCost * 0.3m);
        }
        else
        {
            obj = BulletPoolManger.Instance.GetObject(costGameobject);
            return;
        }

        //����������UI
        HideOptionUI();
    }
    //��̨�����¼�
    public void OnRemoveBtnDown()
    {
        selectedMapCube.RemoveTurret();
        if (selectedMapCube.RemoveTurret()==1)
        {
            ChangeMoney((int)Math.Round(cost*0.5f));
        }
        else
        {
            ChangeMoney((int)(upgradeCost*0.5f));
        }
        HideOptionUI();
    }
    //���ƽ�Ǯ�����仯
    public void ChangeMoney(int change = 0)
    {
      
        money += change;
        gold.text = "��" +money;
    }

  
}
