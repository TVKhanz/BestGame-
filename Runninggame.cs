using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float healthIncrement = 5f;
    public float speed = 5f;
    public float boostedSpeed = 10f;
    public float boostCooldown = 5f; // seconds

    private float currentHealth;
    private float currentSpeed;
    private bool isBoosted;
    private float lastBoostTime;

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        MoveCharacter();
        UpdateHealthBar();
        CheckSpeedBoostExpiration();
    }

    void InitializeGame()
    {
        currentHealth = 0f;
        currentSpeed = speed;
    }

    void MoveCharacter()
    {
        // Di chuyển nhân vật từ trái sang phải khi nhấn nút Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
        }
    }

    void UpdateHealthBar()
    {
        // Tăng lực khi nhấn nút Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth += healthIncrement;

            // Kiểm tra và reset thanh lực khi đạt 100
            if (currentHealth >= maxHealth)
            {
                currentHealth = 0f;
                // Reset tốc độ về mức ban đầu
                currentSpeed = speed;
            }

            // Cập nhật tốc độ dựa trên mốc thanh lực
            UpdateSpeed();
        }
    }

    void UpdateSpeed()
    {
        // Cập nhật tốc độ dựa trên mốc thanh lực
        if (currentHealth >= 25 && currentHealth < 50)
        {
            currentSpeed = boostedSpeed;
        }
        else if (currentHealth >= 50 && currentHealth < 75)
        {
            currentSpeed = boostedSpeed * 1.5f;
        }
        else if (currentHealth >= 75 && currentHealth < 100)
        {
            currentSpeed = boostedSpeed * 2f;
        }
    }

    void CheckSpeedBoostExpiration()
    {
        // Kiểm tra xem thời gian tăng tốc đã hết chưa
        if (isBoosted && Time.time - lastBoostTime >= boostCooldown)
        {
            isBoosted = false;
            // Reset tốc độ về mức ban đầu
            currentSpeed = speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoostPowerup"))
        {
            // Xử lý khi nhân vật va chạm với đối tượng tăng tốc
            isBoosted = true;
            lastBoostTime = Time.time;
            Destroy(other.gameObject); // Đối tượng tăng tốc biến mất khi va chạm
        }
    }
}
