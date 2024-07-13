using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimator : MonoBehaviour
{

    private readonly int HASH_UP = Animator.StringToHash("Up");
    private readonly int HASH_DOWN = Animator.StringToHash("Down");
    private Animator _animator;

    private void Awake()
    {
        
        _animator = GetComponent<Animator>();

    }

    public void SetAnimation(bool isUp)
    {

        if (isUp)
        {

            _animator.SetTrigger(HASH_UP);

        }
        else
        {

            _animator.SetTrigger(HASH_DOWN);

        }

    }

}
