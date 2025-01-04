using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // === プロパティ ===
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private MouseSensitivity mouseSensitivity = new();

    // === 変数 ===
    private bool isGrounded = false;
    private bool isMouseLocked = false;
    private float verticalRotation = 0.0f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private GroundData currentGroundData;

    // === コンポーネント ===
    private AudioSource audioSource;
    private Camera playerCamera;

    // === データベース ===
    private GroundDatabase groundDatabase;


    void Start()
    {
        // マウスモードの切り替え
        ToggleMouseMode();

        // カメラとAudioSourceを取得
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();

        // データベースの参照
        groundDatabase = (GroundDatabase)Resources.Load("Databases/GroundDatabase");
    }

    void Update()
    {
        // マウスモードの切り替え（Escapeキーで）
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleMouseMode();

        // 移動
        if (isGrounded) Move();

        // 視点の回転
        RotateView();
    }

    private void ToggleMouseMode()
    {
        if (isMouseLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isMouseLocked = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseLocked = true;
        }
    }

    private void Move()
    {
        if (!isMouseLocked) return;

        Vector3 CalculateMoveDirection()
        {
            Vector3 direction = Vector3.zero;

            // プレイヤーが向いている方向を基準に移動
            direction += transform.forward * Input.GetAxis("Vertical"); // W/Sまたは矢印キー上下
            direction += transform.right * Input.GetAxis("Horizontal"); // A/Dまたは矢印キー左右

            return direction.normalized; // 正規化
        }

        // 移動速度を計算
        Vector3 moveDirection = CalculateMoveDirection();
        targetVelocity = moveDirection * speed;

        // 移動を適用
        velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10f); // スムーズな加速・減速
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // 足音の再生ハンドラー
        HandleFootsteps(moveDirection);
    }

    private void RotateView()
    {
        if (!isMouseLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity.Horizontal;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity.Vertical;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleFootsteps(Vector3 direction)
    {
        bool isPlaying = audioSource.isPlaying;
        if (audioSource.clip && direction != Vector3.zero && !isPlaying)
        {
            audioSource.Play();
        }
        else if (direction == Vector3.zero && isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            currentGroundData = groundDatabase.GetGroundByName(collision.gameObject.name);
            audioSource.clip = currentGroundData.footstepSound;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            currentGroundData = null;
        }
    }
}
