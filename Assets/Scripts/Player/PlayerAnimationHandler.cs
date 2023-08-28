using UnityEngine;

namespace RRC.Player
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        
        private static readonly int WalkingRich = Animator.StringToHash("IsWalkingRich");
        private static readonly int WalkingPoor = Animator.StringToHash("IsWalkingPoor");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int JumpSad = Animator.StringToHash("JumpSad");
        private static readonly int Cry = Animator.StringToHash("Cry");

        private void Start()
        {
            anim.applyRootMotion = false;
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
                    anim.SetBool(WalkingPoor, true);
                    break;
                case PlayerAnimation.WalkingA:
                    anim.SetBool(WalkingPoor, true);
                    break;
                case PlayerAnimation.WalkingR:
                    anim.SetBool(WalkingRich, true);
                    break;
                case PlayerAnimation.Jump:
                    anim.SetTrigger(Jump);
                    break;
                case PlayerAnimation.Dance:
                    anim.applyRootMotion = true;
                    anim.SetTrigger(Dance);
                    break;
                case PlayerAnimation.Cry:
                    anim.applyRootMotion = true;
                    anim.SetTrigger(Cry);
                    break;
                case PlayerAnimation.JumpSad:
                    anim.SetTrigger(JumpSad);
                    break;
                default:
                    break;
            }
        }

        private void ResetAnimatorValues()
        {
            anim.SetBool(WalkingPoor, false);
            anim.SetBool(WalkingRich, false);
        }
    }
}

