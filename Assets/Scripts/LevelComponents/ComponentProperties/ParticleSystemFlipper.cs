using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(SpriteFlipper))]
public class ParticleSystemFlipper : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    
    private SpriteFlipper spriteFlipper;

    public void Start()
    {
        spriteFlipper = gameObject.GetComponent<SpriteFlipper>();

        Flip();
    }

    public void OnValidate()
    {
        Flip();
    }

    private void Flip()
    {
        if (particleSystem != null && spriteFlipper != null)
        {
            if (spriteFlipper.isFlipped)
            {

            }
            particleSystem.transform.localPosition = new Vector2(
                particleSystem.transform.localPosition.x * (spriteFlipper.isFlipped ? -1 : 1),
                particleSystem.transform.localPosition.y);
        }
    }
}
