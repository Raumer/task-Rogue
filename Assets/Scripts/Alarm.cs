using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _durationAlarm;

    private float _runningTime;
    private float _targetVolume;
    private float _minVolume = 0;
    private float _maxVolume = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<RogueMovement>(out RogueMovement rogueMovement))
        {
            _targetVolume = _maxVolume;
            _audioSource.Play();
            StartCoroutine(AlarmPlay());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<RogueMovement>(out RogueMovement rogueMovement))
        {        
            _targetVolume = _minVolume;
            _runningTime = 0f;
            StartCoroutine(AlarmPlay());
        }
    }

    private IEnumerator AlarmPlay()
    {
        while (_audioSource.volume != _targetVolume)
        {
            if (_runningTime <= _durationAlarm)
            {
                _runningTime += Time.deltaTime;

                float normalizeRunningTime = _runningTime / _durationAlarm;

                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetVolume, normalizeRunningTime);
            }

            if (_audioSource.volume == 0)
            {
                _audioSource.Stop();
            }

            yield return null;
        }
    }
}
