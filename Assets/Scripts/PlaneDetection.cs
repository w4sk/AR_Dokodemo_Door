//平面を検出してその上に何かしらのオブジェクトを置くプログラム
#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(InputSystem))]


public class PlaneDetection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] GameObject placementPrefab;
    ARPlaneManager planeManager;
    ARRaycastManager raycastManager;
    PlayerInput playerInput;

    bool isReady;

    void ShowMessage(string text)
    {
        message.text = $"{text}\n";
    }

    void AddMessage(string text)
    {
        message.text += $"{text}\n ";
    }

    //まずは設定が正確に行われているか否かを確認する
    //初期設定はAwake関数でやっている
    private void Awake()
    {

        if (message == null)
        {
            Application.Quit();
        }

        //それぞれの変数の紐付けを行う
        planeManager = GetComponent<ARPlaneManager>();
        playerInput = GetComponent<PlayerInput>();
        raycastManager = GetComponent<ARRaycastManager>();

        if (placementPrefab == null || planeManager == null || planeManager.planePrefab == null || raycastManager == null || playerInput == null || playerInput.actions == null)
        {
            isReady = false;
            ShowMessage("ERORR: SerialiseField Insufficient Settings");
        }
        else
        {
            isReady = true;
            ShowMessage("Plane Detection");
            AddMessage("Please Displat Plane. After a while, Plane will be detected. If you tap the Plane,then Treasure Box appear.");
        }
    }

    GameObject instantiatedObject = null;

    //アクション時のイベントハンドラー
    void OnTouch(InputValue touchInfo)
    {
        if (!isReady) { return; }

        var touchPosition = touchInfo.Get<Vector2>();
        var hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            if (instantiatedObject == null)
            {
                instantiatedObject = Instantiate(placementPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                instantiatedObject.transform.position = hitPose.position;
            }
        }
    }

    //平面検出時に平面のオブジェクトを非表示にする
    private void Update()
    {
        foreach(ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

}

