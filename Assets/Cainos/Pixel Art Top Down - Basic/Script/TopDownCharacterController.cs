using System.Collections;
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
                        switch(eventTarget.tag){
                            case "Chest":
                                int itemId = eventTarget.GetComponent<OpenChestEvent>().OpenChest();
                                if(itemId > -1){
                                    CommonSys.GetSystem<MapWindow>().DispEventMessage("何かを手に入れた。");
                                } else {
                                    CommonSys.GetSystem<MapWindow>().DispEventMessage("宝箱はからっぽだった。");
                                }
                            break;
                            default:
                                isNormalEvent = false;
                            break;
                        }
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
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // ゴールに到達したら帰還
            if(other.gameObject.tag == "Goal"){
                GetComponent<GaugeStatus>().SetData(CommonSys.GetSystem<MapWindow>().MapClear, "帰還中");
            }

            // 宝箱の開けられる領域に入ったら開けるイベントを発生できるようにする
            if(other.gameObject.tag == "Chest"){
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

            // 宝箱イベントの発生を抑制
            if(other.gameObject.tag == "Chest"){
                isNormalEvent = false;
                eventTarget = null;
            }
        }
    }
}
