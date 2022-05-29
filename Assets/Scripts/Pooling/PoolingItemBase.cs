using UnityEngine;

public class PoolingItemBase : MonoBehaviour
{
    protected bool _isActiveItem;

    private Pool_Base _parentPool;

    protected delegate void OnActiveStateChangedDelegate(bool _activeState);
    protected OnActiveStateChangedDelegate OnActiveStateChanged;

    /// <summary>
    /// Initializes the item with a parent pool and sets the activeState to false.
    /// </summary>
    /// <param name="_parentPool"></param>
    public void Initialize(Pool_Base _parentPool)
    {
        this._parentPool = _parentPool;
        SetActiveState(false);
    }

    /// <summary>
    /// Sets the activeState of the item
    /// </summary>
    /// <param name="_active">New activeState</param>
    public void SetActiveState(bool _active)
    {
        if (_isActiveItem != _active)
        {
            _isActiveItem = _active;
            if (OnActiveStateChanged != null)
            {
                OnActiveStateChanged(_active);
            }
        }
    }

    /// <summary>
    /// Returns the item to its parent pool.
    /// </summary>
    protected void ReturnToPool()
    {
        if (_parentPool == null) return;

        _parentPool.ReturnItem(this);
        SetActiveState(false);
    }
}
