using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Attack")]
    [SerializeField]
    private float _chaseRange = 5f;
    [SerializeField]
    private float _attackRange = 1f;
    [SerializeField]
    private float _attackDelay = 2f;
    [SerializeField]
    private bool _isAttacking = false;

    [Header("Movement")]
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private bool _isStopped = false;

    [Header("Health")]
    [SerializeField]
    private int _maxHealth = 5;
    private int _curHealth = 0;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _curHealth = _maxHealth;
    }

    void Update()
    {
        if (Player.Instance == null) return;

        UpdateMovement();
        UpdateAttack();

        if (_curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void UpdateMovement()
    {
        _agent.SetDestination(Player.Instance.transform.position);
        float distance = Vector3.Distance(Player.Instance.transform.position, this.transform.position);
        _isStopped = !(distance <= _chaseRange && distance >= _attackRange);
        _agent.speed = _isStopped ? 0 : _speed;
        _animator.SetBool("WalkForward", !_isStopped);
        _animator.SetBool("Idle", _isStopped);
    }

    private float attackCooldown = 0;
    void UpdateAttack()
    {
        attackCooldown -= Time.deltaTime;

        _isAttacking = (Vector3.Distance(Player.Instance.transform.position, this.transform.position) <= _attackRange);

        if (_isAttacking && attackCooldown <= 0)
        {
            _animator.SetTrigger("Attack1");
            attackCooldown = _attackDelay;
            Player.Instance.OnDamage?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            _curHealth--;
        }
    }

}
