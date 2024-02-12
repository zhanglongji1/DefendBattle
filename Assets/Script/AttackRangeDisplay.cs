using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDisplay : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float angle = 360;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int quality = 12;
  
    public MeshFilter m_MeshFilter; // ����
    private MeshRenderer MeshRenderer;
    public GameObject sectorObj;
    public static AttackRangeDisplay a_Instant;
    void Awake()
    {
        a_Instant = this;
       // attackRange = Turrets.Instant.checkeDistance/Turrets.Instant.transform.localScale.x;
        
        // �� Start �����д���һ������� Mesh
        //CreateSphereMesh();
    }

    private bool isSectorObjActive = false;

    void Update()
    {
    }
    public void Open(Vector3 vector3, float checkeDistance)
    {
        if (!isSectorObjActive)
        {
            attackRange = checkeDistance;// Turrets.Instant.transform.localScale.x;
        sectorObj = GetSector(Vector3.zero, angle, attackRange, quality);
        sectorObj.transform.position = new Vector3(vector3.x, 1, vector3.z);
        sectorObj.transform.rotation = Quaternion.Euler(90, 0, 0);
        sectorObj.SetActive(true);
        isSectorObjActive = true;  // ����״̬Ϊ����
        }
    }
    public void Off()
    {
        // �������ǰ�ѱ��������ʾ״̬������������
        if (sectorObj != null)
        {
            sectorObj.SetActive(false);
            isSectorObjActive = false;  // ����״̬Ϊ����
        }
    }
    private GameObject GetSector(Vector3 center,float angle,float radius,int triangleCount)
    {
        float eachAngle = angle / triangleCount;
        List<Vector3> vector3s = new List<Vector3>();
        vector3s.Add(center);

        for (int i = 0; i <= triangleCount; i++) // ע������������� <=
        {
            float currentAngle = angle / 2 - eachAngle * i;
            Vector3 vertex = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * radius + center;
            vector3s.Add(vertex);
        }
        return CreateSectorMesh(vector3s);
    }

    private GameObject CreateSectorMesh(List<Vector3> vector3s)
    {
        // ��ʼ������������
        int[] triangles;
        int triangleAmount = vector3s.Count - 2; // ÿ��������3�����㣬����ǰ�������㣨���ĵ�͵�һ�����ζ��㣩
        triangles = new int[3 * triangleAmount]; // �ܹ���Ҫ����������

        // �������������
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;              // ���ĵ�
            triangles[3 * i + 1] = i + 1;      // ��ǰ���ζ���
            triangles[3 * i + 2] = i + 2;      // ��һ�����ζ���
        }

        // ���� UV ����
        Vector2[] uvs = new Vector2[vector3s.Count];
        uvs[0] = new Vector2(vector3s[0].x, vector3s[0].y); // �������ĵ�� UV ����
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vector3s[i].x, 1); // �����ж���� x ������Ϊ UV��y ��������Ϊ 1
        }

        // ��� sectorObj Ϊ�գ��򴴽�һ���µ� GameObject������� MeshFilter �� MeshRenderer ���
        if (sectorObj == null)
        {
            sectorObj = new GameObject("Sector");
            sectorObj.transform.SetParent(transform, false);
            m_MeshFilter = sectorObj.AddComponent<MeshFilter>();
            MeshRenderer = sectorObj.AddComponent<MeshRenderer>();
        }

        // �����������ö��㡢������������ UV
        Mesh mesh = new Mesh();
        mesh.vertices = vector3s.ToArray();
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // �����񸳸� MeshFilter�����ò��ʸ� MeshRenderer
        m_MeshFilter.mesh = mesh;
        MeshRenderer.material = material;

        return sectorObj; // �������ε� GameObject
    }

}
