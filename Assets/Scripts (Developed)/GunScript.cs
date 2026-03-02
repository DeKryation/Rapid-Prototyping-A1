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
        public float reloadTime = 3f;
        private bool isReloading = false;

        public Camera fpsCam;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;

        private float nextTimeToFire = 0f;

        public Animator animator;
        public float knockbackForce = 5f;


        void Start()
        {

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
            ammoDisplay.color = currentRounds < 4 ? new Color(0.8616352f, 0.1761203f, 0.1761203f, 1f) : Color.white;

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
            //insert reload sfx here    
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
                AITankScript tankTarget = bodyHit.transform.GetComponent<AITankScript>() ?? bodyHit.transform.GetComponentInParent<AITankScript>();
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
            //Insert knockback sfx here
            NavMeshAgentKnockback knockback = zombie.GetComponent<NavMeshAgentKnockback>();
            if (knockback == null)
                knockback = zombie.gameObject.AddComponent<NavMeshAgentKnockback>();

            knockback.ApplyKnockback(shotDirection, knockbackForce);
        }

        // New ammo count
        void SetAmmo(int newAmmoCount)
        {
            currentRounds = Mathf.Clamp(newAmmoCount, 0, maxRounds);
            if (isReloading)
            {
                StopCoroutine(Reload());
                isReloading = false;
                animator.SetBool("Reloading", false);
            }
        }

        // New reload speed upgrade.
        void UpgradeReloadSpeed(float percentageDecrease)
        {
            // Clamp percentage between 0 and 100 to prevent invalid values
            percentageDecrease = Mathf.Clamp(percentageDecrease, 0f, 100f);

            // Calculate the new reload time
            float decreaseMultiplier = 1f - (percentageDecrease / 100f);
            reloadTime = reloadTime * decreaseMultiplier;

            // Optional: Prevent reload time from going too low
            reloadTime = Mathf.Max(reloadTime, 0.3f); // Minimum 0.3 seconds

            Debug.Log("Reload time upgraded! New reload time: " + reloadTime + "s");
        }

        // New ammo damage.
        void UpgradeDamage(float newDamage)
        {
            // Set the new damage value
            damage = newDamage;

            // Prevent damage from being set to negative or zero
            damage = Mathf.Max(damage, 1f);

            Debug.Log("Damage upgraded! New damage: " + damage);
        }


        public void UpgradeDamage()
        {
            damage += 5f;
            Debug.Log("Damage Upgraded: " + damage);
        }

        public void UpgradeAmmo()
        {
            maxRounds += 3;
            currentRounds = maxRounds;
            Debug.Log("Ammo Upgraded: " + maxRounds);
        }

        public void UpgradeReload()
        {
            reloadTime *= 0.9f;
            Debug.Log("Reload Faster: " + reloadTime);
        }
    }
}