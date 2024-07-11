using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Look : MonoBehaviour
    {
        public Transform Player;
        public Transform cam;

        public static bool cursorlocked = true;

        public float xSensetivity;
        public float ySensetivity;
        public float maxAngle;

        private Quaternion camCentre;

        void Start()
        {
            camCentre = cam.localRotation; //set roation to center 
        }

        void Update()
        {
            SetCamY();
            SetCamX();
            UpdateCursorLock();
        }


        void SetCamY()
        {
            float trans_input = Input.GetAxis("Mouse Y") * ySensetivity * Time.deltaTime;
            Quaternion trans_adjust = Quaternion.AngleAxis(trans_input, -Vector3.right);
            Quaternion trans_delta = cam.localRotation * trans_adjust;

            if (Quaternion.Angle(camCentre, trans_delta) < maxAngle)
            {
                cam.localRotation = trans_delta;
            }
        }
        void SetCamX()
        {
            float trans_input = Input.GetAxis("Mouse X") * xSensetivity * Time.deltaTime;
            Quaternion trans_adjust = Quaternion.AngleAxis(trans_input, Vector3.up);
            Quaternion trans_delta = Player.localRotation * trans_adjust;
            Player.localRotation = trans_delta;
        }

        void UpdateCursorLock()
        {
            if (cursorlocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (Input.GetButtonDown("Cancel"))
                {
                    cursorlocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                
                if (Input.GetButtonDown("Cancel"))
                {
                    cursorlocked = true;
                }
            }
        }
    }