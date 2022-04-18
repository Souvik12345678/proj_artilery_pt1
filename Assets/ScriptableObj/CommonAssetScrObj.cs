﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CommonAsset", order = 1)]
public class CommonAssetScrObj : ScriptableObject
{
    public GameObject SmokePrefab;
    public GameObject WhiteFlagPrefab;
    public GameObject MuzzleFlashPrefab;
    public GameObject ProjectilePrefab;

    //Tank prefabs
    public GameObject TANK_1;
    public GameObject TANK_2;
    public GameObject TANK_3;
}
