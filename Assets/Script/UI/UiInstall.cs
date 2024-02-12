using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    class UiInstall : MonoBehaviour
    {
        public GameObject Pause;
        public GameObject End;
        public GameObject button;
        public static UiInstall instance;
        //点击设置

        //fps
        public Text fpsText;

        private float deltaTime = 0f;
        private float count;
        private float _avaerageFramerate;
        private void Awake()
        {
            instance = this;
        }
        public void Install()
        {
           button.SetActive(true);
            Pause.SetActive(true);

            Time.timeScale = 0;
        }
        //关闭
        public void CloseSettings()
        {
            // 关闭设置界面，并恢复游戏
            button.SetActive(false);
            Pause.SetActive(false);
            Time.timeScale = 1;
        }
        void Update()
        {
            count++;
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            if (count % 60 == 0)
            {
                fpsText.text = $"FPS: {Mathf.RoundToInt(1.0f / deltaTime)}";
                count = 0;
            }
        }

        public void ReturnMain()
        {
            button.SetActive(false);
            Pause.SetActive(false);
             SceneManager.LoadSceneAsync(0);
            DestroyTag();
        }
        void DestroyTag()
        {
            // 找到场景中所有带有指定标签的游戏对象
            GameObject[] objectsWithTargetTag = GameObject.FindGameObjectsWithTag("Installs");

            // 遍历所有找到的游戏对象
            foreach (GameObject obj in objectsWithTargetTag)
            {

                Destroy(obj);
            }
            Destroy(GameObject.FindGameObjectsWithTag("MainCamera")[0]);
        }
        public void OnExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
   