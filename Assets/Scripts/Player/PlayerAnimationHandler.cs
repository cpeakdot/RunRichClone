using System;
using UnityEngine;

namespace RRC.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int WalkingRich = Animator.StringToHash("IsWalkingRich");
        private static readonly int WalkingPoor = Animator.StringToHash("IsWalkingPoor");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int JumpSad = Animator.StringToHash("JumpSad");
        private static readonly int Cry = Animator.StringToHash("Cry");

        private void Start()
        {
            animator.applyRootMotion = false;
        }

        public void SetAnimation(PlayerAnimation animation)
        {
            // Jump animation has exit time.
            if (animation != PlayerAnimation.Jump)
            {
                ResetAnimatorValues();
            }

            switch (animation)
            {
                case PlayerAnimation.Idle:
                    break;
                case PlayerAnimation.WalkingP:
                    animator.SetBool(WalkingPoor, true);
                    break;
                case PlayerAnimation.WalkingA:
                    animator.SetBool(WalkingPoor, true);
                    break;
                case PlayerAnimation.WalkingR:
                    animator.SetBool(WalkingRich, true);
                    break;
                case PlayerAnimation.Jump:
                    animator.SetTrigger(Jump);
                    break;
                case PlayerAnimation.Dance:
                    animator.applyRootMotion = true;
                    animator.SetTrigger(Dance);
                    break;
                case PlayerAnimation.Cry:
                    animator.applyRootMotion = true;
                    animator.SetTrigger(Cry);
                    break;
                case PlayerAnimation.JumpSad:
                    animator.SetTrigger(JumpSad);
                    break;
                default:
                    break;
            }
        }

        private void ResetAnimatorValues()
        {
            animator.SetBool(WalkingPoor, false);
            animator.SetBool(WalkingRich, false);
        }
    }
}

