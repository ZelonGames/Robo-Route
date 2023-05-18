using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private Squash squash;
    [SerializeField] private ParticleSystem watersplashParticles;

    private void Start()
    {
        squash = GetComponent<Squash>();

        if (!GameHelper.IsUsingMapEditor())
        {
            Destroy(GetComponent<LevelComponentRemover>());
            Destroy(GetComponent<ItemMover>());
        }

        if (GetComponent<SpriteRenderer>().flipX)
        {
            Vector2 waterPos = watersplashParticles.gameObject.transform.localPosition;
            watersplashParticles.gameObject.transform.localPosition = new Vector2(-waterPos.x, waterPos.y);

            Quaternion waterRotation = watersplashParticles.gameObject.transform.localRotation;
            watersplashParticles.gameObject.transform.localRotation = 
                new Quaternion(waterRotation.x, waterRotation.y * -1, waterRotation.z, waterRotation.w);
        }

        GetComponent<EnteredGoalDetector>().GoalEnteredSelf += OnPlaySquash;
    }

    private void OnPlaySquash()
    {
        squash.Play();
    }
}
