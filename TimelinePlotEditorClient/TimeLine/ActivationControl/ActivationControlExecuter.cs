using System;
using UnityEngine.Playables;

public class ActivationControlExecuter : BehaviourExecuterBase
{
    ActivationControlPlayable acBehaviour;
    private bool originalActivation;

    public override void OnPlayableCreate(Playable playable)
    {
        acBehaviour = behaviour as ActivationControlPlayable;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        var roleObj = World.Instance.GetRoleObj(acBehaviour.role);
        originalActivation = roleObj.gameObject.activeSelf;
        if (acBehaviour.type== ActivateType.Activate)
        {
            roleObj.gameObject.SetActive(true);
        }
        else
        {
            roleObj.gameObject.SetActive(false);
        }
    }

    public override void OnBehaviourDone(Playable playable)
    {
        var roleObj = World.Instance.GetRoleObj(acBehaviour.role);
        switch (acBehaviour.playbackState)
        {
            case ActivationControlTrack.PostPlaybackState.Active:
                roleObj.gameObject.SetActive(true);
                break;
            case ActivationControlTrack.PostPlaybackState.Inactive:
                roleObj.gameObject.SetActive(false);
                break;
            case ActivationControlTrack.PostPlaybackState.LeaveAsIs:
                break;
            case ActivationControlTrack.PostPlaybackState.Revert:
                roleObj.gameObject.SetActive(originalActivation);
                break;
        }
    }
}

