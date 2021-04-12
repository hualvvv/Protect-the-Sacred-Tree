using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    //三種類の砲塔に関するパラメータ。
    public TurretDate defaultTurretDate;
    public TurretDate fireTurretDate;
    public TurretDate magicTurretDate;
    //現在選択されている砲塔。（建造する予定の砲塔）
    public TurretDate selectTurretDate;
    //資金テキストを取得する。
    public Text moneyText;
    public Text moneyText_2;
    //現在持っている資金を表す。
    public float money = 10000;
    //建造ボタン/Upgradeボタン/Removeボタンを押したかどうか。
    [HideInInspector]
    public bool canBuild = false;
    [HideInInspector]
    public bool canUpgrade = false;
    [HideInInspector]
    public bool canRemove = false;

    //moneyclick動画を取得。
    public Animator animator;
    //自身のInstanceを作る。
    public static BuildManager instance;
    //ButtonAudio。
    public AudioSource buttonAudio;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        moneyText_2.text = moneyText.text;
    }
    //資金を変更する関数。
    public void changMoney(float chang = 0)
    {
        money += chang;
        moneyText.text = "￥" + money;
    }

    //いま選択しているの砲塔はどちなのかを判断する。
    public void OnDefaultTurretSelected(bool isOn)
    {
        selectTurretDate = defaultTurretDate;
        buttonAudio.Play();
    }

    public void OnFireTurretSelected(bool isON)
    {
        selectTurretDate = fireTurretDate;
        buttonAudio.Play();
    }
    public void OnMagicTurretSelected(bool isON)
    {
        selectTurretDate = magicTurretDate;
        buttonAudio.Play();
    }

    
    
    //建造バタンを押すと。
    public void OnBuildButtonDown()
    {
        if(selectTurretDate.TurretPrefab!= null)
        {
            canBuild = true;
        }
    }
    //建造バタンを離すと。
    public void OnBuildButtonUp()
    {
        canBuild = false;
    }

    //Upgradeバタンを押すと。
    public void OnUpgradeButtonDown()
    {
        if (selectTurretDate.turretUpgradedPrefab != null)
        {
            canUpgrade = true;
        }
    }

    //Upgradeバタンを離すと。
    public void OnUpgradeButtonUp()
    {
        canUpgrade = false;
    }


    //Removeバタンを押すと。
    public void OnRemoveButtonDown()
    {
        if (selectTurretDate.turretUpgradedPrefab != null)
        {
            canRemove = true;
        }
    }

    //Removeバタンを離すと。
    public void OnRemoveButtonUp()
    {
        canRemove = false;
    }
}
