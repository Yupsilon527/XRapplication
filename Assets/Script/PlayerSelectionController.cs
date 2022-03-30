using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionController : MonoBehaviour
{
    public bool RightHand = false;
    private void Update()
    {
        HandleParticles();
    }
    void HandleParticles()
    {
        ParticleSystem particle = RightHand ? PlayerSelectionBoundaries.main.rightFingerParticle : PlayerSelectionBoundaries.main.leftFingerParticle;

        if (transform.position.y < PlayerSelectionBoundaries.main.transform.position.y)
        {
            particle.transform.localPosition = new Vector3(particle.transform.position.x, 0, particle.transform.position.z);

            if (!particle.isEmitting)
            {
                particle.Play();
            }
        }
        else
        {
            if (particle.isEmitting)
            {
                particle.Stop();
            }
        }
    }
}
