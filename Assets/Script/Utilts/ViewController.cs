using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {
    //相机移动速度 控制相机水平偏移
	public float speed = 1;
    //相机缩放速度 控制相机高度
    public float zoomSpeed = 60;
	// Update is called once per frame
	void Update () {
        //输入键盘的wasd
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
        //输入鼠标的滚轮
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        //根据输入和速率对相机水平偏移和高度的变换
        transform.Translate(new Vector3(h*speed, -mouse*zoomSpeed, v*speed) * Time.deltaTime , Space.World);
	}

}
