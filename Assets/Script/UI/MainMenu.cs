using Assets.Script.Manager;
using Assets.Script.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Loading;
    public Slider slider;
    public Text text;
 

    private void Awake()
    {
        DataManager.Instance.Load();
    }
    public void StartGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadPlay());
   
    }
    IEnumerator LoadPlay()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(1);
        for (float i = 50; i < 100;) {
            i+= Random.Range(0.1f, 1.5f);
            text.text = i.ToString()+"%";
            yield return new WaitForSeconds(3f);

        }
        yield return null;
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
