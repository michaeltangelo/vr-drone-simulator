using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
    public class AeroplanePropellerAnimator : MonoBehaviour
    {
        [SerializeField] private Transform m_PropellorModel;                          // The model of the the aeroplane's propellor.
        [SerializeField] private Transform m_PropellorBlur;                           // The plane used for the blurred propellor textures.
        [SerializeField] private Texture2D[] m_PropellorBlurTextures;                 // An array of increasingly blurred propellor textures.
        [SerializeField] [Range(0f, 1f)] private float m_ThrottleBlurStart = 0.25f;   // The point at which the blurred textures start.
        [SerializeField] [Range(0f, 1f)] private float m_ThrottleBlurEnd = 0.5f;      // The point at which the blurred textures stop changing.
        [SerializeField] private float m_MaxRpm = 2000;                               // The maximum speed the propellor can turn at.

        private AeroplaneController m_Plane;      // Reference to the aeroplane controller.
        private int m_PropellorBlurState = -1;    // To store the state of the blurred textures.
        private const float k_RpmToDps = 60f;     // For converting from revs per minute to degrees per second.
        private Renderer m_PropellorModelRenderer;
        private Renderer m_PropellorBlurRenderer;
<<<<<<< HEAD
        private GameObject Propeller;
=======

>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768

        private void Awake()
        {
            // Set up the reference to the aeroplane controller.
            m_Plane = GetComponent<AeroplaneController>();
<<<<<<< HEAD
            Propeller = GameObject.Find("propeller");
            //m_PropellorModelRenderer = m_PropellorModel.GetComponent<Renderer>();
            m_PropellorModelRenderer = Propeller.GetComponent<Renderer>();
=======

            m_PropellorModelRenderer = m_PropellorModel.GetComponent<Renderer>();
>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768
            m_PropellorBlurRenderer = m_PropellorBlur.GetComponent<Renderer>();

            // Set the propellor blur gameobject's parent to be the propellor.
            m_PropellorBlur.parent = m_PropellorModel;
        }


        private void Update()
        {
            // Rotate the propellor model at a rate proportional to the throttle.
<<<<<<< HEAD
            m_PropellorModel.Rotate(0, 0, m_MaxRpm * m_Plane.Throttle * Time.deltaTime * k_RpmToDps);
=======
            m_PropellorModel.Rotate(0, m_MaxRpm*m_Plane.Throttle*Time.deltaTime*k_RpmToDps, 0);
>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768

            // Create an integer for the new state of the blur textures.
            var newBlurState = 0;

            // choose between the blurred textures, if the throttle is high enough
            if (m_Plane.Throttle > m_ThrottleBlurStart)
            {
                var throttleBlurProportion = Mathf.InverseLerp(m_ThrottleBlurStart, m_ThrottleBlurEnd, m_Plane.Throttle);
                newBlurState = Mathf.FloorToInt(throttleBlurProportion*(m_PropellorBlurTextures.Length - 1));
            }

            // If the blur state has changed
            if (newBlurState != m_PropellorBlurState)
            {
                m_PropellorBlurState = newBlurState;

                if (m_PropellorBlurState == 0)
                {
                    // switch to using the 'real' propellor model
                    m_PropellorModelRenderer.enabled = true;
                    m_PropellorBlurRenderer.enabled = false;
                }
                else
                {
                    // Otherwise turn off the propellor model and turn on the blur.
<<<<<<< HEAD
                    m_PropellorModelRenderer.enabled = true;
=======
                    m_PropellorModelRenderer.enabled = false;
>>>>>>> c829e0fbbc70cd9a7f46cc9d20fa50fb19db1768
                    m_PropellorBlurRenderer.enabled = true;

                    // set the appropriate texture from the blur array
                    m_PropellorBlurRenderer.material.mainTexture = m_PropellorBlurTextures[m_PropellorBlurState];
                }
            }
        }
    }
}
