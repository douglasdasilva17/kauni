using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformationManager : MonoBehaviour
{
    public GameObject[] ExternalAnimals;
    public Animator Animator;
    public AudioSource SummonAnimal;
    public GameObject SummonParticle;
    private bool _isPerforming;

    private void Start()
    {
        SummonParticle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _isPerforming = !_isPerforming;

            if (_isPerforming)
            {
                SummonAnimal.Play();
                SummonParticle.SetActive(true);
            }
            else
                SummonParticle.SetActive(false);
        }

        Animator.SetBool("Bonfire", _isPerforming);
    }
}
