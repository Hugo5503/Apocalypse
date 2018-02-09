using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    void Damage(T damageTaken);
}

public interface IDestroyable
{
    void DestroyAble();
}

public interface IInteractable<T>
{
    GameObject Interactable(T gameObject);
}
