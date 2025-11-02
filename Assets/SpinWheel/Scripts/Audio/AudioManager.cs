using SpinWheel.Scripts.Audio;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpineWheel.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private AudioPlayer _audioPlayer;

        private void Awake()
        {
            _audioPlayer = GetComponent<AudioPlayer>();
        }

        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnWheelSpin>(OnSpin);
            EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);
            EventBroker.Instance.AddEventListener<ZoneSpinRequested>(OnZoneReach);
            EventBroker.Instance.AddEventListener<OnGiveReward>(OnReward);
            EventBroker.Instance.AddEventListener<OnPlayerDies>(OnGameEnd);
            EventBroker.Instance.AddEventListener<OnSpinSkipRequested>(OnSpinSkip);
            EventBroker.Instance.AddEventListener<OnReviveRequested>(OnRevive);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGiveUp);
        }

        private void OnGiveUp(OnGameEnds e)
        {
            _audioPlayer.Stop(AudioKey.PlayerDie);
        }

        private void OnRevive(OnReviveRequested e)
        {
            _audioPlayer.Stop(AudioKey.PlayerDie);
        }

        private void OnSpinSkip(OnSpinSkipRequested e)
        {
            _audioPlayer.Stop(AudioKey.WheelSpin);
        }

        private void OnReward(OnGiveReward e)
        {
            _audioPlayer.Play(AudioKey.Card);
        }

        private void OnGameEnd(OnPlayerDies e)
        {
            _audioPlayer.Play(AudioKey.PlayerDie);
        }

        private void OnZoneReach(ZoneSpinRequested e)
        {
            _audioPlayer.Play(AudioKey.ZoneReach);
        }

        private void OnStop(OnWheelStop e)
        {
            _audioPlayer.Stop(AudioKey.WheelSpin);
        }

        private void OnSpin(OnWheelSpin e)
        {
            _audioPlayer.Play(AudioKey.WheelSpin);
        }
    }
}