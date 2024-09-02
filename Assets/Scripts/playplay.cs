using System.Collections;
using UnityEngine;
using TMPro;

public class playplay : MonoBehaviour
{
    [SerializeField]
    private float z_MoveSpeed = 1f;
    private BoxCollider2D z_BoxCollider;

    [SerializeField]
    GameObject Player;
    [SerializeField]
    public int HP = 100;
    [SerializeField]
    private float shield = 0;
    [SerializeField]
    GameObject Blood;
    private Animator z_Animator;
    [SerializeField]
    private GameObject sword;

    private Camera mainCamera;
    [SerializeField]
    private GameObject enemyCollisionEffect;

    private bool isAttacking = false;
    public bool IsAttacking { get; private set; }
    private bool isDeath = false;
    private bool isInDanger = false;
    private bool isTakingDamage = false;
    private bool isInDangerZone;
    [SerializeField]
    private TextMeshProUGUI HPtext;
    [SerializeField]
    private TextMeshProUGUI shieldText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score = 0;
    [SerializeField]
    private GameObject aboveCharacterEffect;

    private void Start()
    {
        mainCamera = Camera.main;
        z_BoxCollider = GetComponent<BoxCollider2D>();
        z_Animator = GetComponent<Animator>();
        SetHPtext(HP.ToString());
        SetShieldText(shield.ToString());
    }

    public void SetHPtext(string text)
    {
        HPtext.text = text;
    }

    public void SetShieldText(string text)
    {
        shieldText.text = text;
    }

    private void FixedUpdate()
    {
        HandleInput();
        if (!isAttacking)
        {
            RotateWeaponTowardsMouse();
        }

        Movement();
        CheckDangerZone();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            score += 5;
            UpdateScoreText();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {

            StartCoroutine(ShowCollisionEffect());
        }
        if (collision.gameObject.CompareTag("Goblin"))
        {
            Debug.Log("Goblin collision detected."); // Çarpýþmanýn algýlandýðýný kontrol etmek için
            HandleGoblinCollision(collision);
        }
    }

    private IEnumerator ShowCollisionEffect()
    {
        Vector3 offsetPosition = transform.position + new Vector3(0, 0.20f, 0);
        GameObject effectInstance = Instantiate(enemyCollisionEffect, offsetPosition, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(effectInstance);
    }

    private void HandleGoblinCollision(Collider2D collision)
    {
        Debug.Log("Handling goblin collision..."); // Çarpýþma iþlemi baþlatýlýyor mu?

        // Goblinden uzaklaþma yönünü hesapla
        Vector3 directionAwayFromGoblin = (transform.position - collision.transform.position).normalized;
        Vector3 moveBackPosition = transform.position + directionAwayFromGoblin * 0.20f; // 2 birim geri hareket et

        // Oyuncuyu geri taþý
        transform.position = moveBackPosition;
        Debug.Log("Moved back to: " + moveBackPosition); // Hareketin gerçekleþip gerçekleþmediðini kontrol et

        // Hýzý geçici olarak azalt
        StartCoroutine(ReduceSpeedTemporarily());
    }

    private IEnumerator ReduceSpeedTemporarily()
    {
        Vector3 offsetPosition = transform.position + new Vector3(0, 0.20f, 0);
        GameObject effectInstance = Instantiate(aboveCharacterEffect, offsetPosition, Quaternion.identity);
        float originalSpeed = 1f;
        z_MoveSpeed /= 2; // Halve the speed
        Debug.Log("Speed reduced to: " + z_MoveSpeed); // Debugging line
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        Destroy(effectInstance);
        z_MoveSpeed = originalSpeed; // Restore original speed
        Debug.Log("Speed restored to: " + z_MoveSpeed); // Debugging line
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    private IEnumerator PerformAttack()
    {
        StartAttack();
        yield return new WaitForSeconds(0.1f);
        EndAttack();
    }

    private void StartAttack()
    {
        isAttacking = true;
        sword.transform.Rotate(new Vector3(0, 0, -80));
    }

    private void EndAttack()
    {
        sword.transform.Rotate(new Vector3(0, 0, 80));
        isAttacking = false;
    }

    private void Movement()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDelta = new Vector2(MoveX, MoveY);

        if (moveDelta.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        RaycastHit2D castResult;

        // x-axis
        castResult = Physics2D.BoxCast(transform.position, z_BoxCollider.size, 0, new Vector2(MoveX, 0), Mathf.Abs(MoveX * Time.fixedDeltaTime * z_MoveSpeed), LayerMask.GetMask("Enemy", "BlockMove"));
        if (castResult.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * z_MoveSpeed, 0, 0);
        }

        // y-axis
        castResult = Physics2D.BoxCast(transform.position, z_BoxCollider.size, 0, new Vector2(0, MoveY), Mathf.Abs(MoveY * Time.fixedDeltaTime * z_MoveSpeed), LayerMask.GetMask("Enemy", "BlockMove"));
        if (castResult.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.fixedDeltaTime * z_MoveSpeed, 0);
        }
        bool isWalking = moveDelta.magnitude > 0;
        z_Animator.SetBool("isWalking", isWalking);
    }

    private void CheckDangerZone()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, z_BoxCollider.size, 0);
        isInDangerZone = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("DangerZones") || collider.CompareTag("Enemy") || collider.CompareTag("FireBall") || collider.CompareTag("Wizard"))
            {
                isInDangerZone = true;

                if (!isTakingDamage)
                {
                    StartCoroutine(TakeDamageOverTime());
                }
                break;
            }
            if (collider.CompareTag("Goblin"))
            {
                isInDangerZone = true;
                if (!isTakingDamage)
                {
                    StartCoroutine(TakeDamageOverTimeForGoblin());
                }
            }
            if (collider.CompareTag("SnowBalls"))
            {
                isInDangerZone = true;
                if (!isTakingDamage)
                {
                    StartCoroutine(ReduceSpeedTemporarily());
                    StartCoroutine(TakeDamageOverTimeForGoblin());
                }
            }
        }
    }

    private IEnumerator TakeDamageOverTime()
    {
        isTakingDamage = true;
        while (isInDangerZone && !isDeath)
        {
            if (shield > 0)
            {
                shield -= 5;
                if (shield < 0)
                {
                    shield = 0; // Ensure shield does not go negative
                }
                SetShieldText(shield.ToString()); // Update shield text
            }
            else
            {
                HP -= 5;
            }

            SetHPtext(HP.ToString());
            StartCoroutine(SpawnBloodWithDelay(0.5f));
            Debug.Log("HP: " + HP + ", Shield: " + shield);

            if (HP <= 0)
            {
                isDeath = true;
                Debug.Log("Player has died.");
                Destroy(Player);
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
        isTakingDamage = false;
    }

    private IEnumerator TakeDamageOverTimeForGoblin()
    {
        isTakingDamage = true;
        while (isInDangerZone && !isDeath)
        {
            if (shield > 0)
            {
                shield -= 25;
                if (shield < 0)
                {
                    shield = 0; // Ensure shield does not go negative
                }
                SetShieldText(shield.ToString()); // Update shield text
            }
            else
            {
                HP -= 10;
            }

            SetHPtext(HP.ToString());
            StartCoroutine(SpawnBloodWithDelay(0.5f));
            Debug.Log("HP: " + HP + ", Shield: " + shield);

            if (HP <= 0)
            {
                isDeath = true;
                Debug.Log("Player has died.");
                Destroy(Player);
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
        isTakingDamage = false;
    }

    public void IncreaseHP(int amount)
    {
        HP += amount;
        if (HP >= 100)
        {
            HP = 100;
        }

        SetHPtext(HP.ToString());
        Debug.Log("HP increased. Current HP: " + HP);
    }

    public void IncreaseSpeed(float amount)
    {
        StartCoroutine(IncreaseSpeedTemporarily(amount, 5f));
    }

    private IEnumerator IncreaseSpeedTemporarily(float multiplier, float duration)
    {
        float originalSpeed = z_MoveSpeed;
        z_MoveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        z_MoveSpeed = originalSpeed;
    }

    public void IncreaseShield(float amount)
    {
        shield += amount;
        SetShieldText(shield.ToString()); // Update shield text
        Debug.Log("Shield increased. Current Shield: " + shield);
    }

    private IEnumerator SpawnBloodWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject newBlood = Instantiate(Blood, transform.position, Quaternion.identity);
        Destroy(newBlood, 4.0f);
    }

    private void RotateWeaponTowardsMouse()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Vector3 direction = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (transform.localScale.x < 0)
        {
            angle += 270;
        }

        sword.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (mouseScreenPosition.x < Screen.width / 2)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}