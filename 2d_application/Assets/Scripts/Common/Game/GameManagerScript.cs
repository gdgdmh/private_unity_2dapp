﻿using UnityEngine;

/**
 * ゲームマネージャー(主なゲーム中の流れを管理する)
 */
public class GameManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ShareData.Instance.game_parameter_.Restart();

        scene_ = Scene.kNone;
        // 停止状態にしておく
        pauser_.is_pause_ = true;
    }

    // Update is called once per frame
    void Update () {
	
        // シーンが切り替わったときに一回だけ実行
        if (past_scene_ != scene_) {
            switch (scene_) {
            case Scene.kPlaying:
                SceneInitPlaying();
                break;
            case Scene.kClearEffect:
                SceneInitClearEffect();
                break;
            default:
                break;
            }
            past_scene_ = scene_;
        }

        // 現在のシーンを毎回実行
        switch (scene_) {
        case Scene.kNone:
            SceneNone();
            break;
        case Scene.kSetting:
            SceneSetting();
            break;
        case Scene.kPlaying:
            ScenePlaying();
            break;
        case Scene.kClearEffect:
            SceneClearEffect();
            break;
        case Scene.kFailureEffect:
            SceneFailureEffect();
            break;
        case Scene.kEndWait:
            SceneEndWait();
            break;
        case Scene.kNextScene:
            SceneNextScene();
            break;
        default:
            break;
        }
	}

    private void SceneNone() {
        // 特に無いのでセッティングへ
        scene_ = Scene.kSetting;
    }

    private void SceneSetting() {
        // 現在はまだセッティングが無いのですぐ次へ
        scene_ = Scene.kPlaying;
    }

    private void SceneInitPlaying() {
        // ポーズを解除
        pauser_.is_pause_ = false;
    }

    private void ScenePlaying() {
        // クリア条件や敗北条件を満たしていたら is_finish_がtrueになる
        if (ShareData.Instance.game_parameter_.is_finish_ == true) {
            if (ShareData.Instance.game_parameter_.is_clear_ == true) {
                // クリア
				scene_ = Scene.kClearEffect;
                //scene_ = Scene.kNextScene;
            } else {
                // 失敗
				scene_ = Scene.kFailureEffect;

            }
        }
    }
    private void SceneInitClearEffect() {
        // アクティブ状態にする
        clear_animation_root_.SetActive(true);

        Animator animator = clear_animation_root_.GetComponent<Animator>();
        animator.Rebind();
        // Animatorから再生
        animator.Play("clear", 0, 0.0f);
    }
    private void SceneClearEffect() {
        Animator animator = clear_animation_root_.GetComponent<Animator>();
        //MhCommon.Print("SceneClearEffect time " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        // 再生終了しているなら次のシーンへ
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            animator.Stop();
            scene_ = Scene.kEndWait;
        }
    }

    private void SceneFailureEffect() {
    }

    private void SceneEndWait() {
        // 画面にタッチしたら次のシーンへ
        // できればボタンとか用意して次に進む、リトライなどの選択をさせる
        if (TouchManager.Instance.IsTouchEnded()) {
            scene_ = Scene.kNextScene;
        }
    }

    private void SceneNextScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }

    // ポーズシステム
    public Pauser pauser_;
    public GameObject clear_animation_root_;

    private enum Scene {
        kNone,          // 特に何もしない
        kSetting,       // セッティング(オブジェクト配置中)
        kPlaying,       // ゲーム中
        kClearEffect,   // クリアエフェクト出す
        kFailureEffect, // 失敗エフェクトを出す
        kEndWait,       // 終了してユーザーの能動的な操作を待つ
        kNextScene,     // 次のシーンへ
    }

    private Scene scene_; // シーン
    private Scene past_scene_; // 前のシーン

}
