using System.Collections;
using System.Linq;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackFrequencyPhase1 = 10f;
    [SerializeField] private float attackFrequencyPhase2 = 2f;
    [SerializeField] private GameObject GhostPrefab;
    private float AttackTimer = 0f;
    private float AttackTimer2 = 0f;
    private float EndTimer = 0f;
    private int phase = 1;
    private float healthBarTargetFillAmount = 1f;
    private BossStage2 stage2;
    private bool isDead = false;

    public override void Start()
    {
        base.Start();
        PlayerSpotted = true;
        player.PlayerSpotted = true;
        stage2 = FindObjectOfType<BossStage2>();
    }

    public override void Update()
    {
        base.Update();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, healthBarTargetFillAmount, Time.deltaTime);
        if (phase == 2 && !stage2.Stage2)
        {
            stage2.Stage2 = true;
            _animator.SetBool("Stage2", true);
            FindObjectsOfType<Ghost>().ToList().ForEach(g => g.MakeDead());
            
        }
        if (isDead) EndTimer += Time.deltaTime;
        if ((transform.position - player.transform.position).magnitude < 25f) player.PlayerSpotted = true;
        if (EndTimer > 4f && phase != 3)
        {
            phase = 3;
            player.TriggerEnding();
        }
    }

    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (healthbar is not null)
        {
            healthBarTargetFillAmount = Normalize(HP, 0,maxHP, 0, 1);
        }
        if (HP <= maxHP/2) phase = 2;
        if (HP <= 0) MakeDead();
    }
    
    protected override void MovementHandler()
    {
        MovementTimer += Time.deltaTime;
        direction = (transform.position - player.transform.position).normalized;
    }
    
    protected override void ApplyMovement()
    {
        if (isDead) return;
        var force = direction * 0;
        if (direction.x > 0f) FlipVisual(true);
        else if (direction.x < 0f) FlipVisual(false);
        rb.velocity = Vector2.Lerp(rb.velocity, force, Time.deltaTime * 10f);
    }
    
    public float GetDistanceToGround() {
        //Debug.DrawRay(nextPosition ?? transform.position, Vector2.down * 20, Color.green, 1, false);
        var hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            20,
            LayerMask.GetMask("Ground"));
        if (hit) return hit.distance;
        return -1;
    }

    public float GetDistanceToPlayer()
    {
        return (transform.position - player.transform.position).magnitude;
    }

    protected override void AttackHandler()
    {
        AttackTimer += Time.deltaTime;
        AttackTimer2 += Time.deltaTime;
        switch (phase)
        {
            case 1:
                if (AttackTimer > attackFrequencyPhase2)
                {
                    Instantiate(projectilePrefab, transform.position - new Vector3(direction.x, direction.y, 0) * 2, Quaternion.identity);
                    _animator.SetTrigger("Summon");
                    AttackTimer = 0f;
                }

                if (AttackTimer2 > attackFrequencyPhase1)
                {
                    StartCoroutine(SpawnEnemies());
                    AttackTimer2 = 0f;
                }
                break;
            case 2:
                if (AttackTimer > attackFrequencyPhase2)
                {
                    _animator.SetTrigger("Stage2Attack");
                    AttackTimer = 0f;
                }

                break;
        }
       
    }
    void FlipVisual(bool right)
    {
        isFacingRight = right;
        Vector3 ls = transform.localScale;
        //if (Math.Abs(ls.x) > ls.x && isFacingRight) ls.x *= -1;
        //else if (Mathf.Approximately(Math.Abs(ls.x), ls.x) && !isFacingRight) ls.x *= -1;
        if (!right && transform.rotation.eulerAngles.y == 0)transform.RotateAround(transform.parent.position, Vector3.up, 180);
        else if (right && transform.rotation.eulerAngles.y == 180)transform.RotateAround(transform.parent.position, Vector3.up, 180);
        //transform.localScale = ls;
    }
    
    float Normalize(float val, float valmin, float valmax, float min, float max) 
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

    public override void MakeDead()
    {
        player.kills++;
        phase = 0;
        isDead = true;
        _animator.SetTrigger("Dead");
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        int numOfCollidersActive = GetComponents<Collider2D>().Count(g => g.isActiveAndEnabled);
        if (other.gameObject.CompareTag("Player") && numOfCollidersActive == 2)
        {
            Debug.Log(transform.name);
            player.GetDamage(10f);
        }
    }
    
    private IEnumerator SpawnEnemies()
    {
        if (player.gameObject.activeSelf && phase == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 playerPosition = player.GetComponent<BoxCollider2D>().bounds.center;

                float spawnOffsetX = UnityEngine.Random.Range(7f, 9f);
                bool spawnOnLeft = i == 0;

                if (spawnOnLeft)
                    spawnOffsetX *= -1;

                Vector2 spawnPosition = new Vector2(playerPosition.x + spawnOffsetX, playerPosition.y);

                GameObject enemyInstance = Instantiate(GhostPrefab, spawnPosition, Quaternion.identity);
                enemyInstance.transform.localScale = GhostPrefab.transform.localScale;

                if (spawnOnLeft)
                    enemyInstance.transform.localScale = new Vector3(-enemyInstance.transform.localScale.x,
                        enemyInstance.transform.localScale.y,
                        enemyInstance.transform.localScale.z);
            }
        }
        yield break;
    }
}
