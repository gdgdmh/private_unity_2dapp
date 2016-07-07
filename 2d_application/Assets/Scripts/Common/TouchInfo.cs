using UnityEngine;
using System.Collections;

// タッチ情報データ
public class TouchInfo {

    public enum TouchStatus : int {
        kNone,        // タッチ状態が何もない
        kBegan,       // 押し始め
        kMoved,       // 押し続けていて移動中
        kStationary,  // 押し続けているけど動いてない
        kEnded,       // タッチ終了
        kCanceled     // タッチ状態がキャンセルされた
    };

    // 無効なタッチId(直接値を参照するのは非推奨調べるときはIsTouchIdInvalidを使用する)
    public static int kInvalidTouchId = -1;

    // 現在のタッチID(デフォルトは-1)
    public int touch_id_ { set; get; }
    // タッチ位置
    public Vector3 position_ { set; get; }
    // タッチ状態
    public TouchStatus status_ { set; get; }

    public TouchInfo() {
        Clear();
    }

    // 初期状態にする
    public void Clear() {
        touch_id_ = kInvalidTouchId;
        position_ = Vector3.zero; // (0, 0, 0)に設定
        status_ = TouchStatus.kNone;
    }

    // 情報をコピーする
    public void Copy(TouchInfo copy_source) {
        touch_id_ = copy_source.touch_id_;
        position_ = copy_source.position_;
        status_ = copy_source.status_;
    }

    // 位置が等しいかを比較
    public bool IsPositionEquals(TouchInfo copy_source) {
        if ((position_.x == copy_source.position_.x)
            && (position_.y == copy_source.position_.y)
            && (position_.z == copy_source.position_.z)) {
            return true;
        }
        return false;
    }

    public bool Equals(TouchInfo info) {
        if (info == null) {
            return false;
        }

        if ((touch_id_ == info.touch_id_)
            && (position_.x == info.position_.x)
            && (position_.y == info.position_.y)
            && (position_.z == info.position_.z)
            && (status_ == info.status_)
            ) {
            return true;
        }
        return false;
    }

    // タッチIdが無効かどうか
    public bool IsTouchIdInvalid() {
        return IsTouchIdInvalid(touch_id_);
    }

    // タッチIdが無効かどうか
    public static bool IsTouchIdInvalid(int id) {
        if (id == kInvalidTouchId) {
            return true;
        }
        return false;
    }

    // ステータスによって文字列を返す
    public static string GetStatusString(TouchStatus touch_status) {
        switch (touch_status) {
        case TouchStatus.kNone:
            return "None";
        case TouchStatus.kBegan:
            return "Began";
        case TouchStatus.kMoved:
            return "Moved";
        case TouchStatus.kStationary:
            return "Stationary";
        case TouchStatus.kEnded:
            return "Ended";
        case TouchStatus.kCanceled:
            return "Canceled";
        default:
            return "Unknown";
        }
    }

    /**
     * 現在の状態の出力
     */
    public void Print() {
        MhCommon.Print("TouchInfo::Print touch_id_=" + touch_id_ +
            " position x=" + position_.x + " y=" + position_.y + " z=" + position_.z +
            "status=" + GetStatusString(status_));
    }
}
