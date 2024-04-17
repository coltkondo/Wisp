using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestop_Anim : StateMachineBehaviour
{
    private GameObject Anim_UI;
    private Animator anim;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Anim_UI = GameObject.FindGameObjectWithTag("Timestop_Anim");
        anim = animator;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        /*Debug.Log("" + anim.GetCurrentAnimatorStateInfo(0).IsName("TimestopAnim"));
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Stopped"))
        {
            Debug.Log("Animation Stop Time");
            anim.SetBool("isPlaying", false);
            anim.SetBool("Play", false);
            anim.SetBool("isReady", true);
            Anim_UI.SetActive(false);
        }


        if (anim.GetBool("Play") == true)
        {
            if (anim.GetBool("isPlaying") == false)
            {
                Debug.Log("Begin playing anim");
                //Anim_UI.SetActive(true);
                anim.SetBool("isPlaying", true);
            }
        }*/




    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Animation Ended");
        anim.SetBool("Play", false);
        Anim_UI.SetActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
