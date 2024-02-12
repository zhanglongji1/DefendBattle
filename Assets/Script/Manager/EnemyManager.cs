using Assets.Script.Data;
using Assets.Script.Manager;
using Assets.Script.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public GameObject Enemy;
    private Transform startingPoint;
    int id ;
    public Text level;
    public Text wave;//����
    GameObject gameEnemy;
    //��Ϸ������UI
    public GameObject endUI;
    //������Ϣ
    public Text endMessage;
    public Dictionary<int, GenerateTimeData> GenerateTimeData ;
    private Coroutine myCoroutine;// ����һ��Э�̵�����
    public List<GameObject> enemiesInRange = new List<GameObject>();//�����б�
    int i, j = 0;
    public static EnemyManager _instance;
    void Start()
    { _instance = this;
        id = SceneManager.GetActiveScene().buildIndex;
        startingPoint = GameObject.Find("Navigation").transform.GetChild(0);
        GenerateTimeData = DataManager.Instance.GenerateTimeData;
        myCoroutine = StartCoroutine(GenerateEnemy());
        level.text = id.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EndManager.instance.hp == 0)
        {
            Lose();
        }
        

    }
    //��Ϸʧ�ܺ�ֹͣ���˵�����
    public void Stop()
    {
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
            
        }
    }

    IEnumerator GenerateEnemy()
    {
        
        for (i = 0; i < GenerateTimeData[id].wave; i++)
        {
        
            wave.text = (i+1).ToString();
            for ( j= 0; j < GenerateTimeData[id].count; j++)
            {
              gameEnemy= BulletPoolManger.Instance.GetObject(Enemy);
                gameEnemy.transform.position = startingPoint.position;
                enemiesInRange.Add(gameEnemy);


                if (j!= GenerateTimeData[id].count)
                {
                    yield return new WaitForSeconds(GenerateTimeData[id].rate);
                }
            }
            yield return new WaitForSeconds(GenerateTimeData[id].time);
            if ((i + 1) == 12)
            {
                id = id + 1;
                i = -1;
                SceneManager.LoadSceneAsync(id);
                yield return new WaitForSeconds(1f);
                startingPoint = GameObject.Find("Navigation").transform.GetChild(0);
                level.text = id.ToString();
                
            }
        }
        if (id == 10 && j > GenerateTimeData[id].count)
        {
            Win();
        }

    }
    //���¿�ʼ
    public void ReturnMain()
    {
       
        UiInstall.instance.End.SetActive(false);
        i = -1;
        EndManager.instance.hp = 10;
        TurretManager.instance.money= DataManager.Instance.GenerateTimeData[SceneManager.GetActiveScene().buildIndex].gold;
        TurretManager.instance.gold.text = DataManager.Instance.GenerateTimeData[SceneManager.GetActiveScene().buildIndex].gold.ToString();

        DestroyTag();
        myCoroutine = StartCoroutine(GenerateEnemy());

    }
    void DestroyTag()
    {
        // �ҵ����������д���ָ����ǩ����Ϸ����
        GameObject[] objectsWithTargetTag = GameObject.FindGameObjectsWithTag("Turret");
        GameObject[] map = GameObject.FindGameObjectsWithTag("Barbette");
        // ���������ҵ�����Ϸ����
        foreach (GameObject obj in objectsWithTargetTag)
        {
           
                    Destroy(obj);
        }
        // ���������ҵ�����Ϸ����
        foreach (GameObject obj in map)
        {

           obj.GetComponent<MeshRenderer>().material.color = MapNode.Map.color;
        }
       
    }
    //ʤ��
    public void Win()
    {
        //��ʾUI�����ʤ����Ϣ 
        endUI.SetActive(true);
        endMessage.text = "ʤ��!";
    }
    //��Ϸʧ��
    public void Lose()
    {
        //ֹͣ��������
        Stop(); //Time.timeScale = 0;
        endUI.SetActive(true);
        endMessage.text = "ʧ��!";

        GameObject[] objectsWithTargetTag = GameObject.FindGameObjectsWithTag("Enemy");

        // ���������ҵ�����Ϸ����
        foreach (GameObject obj in objectsWithTargetTag)
        {

            BulletPoolManger.Instance.PushObject(obj);
        }


    }
}
