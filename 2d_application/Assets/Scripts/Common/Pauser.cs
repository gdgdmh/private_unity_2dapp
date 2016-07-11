using UnityEngine;
using System.Collections;

/**
 * ポーズ(一時停止)を行うスクリプト
 * このスクリプト以下にあるchildが影響を受ける
 *
 */
public class Pauser : MonoBehaviour {

    // ポーズ状態か
    public bool is_pause_ = false;

    // 無視するGameObject
    public GameObject[] igunore_objects_ = null;
    // 前回のポーズ状態
    bool past_is_pause_ = false;

    // pauseの際に保持するデータ
    RigidbodyVelocityData[] rigidbody_velocity_datas_;
    // pauseの際に影響を受けるrigidbody
    Rigidbody[] pause_rigidbodies_;
    Rigidbody2dVelocityData[] rigidbody2d_velocity_datas_;
    Rigidbody2D[] pause_rigidbody2ds_;

    MonoBehaviour[] pause_monobehaviours_;

    void Update() {
        // ポーズ状態の変更
        if (past_is_pause_ != is_pause_) {
            if (is_pause_ == true) {
                ProcessSuspend();
            } else {
                ProcessResume();
            }
            past_is_pause_ = is_pause_;
        }

    }

    void ProcessSuspend() {
        // Sleep中ではなく、無視オブジェクトに含まれていないRigidbodyを取得
        {
            System.Predicate<Rigidbody> rigidbody_predicate =
                (obj) => !obj.IsSleeping()
                && System.Array.FindIndex<GameObject>(igunore_objects_, game_object => game_object != obj.gameObject) < 0;
            pause_rigidbodies_ = System.Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbody_predicate);
            rigidbody_velocity_datas_ = new RigidbodyVelocityData[pause_rigidbodies_.Length];
            // Sleep()すると速度、角速度が保存されないので、保存してからSleepさせる
            for (int i = 0; i < pause_rigidbodies_.Length; ++i) {
                rigidbody_velocity_datas_[i] = new RigidbodyVelocityData(pause_rigidbodies_[i]);
                pause_rigidbodies_[i].Sleep();
            }
        }
        // 2D
        {
            System.Predicate<Rigidbody2D> rigidbody_predicate =
                (obj) => !obj.IsSleeping()
                && System.Array.FindIndex<GameObject>(igunore_objects_, game_object => game_object != obj.gameObject) < 0;
            pause_rigidbody2ds_ = System.Array.FindAll(transform.GetComponentsInChildren<Rigidbody2D>(), rigidbody_predicate);
            rigidbody2d_velocity_datas_ = new Rigidbody2dVelocityData[pause_rigidbody2ds_.Length];
            // Sleep()すると速度、角速度が保存されないので、保存してからSleepさせる
            for (int i = 0; i < pause_rigidbody2ds_.Length; ++i) {
                rigidbody2d_velocity_datas_[i] = new Rigidbody2dVelocityData(pause_rigidbody2ds_[i]);
                pause_rigidbody2ds_[i].Sleep();
            }
        }

        // MonoBehaviourの停止
        System.Predicate<MonoBehaviour> monobehaviour_predicate =
            (obj) => obj.enabled
            && obj != this
            && System.Array.FindIndex<GameObject>(igunore_objects_, game_object => game_object == obj.gameObject) < 0;
        pause_monobehaviours_ = System.Array.FindAll(transform.GetComponentsInChildren<MonoBehaviour>(), monobehaviour_predicate);
        foreach (var monobehaviour in pause_monobehaviours_) {
            monobehaviour.enabled = false;
        }
    }

    void ProcessResume() {
        // Rigidbodyの起動
        // Velocityの値は失われているので復元
        for (int i = 0; i < pause_rigidbodies_.Length; ++i) {
            pause_rigidbodies_[i].WakeUp();
            pause_rigidbodies_[i].velocity = rigidbody_velocity_datas_[i].velocity_;
            pause_rigidbodies_[i].angularVelocity = rigidbody_velocity_datas_[i].angular_velocity_;
        }
        for (int i = 0; i < pause_rigidbody2ds_.Length; ++i) {
            pause_rigidbody2ds_[i].WakeUp();
            pause_rigidbody2ds_[i].velocity = rigidbody2d_velocity_datas_[i].velocity_;
            pause_rigidbody2ds_[i].angularVelocity = rigidbody2d_velocity_datas_[i].angular_velocity_;
        }
        foreach (var monobehaviour in pause_monobehaviours_) {
            monobehaviour.enabled = true;
        }
    }
}
