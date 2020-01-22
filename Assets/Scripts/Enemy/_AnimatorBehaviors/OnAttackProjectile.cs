using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behavior script used for characters to fire projectile(s) when entering a animator state
 * Offers different firing modes, and other options
 */
public class OnAttackProjectile : StateMachineBehaviour
{
    //Be careful not to modify these variables in the script, as changes aren't reset on OnStateEnter
    [Tooltip("How to fire the projectile.Current options are: \"straight\", \"arc\"")]
    public string FiringMode;
    [Tooltip("Amount of time that passes before attacking starts (waiting for animation)")]
    public float AttackStartBuffer;
    public GameObject Projectile;
    public AudioClip AttackSFX; //Can be null
    public float AttackVolume = 1;

    [Tooltip("How many times to fire")]
    public int AttackCount = 1;
    [Tooltip("Delay between shots (if mutiple shots are fired)")]
    public float ShotDelay;

    [Tooltip("How quickly a straight projectile travels. Used for 'straight' firing mode")]
    public float ProjectileSpeed;
    [Tooltip("How long an arc-ed projectile is in the air. Used for 'arc' firing mode")]
    public float ProjectileHangTime;

    private int _attack_count;
    private float _startTime;
    private float _time_of_last_shot;
    private GameObject _gameObject;
    private GameObject _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _startTime = Time.time;
        _time_of_last_shot = Time.time - ShotDelay; //first shot will never be delayed by this condition
        _attack_count = AttackCount;
        _gameObject = animator.gameObject;
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck

        if (FiringMode == "straight")
        {
            if (_player.transform.position.x > _gameObject.transform.position.x)
                _gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            else
                _gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (Time.time > _startTime + AttackStartBuffer && Time.time > _time_of_last_shot + ShotDelay && _attack_count > 0)
        {
            if (AttackSFX != null)
                animator.GetComponent<AudioSource>().PlayOneShot(AttackSFX, AttackVolume);

            GameObject projectile;
            Rigidbody2D rb;

            switch (FiringMode)
            {
                case "straight":
                    projectile = Instantiate(Projectile, _gameObject.transform.position, _gameObject.transform.rotation);
                    rb = projectile.GetComponent<Rigidbody2D>();
                    rb.velocity = new Vector2(Mathf.Sign((_gameObject.transform.rotation.eulerAngles.y - 90) * -1) * ProjectileSpeed, 0);
                    break;
                case "arc":
                    projectile = Instantiate(Projectile, _gameObject.transform.position, new Quaternion());
                    rb = projectile.GetComponent<Rigidbody2D>();

                    float range = _player.transform.position.x - _gameObject.transform.position.x;
                    float angle = Mathf.Atan(Physics2D.gravity.y * rb.gravityScale * ProjectileHangTime * ProjectileHangTime / (2 * range));
                    float velocity = range / (ProjectileHangTime * Mathf.Cos(angle));

                    rb.velocity = new Vector2(velocity * Mathf.Cos(angle), Mathf.Abs(velocity * Mathf.Sin(angle)));
                    break;
            }

            _time_of_last_shot = Time.time;
            _attack_count--;
        }
    }
}
