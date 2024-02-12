using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDisplay : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float angle = 360;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int quality = 12;
  
    public MeshFilter m_MeshFilter; // 材质
    private MeshRenderer MeshRenderer;
    public GameObject sectorObj;
    public static AttackRangeDisplay a_Instant;
    void Awake()
    {
        a_Instant = this;
       // attackRange = Turrets.Instant.checkeDistance/Turrets.Instant.transform.localScale.x;
        
        // 在 Start 方法中创建一个球体的 Mesh
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
        isSectorObjActive = true;  // 更新状态为激活
        }
    }
    public void Off()
    {
        // 如果对象当前已被激活（即显示状态），则隐藏它
        if (sectorObj != null)
        {
            sectorObj.SetActive(false);
            isSectorObjActive = false;  // 更新状态为隐藏
        }
    }
    private GameObject GetSector(Vector3 center,float angle,float radius,int triangleCount)
    {
        float eachAngle = angle / triangleCount;
        List<Vector3> vector3s = new List<Vector3>();
        vector3s.Add(center);

        for (int i = 0; i <= triangleCount; i++) // 注意这里的条件是 <=
        {
            float currentAngle = angle / 2 - eachAngle * i;
            Vector3 vertex = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0) * radius + center;
            vector3s.Add(vertex);
        }
        return CreateSectorMesh(vector3s);
    }

    private GameObject CreateSectorMesh(List<Vector3> vector3s)
    {
        // 初始化三角形数组
        int[] triangles;
        int triangleAmount = vector3s.Count - 2; // 每个三角形3个顶点，忽略前两个顶点（中心点和第一个扇形顶点）
        triangles = new int[3 * triangleAmount]; // 总共需要的索引数量

        // 填充三角形数组
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;              // 中心点
            triangles[3 * i + 1] = i + 1;      // 当前扇形顶点
            triangles[3 * i + 2] = i + 2;      // 下一个扇形顶点
        }

        // 创建 UV 坐标
        Vector2[] uvs = new Vector2[vector3s.Count];
        uvs[0] = new Vector2(vector3s[0].x, vector3s[0].y); // 设置中心点的 UV 坐标
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vector3s[i].x, 1); // 将所有顶点的 x 坐标作为 UV，y 坐标设置为 1
        }

        // 如果 sectorObj 为空，则创建一个新的 GameObject，并添加 MeshFilter 和 MeshRenderer 组件
        if (sectorObj == null)
        {
            sectorObj = new GameObject("Sector");
            sectorObj.transform.SetParent(transform, false);
            m_MeshFilter = sectorObj.AddComponent<MeshFilter>();
            MeshRenderer = sectorObj.AddComponent<MeshRenderer>();
        }

        // 创建网格并设置顶点、三角形索引和 UV
        Mesh mesh = new Mesh();
        mesh.vertices = vector3s.ToArray();
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // 将网格赋给 MeshFilter，设置材质给 MeshRenderer
        m_MeshFilter.mesh = mesh;
        MeshRenderer.material = material;

        return sectorObj; // 返回扇形的 GameObject
    }

}
