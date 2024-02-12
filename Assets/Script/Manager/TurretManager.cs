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
  
    private int cost;//花费
    private int upgradeCost;//升级
    public Text gold;
    public int money;

    private MapNode selectedMapCube;
    public GameObject optionCanvas;
    public Button upgradeBtn;
    public GameObject costGameobject;
    GameObject obj;
    public static TurretManager instance;
    public List<GameObject> turretsInRange = new List<GameObject>();//炮塔列表
    MapNode mapCube;
    void Start()
    {   
        instance = this;
        money= DataManager.Instance.GenerateTimeData[SceneManager.GetActiveScene().buildIndex].gold;
        gold.text = "￥" +money;

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
        // 检查是否有触摸输入
        if (Input.GetMouseButtonDown(0))
        {

            // 检查触摸点是否在UI元素上   IsPointerOverGame0bject
            if (Input.GetTouch(0).phase == TouchPhase.Began&&!(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))) 
            {
                // 如果不在UI上，执行触摸操作
                CreateTurret();
            }
        }
    }

    private void HandleMouseInput()
    {
        // 检查鼠标左键是否按下
        if (Input.GetMouseButtonDown(0))
        {
            // 检查鼠标是否在UI元素上
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // 如果不在UI上，执行鼠标操作
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

            //可以创建，并先判断金钱数目
            if (money >= cost)
            {

                //减少金钱数目并调用建造方法
                ChangeMoney(-cost);
                mapCube.BuildTurret(selectedTurretData);
                background.SetActive(false);
                selectedTurretData = null;
            }
            else
            {
                obj = BulletPoolManger.Instance.GetObject(costGameobject);

                //StartCoroutine(DelayAction(() => BulletPoolManger.Instance.PushObject(obj), 1f));
                Debug.Log("不足");
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
            //获取MapCube类对象 
            mapCube = hit.collider.GetComponent<MapNode>();
            if (mapCube.turretGo == null)
            {
                background.SetActive(true);
              
            }
            if (mapCube.turretGo != null)
            {
                Turrets.Instant.currentState = TurretData.states.Upgrade;
                mapCube.addBuff();
                //当再点击一下炮台且升级或拆除炮台的UI还未隐藏 
                //否则就显示UI ，传入显示位置，还有是否 升级的判定，用于控制升级按钮的弃用
                    ShowOptionUI(mapCube.transform.position, mapCube.turrets.checkeDistance, mapCube.isUpgraded);
                
                selectedMapCube = mapCube;
            }
        }
        if (!isCollider||(mapCube != selectedMapCube && optionCanvas.activeInHierarchy))
        {
            //隐藏UI
            Invoke("HideOptionUI", 0.3f);
        }
    }


    //显示升级或拆除UI的函数，第一个参数是显示位置 ，第二个是升级按钮是否弃用
    void ShowOptionUI(Vector3 pos, float checkeDistance ,bool isDisable = false)
    {
        
        //手动隐藏
        optionCanvas.SetActive(false);
        AttackRangeDisplay.a_Instant.Off();
        //再启用回来
        optionCanvas.SetActive(true);

        //让 UI显示在炮台位置
        optionCanvas.transform.position = new Vector3(pos.x,pos.y+15,pos.z) ;

        if (isDisable) upgradeBtn.GetComponentInChildren<Text>().text = "Max";
        else upgradeBtn.GetComponentInChildren<Text>().text = "升级";
        AttackRangeDisplay.a_Instant.Open(pos, checkeDistance);
    }

    public void HideOptionUI()
    {
       
        //隐藏UI
        AttackRangeDisplay.a_Instant.Off();
        optionCanvas.SetActive(false);
        if (optionCanvas.activeSelf)
        {
            Turrets.Instant.currentState = TurretData.states.Idle;
        }
        
    }
 

    //炮台升级按钮事件
    public void OnUpgradeBtnDown()
    {
        if (upgradeBtn.GetComponentInChildren<Text>().text == "Max")
        {
            Invoke("HideOptionUI", 0.3f);
            return;
        }
        //可以创建，并先判断金钱数目
        if (money >= upgradeCost)
        {
            
            //减少金钱数目并调用建造方法
            ChangeMoney(-upgradeCost);
            selectedMapCube.UpgradeTurret();
            upgradeCost +=(int)(upgradeCost * 0.3m);
        }
        else
        {
            obj = BulletPoolManger.Instance.GetObject(costGameobject);
            return;
        }

        //升级完隐藏UI
        HideOptionUI();
    }
    //炮台销毁事件
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
    //控制金钱数量变化
    public void ChangeMoney(int change = 0)
    {
      
        money += change;
        gold.text = "￥" +money;
    }

  
}
