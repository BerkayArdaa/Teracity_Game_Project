
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject throwableObject;
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
    public int score = 0;
    [SerializeField]
    private GameObject aboveCharacterEffect;

    [SerializeField]
    private GameObject fireEffectAbovePlayer; // Assign this in the Inspector with the desired prefab
    [SerializeField]
    private GameObject kilic;  // Mevcut sword GameObject baðlantýsý
    [SerializeField]
    private GameObject bow;    // Bow GameObject için eklemelisin

    private GameObject activeWeapon;  // Aktif silahý tutacak deðiþken

    [SerializeField] private GameObject arrowPrefab; // Ok prefab'ý için referans
    [SerializeField] private Transform launchPoint; // Okun fýrlatýlacaðý nokta

    private bool isSpeedBoosted = false; // Hýz artýrmanýn aktif olup olmadýðýný takip eden deðiþken
    private float speedMultiplier = 1f; // Mevcut hýz çarpanýný takip edin

    private void Start()
    {

        mainCamera = Camera.main;
        z_BoxCollider = GetComponent<BoxCollider2D>();
        z_Animator = GetComponent<Animator>();
        SetHPtext(HP.ToString());
        SetShieldText(shield.ToString());
        activeWeapon = sword;  // Baþlangýç silahý olarak kýlýç
        sword.SetActive(true); // Kýlýç aktif
        bow.SetActive(false);  // Ok pasif


    }

    public void SetHPtext(string text)
    {
        HPtext.text = text;
    }

    public void SetShieldText(string text)
    {
        shieldText.text = text;
    }

    private void Update()
    {
        HandleInput();
        if (!isAttacking)
        {
            RotateWeaponTowardsMouse();
        }

        Movement();
        CheckDangerZone();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }
    private void SwitchWeapon()
    {
        if (activeWeapon == sword)
        {
            activeWeapon = bow;
            sword.SetActive(false);
            bow.SetActive(true);
        }
        else
        {
            activeWeapon = sword;
            bow.SetActive(false);
            sword.SetActive(true);
        }
    }

  
    // Method to handle iceball collision
    // Method to handle iceball collision
    private void HandleIceBallCollision()
    {
        // Apply the iceball effect
        StartCoroutine(ReduceSpeedTemporarily());

        // Reduce shield or HP
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

        Debug.Log("Player hit by iceball. Current HP: " + HP);

        // Check if player is dead
        if (HP <= 0)
        {
            isDeath = true;
            Debug.Log("Player has died.");
            Destroy(Player);
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
        if (collision.gameObject.CompareTag("GCoin"))
        {
            score += 100;
            UpdateScoreText();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(ShowCollisionEffect());
        }
        if (collision.gameObject.CompareTag("Goblin"))
        {
            Debug.Log("Goblin collision detected."); // Check if the collision is detected
            HandleGoblinCollision(collision);
        }
        if (collision.gameObject.CompareTag("Firee"))
        {
            StartCoroutine(HandleFireCollision());
        }
        if (collision.gameObject.CompareTag("SnowBalls")) // Check for iceball collision
        {
            HandleIceBallCollision(); // Call the method to handle iceball effects
            Destroy(collision.gameObject); // Destroy the iceball after it hits the player
        }
        if (collision.gameObject.CompareTag("BossBone"))
        {
            HandleBossBoneCollision(); // Bossbone ile çarpýþmayý ele al
           
        }
        if (collision.gameObject.CompareTag("RedOne"))
        {
            Debug.Log("Hit by RedOne!");
            HandleRedOneCollision(); // redOne ile çarpýþmayý ele al
        }
    }
    private void HandleRedOneCollision()
    {
        HP -= 20;
        SetHPtext(HP.ToString());
        Debug.Log("Player hit by redOne. Current HP: " + HP);

        if (HP <= 0)
        {
            isDeath = true;
            Debug.Log("Player has died.");
            Destroy(Player);
        }
    }
    private void HandleBossBoneCollision()
    {
        HP -= 5;
        SetHPtext(HP.ToString());
        Debug.Log("Player hit by bossbone. Current HP: " + HP);

        if (HP <= 0)
        {
            isDeath = true;
            Debug.Log("Player has died.");
            Destroy(Player);
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

    private IEnumerator HandleFireCollision()
    {
        Vector3 offsetPosition = transform.position + new Vector3(0, 0.20f, 0);
        GameObject fireEffectInstance = Instantiate(fireEffectAbovePlayer, offsetPosition, Quaternion.identity);

        for (int i = 0; i < 3; i++)
        {
            if (HP > 0)
            {
                HP -= 5;
                SetHPtext(HP.ToString());
                Debug.Log("Player took fire damage. Current HP: " + HP);

                if (HP <= 0)
                {
                    isDeath = true;
                    Debug.Log("Player has died.");
                    Destroy(Player);
                    break;
                }
            }
            yield return new WaitForSeconds(1f); // Damage interval
        }

        yield return new WaitForSeconds(2f); // Effect display time
        Destroy(fireEffectInstance);
    }

    public void UpdateScoreText()
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

        Vector2 moveDelta = new Vector2(MoveX, MoveY).normalized; // Normalize to ensure consistent speed

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
        castResult = Physics2D.BoxCast(transform.position, z_BoxCollider.size, 0, new Vector2(MoveX, 0), Mathf.Abs(MoveX * Time.deltaTime * z_MoveSpeed), LayerMask.GetMask("Enemy", "BlockMove"));
        if (castResult.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * z_MoveSpeed, 0, 0);
        }

        // y-axis
        castResult = Physics2D.BoxCast(transform.position, z_BoxCollider.size, 0, new Vector2(0, MoveY), Mathf.Abs(MoveY * Time.deltaTime * z_MoveSpeed), LayerMask.GetMask("Enemy", "BlockMove"));
        if (castResult.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime * z_MoveSpeed, 0);
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
        amount = 20;
        HP += amount;
        if (HP >= 100)
        {
            HP = 100;
        }

        SetHPtext(HP.ToString());
        Debug.Log("HP increased. Current HP: " + HP);
    }

    public void IncreaseSpeed(float multiplier)
    {
        if (!isSpeedBoosted)
        {
            StartCoroutine(IncreaseSpeedTemporarily(multiplier, 5f));
        }
    }

    private IEnumerator IncreaseSpeedTemporarily(float multiplier, float duration)
    {
        isSpeedBoosted = true;
        speedMultiplier *= multiplier;
        z_MoveSpeed *= multiplier; // Mevcut hýzý artýrýn

        yield return new WaitForSeconds(duration);

        speedMultiplier /= multiplier;
        z_MoveSpeed /= multiplier; // Orijinal hýza geri dönün
        isSpeedBoosted = false;
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

        // Yönü ve kullanýlan silaha göre dönüþ açýsýný ayarla
        if (transform.localScale.x < 0)
        {
            angle += 180; // Eðer karakter ters dönükse, silahýn da ters dönmesi için 180 derece ekleyin
        }

        // Aktif silahýn dönüþünü ayarla
        activeWeapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Karakterin yönünü ayarla

    }

    // Inside PlayerMovement class
    public void DecreaseScore(int amount)
    {
        if (score >= amount)
        {
            score -= amount;
            UpdateScoreText();
            Debug.Log("Score decreased. Current score: " + score);
        }
        else
        {
            Debug.Log("Not enough score to decrease.");
        }
    }
    void FireArrow()
    {
        if (arrowPrefab != null && launchPoint != null)
        {
            // Oku fýrlatma noktasýndan instantiate eder
            GameObject newArrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity);

            // Mouse pozisyonunu al ve dünya koordinatlarýna çevir
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = 10.0f; // Kamera'dan düzlem yüzeyine olan mesafeyi ayarlayýn
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0; // Z koordinatýný sýfýrla çünkü 2D oyun yapýyorsun

            // Hedef yönünü hesapla
            Vector2 direction = (mouseWorldPosition - launchPoint.position).normalized;

            // Okun fiziksel hareketini saðlar
            Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f; // Oku hedefe doðru fýrlat, hýzýný ayarla
            }
        }
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeWeapon == sword && !isAttacking)
        {
            ThrowObject(); // Fýrlatma iþlemi
            StartCoroutine(PerformAttack()); // Kýlýçla saldýrý animasyonu
        }
        if (Input.GetMouseButtonDown(0) && activeWeapon == bow)
        {
            FireArrow();
        }
    }
    private void ThrowObject()
    {
        if (throwableObject != null)
        {
            // Fýrlatma objesini instantiate et
            GameObject thrownObj = Instantiate(throwableObject, launchPoint.position, Quaternion.identity);

            // Mouse pozisyonunu al ve dünya koordinatlarýna çevir
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            mouseWorldPosition.z = 0; // Z koordinatýný sýfýrla çünkü 2D oyun yapýyorsun

            // Hedef yönünü hesapla
            Vector2 direction = (mouseWorldPosition - launchPoint.position).normalized;

            // Objeye fiziksel hareket ekle
            Rigidbody2D rb = thrownObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f; // Objenin hýzýný ayarla
            }

            // Objenin dönüþ açýsýný mouse yönüne ayarla
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            thrownObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }


}
