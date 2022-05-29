using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    private GameObject _target;

    private void Update()
    {
        if (_target)
        {
            Vector3 position = new Vector3(_target.transform.position.x, _target.transform.position.y, -1);
            transform.position = position;
        }
    }

    public void SetTarget(GameObject _targetToFollow)
    {
        _target = _targetToFollow;
    }

    #region CameraShake
    //The amount to shake this frame.
    private float _shakeAmount;
    //The duration this frame.
    private float _shakeDuration;
    
    //A percentage (0-1) representing the amount of shake to be applied when setting rotation.
    private float _shakePercentage;
    //The initial shake amount (to determine percentage), set when ShakeCamera is called.
    private float _startAmount;
    //The initial shake duration, set when ShakeCamera is called.
    private float _startDuration;

    //Is the coroutine running right now?
    private bool _isRunning = false; 
    //Smooth rotation?
    public bool Smooth;
    //Amount to smooth
    public float SmoothAmount = 5f;

    public void ShakeCamera(float amount, float duration)
    {
        //Add to the current amount.
        _shakeAmount += amount;
        //Reset the start amount, to determine percentage.
        _startAmount = _shakeAmount;
        //Add to the current time.
        _shakeDuration += duration;
        //Reset the start time.
        _startDuration = _shakeDuration;

        //Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
        if (!_isRunning) StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _isRunning = true;

        while (_shakeDuration > 0.01f)
        {
            //A Vector3 to add to the Local Rotation
            Vector3 rotationAmount = Random.insideUnitSphere * _shakeAmount;
            //Don't change the Z; it looks funny.
            rotationAmount.x = 0;
            rotationAmount.y = 0;

            //Used to set the amount of shake (% * startAmount).
            _shakePercentage = _shakeDuration / _startDuration;

            //Set the amount of shake (% * startAmount).
            _shakeAmount = _startAmount * _shakePercentage;
            //Lerp the time, so it is less and tapers off towards the end.
            _shakeDuration = Mathf.Lerp(_shakeDuration, 0, Time.deltaTime);
            
            if (Smooth)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * SmoothAmount);
            else
                //Set the local rotation the be the rotation amount.
                transform.localRotation = Quaternion.Euler(rotationAmount);

            yield return null;
        }
        //Set the local rotation to 0 when done, just to get rid of any fudging stuff.
        transform.localRotation = Quaternion.identity;
        _isRunning = false;
    }
    #endregion
}