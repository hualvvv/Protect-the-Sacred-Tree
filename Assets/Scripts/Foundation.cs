using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundation : MonoBehaviour
{
    //現在プラットフォーム（基盤）上の砲塔。
    [HideInInspector]
    public GameObject currentTurret;
    //選択UIを取得する。
    public GameObject EOpenUI;
    public GameObject turretSwitchUI;
    public GameObject upgradeRemoveUI;
    public GameObject removeUI;
    //Buildmanagerを取得する。
    public BuildManager buildManager;
    //建造する時/Upgradeする時のエフェクト。
    public GameObject buildEffect;
    public GameObject upgradeEffect;
    //プラットフォームの位置を提示するためのBuildMark。
    public GameObject buildMark;
    //砲塔は既にUpgradeしましたか。
    private bool finishGraded=false;
    //Eで砲塔操作することができる。
    private bool canEOpenUI = true;
    //
    private bool canStartBuildAndUpgrade = false;

    private void Update()
    {
        if (canStartBuildAndUpgrade == true)
        {
            StartBuildAndUpgrade();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (canEOpenUI == true)
        {
            EOpenUI.SetActive(true);
        }

        //Eで砲塔操作するUIを開く。
        if (Input.GetKey(KeyCode.E)&&canEOpenUI==true)
        {
            canEOpenUI = false;

            //Eを押す時、建造UI/UpgradeUIがクローズされている場合は、オープンします。
            //オープンした場合は、クローズします。
            if (turretSwitchUI.activeSelf == false &&
                upgradeRemoveUI.activeSelf == false &&
                removeUI.activeSelf == false)
            {
                canStartBuildAndUpgrade=true;
                
                Invoke("OpenUIActive", 0.1f);  
            }
            else
            {
                CloseUI();

                Invoke("OpenUIActive", 0.1f);
            }
        }
    }

    /// <summary>
    /// 建设するUIとアップグレードUIする、どちを現すのかを判断。
    /// </summary>
    private void StartBuildAndUpgrade()
    {
        //プラットフォームのtiggerに衝突するとマウスが表示されます。
        Cursor.visible = true;

        //プラットフォーム上は建造されてない時。
        if (currentTurret == null)
        {

            //プラットフォーム上は建造されてないと、建造UIを現す。
            turretSwitchUI.SetActive(true);

            //建造出来るかとかを引き続き判断。
            BuildTurret();
        }
        else
        {
            if (finishGraded == false)
            {
                //プラットフォーム上は建造されましたら、UpgradeUIを現す。
                upgradeRemoveUI.SetActive(true);
                //Upgrade出来るかとかを引き続き判断。
                TurretUpgrade();
            }
            else
            {
                //RemoveUIを現す。
                removeUI.SetActive(true);
                //Remove出来るかとかを引き続き判断。
                TurretRemove();
            }
        }
    }

    /// <summary>
    /// 建造出来るかとかを判断。
    /// </summary>
    public void BuildTurret()
    {
        if (buildManager.canBuild == true)
        {
            //お金が足りているかどうかを判断して、相当なお金を差し引いて砲塔を建設する。
            if (buildManager.money >= buildManager.selectTurretDate.costDefault)
            {
                buildManager.changMoney(-buildManager.selectTurretDate.costDefault);
                //選択された砲塔を建造する。
                Build(buildManager.selectTurretDate.TurretPrefab);
                buildManager.canBuild = false;
                //UIを閉じる。
                CloseUI();
                //位置を提示するためのBuildMarkを廃棄する。
                Destroy(buildMark);
            }
            else
            {
                //お金不足を提示する。
                buildManager.animator.SetTrigger("Flicker");
            }
        }
    }


    /// <summary>
    /// 砲塔はUpgrade出来るかとかを判断。
    /// </summary>
    public void TurretUpgrade()
    {
        if (buildManager.canUpgrade == true)
        {
            if (buildManager.money >= buildManager.selectTurretDate.costUpgraded)
            {
                buildManager.changMoney(-buildManager.selectTurretDate.costUpgraded);
                //砲塔をUpgrade。
                Upgrade(buildManager.selectTurretDate.turretUpgradedPrefab);
                buildManager.canUpgrade = false;
                //UIを閉じる。
                CloseUI();
                //既にUpgrade完了。
                finishGraded = true;
            }
            else
            {
                //お金不足を提示する。
                buildManager.animator.SetTrigger("Flicker");
            }
        }
        //同時に、Removeバタンも利用できる。
        TurretRemove();
    }

    /// <summary>
    /// 砲塔をRemove出来るかとかを判断。
    /// </summary>
    void TurretRemove()
    {
        if (buildManager.canRemove == true)
        {
            Remove();
            buildManager.canRemove = false;
            //UIを閉じる。
            CloseUI();
            //砲塔をRemove完了すれば、finishGradedはFalseになる。
            finishGraded = false;
        }
    }

    //プラットフォームのTiggerから離れると、UIを閉じる。
    private void OnTriggerExit(Collider other)
    {
        CloseUI();
        EOpenUI.SetActive(false);
    }

    void OpenUIActive()
    {
        canEOpenUI = true;
    }

    void CloseUI()
    {
        //Tiggerから離れると、UIを閉じて、マウスを非表示にする。
        turretSwitchUI.SetActive(false);
        upgradeRemoveUI.SetActive(false);
        removeUI.SetActive(false);
        canStartBuildAndUpgrade = false;
        //マウスを閉じるのが少し遅くなり、左右マウスボタンの攻撃触発しないようにする。
        Invoke("CloseCursor", Time.deltaTime);
    }

    void CloseCursor()
    {
        Cursor.visible = false;
    }

    //砲塔建造を実施する。
    void Build(GameObject turretPrefab)
    {
        //砲塔を建造し、エフェクトを生成する。
        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        GameObject buildEff = Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(buildEff, 1);

        GetComponent<AudioSource>().Play();
    }

    //砲塔Upgradeを実施する。
    void Upgrade(GameObject turretUpgradedPrefab)
    {
        //元々の砲塔を廃棄し、新たなUpgrade砲塔をつくります。
        Destroy(currentTurret);
        GameObject buildEff = Instantiate(upgradeEffect, transform.position, Quaternion.identity);
        currentTurret = Instantiate(turretUpgradedPrefab, transform.position, Quaternion.identity);
        Destroy(buildEff,2f);

        GetComponent<AudioSource>().Play();
    }

    //砲塔を廃棄する。
    void Remove()
    {
        Destroy(currentTurret);
        GameObject buildEff = Instantiate(upgradeEffect, transform.position, Quaternion.identity);
        Destroy(buildEff, 2f);

        GetComponent<AudioSource>().Play();
    }
}
