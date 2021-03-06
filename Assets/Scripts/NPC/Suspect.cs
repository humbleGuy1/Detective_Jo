using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform _headPoint;
    [SerializeField] private SusAnimator _susAnimator;
    [SerializeField] private int _maxHealth;
    [SerializeField] private DeathHandler _deathHandler;
    [SerializeField] private VoiceLineHandler _voiceLineHandler;

    private Health _health;
    private VoiceLines _voiceLines = new VoiceLines(new DefaultSuspectVoiceLines());

    public Health health => _health;
    public bool IsGuilty => _deathHandler.isDead;
    public Transform headPoint => _headPoint;
    public SusAnimator susAnimator => _susAnimator;

    private void Awake()
    {
        _health = new Health(_maxHealth, _deathHandler);
    }

    public void TakeDamage()
    {
        if (_deathHandler.isDead)
            return;

        _susAnimator.TriggerHit();
        health.Decrease(1);
        _voiceLineHandler.CreateVoiceLine(_voiceLines);

        FinishHandler.Instance.OnProofShown();
    }
}
