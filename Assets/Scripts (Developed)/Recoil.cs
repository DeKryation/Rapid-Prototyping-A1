using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class Recoil : MonoBehaviour
    {
        public FirstPersonController fpsController;
        public float upRecoil;
        public float sideRecoil;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                fpsController.m_MouseLook.AddRecoil(upRecoil, sideRecoil);
            }
        }
    }
}
