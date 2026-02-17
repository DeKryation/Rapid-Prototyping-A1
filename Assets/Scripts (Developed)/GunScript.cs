using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class GunScript : MonoBehaviour
    {
        public float damage = 10f;
        public float range = 100f;
        public float fireRate = 10f;

        public AudioSource shootse;

        public Text ammoDisplay;

        public FirstPersonController fpsController;
        public float upRecoil;
        public float sideRecoil;

        public int maxRounds = 12;
        private int currentRounds;
        public float reloadTime = 1f;
        private bool isReloading = false;

        public Camera fpsCam;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;

        private float nextTimeToFire = 0f;

        public Animator animator;
        public float knockbackForce = 5f;


        void Start()
        {
            if (currentRounds == -1)
                currentRounds = maxRounds;

            shootse = GetComponent<AudioSource>();

        }

        void OnEnable()
        {
            isReloading = false;
            animator.SetBool("Reloading", false);
        }

        void Update()
        {
            ammoDisplay.text = currentRounds.ToString();
            if (isReloading)
                return;

            if (currentRounds <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                fpsController.m_MouseLook.AddRecoil(upRecoil, sideRecoil);
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                shootse.Play();
            }
        }
        IEnumerator Reload()
        {
            isReloading = true;
            Debug.Log("Reloading");
            animator.SetBool("Reloading", true);

            yield return new WaitForSeconds(reloadTime - .30f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(.30f);

            currentRounds = maxRounds;
            isReloading = false;
        }

        void Shoot()
        {
            muzzleFlash.Play();
            currentRounds--;

            int headLayer = LayerMask.GetMask("ZombieHeadLayer");
            int defaultLayer = LayerMask.GetMask("Default");

            RaycastHit headHit;
            RaycastHit bodyHit;

            bool hitHead = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out headHit, range, headLayer);
            bool hitBody = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out bodyHit, range, defaultLayer);

            if (hitHead)
            {
                Debug.Log("HEADSHOT!");
                AIScript zombieHead = headHit.transform.GetComponentInParent<AIScript>();
                AIExploder exploderHead = headHit.transform.GetComponentInParent<AIExploder>();
                if (zombieHead != null) zombieHead.TakeDamage(9999f);
                else if (exploderHead != null) exploderHead.TakeDamage(9999f);

                
            }
            else if (hitBody)
            {
                AIScript target = bodyHit.transform.GetComponent<AIScript>() ?? bodyHit.transform.GetComponentInParent<AIScript>();
                AIExploder exploderTarget = bodyHit.transform.GetComponent<AIExploder>() ?? bodyHit.transform.GetComponentInParent<AIExploder>();

                if (target != null)
                {
                    target.TakeDamage(damage);
                    ApplyKnockback(target.transform, fpsCam.transform.forward);
                }
                else if (exploderTarget != null)
                {
                    exploderTarget.TakeDamage(damage);
                    ApplyKnockback(exploderTarget.transform, fpsCam.transform.forward);
                }

               
            }
        }




        void ApplyKnockback(Transform zombie, Vector3 shotDirection)
        {
            NavMeshAgentKnockback knockback = zombie.GetComponent<NavMeshAgentKnockback>();
            if (knockback == null)
                knockback = zombie.gameObject.AddComponent<NavMeshAgentKnockback>();

            knockback.ApplyKnockback(shotDirection, knockbackForce);
        }
    }
}
