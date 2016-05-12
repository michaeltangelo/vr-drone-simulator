using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
    [RequireComponent(typeof (AeroplaneController))]
    public class AeroplaneUserControl4Axis : MonoBehaviour
    {
        // these max angles are only used on mobile, due to the way pitch and roll input are handled
        public float maxRollAngle = 80;
        public float maxPitchAngle = 80;
<<<<<<< HEAD
        public bool controllerEnabled = false;
        private bool pilotMode = true;

        public string getThrottleClamped()
        {
            return m_Throttle.ToString();
        }
=======
>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768

        // reference to the aeroplane that we're controlling
        private AeroplaneController m_Aeroplane;
        private float m_Throttle;
        private bool m_AirBrakes;
        private float m_Yaw;
<<<<<<< HEAD
        private bool m_ToggleOperatorMode;

        public void setPilotMode(bool boolean)
        {
            pilotMode = boolean;
        }

        public bool getPilotMode()
        {
            return pilotMode;
        }

        public void togglePilotMode()
        {
            pilotMode = !pilotMode;
        }
=======

>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768

        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Aeroplane = GetComponent<AeroplaneController>();
        }


        private void FixedUpdate()
        {
<<<<<<< HEAD
            float roll;
            float pitch;
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            if (!controllerEnabled)
            {
                roll = CrossPlatformInputManager.GetAxis("Horizontal");
                pitch = CrossPlatformInputManager.GetAxis("Vertical");
                m_AirBrakes = CrossPlatformInputManager.GetButton("Fire1");
                m_Yaw = CrossPlatformInputManager.GetAxis("Horizontal");
                m_Throttle = CrossPlatformInputManager.GetAxis("Throttle_Vertical");
            }
            else
            {
                // ps4 controller map https://www.reddit.com/r/Unity3D/comments/1syswe/ps4_controller_map_for_unit/y
                roll = CrossPlatformInputManager.GetAxis("PS3ControllerRightX");
                pitch = CrossPlatformInputManager.GetAxis("PS3ControllerRightY");
                m_AirBrakes = CrossPlatformInputManager.GetButton("Fire1"); // Joystick 4 aka L1 on PS4 Controller
                m_Yaw = CrossPlatformInputManager.GetAxis("PS3ControllerRightX");
                m_Throttle = CrossPlatformInputManager.GetAxis("PS3ControllerThrottleY");
                m_ToggleOperatorMode = CrossPlatformInputManager.GetButtonDown("PS3ControllerOButton");
            }
#if MOBILE_INPUT
        AdjustInputForMobileControls(ref roll, ref pitch, ref m_Throttle);
#endif

            if (m_ToggleOperatorMode) togglePilotMode();

            // Pass the input to the aeroplane
            // if pilot mode disabled, send no signals to plane except throttle and turn camera instead
            if (pilotMode)
            {
                m_Aeroplane.Move(roll, pitch, m_Yaw, m_Throttle, m_AirBrakes);
            }
            else
            {
                m_Aeroplane.Move(0, 0, 0, m_Throttle, m_AirBrakes);
            }
=======
            // Read input for the pitch, yaw, roll and throttle of the aeroplane.
            float roll = CrossPlatformInputManager.GetAxis("Horizontal");
            float pitch = CrossPlatformInputManager.GetAxis("Vertical");
            m_AirBrakes = CrossPlatformInputManager.GetButton("Fire1");
            m_Yaw = CrossPlatformInputManager.GetAxis("Horizontal");
            m_Throttle = CrossPlatformInputManager.GetAxis("Throttle_Vertical");
#if MOBILE_INPUT
        AdjustInputForMobileControls(ref roll, ref pitch, ref m_Throttle);
#endif
            // Pass the input to the aeroplane
            m_Aeroplane.Move(roll, pitch, m_Yaw, m_Throttle, m_AirBrakes);
>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768
        }


        private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
        {
            // because mobile tilt is used for roll and pitch, we help out by
            // assuming that a centered level device means the user
            // wants to fly straight and level!

            // this means on mobile, the input represents the *desired* roll angle of the aeroplane,
            // and the roll input is calculated to achieve that.
            // whereas on non-mobile, the input directly controls the roll of the aeroplane.

            float intendedRollAngle = roll*maxRollAngle*Mathf.Deg2Rad;
            float intendedPitchAngle = pitch*maxPitchAngle*Mathf.Deg2Rad;
            roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
            pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);
        }
    }
}
