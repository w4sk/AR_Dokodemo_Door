#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
using System;

//まず、ARPlaneにおくOBjectの数を５個とかにしたい
namespace AR_TreasureHunt
{

    public class ExInteraction : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI message;
        [SerializeField] ARPlaneManager planeManager;
        [SerializeField] ARPlacementInteractable placementInteractable;
        [SerializeField] ARGestureInteractor gestureInteractor;

        Animator anim;
        bool isReady;

        void ShowMessage(string text) { message.text = $"{text}\n"; }
        void AddMessage(string text) { message.text += $"{text}\n"; }

        private void Awake()
        {
            if (message == null) { Application.Quit(); }
            if (planeManager == null || planeManager.planePrefab == null || placementInteractable == null || placementInteractable.placementPrefab == null || gestureInteractor == null)
            {
                isReady = false;
                ShowMessage("エラー: SerializedFieldの設定不備");
            }
            else
            {
                isReady = true;
                ShowMessage("床を撮影してください。しばらくすると、平面が検出されます。平面をタップするとどこでもドアが表示されます。");
            }
        }

        private void OnEnable()
        {
            placementInteractable.objectPlaced.AddListener(OnObjectPlaced);
            gestureInteractor.hoverEntered.AddListener(OnHoverEntered);
            gestureInteractor.hoverExited.AddListener(OnHoverExited);
        }

        bool hasPlaced = false;
        int placednum = 0;

        // 引数に問題あり
        private void OnObjectPlaced(ARObjectPlacementEventArgs arg0)
        {
            placednum += 1;

            //ここで配置する数を決定
            if (placednum > 1)
            {
                Destroy(arg0.placementObject);
                return;
            }
            

            var selectInteractable = arg0.placementObject.GetComponent<ARSelectionInteractable>();
            
            anim = arg0.placementObject.GetComponentInChildren<Animator>();
            if (anim == null)
            {
                ShowMessage("エラー: animatorの設定不備");
            }
            

            if (selectInteractable != null)
            {
                selectInteractable.selectEntered.AddListener(OnSelectEntered);
                selectInteractable.selectExited.AddListener(OnSelectExited);
                hasPlaced = true;
            }
            else
            {
                isReady = false;
                ShowMessage("エラー: ARSelectionInteractableの設定不備");

            }
        }

        string hoverStatus = "";
        string selectStatus = "";


        void OnHoverEntered(HoverEnterEventArgs arg0)
        {
            hoverStatus = $"対話可: {arg0.interactable.name}";
        }
        //対話不可な状態がない→おかしい
        void OnHoverExited(HoverExitEventArgs arg0) { hoverStatus = "対話不可"; }

        void OnSelectEntered(SelectEnterEventArgs arg0)
        {
            selectStatus = $"選択中: {arg0.interactable.gameObject.name}";
            //Animatorを取得するのは選択したオブジェクトのAnimmatorである必要がある
            //anim = arg0.interactable.GetComponentInChildren<Animator>();
            //anim = GetComponentInChildren<Animator>();
            anim.SetBool("Rot", true);
            
        }

        void OnSelectExited(SelectExitEventArgs arg0)
        {
            selectStatus = "選択解除";
            //anim = arg0.interactable.GetComponentInChildren<Animator>();
            anim.SetBool("Rot", false);
        }

        // Update is called once per frame　これが呼ばれる前に何かしらが起こっている
        void Update()
        {
            if (!isReady || !hasPlaced) { return; }

            foreach (ARPlane plane in planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            ShowMessage(hoverStatus);
            AddMessage(selectStatus);
        }
    }
}