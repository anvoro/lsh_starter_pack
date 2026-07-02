using UnityEngine;

namespace Lecture1.StateMachine.BadCase
{
    /// <summary>
    /// ⚠️ АНТИПРИМЕР. Тот же контроллер через пару месяцев разработки.
    /// Каждое новое действие = новый флаг + правки во ВСЕХ старых блоках.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControllerNoFsm : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _crouchSpeed = 3f;
        [SerializeField] private float _jumpForce = 12f;
        [SerializeField] private float _dashSpeed = 20f;
        [SerializeField] private float _dashDuration = 0.2f;

        private Rigidbody2D _body;
        private bool _isGrounded;

        // ЗАПАХ КОДА №1: "болото флагов".
        // 5 флагов = 2^5 = 32 комбинации. Осмысленных — штук шесть.
        // Остальные 26 (isDashing && isStunned, isJumping && isCrouching...)
        // ничем не запрещены. Корректность держится только на внимательности.
        private bool _isJumping;
        private bool _isFalling;
        private bool _isCrouching;
        private bool _isDashing;
        private bool _isStunned;

        private float _dashTimer;
        private float _stunTimer;

        private void Awake() => _body = GetComponent<Rigidbody2D>();

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            bool jumpPressed = Input.GetButtonDown("Jump");
            bool dashPressed = Input.GetKeyDown(KeyCode.LeftShift);
            bool crouchHeld = Input.GetKey(KeyCode.S);

            // --- Оглушение перекрывает всё ---
            if (_isStunned)
            {
                _stunTimer -= Time.deltaTime;
                if (_stunTimer <= 0f) _isStunned = false;
                return; // ⚠️ ранний выход. А если нас оглушили ВО ВРЕМЯ дэша?
                // isDashing остался true, dashTimer заморожен.
                // После стана дэш "продолжится" из мёртвой точки. Баг.
                // В FSM это невозможно: переход Dash -> Stunned
                // ОБЯЗАН пройти через Dash.Exit(), где всё чистится.
            }

            // --- Дэш ---
            // ЗАПАХ КОДА №2: чтобы добавить ОДНО действие, нужно вспомнить
            // и перечислить ВСЕ существующие. Забыли одно — получили баг.
            if (dashPressed && !_isDashing && !_isCrouching && !_isStunned
                && !_isJumping && !_isFalling) // а можно ли дэш в воздухе? спорили неделю
            {
                _isDashing = true;
                _dashTimer = _dashDuration;
            }

            if (_isDashing)
            {
                _dashTimer -= Time.deltaTime;
                _body.linearVelocity = new Vector2(
                    Mathf.Sign(transform.localScale.x) * _dashSpeed, 0f);
                if (_dashTimer <= 0f) _isDashing = false;
                return; // ⚠️ ещё один ранний выход. Нажатие прыжка в этом
                // кадре потеряно навсегда (GetButtonDown не повторится).
            }

            // --- Присед ---
            // ЗАПАХ КОДА №3: каждое условие здесь — чей-то закрытый багрепорт.
            if (crouchHeld && _isGrounded && !_isJumping && !_isFalling && !_isDashing)
                _isCrouching = true;
            else if (!crouchHeld)
                _isCrouching = false;

            // --- Прыжок ---
            if (jumpPressed && _isGrounded && !_isJumping && !_isFalling
                && !_isCrouching) // ← это условие растёт с каждой новой фичей
            {
                _body.linearVelocity = new Vector2(_body.linearVelocity.x, _jumpForce);
                _isJumping = true;
            }

            // --- Пик дуги ---
            if (_isJumping && _body.linearVelocity.y <= 0f)
            {
                _isJumping = false;
                _isFalling = true;
            }

            // --- Сошли с края ---
            if (!_isGrounded && !_isJumping && !_isFalling && !_isDashing)
                _isFalling = true;

            // --- Приземление ---
            if (_isFalling && _isGrounded)
                _isFalling = false;

            // --- Движение ---
            // ЗАПАХ КОДА №4: скорость — функция от комбинации флагов.
            // Каждая новая механика добавляет сюда ветку.
            float speed = _isCrouching ? _crouchSpeed : _moveSpeed;

            _body.linearVelocity = new Vector2(
                horizontal * speed,
                _body.linearVelocity.y);

            if (!_isJumping && !_isFalling && Mathf.Abs(horizontal) < 0.01f)
                _body.linearVelocity = new Vector2(0f, _body.linearVelocity.y);

            // --- Анимации ---
            // ЗАПАХ КОДА №5: порядок else-if — это СКРЫТЫЕ приоритеты состояний.
            // Они нигде не задокументированы. Переставили строки — сломали игру.
            // if (isStunned) animator.Play("Stunned");
            // else if (isDashing) animator.Play("Dash");
            // else if (isCrouching) animator.Play("Crouch");
            // else if (isJumping) animator.Play("Jump");
            // else if (isFalling) animator.Play("Fall");
            // else if (Mathf.Abs(horizontal) > 0.01f) animator.Play("Run");
            // else animator.Play("Idle");
        }

        private void OnCollisionEnter2D(Collision2D _) => _isGrounded = true;
        private void OnCollisionExit2D(Collision2D _)  => _isGrounded = false;
    }
}