using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class BuildMenu : MonoBehaviour
{
    public Scrollbar buildMenu;

    public Text[] gold;

    private void Update()
    {
        foreach (var item in gold)
        { 
            if (TurretManager.instance.money > int.Parse(Regex.Match(item.text, @"\d+").Value))
            {
                item.color = Color.green;
            }
            else { item.color = Color.red; }
        }
  
    }
  
    public void Shift(int direction)
    {
        if (direction==1)
        {
            buildMenu.value=buildMenu.value-0.5f;
        }
        else
        {
            buildMenu.value = buildMenu.value + 0.5f;
        }
    }
    public float timerDuration = 5f;


    public void Off()
    {
        TurretManager.Instance.selectedTurretData = null;
        this.gameObject.SetActive(false);
    }



}
