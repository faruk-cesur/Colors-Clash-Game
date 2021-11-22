using System;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        AnimationController.Instance.RunAnimation(_animator);
    }

    public void PlayerStackDeath()
    {
        AnimationController.Instance.DeathAnimation(_animator);
    }

    public void OnTriggerEnter(Collider other)
    {
        Trap trap = other.GetComponentInParent<Trap>();
        PlayerController player = GetComponentInParent<PlayerController>();

        if (trap)
        {
            player.stackGameObjectList.Remove(gameObject);
            gameObject.transform.SetParent(null);
            gameObject.GetComponent<PlayerStack>().PlayerStackDeath();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
            player.CalculateStackPositions();
        }
    }
}