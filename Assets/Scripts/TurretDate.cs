using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurretDate
{
    //砲塔に関するパラメータ。
    public GameObject TurretPrefab;
    public int costDefault;
    public GameObject turretUpgradedPrefab;
    public int costUpgraded;
    public TurretType type; 
}
public enum TurretType
{
    DefaultTurret,
    FireTurret,
    MagicTurret
}
