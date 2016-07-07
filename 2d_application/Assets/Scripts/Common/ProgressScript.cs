using UnityEngine;
using System.Collections;

public class ProgressScript : MonoBehaviour {

     

	// Use this for initialization
	void Start () {
        scene_ = Scene.kStart;
        fill_amount_ = 0.0f;
        progress_image_.fillAmount = fill_amount_;
    }
	
	// Update is called once per frame
	void Update () {

        switch (scene_) {
            // シーン割り当てなし
        case Scene.kNone:
            break;
            // 開始シーン
        case Scene.kStart:
            // 更新シーンへ
            scene_ = Scene.kUpdate;
            break;
            // プログレス更新処理
        case Scene.kUpdate:
            fill_amount_ += (1.0f * Time.deltaTime);
            progress_image_.fillAmount = fill_amount_;
            if (fill_amount_ > 1.0f) {
                fill_amount_ = 0.0f;
            }
            break;
        default:
            break;

        }

    }

    /**
     * 再度開始する
     */
    public void Restart() {
        scene_ = Scene.kStart;
        fill_amount_ = 0.0f;
        progress_image_.fillAmount = fill_amount_;
    }

    /**
     * プログレスを終了する
     */
    public void Finish() {
        // ステータスをなし、fillAmountを0に設定(非表示になる)
        scene_ = Scene.kNone;
        fill_amount_ = 0.0f;
        progress_image_.fillAmount = fill_amount_;
    }

    /**
     * 一時停止
     */
    public void Pause() {
        scene_ = Scene.kNone;
    }

    /**
     * 再開(fillAmountは前の状態から再開)
     */
    public void Resume() {
        scene_ = Scene.kStart;
    }

    // 外部参照
    public UnityEngine.UI.Image progress_image_; // プログレスのイメージ

    // シーン定義
    private enum Scene {
        kNone,
        kStart,
        kUpdate,
    }

    private const float kLoopTime = 30.0f;

    private Scene scene_ = Scene.kNone;
    private float fill_amount_ = 0.0f; // プログレスの表示パラメータ(0.0～1.0)
}
