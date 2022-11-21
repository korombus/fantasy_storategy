﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private bool dash = false;
        private Vector2 beforeMoveVec = Vector2.zero;

        private Animator animator;

        private bool isNormalEvent = false;
        private GameObject eventTarget = null;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // 一時停止になってない場合は移動許可
            if(!MapWindow.PAUSE){
                Vector2 dir = Vector2.zero;

                if(isNormalEvent){
                    if(Input.GetKeyDown(KeyCode.JoystickButton0)){
                        // イベント管理
                        this.GetComponent<CharactorEventController>().CharactorEvent(eventTarget);
                    }
                }

                // ダッシュ操作
                dash = false;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton1)){
                    dash = true;
                }

                // 移動
                dir.x = Input.GetAxis("Horizontal");
                dir.y = Input.GetAxis("Vertical");
                
                // 左向き
                if (Input.GetAxis("Horizontal") < 0)
                {
                    animator.SetInteger("Direction", 3);
                }
                // 右向き
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    animator.SetInteger("Direction", 2);
                }

                // 上向き
                if (Input.GetAxis("Vertical") > 0)
                {
                    animator.SetInteger("Direction", 1);
                }
                // 下向き
                else if (Input.GetAxis("Vertical") < 0)
                {
                    animator.SetInteger("Direction", 0);
                }

                dir.Normalize();
                animator.SetBool("IsMoving", dir.magnitude > 0);

                GetComponent<Rigidbody2D>().velocity = speed * dir * (dash ? 1.5f : 1);
            } else {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // ゴールに到達したら帰還
            if(other.gameObject.tag == "Goal"){
                GetComponent<GaugeStatus>().SetData(CommonSys.GetSystem<MapWindow>().MapClear, "帰還中");
            }

            // 通常イベントを発生させる
            if(EventTags.isNormalEventTag(other.gameObject.tag)){
                isNormalEvent = true;
                eventTarget = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // ゴールから出たらゲージ初期化
            if(other.gameObject.tag == "Goal"){
                GetComponent<GaugeStatus>().StopGauge();
            }

            // 通常イベントの発生を抑制
            if(EventTags.isNormalEventTag(other.gameObject.tag)){
                isNormalEvent = false;
                eventTarget = null;
            }
        }
    }
}
