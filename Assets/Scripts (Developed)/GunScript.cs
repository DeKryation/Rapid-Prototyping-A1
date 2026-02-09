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

        public int maxRounds = 30;
        private int currentRounds;
        public float reloadTime = 1f;
        private bool isReloading = false;

        public Camera fpsCam;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;

        private float nextTimeToFire = 0f;

        public Animator animator;

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

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                AIScript target = hit.transform.GetComponent<AIScript>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }



                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);

            }
        }
    }
}
