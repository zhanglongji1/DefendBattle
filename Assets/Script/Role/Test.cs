using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public Text text;

    public void ChanceUp(float atk)
    {
        text.text = atk.ToString();
    }
    private void Update()
    {
        Invoke("Die", 0.3f);
    }
    void Die()
    {

        Destroy(this.gameObject);

    }

}
