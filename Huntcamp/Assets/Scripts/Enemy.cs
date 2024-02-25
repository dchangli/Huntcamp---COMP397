using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Attack")]
    public float ChaseRange = 5f;
    public float AttackRange = 1f;
    public float AttackDelay = 2f;
    public bool IsAttacking = false;

    [Header("Movement")]
    public float Speed = 2f;
    public bool IsStopped = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Player.Instance == null) return;

        UpdateMovement();
        UpdateAttack();
    }

    void UpdateMovement()
    {
        _agent.SetDestination(Player.Instance.transform.position);
        float distance = Vector3.Distance(Player.Instance.transform.position, this.transform.position);
        IsStopped = !(distance <= ChaseRange && distance >= AttackRange);
        _agent.speed = IsStopped ? 0 : Speed;
        _animator.SetBool("WalkForward", !IsStopped);
        _animator.SetBool("Idle", IsStopped);
    }

    private float attackCooldown = 0;
    void UpdateAttack()
    {
        attackCooldown -= Time.deltaTime;

        IsAttacking = (Vector3.Distance(Player.Instance.transform.position, this.transform.position) <= AttackRange);

        if (IsAttacking && attackCooldown <= 0)
        {
            _animator.SetTrigger("Attack1");
            attackCooldown = AttackDelay;
            Player.Instance.OnDamage?.Invoke(this);
        }
    }

}
