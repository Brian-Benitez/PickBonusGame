using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParticleBehavior : MonoBehaviour
{
    [Header("Particles")]
    public List<ParticleSystem> TwoXParticles;
    public List<ParticleSystem> FourXParticles;
    //public List<ParticleSystem> EightXParticles;

    [Header("Scripts")]
    public MultiplierChestFeature NumOfChests;

    /// <summary>
    /// Enables particles, depening on what level in the feature mult it is.
    /// </summary>
    public void ChecktiersAndSetParticles(int index)
    {
        if (NumOfChests.FeatureMultsTierList[index] == 2)
        {
            Debug.Log("Playing");
            TwoXParticles.ToList().ForEach(TwoXParticle => { TwoXParticle.gameObject.SetActive(true); TwoXParticle.Play(); });
            Debug.Log("_particle play " + NumOfChests.FeatureMultsTierList[index]);
        }
        if (NumOfChests.FeatureMultsTierList[index] == 4)
        {
            TwoXParticles.ToList().ForEach(TwoXParticle => { TwoXParticle.gameObject.SetActive(false); TwoXParticle.Pause(); });
            FourXParticles.ToList().ForEach(FourxParticle => { FourxParticle.gameObject.SetActive(true); FourxParticle.Play(); });
           Debug.Log("__particle play");
           Debug.Log("_particle play " + NumOfChests.FeatureMultsTierList[index]);
        }
        else if (NumOfChests.FeatureMultsTierList[index] == 8)
        {
            FourXParticles.ToList().ForEach(FourXParticle => { FourXParticle.gameObject.SetActive(false); FourXParticle.Pause(); });
            //EightXParticles.ToList().ForEach(y => { y.gameObject.SetActive(true); y.Play(); });
            Debug.Log("____particle play");
        }
    }

    public void DisableAllParticles()
    {
        TwoXParticles.ToList().ForEach(b => { b.Stop(); });
        Debug.Log("Stop playing particles");

        FourXParticles.ToList().ForEach(h => { h.gameObject.SetActive(false); h.Pause(); });

        ///EightXParticles.ToList().ForEach(y => { y.gameObject.SetActive(false); y.Pause(); });
    }
}