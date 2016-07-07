using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * プラットフォームのラッパーをするタッチ管理クラス(基本的にはシングルタッチ)
 * 将来的にマルチタッチ対応にするかもしれません
 */
public class TouchManager : SingletonMonoBehaviour<TouchManager>
{

    // 現在のタッチ情報
    private TouchInfo current_info_ { set; get; }

    // PCのタッチをエミュレーションするための情報
    private TouchInfo past_touch_info_ { set; get; }

    /**
     * 初期化処理
     */
    void Awake() {
        if (this != Instance) {
            // 既に存在しているなら破棄
            Destroy(this);
        } else {
            // シーン遷移破棄させない
            DontDestroyOnLoad(this.gameObject);
        }
        current_info_ = null;
        past_touch_info_ = null;
        current_info_ = new TouchInfo();
        past_touch_info_ = new TouchInfo();
    }

    void Update() {

        // タッチ状態の更新
        if (IsTouchPlatform() == true) {
            // タッチプラットフォーム
            SetTouchInfoForTouchPlatform();
        } else {
            // PC
            SetTouchInfoForPC();
        }
    }

    void OnDestroy() {
        // 念の為にnullにしておく
        current_info_ = null;
        past_touch_info_ = null;
    }

    public bool IsTouchNone() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kNone) {
            return true;
        }
        return false;
    }


    public bool IsTouchBegan() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kBegan) {
            return true;
        }
        return false;
    }

    public bool IsTouchMoved() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kMoved) {
            return true;
        }
        return false;
    }

    public bool IsTouchStationary() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kStationary) {
            return true;
        }
        return false;
    }

    public bool IsTouchEnded() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kEnded) {
            return true;
        }
        return false;
    }

    public bool IsTouchCanceled() {
        if (current_info_.status_ == TouchInfo.TouchStatus.kCanceled) {
            return true;
        }
        return false;
    }

    /**
     * タッチ位置を取得(ゲーム内解像度の値を返す)
     */
    public Vector3 GetTouchPosition() {
        return GetTouchPosition(ApplicationConst.GetWidth(), ApplicationConst.GetHeight());
    }

    /**
     * タッチ位置の純粋な値を返す(つまりスクリーン座標)
     */
    public Vector3 GetRawTouchPosition() {
        return current_info_.position_;
    }

    /**
     * 
     * width 画面解像度W
     * height 画面解像度H
     */
    public Vector3 GetTouchPosition(float width, float height) {
        Vector3 v;
        float ratio_x = ((float)width / (float)Screen.width);
        float ratio_y = ((float)height / (float)Screen.height);

        //float offset_x = width - ((float)Screen.width * ratio_x) / 2;

        v.x = current_info_.position_.x * ratio_x;
        v.y = current_info_.position_.y * ratio_y;
        v.z = current_info_.position_.z;

        /*
    int32_t offsetX = (YukiApplication::getWidth() - (width * m_adjustRatio.x)) / 2;
    int32_t offsetY = ((YukiApplication::getHeight()) - (height * m_adjustRatio.y)) / 2;

         */

        return v;
    }

    /**
     * 現在のタッチ状態の出力
     */
    public void PrintCurrentInfo() {
        // 前回とタッチ状態が異なっていたら出力
        if (current_info_.Equals(past_touch_info_) == false) {
            current_info_.Print();
        }
    }

    private bool IsTouchPlatform() {
        // タッチ系のプラットフォーム
        if ((Application.platform == RuntimePlatform.Android)
            || (Application.platform == RuntimePlatform.IPhonePlayer)) {
            // 今はAndroidとiOSのみ
            // 今後対応するタッチプラットフォームを拡大する際にはifの中に追加して下さい
            return true;
        }
        return false;
    }

    // TouchからTouchInfoへ設定
    private void SetTouchToTouchInfo(Touch touch) {
        current_info_.touch_id_ = touch.fingerId;
        current_info_.position_ = touch.position;

        switch (touch.phase) {
        case TouchPhase.Began:
            current_info_.status_ = TouchInfo.TouchStatus.kBegan;
            break;
        case TouchPhase.Moved:
            current_info_.status_ = TouchInfo.TouchStatus.kMoved;
            break;
        case TouchPhase.Stationary:
            current_info_.status_ = TouchInfo.TouchStatus.kStationary;
            break;
        case TouchPhase.Ended:
            current_info_.status_ = TouchInfo.TouchStatus.kEnded;
            break;
        case TouchPhase.Canceled:
            current_info_.status_ = TouchInfo.TouchStatus.kCanceled;
            break;
        default:
            break;
        }
    }

    // タッチプラットフォーム系にTouchInfoを設定
    private void SetTouchInfoForTouchPlatform()
    {
        // 前の状態を保存
        past_touch_info_.Copy(current_info_);

        //MhCommon.Print("TouchManager::SetTouchInfoForTouchPlatform");
        int id = current_info_.touch_id_;
        // 情報をリセットしておく
        current_info_.Clear();

        // タッチ情報無し
        if (Input.touchCount <= 0) {
            //MhCommon.Print("touch none");
            return;
        }

        // タッチIdが有効なので前回と同じタッチを探す
        if (TouchInfo.IsTouchIdInvalid(id) == false) {
            //MhCommon.Print("not invalid");
            for (int i = 0; i < Input.touchCount; ++i) {
                //MhCommon.Print("finding id=" + id + " fid=" + Input.GetTouch(i).fingerId);
                if (id == Input.GetTouch(i).fingerId) {
                    // 情報をセット
                    SetTouchToTouchInfo(Input.GetTouch(i));
                    return;
                }
            }
        } else {
            // 既存のタッチがないので新規タッチがあったら情報をセット
            if (Input.GetTouch(0).phase == TouchPhase.Began) {
                SetTouchToTouchInfo(Input.GetTouch(0));
            }
        }
    }

    // PC系のプラットフォームにTouchInfoを設定
    private void SetTouchInfoForPC() {

        // 前の状態を保存
        past_touch_info_.Copy(current_info_);

        TouchInfo.TouchStatus status = current_info_.status_;
        switch (status) {
        case TouchInfo.TouchStatus.kNone:
            // 押したりしていない状態で押されたらBeganへ移行
            if (Input.GetMouseButtonDown(0) == true) {
                // タッチidは0固定
                current_info_.touch_id_ = 0;
                // 位置
                current_info_.position_ = Input.mousePosition;
                current_info_.status_ = TouchInfo.TouchStatus.kBegan;
            } else {
                // デフォルトの値
                current_info_.Clear();
            }
                break;
        case TouchInfo.TouchStatus.kBegan:
            // 位置を設定
            current_info_.position_ = Input.mousePosition;
            if (Input.GetMouseButton(0) == true) {
                // 押しっぱなしなのでMovedかStationaryへ移行
                if (current_info_.IsPositionEquals(past_touch_info_) == true) {
                    current_info_.status_ = TouchInfo.TouchStatus.kStationary;
                } else {
                    current_info_.status_ = TouchInfo.TouchStatus.kMoved;
                }
            } else {
                // 持ち上げられたのでkEndedへ
                current_info_.status_ = TouchInfo.TouchStatus.kEnded;
            }
            break;
        case TouchInfo.TouchStatus.kMoved:
            current_info_.position_ = Input.mousePosition;
            if (Input.GetMouseButton(0) == false) {
                // 持ち上げられたのでkEndedへ
                current_info_.status_ = TouchInfo.TouchStatus.kEnded;
            } else {
                // MovedかStationaryへ移行
                if (current_info_.IsPositionEquals(past_touch_info_) == true) {
                    current_info_.status_ = TouchInfo.TouchStatus.kStationary;
                } else {
                    current_info_.status_ = TouchInfo.TouchStatus.kMoved;
                }
            }
            break;
        case TouchInfo.TouchStatus.kStationary:

            current_info_.position_ = Input.mousePosition;
            MhCommon.Print("x " + current_info_.position_.x + " y " + current_info_.position_.y + " z " + current_info_.position_.z);

            if (Input.GetMouseButton(0) == false) {
                // 持ち上げられたのでkEndedへ
                current_info_.status_ = TouchInfo.TouchStatus.kEnded;
            } else {
                // MovedかStationaryへ移行
                if (current_info_.IsPositionEquals(past_touch_info_) == true) {
                    current_info_.status_ = TouchInfo.TouchStatus.kStationary;
                } else {
                    current_info_.status_ = TouchInfo.TouchStatus.kMoved;
                }
            }
            break;
        case TouchInfo.TouchStatus.kEnded:
        case TouchInfo.TouchStatus.kCanceled:
            // kEndedになった瞬間に押されたらkBeganへ移行
            if (Input.GetMouseButton(0) == true) {
                // タッチidは0固定
                current_info_.touch_id_ = 0;
                // 位置
                current_info_.position_ = Input.mousePosition;
                current_info_.status_ = TouchInfo.TouchStatus.kBegan;
            } else {
                // デフォルト状態に戻す
                current_info_.Clear();
            }
            break;
        default:
            break;
        }
    }
}
