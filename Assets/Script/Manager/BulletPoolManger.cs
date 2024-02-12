using Assets.Script.Utilts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManger : Singleton<BulletPoolManger>
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;

    // �����,�����ṩ�ķ���
    // ��ȡһ������
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;//�������

        if (pool == null) //������û�ж����ʱ����һ�ν�����Ϸ�����л��˳��������½�һ���������Ϸ��Ʒ������ֵ�
        {
            pool = new GameObject("ObjectPool");
            objectPool = new Dictionary<string, Queue<GameObject>>();
        }
        //���������û�и���Ʒ
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            //ʵ���������������
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);

            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            //���õ�����ڵ�����Ʒ�£��������
            _object.transform.SetParent(childPool.transform);
        }
        //�Ӷ�������ȡ���󣬷���
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }
    

    // ����һ������
    public void PushObject(GameObject prefab)
    {
        //ͨ��Instantiateʵ��������Ʒ�����У�Clone����׺�����ǽ���ȥ���ٴ洢
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(_name))
            objectPool.Add(_name, new Queue<GameObject>());
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }

}
