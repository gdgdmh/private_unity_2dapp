using UnityEngine;
using System.Collections;

public class PauseObject : MonoBehaviour {


    // 初期化
	void Start () {
	    // ポーズ対象に追加する
        pause_lists_.Add(this);
	}

    void OnDestroy() {
        // 破棄されるときはポーズ対象から除外
        pause_lists_.Remove(this);
    }

    // ポーズ
    void OnPause() {
        if (pause_behaviours_ != null) {
            // まだ生成されてない
            return;
        }

        // 有効なbehaviourを取得
        pause_behaviours_ = System.Array.FindAll(GetComponentsInChildren<Behaviour>(),
            (obj) => { return obj.enabled; });
        // そのオブジェクトを無効にする
        foreach (var component in pause_behaviours_) {
            component.enabled = false;
        }

        // rigidbodyはenabled=falseでは止まらない
        // スリープ状態にすることで停止するけど、velocityやangularVelocityの情報が失われるので保存する
        rigidbodies_ = System.Array.FindAll(GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
        rigidbody_velocities_ = new Vector3[rigidbodies_.Length];
        rigidbody_angular_velocities_ = new Vector3[rigidbodies_.Length];
        for (var i = 0; i < rigidbodies_.Length; ++i) {
            rigidbody_velocities_[i] = rigidbodies_[i].velocity;
            rigidbody_angular_velocities_[i] = rigidbodies_[i].angularVelocity;
            rigidbodies_[i].Sleep();
        }
        // 2D
        rigidbody2ds_ = System.Array.FindAll(GetComponentsInChildren<Rigidbody2D>(), (obj) => { return !obj.IsSleeping(); });
        rigidbody2d_velocities_ = new Vector2[rigidbody2ds_.Length];
        rigidbody2d_angular_velocities_ = new float[rigidbody2ds_.Length];
        for (var i = 0; i < rigidbody2ds_.Length; ++i) {
            rigidbody2d_velocities_[i] = rigidbody2ds_[i].velocity;
            rigidbody2d_angular_velocities_[i] = rigidbody2ds_[i].angularVelocity;
            rigidbody2ds_[i].Sleep();
        }
    }

    // ポーズ解除
    void OnResume() {
        if (pause_behaviours_ == null) {
            return;
        }
        // ポーズ前の状態に復元
        foreach (var component in pause_behaviours_) {
            component.enabled = true;
        }
        // rigidbodyを起動
        // velocityやangularVelocityの情報が失われているので復元する
        for (var i = 0; i < rigidbodies_.Length; ++i) {
            rigidbodies_[i].WakeUp();
            rigidbodies_[i].velocity = rigidbody_velocities_[i];
            rigidbodies_[i].angularVelocity = rigidbody_angular_velocities_[i];
        }
        for (var i = 0; i < rigidbody2ds_.Length; ++i) {
            rigidbody2ds_[i].WakeUp();
            rigidbody2ds_[i].velocity = rigidbody2d_velocities_[i];
            rigidbody2ds_[i].angularVelocity = rigidbody2d_angular_velocities_[i];
        }

        pause_behaviours_ = null;
        rigidbodies_ = null;
        rigidbody_velocities_ = null;
        rigidbody2ds_ = null;
        rigidbody2d_velocities_ = null;
        rigidbody2d_angular_velocities_ = null;
    }

    // ポーズ処理
    public static void ProcessPause() {
        foreach (var obj in pause_lists_) {
            obj.OnPause();
        }
    }

    // ポーズ解除処理
    public static void ProcessResume() {
        foreach (var obj in pause_lists_) {
            obj.OnResume();
        }
    }


    // Update is called once per frame
    void Update () {
	
	}

    // ポーズ対象スクリプト
    static System.Collections.Generic.List<PauseObject> pause_lists_ = new System.Collections.Generic.List<PauseObject>();
    // ポーズ対象コンポーネント
    Behaviour[] pause_behaviours_ = null;

    // Rigidbodyは中断再開時に速度と角速度が保存されないので手動で保存する必要がある
    Rigidbody[] rigidbodies_ = null;
    Vector3[] rigidbody_velocities_ = null;
    Vector3[] rigidbody_angular_velocities_ = null;
    Rigidbody2D[] rigidbody2ds_ = null;
    Vector2[] rigidbody2d_velocities_ = null;
    float[] rigidbody2d_angular_velocities_ = null;

}
