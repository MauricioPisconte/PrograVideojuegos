using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossIdleState : FSMState<BossController>
    {
        private void Update()
        {
            Debug.Log("oaa");
        }

        public BossIdleState(BossController controller) : base(controller)
        {
            // Transiciones
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    return Vector3.Distance(
                        mController.transform.position,
                        mController.Player.transform.position
                    ) < mController.WakeDistance;
                },
                getNextState: () => {
                    return new BossMovingState(mController);
                }
            ));

            /*Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    return mController.soltar;
                },
                getNextState: () => {
                    return new BossAttackingState(mController);
                }
            ));*/
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter IdleState");
            mController.animator.SetBool("IsMoving", false);
            mController.animator.SetFloat("Horizontal", 0f);
            mController.animator.SetFloat("Vertical", -1f);
            mController.AttackingEnd = false;
        }

        public override void OnExit()
        {
            Debug.Log("OnExit IdleState");
        }

        public override void OnUpdate(float deltaTime)
        { }



    }

}