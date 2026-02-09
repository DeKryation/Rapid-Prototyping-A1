using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {

	public float rotateSpeed = 500f;

	void Update () {
		transform.Rotate (0f, rotateSpeed * Time.deltaTime, 0f);	
	}
}
