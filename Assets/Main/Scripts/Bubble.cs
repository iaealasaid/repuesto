using UnityEngine;
using UnityEngine.InputSystem;

public class bubble : MonoBehaviour
{
    [Header("Burbuja")]
    public float bubbleDuration = 5f;
    public float floatForce = 8f;
    public float floatDampening = 0.88f;
    public float cooldownTime = 3f;

    [Header("Control vertical")]
    public float extraUpForce = 6f;
    public float extraDownForce = 10f;

    [Header("Colliders")]
    public Collider playerCollider;
    public Collider bubbleCollider;

    [Header("Visual")]
    public GameObject bubbleVisual;
    public ParticleSystem bubblePopVFX;

    [HideInInspector] public bool bubbleActive;
    [HideInInspector] public float bubbleTimer;
    [HideInInspector] public float cooldownTimer;
    [HideInInspector] public bool onCooldown;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (bubbleVisual) bubbleVisual.SetActive(false);
        if (bubbleCollider) bubbleCollider.enabled = false;
    }

    void Update()
    {
        // Cooldown
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f) onCooldown = false;
        }

        // Timer
        if (bubbleActive)
        {
            bubbleTimer -= Time.deltaTime;
            if (bubbleTimer <= 0f) Deactivate();
        }

        // Visual
        if (bubbleVisual && bubbleVisual.activeSelf)
        {
            bubbleVisual.transform.localPosition = new Vector3(0, 0, -0.01f);
            Vector3 scale = bubbleVisual.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            bubbleVisual.transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (!bubbleActive) return;

        float gravComp = Mathf.Abs(Physics.gravity.y) * rb.mass;
        rb.AddForce(Vector3.up * (gravComp + floatForce), ForceMode.Force);

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            rb.AddForce(Vector3.up * extraUpForce, ForceMode.Force);
        }

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            rb.AddForce(Vector3.down * extraDownForce, ForceMode.Force);
        }

        Vector3 v = rb.linearVelocity;
        v.y *= floatDampening;
        rb.linearVelocity = v;
    }

    public void TryActivate()
    {
        if (bubbleActive || onCooldown) return;

        bubbleActive = true;
        bubbleTimer = bubbleDuration;

        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(v.x, Mathf.Min(v.y, 0f) * 0.2f, v.z);

        if (bubbleVisual) bubbleVisual.SetActive(true);

        playerCollider.enabled = false;
        bubbleCollider.enabled = true;
    }

    public void Deactivate()
    {
        bubbleActive = false;
        onCooldown = true;
        cooldownTimer = cooldownTime;

        if (bubbleVisual) bubbleVisual.SetActive(false);
        if (bubblePopVFX) bubblePopVFX.Play();

        playerCollider.enabled = true;
        bubbleCollider.enabled = false;

        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(v.x, Mathf.Min(v.y, 2f), v.z);
    }

    public float BubbleRatio => bubbleActive ? bubbleTimer / bubbleDuration : 0f;
    public float CooldownRatio => onCooldown ? cooldownTimer / cooldownTime : 0f;
}