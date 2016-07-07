using UnityEngine;
using System.Collections;

/**
 * オブジェクトがゴールしたかどうかのスクリプト
 */
public class GoalCheckScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        touch_time_ = 0;
        is_touching_ = false;
        is_goal_ = false;
    }
	
	// Update is called once per frame
	void Update () {
        // 一定時間触れ続けているならゴール
        if ((is_touching_ == true) && (is_goal_ == false)) {
            if (touch_time_ >= kGoalTouchTime) {
                MhCommon.Print("GoalCheckScript::Update goal");
                is_goal_ = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // タイムはリセット
        touch_time_ = 0;
        // 触れている
        is_touching_ = true;
    }

    void OnTriggerExit2D(Collider2D other) {
        touch_time_ = 0;
        // 触れていないことにする
        is_touching_ = false;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (is_goal_ == false) {
            // 触れているタイムを加算(オーバーフローしにくいようにゴール確定後は加算しない)
            touch_time_ += Time.deltaTime;
        }
    }

    // オブジェクト用の当たり判定
    public PolygonCollider2D object_collider_;
    // ゴール用の当たり判定
    public PolygonCollider2D goal_collider_;

    // ゴールしたことになる時間(2秒くらい)
    private const float kGoalTouchTime = 1.0f;

    // 触れている時間
    private float touch_time_ = 0;
    // 触れ続けているか
    private bool is_touching_ = false;
    // ゴールしているか
    private bool is_goal_ { set; get; }
}
