using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //プレーヤーとカメラの位置を取得し、間のオフセットを取得する。
    public Transform playerTransform;
    private Vector3 cameraRotation;
    private Vector3 offset;

    //カメラの回転速度、垂直方向の角度制限。
    private float rotateSpeed = 3;
    private float minCameraAngel = -75;
    private float maxCameraAngel = 40;
    
    public PlayerController playerController;

    void Start()
    {
        offset = playerTransform.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position - offset;
    }

    void Update()
    {
        //一部UIが表示されていない場合、マウスはレンズの回転を制御する。
        //UIがトリガされると、レンズは制御されない。
        if (GameManager.instance.turretSwitchUI.activeSelf == false&&
            GameManager.instance.upgradeRemoveUI.activeSelf==false&&
            GameManager.instance.removeUI.activeSelf==false)
        {
            float _MouseY = Input.GetAxis("Mouse Y");
            cameraRotation.x -= _MouseY * rotateSpeed;
            //キャラクターが敵に撃退される段階でなければ、レンズを平行移動させることがでる。
            if (playerController.repelled == false)
            {
                float _MouseX = Input.GetAxis("Mouse X");
                cameraRotation.y += _MouseX * rotateSpeed;
            }

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, minCameraAngel, maxCameraAngel);
            transform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
            //キャラクターはレンズに従って回転する。
            playerTransform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        }
    }
}
