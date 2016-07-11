using UnityEngine;
using System.Collections;

public class ThrowAnimation00ChaHandScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        animator_ = GetComponent<Animator>();
        MhCommon.Assert(animator_ != null, "ThrowAnimation00ChaHandScript::Start animator_ null");
        is_animation_active_ = true;
    }
	
	// Update is called once per frame
	void Update () {
        is_animation_active_ = true;
        // アニメーションの割合を0.0～1.0fで返してくれるので1.0fなら終了とみなす
        if (animator_.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            //MhCommon.Print("ThrowAnimation00ChaHandScript::Update finish");
            is_animation_active_ = false;
        }
	}


    private Animator animator_ = null;
    private bool is_animation_active_ { set; get; }
}
