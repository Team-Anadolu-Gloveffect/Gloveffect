using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldObject;
    public float shieldDuration = 5f;
    public float shieldCooldown = 10f;
    public bool isShielding = false;
    private float shieldTimer = 0f;
    public bool activateShield = false;
    
    [Header("Combat Actions")]
    public bool activateKick = false;
    public bool activateCrossPunch = false;
    public bool activateUpperCut = false;
    
    [Header("Health")]
    public float PlayerHealth = 1f;

    private void Start()
    {
        shieldObject.SetActive(false);
    }

    private void Update()
    {
        HandleShield();
        HandleAttacks();
    }

    private void HandleShield()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isShielding && shieldTimer <= 0f && activateShield)
        {
            ActivateShield();
        }

        if (isShielding)
        {
            UpdateShieldTimer();
        }
        else if (shieldTimer > 0f)
        {
            DecreaseShieldTimer();
        }
    }

    private void ActivateShield()
    {
        shieldObject.SetActive(true);
        isShielding = true;
        shieldTimer = shieldDuration;
        PlayerHealth = 2;
    }

    private void UpdateShieldTimer()
    {
        shieldTimer -= Time.deltaTime;
        if (shieldTimer <= 0f)
        {
            DeactivateShield();
        }
    }

    private void DeactivateShield()
    {
        shieldObject.SetActive(false);
        isShielding = false;
        shieldTimer = shieldCooldown;
        PlayerHealth = 1;
    }

    private void DecreaseShieldTimer()
    {
        shieldTimer -= Time.deltaTime;
    }

    private void HandleAttacks()
    {
        if (Input.GetKeyDown(KeyCode.G) && activateKick)
        {
            PerformAttack("Kick");
        }
        else if (Input.GetKeyDown(KeyCode.F) && activateCrossPunch)
        {
            PerformAttack("CrossPunch");
        }
        else if (Input.GetKeyDown(KeyCode.H) && activateUpperCut)
        {
            PerformAttack("UpperCut");
        }
    }

    private void PerformAttack(string attackType)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.red, 2f);
            if (hit.transform.CompareTag("FlametronEnemy"))
            {
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(5);
                }

                Renderer enemyRenderer = hit.transform.GetComponent<Renderer>();
                if (enemyRenderer != null)
                {
                    enemyRenderer.material.color = Color.red;
                    StartCoroutine(ResetColor(enemyRenderer, 0.1f));
                }
            }
        }
    }

    private IEnumerator ResetColor(Renderer renderer, float delay)
    {
        yield return new WaitForSeconds(delay);
        renderer.material.color = Color.black;
    }
}