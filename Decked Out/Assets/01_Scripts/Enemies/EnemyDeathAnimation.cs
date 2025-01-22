using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimation : MonoBehaviour
{
    [SerializeField] string _deathAnmationStateName;

    Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public float PlayDeathAnimation()
    {
        _animator.Play(_deathAnmationStateName);
        return _animator.GetCurrentAnimatorStateInfo(0).length + 0.1f;
    }
}
