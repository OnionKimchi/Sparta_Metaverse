using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero; // 넉백 방향과 세기
    private float knockbackDuration = 0.0f; // 넉백 지속 시간


    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;
    public float rotZ { get; private set; } // 외부에서 읽기만 가능하도록 설정

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0) return; // 게임이 멈춘 상태에서는 Update 중단
        HandleAction();
        // 속도가 0이 아닐 때만 방향을 기록
        if (_rigidbody.velocity != Vector2.zero)
        {
            Rotate(lookDirection); // 기록된 방향을 기반으로 rotZ 계산
        }

        UpdateSpriteFlip();
    }

    protected virtual void FixedUpdate()
    {
        if (Time.timeScale == 0) return;
        Movment(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    protected virtual void HandleAction()
    {

    }

    private void Movment(Vector2 direction)
    {
        
            direction = direction * 5;
        // 넉백 처리
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; // 이동 속도 감소
            direction += knockback; // 넉백 방향 추가
        }

        _rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    protected void Rotate(Vector2 direction)
    {
        Vector2 velocity = _rigidbody.velocity;

        if (velocity == Vector2.zero)
        {
            return;
        }

        rotZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        Debug.Log($"BaseController: rotZ = {rotZ}"); // 디버깅 메시지 추가
    }

   
    protected void UpdateSpriteFlip()
    {
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        characterRenderer.flipX = isLeft;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power; // 넉백 방향 계산
    }
}