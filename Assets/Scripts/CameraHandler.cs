using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float _yOffset;
    [SerializeField] private float _downOffset;
    [SerializeField] private float _duration;
    [SerializeField] private float _sitDownDuration;
    [SerializeField] private float _standUpDuration;
    [SerializeField] private Transform _targetPoint;

    private Tweener _tween;
    private bool _isWalking;
    private bool _isDisabled;
    private Coroutine _coroutine;

    public void Disable()
    {
        _isDisabled = true;

        if (_tween != null)
            _tween.Kill();
    }

    public void BeginShake()
    {
        if (_isDisabled)
            return;

        if (_tween != null)
            _tween.Kill();

        _tween = transform.DOLocalMoveY(_yOffset, _duration).SetLoops(-1, LoopType.Yoyo);

        _isWalking = true;
    }

    public void EndShake()
    {
        if (_isDisabled)
            return;

        if (_tween != null)
            _tween.Kill();

        _tween = transform.DOLocalMoveY(0, _duration);

        _isWalking = false;
    }

    public void SitDown()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(SittingDown());
    }

    public void LookAt(Transform lookAtPoint, float lookDuration, Action onAniomationEnd = null)
    {
        if (_tween != null)
            _tween.Kill();

        Transform targetPointTransform = _targetPoint;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLookAt(lookAtPoint.position, _duration));
        sequence.AppendInterval(lookDuration);
        sequence.Append(transform.DOLookAt(targetPointTransform.position, _duration));

        if (onAniomationEnd != null)
            StartCoroutine(Delay(onAniomationEnd));
    }

    private IEnumerator Delay(Action onAniomationEnd)
    {
        yield return new WaitForSeconds(_duration);

        onAniomationEnd();

        if (_isWalking)
            BeginShake();
    }

    private IEnumerator SittingDown()
    {
        if (_tween != null)
            _tween.Kill();

        _tween = transform.DOLocalMoveY(_downOffset, _sitDownDuration);

        yield return new WaitForSeconds(_sitDownDuration);

        if (_tween != null)
            _tween.Kill();

        _tween = transform.DOLocalMoveY(0, _standUpDuration);

        yield return new WaitForSeconds(_standUpDuration);

        if (_isWalking)
        {
            BeginShake();
        }
    }
}
