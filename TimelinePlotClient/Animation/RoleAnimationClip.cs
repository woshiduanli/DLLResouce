using UnityEngine;
using UnityEngine.Playables;

public class RoleAnimationClip : PlayableAsset
{

    [Header("动作名称")]
    public string AnimationName;
    [HideInInspector]
    public RoleData Role;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimationPlayable>.Create(graph);
        AnimationPlayable animationPlayable = playable.GetBehaviour();
        if (animationPlayable == null)
            return playable;
        animationPlayable.AnimationName = AnimationName;
        animationPlayable.Role = Role;
        animationPlayable.executer = BehaviourExecuterFactory.GetAnimationExecuter(animationPlayable);
        if (animationPlayable.executer == null)
            return playable;
        animationPlayable.executer.OnPlayableCreate(playable);
        return playable;
    }

}




public class AnimationPlayable : MYPlayableBehaviour
{
    public RoleData Role;
    public string AnimationName;

}
