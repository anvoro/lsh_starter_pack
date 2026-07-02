using UnityEngine;

namespace Lecture1.StateMachine.GoodCase.States
{
    /// <summary>
    /// Покой. Наследуется от GroundedState, поэтому "бесплатно" умеет:
    /// прыгать по кнопке и падать при сходе с платформы.
    /// Собственной логики осталось совсем мало — в этом выгода иерархии.
    /// </summary>
    public class IdleState : GroundedState
    {
        public IdleState(PlayerController p, StateMachine m) : base(p, m) { }

        public override void Enter()
        {
            // player.Animator.Play("Idle");
            Player.Body.linearVelocity = new Vector2(0f, Player.Body.linearVelocity.y);
        }

        public override void Tick()
        {
            // Сначала общие переходы суперсостояния (прыжок, падение).
            // Если один из них сработал — это состояние уже неактивно, выходим.
            if (TryCommonTransitions())
            {
                return;
            }

            // Затем собственный переход состояния: появился ввод -> бежим.
            if (Mathf.Abs(Player.Input.Horizontal) > 0.01f)
            {
                Machine.ChangeState(Player.MoveState);
            }
        }
    }
}