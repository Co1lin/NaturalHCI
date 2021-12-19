using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestureCircle : GestureWidget
{
    private float prev_time = 0.0f;
    private int[] angle_bucket;
    private int[] dp_bucket;
    private const int angle_bucket_size = 18;
    private float duration = 2.0f;
    private List<Vector3> history;
    public override void Start()
    {
        base.Start();
        angle_bucket = new int[angle_bucket_size];
        dp_bucket = new int[angle_bucket_size];
        history = new List<Vector3>();
    }

    public override bool GestureCondition()
    {
        if(Time.time - prev_time > duration){
            Vector3 center = Vector3.zero;
            for (int i = 0; i < history.Count; i++) {
                center += history[i];
            }
            center /= history.Count;

            for (int i = 0; i < angle_bucket_size; i++) {
                angle_bucket[i] = -1;
                dp_bucket[i] = 0;
            }

            for (int i = 0; i < history.Count; i++) {
                Vector3 vec = history[i] - center;
                // Debug.Log("center: "+center.ToString()+" history: "+history[i].ToString()+" delta: "+vec.ToString()+" delta_len:"+vec.sqrMagnitude.ToString());
                if (vec.sqrMagnitude < 0.001f) continue;
                Vector3 y = new Vector3(0, 1, 0);
                Vector3 z = new Vector3(0, 0, 1);
                float angle_y = Vector3.Angle(vec, y);
                float angle_z = Vector3.Angle(vec, z);
                int angle = (int)(angle_z < 90 ? angle_y : 360 - angle_y);
                angle = angle / (360 / angle_bucket_size);
                if (angle >= angle_bucket_size) angle = angle_bucket_size - 1;
                angle_bucket[angle]++;
                for (int k = 0; k <= angle; ++k)
                    dp_bucket[angle] = Math.Max(dp_bucket[angle], dp_bucket[k]+1);
            }

            // Debug.Log("hl: " + history.Count);
            // for(int i = 0 ; i < angle_bucket_size ; i++)
            //     Debug.Log("angle_bucket["+i.ToString()+"]: "+angle_bucket[i].ToString());
            int cnt = 0;
            for(int i = 0 ; i < angle_bucket_size; i++){
                if(angle_bucket[i] != -1) cnt += 1;
                if (i > 0) dp_bucket[i] = Math.Max(dp_bucket[i], dp_bucket[i-1]);
            }
            if (cnt >= angle_bucket_size - 3) {
                float rate = dp_bucket[angle_bucket_size-1] / (float)(history.Count);
                Debug.Log("rate: "+rate.ToString()); // > 0.4 顺时针， < 0.4 逆时针
                if (rate > 0.4f) {
                    _animator.speed += 0.6f;
                } else {
                    _animator.speed -= 0.6f;
                }
                prev_time = Time.time;
                history.Clear();
                return rate > 0.4f;
            }
        }
        if (
            HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, _handedness_right, out var p)
        ) {
            Vector3 vec = p.Position;
            // Debug.Log("*history: "+vec.ToString());
            vec = new Vector3(0, vec.y, vec.z);
            history.Add(vec);
        }
        return false;
    }
}