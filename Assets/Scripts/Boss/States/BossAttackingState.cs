using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossAttackingState : FSMState<BossController>
    {
        public BossAttackingState(BossController controller) : base(controller)
        {
            Transitions.Add(new FSMTransition<BossController>(
                isValid: () => {
                    return mController.AttackingEnd;
                },
                getNextState: () => {
                    return new BossIdleState(mController);
                }
            ));
        }

        public override void OnEnter()
        {
            Debug.Log("OnEnter AttackingState");
            //mController.InstanciarEnemigosFrente();
            //mController.animator.SetTrigger("Attack");
            //mController.hitBox.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            Debug.Log("OnExit AttackingState");
            //mController.soltar = false;
            //mController.hitBox.gameObject.SetActive(false);
        }

        public override void OnUpdate(float deltaTime)
        { }
    }
}
