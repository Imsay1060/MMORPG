using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum Weapons
    {
        None,
        Sword,
        Katana
    }

    Weapons currentWeapon = Weapons.None;

    [SerializeField] GameObject sword, katana;
    [SerializeField] AudioSource characterSounds;
    [SerializeField] AudioClip jump, swingSound;
    [SerializeField] Image swordUI, katanaUI, cursor;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator anim;
    [SerializeField] float shiftSpeed = 10f;
    [SerializeField] float jumpForce = 7f;
    [SerializeField] float movementSpeed = 5f;
    Vector3 direction;
    float currentSpeed;
    float stamina = 5f;
    [SerializeField] private int health = 100;
    public bool isDead;
    [SerializeField] GameObject damageUi;
    bool isGrounded;
    bool hasSword, hasKatana;
    bool RunLeft;
    bool RunRight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentSpeed = movementSpeed;

        if (false)
        {
            transform.Find("Main Camera").gameObject.SetActive(false);
            transform.Find("Canvas").gameObject.SetActive(false);
            this.enabled = false;
        }
    }

    public void ChangeHealth(int count)
    {
        health -= count;
        damageUi.SetActive(true);
        Invoke("RemoveDamageUi", 0.1f);
        if (health <= 0)
        {
            isDead = true;
            anim.SetBool("Die", true);
            transform.Find("Main Camera").GetComponent<ThirdPersonCamera>().isSpectator = true;
            ChooseWeapon(Weapons.None);
            this.enabled = false;
        }
    }

    void RemoveDamageUi()
    {
        damageUi.SetActive(false);
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);

        if (direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("RunRight", moveHorizontal > 0);
            anim.SetBool("RunLeft", moveHorizontal < 0);
            anim.SetBool("Run", true);
            if (!characterSounds.isPlaying && isGrounded)
            {
                characterSounds.Play();
            }
        }
        else
        {
            anim.SetBool("Run", false);
            characterSounds.Stop();
        }

        anim.SetBool("RunRight", moveHorizontal > 0);
        anim.SetBool("RunLeft", moveHorizontal < 0);

        // Skok
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isGrounded = false;
            characterSounds.Stop();
            AudioSource.PlayClipAtPoint(jump, transform.position);
            anim.SetBool("Jump", true);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }
        else
        {
            stamina += Time.deltaTime;
            currentSpeed = movementSpeed;
        }

        stamina = Mathf.Clamp(stamina, 0, 5f);

        // Wybór broni
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasSword)
        {
            ChooseWeapon(Weapons.Sword);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasKatana)
        {
            ChooseWeapon(Weapons.Katana);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChooseWeapon(Weapons.None);
        }

        // Atak
        if (Input.GetMouseButtonDown(0) && currentWeapon != Weapons.None)
        {
            Attack();
        }

        // Śmierć
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void ChooseWeapon(Weapons selectedWeapon)
    {
        currentWeapon = selectedWeapon;
        anim.SetBool("Sword", currentWeapon == Weapons.Sword);
        anim.SetBool("Katana", currentWeapon == Weapons.Katana);
        anim.SetBool("NoWeapon", currentWeapon == Weapons.None);

        sword.SetActive(currentWeapon == Weapons.Sword);
        katana.SetActive(currentWeapon == Weapons.Katana);

        cursor.enabled = currentWeapon != Weapons.None;
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        characterSounds.PlayOneShot(swingSound);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "sword":
                if (!hasSword)
                {
                    hasSword = true;
                    swordUI.color = Color.white;
                    ChooseWeapon(Weapons.Sword);
                }
                break;

            case "katana":
                if (!hasKatana)
                {
                    hasKatana = true;
                    katanaUI.color = Color.white;
                    ChooseWeapon(Weapons.Katana);
                }
                break;
            default:
                break;
        }
        Destroy(other.gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
        }
    }

    private void Die()
    {
        isDead = true;
    }
}


