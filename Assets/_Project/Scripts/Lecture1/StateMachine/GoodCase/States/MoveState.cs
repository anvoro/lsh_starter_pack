using UnityEngine;

namespace Lecture1.StateMachine.GoodCase.States
{
    /// <summary>
    /// Бег по земле. Как и Idle, наследует общие переходы от GroundedState.
    /// Сравните с версией без иерархии: там проверка прыжка дублировалась
    /// и в Idle, и в Move. Теперь она в одном месте — в суперсостоянии.
    /// </summary>
    public class MoveState : GroundedState
    {
        public MoveState(PlayerController p, StateMachine m) : base(p, m) { }

        public override void Enter()
        {
            // player.Animator.Play("Run");
        }

        public override void Tick()
        {
            // Общие переходы — всегда первыми.
            if (TryCommonTransitions())
            {
                return;
            }

            // Собственный переход: ввод пропал -> покой.
            if (Mathf.Abs(Player.Input.Horizontal) < 0.01f)
            {
                Machine.ChangeState(Player.IdleState);
                return;
            }

            // Собственное поведение: движение по земле.
            Player.Body.linearVelocity = new Vector2(
                Player.Input.Horizontal * Player.MoveSpeed,
                Player.Body.linearVelocity.y);
        }
    }
}