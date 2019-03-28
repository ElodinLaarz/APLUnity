using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOrientation : MonoBehaviour
{
    private Animator w_Animator;
    private Transform pivotPoint;
 
 void Update()
    {
        w_Animator = transform.GetComponent(typeof(Animator)) as Animator;
        pivotPoint = transform.GetChild(0);

        if (w_Animator.GetCurrentAnimatorStateInfo(0).IsName("Wiz_Walk_Left") || w_Animator.GetCurrentAnimatorStateInfo(0).IsName("Wiz_Idle_Left")) {
            pivotPoint.localScale = new Vector3(1,-1,1);
        }
        else {
            pivotPoint.localScale = Vector3.one;
        }
    }
}
