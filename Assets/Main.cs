using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static readonly int Points = Shader.PropertyToID("_Points");
    private static readonly int PointCount = Shader.PropertyToID("_PointCount");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    [SerializeField] private Material drawLineByFloatArrayMaterial;
    [SerializeField] private Material drawLineByTexMaterial;

    readonly Vector2[] p = new Vector2[]
    {
        new Vector2(0.00f, 0.00f),
        new Vector2(0.05f, 0.65f),
        new Vector2(0.10f, 0.79f),
        new Vector2(0.15f, 0.90f),
        new Vector2(0.20f, 0.98f),
        new Vector2(0.30f, 0.98f),
        new Vector2(0.35f, 0.90f),
        new Vector2(0.40f, 0.79f),
        new Vector2(0.45f, 0.65f),
        new Vector2(0.55f, 0.35f),
        new Vector2(0.60f, 0.21f),
        new Vector2(0.65f, 0.10f),
        new Vector2(0.70f, 0.02f),
        new Vector2(0.75f, 0.00f),
        new Vector2(0.80f, 0.02f),
        new Vector2(0.85f, 0.10f),
        new Vector2(0.90f, 0.21f),
        new Vector2(0.95f, 0.35f),
        new Vector2(1f, 1f),
    };

    private void Start()
    {
        InitDrawLineByTexMaterial();
        InitDrawLineByFloatArrayMaterial();
    }

    private void InitDrawLineByFloatArrayMaterial()
    {
        //目前shader中限制最大数量点为200个    可通过将 float _Points[400]; 更改最大值  扩大数组数量就可以 目标数量x2
        float[] pos = new float[p.Length * 2];
        for (int i = 0; i < p.Length; i++)
        {
            pos[i * 2] = p[i].x;
            pos[i * 2 + 1] = p[i].y;
        }

        drawLineByFloatArrayMaterial.SetFloatArray(Points, pos);
        drawLineByFloatArrayMaterial.SetInt(PointCount, p.Length);
    }

    private void InitDrawLineByTexMaterial()
    {
        var p = new Vector2[]
        {
            new Vector2(0.00f, 0.00f),
            new Vector2(0.05f, 0.65f),
            new Vector2(0.10f, 0.79f),
            new Vector2(0.15f, 0.90f),
            new Vector2(0.20f, 0.98f),
            new Vector2(0.30f, 0.98f),
            new Vector2(0.35f, 0.90f),
            new Vector2(0.40f, 0.79f),
            new Vector2(0.45f, 0.65f),
            new Vector2(0.55f, 0.35f),
            new Vector2(0.60f, 0.21f),
            new Vector2(0.65f, 0.10f),
            new Vector2(0.70f, 0.02f),
            new Vector2(0.75f, 0.00f),
            new Vector2(0.80f, 0.02f),
            new Vector2(0.85f, 0.10f),
            new Vector2(0.90f, 0.21f),
            new Vector2(0.95f, 0.35f),
            new Vector2(1f, 1f),
        };

        var tex = new Texture2D(p.Length, 1, TextureFormat.RGBAFloat, false);
        tex.filterMode = FilterMode.Point;
        for (int i = 0; i < p.Length; i++)
        {
            tex.SetPixel(i, 0, new Color(p[i].x, p[i].y, 1, 1));
        }

        tex.Apply();
        drawLineByTexMaterial.SetInt(PointCount, p.Length);
        drawLineByTexMaterial.SetTexture(MainTex, tex);
    }
}