using UnityEngine;

namespace Lecture1.StateMachine.GoodCase
{
    /// <summary>
    /// Зачем выделять ввод в отдельный класс?
    ///   1) Состояния не зависят от способа ввода. Захотим геймпад,
    ///      сенсорный экран или управление ИИ — заменим только этот класс.
    ///   2) Ввод читается РОВНО ОДИН РАЗ за кадр (в PlayerController),
    ///      и все состояния видят одни и те же значения.
    /// </summary>
    public class InputReader
    {
        /// <summary>Горизонтальная ось: -1 (влево), 0, +1 (вправо).</summary>
        public float Horizontal { get; private set; }

        /// <summary>true только в ТОТ кадр, когда кнопка прыжка была нажата.</summary>
        public bool JumpPressed { get; private set; }

        public void Read()
        {
            // GetAxisRaw возвращает "сырое" значение без сглаживания (-1, 0, 1) —
            // для платформера это даёт мгновенный отклик на нажатие.
            Horizontal = Input.GetAxisRaw("Horizontal");

            // GetButtonDown срабатывает один кадр — именно то,
            // что нужно для одиночного действия вроде прыжка.
            // "Jump" настраивается в Edit -> Project Settings -> Input Manager.
            JumpPressed = Input.GetButtonDown("Jump");
        }
    }
}