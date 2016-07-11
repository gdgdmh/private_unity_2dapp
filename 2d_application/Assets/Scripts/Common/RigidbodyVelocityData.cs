using UnityEngine;
using System.Collections;

public class RigidbodyVelocityData {
    public RigidbodyVelocityData(Rigidbody rigidbody) {
        // 引数のrigidbodyの値を設定
        velocity_ = rigidbody.velocity;
        angular_velocity_ = rigidbody.angularVelocity;
    }

    public Vector3 velocity_;
    public Vector3 angular_velocity_;

}
