using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {
	public Text text;
	public float updateRate = 1f;

	private float lastTime;

	void Start() {
		lastTime = 0;
		UpdateFPS ();
	}

	private void UpdateFPS() {
		float fps = 1 / Time.deltaTime;
		text.text = fps.ToString ("F1") + " FPS";
	}

	// Update is called once per frame
	void LateUpdate () {
		if (lastTime < Time.time - updateRate) {
			UpdateFPS ();
			lastTime = Time.time;
		}
	}
}
