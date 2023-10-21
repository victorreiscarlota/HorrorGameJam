using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : PlayerModule
{
    public AudioSource footstepAudioSource;
    public float minSpeedForFootsteps = 1.0f;
    public float minFootstepInterval = 0.4f;
    public float maxFootstepInterval = 0.5f;
    private bool isPlayingFootsteps = false;

    public override void Tick()
    {
        base.Tick();
        float playerSpeed = ThisPlayer.playerMovement.currentSpeed;

        if (playerSpeed >= minSpeedForFootsteps && playerSpeed <= ThisPlayer.playerMovement.MaxSpeed)
        {
            // Calcule a duração do intervalo com base na velocidade.
            float interval = Mathf.Lerp(1.0f, 0.1f, (playerSpeed - minSpeedForFootsteps) / (ThisPlayer.playerMovement.MaxSpeed - minSpeedForFootsteps));
            interval = Mathf.Clamp(interval, minFootstepInterval, maxFootstepInterval);
            if (!isPlayingFootsteps) StartCoroutine(PlayFootsteps(interval));
        }
    }

    IEnumerator PlayFootsteps(float interval)
    {
        isPlayingFootsteps = true;
        footstepAudioSource.Play();
        yield return new WaitForSeconds(interval);
        isPlayingFootsteps = false;
    }
}