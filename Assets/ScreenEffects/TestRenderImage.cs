﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour {
    #region Variables
    public Shader curShader;
    public float grayScaleAmount = 1.0f;

    private Material curMaterial;
    #endregion

    #region Properties
    Material material {
        get {
            if (curMaterial == null) {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
		if (!SystemInfo.supportsImageEffects) {
            enabled = false;
            return;
        }
        if (!curShader && !curShader.isSupported) {
            enabled = false;
        }
	}

    private void Update() {
        grayScaleAmount = Mathf.Clamp(grayScaleAmount, 0.0f, 1.0f);
    }

    private void OnDisable() {
        if (curMaterial) {
            DestroyImmediate(curMaterial);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (curShader != null) {
            material.SetFloat("_LuminosityAmount", grayScaleAmount);
            Graphics.Blit(source, destination, material);
        } else {
            Graphics.Blit(source, destination);
        }
    }
}
