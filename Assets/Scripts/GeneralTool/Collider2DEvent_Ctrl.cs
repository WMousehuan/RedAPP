using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Collider2DEvent_Ctrl : MonoBehaviour
{
    public bool isActive = true;

    public List<Collider2D> colliders;
    public HashSet<Collider2D> entered = new HashSet<Collider2D>();
    public UnityEvent<Collider2D> triggerEnterEvent;
    public UnityEvent<Collision2D> colliderEnterEvent;
    public UnityEvent<Collider2D> triggerExitEvent;
    public UnityEvent<Collision2D> colliderExitEvent;

    public virtual void TriggerEnterEvent(bool isReal)
    {
        for (int i = colliders.Count - 1; i >= 0; i--)
        {
            if (i < colliders.Count)
            {
                Collider2D collider2D = colliders[i];
                if (isReal || !entered.Contains(collider2D))
                {
                    TriggerEnterEvent(collider2D);
                }
            }
        }
    }
    /// <summary>
    /// 排列远近后触发
    /// </summary>
    /// <param name="isReal"></param>
    public virtual void TriggerClosestEnterEvent(bool isReal,GeneralTool_Ctrl.ReturnBoolDelegate breakJudge=null)
    {
        for (int j = 0; j < colliders.Count; j++)
        {
            for (int i = 0; i < colliders.Count - 1; i++)
            {
                if ((colliders[i].transform.position-transform.position).magnitude < (colliders[i+1].transform.position - transform.position).magnitude)
                {
                    Collider2D temp = colliders[i];
                    colliders[i] = colliders[i + 1];
                    colliders[i + 1] = temp;
                }
            }
        }

        for (int i = colliders.Count - 1; i >= 0; i--)
        {
            if (i < colliders.Count)
            {
                Collider2D collider2D = colliders[i];
                if (isReal || !entered.Contains(collider2D))
                {
                    TriggerEnterEvent(collider2D);
                }
                if (breakJudge?.Invoke() ?? false)
                {
                    break;
                }
            }
        }
    }
    public virtual void TriggerEnterEvent(Collider2D collider2D)
    {
        triggerEnterEvent?.Invoke(collider2D);
        entered.Add(collider2D);
    }
    public virtual void TriggerExitEvent(Collider2D collider2D)
    {
        triggerExitEvent?.Invoke(collider2D);
        entered.Remove(collider2D);
    }
    public virtual void ColliderEnterEvent(Collision2D collistion2D)
    {
        colliderEnterEvent?.Invoke(collistion2D);
        entered.Add(collistion2D.collider);
    }
    public virtual void ColliderExitEvent(Collision2D collistion2D)
    {
        colliderExitEvent?.Invoke(collistion2D);
        entered.Remove(collistion2D.collider);
    }
    public void OnEnable()
    {
        colliders.Clear();
        entered.Clear();
    }
    public void OnDisable()
    {
        entered.Clear();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!colliders.Contains(collision.collider))
        {
            colliders.Add(collision.collider);
        }
        if (!isActive)
        {
            return;
        }

        if (!entered.Contains(collision.collider))
        {
            ColliderEnterEvent(collision);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Collider2D currentCollider = null;
        if (colliders.Contains(collision.collider))
        {
            currentCollider = collision.collider;
            colliders.Remove(collision.collider);
        }
        if (!isActive)
        {
            return;
        }
        if (currentCollider)
        {
            ColliderExitEvent(collision);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!colliders.Contains(collider))
        {
            colliders.Add(collider);
        }
        if (!isActive)
        {
            return;
        }
        if (!entered.Contains(collider))
        {
            TriggerEnterEvent(collider);

            if (!collider.gameObject.activeSelf)
            {
                if (colliders.Contains(collider))
                {
                    colliders.Remove(collider);
                }
                if (entered.Contains(collider))
                {
                    entered.Remove(collider);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        Collider2D currentCollider = null;
        if (colliders.Contains(collider))
        {
            currentCollider = collider;
            colliders.Remove(collider);
        }
        if (!isActive)
        {
            return;
        }
        if (currentCollider)
        {
            TriggerExitEvent(collider);
        }
    }

}
