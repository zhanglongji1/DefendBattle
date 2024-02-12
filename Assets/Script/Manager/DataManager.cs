using Assets.Script.Data;
using Assets.Script.Utilts;
using Newtonsoft.Json;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using static Turret;

namespace Assets.Script.Manager
{
    class DataManager : MonoSingleton<DataManager> 
    { 
        public string DataPath;
        public Dictionary<int,  GenerateTimeData> GenerateTimeData = null;
        public Dictionary<int, TurretData> Turrets = null;
        public Dictionary<int, EnemyData> Enemys = null;
        string json;


        public void Load()
        {
            StartCoroutine(LoadWeaponProperty());
            StopCoroutine(LoadWeaponProperty());
        }
        //private void LoadWin()
        //{
        //    json = File.ReadAllText(Application.streamingAssetsPath + "/GenerateData.txt");
        //    this.GenerateTimeData = JsonConvert.DeserializeObject<Dictionary<int, GenerateTimeData>>(json);

        //    json = File.ReadAllText(Application.streamingAssetsPath + "/NpcDefine1.txt");
        //    this.Turrets = JsonConvert.DeserializeObject<Dictionary<int, TurretData>>(json);
        //    json = File.ReadAllText(Application.streamingAssetsPath + "/MonsterDefine.txt");
        //    this.Enemys = JsonConvert.DeserializeObject<Dictionary<int, EnemyData>>(json);
        //}
        private System.Collections.IEnumerator LoadWeaponProperty()
        { 

            string filePath = Application.streamingAssetsPath + "/GenerateData.txt";
            
            //UnityWebRequest
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(filePath);
            yield return unityWebRequest.SendWebRequest();
             json = unityWebRequest.downloadHandler.text;
            this.GenerateTimeData = JsonConvert.DeserializeObject<Dictionary<int,GenerateTimeData >>(json);

            filePath = Application.streamingAssetsPath + "/NpcDefine1.txt";
            //UnityWebRequest
            unityWebRequest = UnityWebRequest.Get(filePath);
            yield return unityWebRequest.SendWebRequest();
            json = unityWebRequest.downloadHandler.text;
            this.Turrets = JsonConvert.DeserializeObject<Dictionary<int, TurretData>>(json);
            filePath = Application.streamingAssetsPath + "/MonsterDefine.txt";
            //UnityWebRequest
             unityWebRequest = UnityWebRequest.Get(filePath);
            yield return unityWebRequest.SendWebRequest();
            json = unityWebRequest.downloadHandler.text;
            this.Enemys = JsonConvert.DeserializeObject<Dictionary<int, EnemyData>>(json);
        }

    }
}
