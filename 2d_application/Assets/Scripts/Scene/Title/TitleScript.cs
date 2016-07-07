using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

      // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        // タッチ完了
        if (TouchManager.Instance.IsTouchEnded() == true) {
            // メインメニューへ
            MhCommon.Print("TitleScript::Update change scene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("game_tutorial");
        }
        // タッチテスト 
        //Vector3 v = TouchManager.Instance.GetTouchPosition();
        //MhCommon.Print("x=" + v.x + " y=" + v.y);
    }
}
