using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private LevelsList _levelList;
    [SerializeField] private bool _InitialLevel;

    private WinnerDecider _winnderDecider;
    private IntegrationMetric _integrationMetric = new IntegrationMetric();
    private float _timePassed;

    public int Counter { get; private set; }

    private void Awake()
    {
        _winnderDecider = FindObjectOfType<WinnerDecider>();

        Counter = SaveSystem.LoadLevelsProgression();
    }

    private void Start()
    {
        _timePassed = Time.time;

        if (_InitialLevel == false)
            _integrationMetric.OnLevelStart(Counter);
    }

    private void OnEnable()
    {
        if (_winnderDecider != null)
            _winnderDecider.Victory += OnLevelCompleted;
    }

    private void OnDisable()
    {
        if (_winnderDecider != null)
            _winnderDecider.Victory -= OnLevelCompleted;
    }

    public void LoadNextLevel()
    {
        if (Counter > _levelList.SceneCount)
            _levelList.GetRandomScene(Counter).LoadSceneAsync();
        else
            _levelList.GetScene(Counter).LoadSceneAsync();
    }

    public void RestartLevel()
    {
        _integrationMetric.OnRestartLevel(Counter);

        var scene = _levelList.GetCurrentScene();

        Addressables.LoadSceneAsync(scene);
    }

    public void OnLevelCompleted()
    { 
        _integrationMetric.OnLevelComplete(GetTime(), Counter);

        Counter++;

        SaveSystem.SaveLevelsProgression(Counter);
    }

    public void OnLevelFailed()
    {
        _integrationMetric.OnLevelFail(GetTime(), Counter);
    }

    private int GetTime()
    {
        return (int)(Time.time - _timePassed);
    }
}
