using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cinemachine.Timeline
{
    public sealed class CinemachineShotPlayable : PlayableBehaviour
    {
        public CinemachineVirtualCameraBase VirtualCamera;
        public string LookAtBoneName;
        public string FollowBoneName;
    }

    public sealed class CinemachineShot : PlayableAsset, IPropertyPreview
    {
        public ExposedReference<CinemachineVirtualCameraBase> VirtualCamera;
        public string LookAtBoneName;
        public string FollowBoneName;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CinemachineShotPlayable>.Create(graph);
            var behaviour = playable.GetBehaviour();
            if (behaviour == null)
                return playable;
            behaviour.VirtualCamera = VirtualCamera.Resolve(graph.GetResolver());
            if (behaviour.VirtualCamera == null)
                return playable;
            behaviour.LookAtBoneName = LookAtBoneName;
            behaviour.FollowBoneName = FollowBoneName;
            return playable;
        }

        // IPropertyPreview implementation
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            driver.AddFromName<Transform>("m_LocalPosition.x");
            driver.AddFromName<Transform>("m_LocalPosition.y");
            driver.AddFromName<Transform>("m_LocalPosition.z");
            driver.AddFromName<Transform>("m_LocalRotation.x");
            driver.AddFromName<Transform>("m_LocalRotation.y");
            driver.AddFromName<Transform>("m_LocalRotation.z");

            driver.AddFromName<Camera>("field of view");
            driver.AddFromName<Camera>("near clip plane");
            driver.AddFromName<Camera>("far clip plane");
        }
    }
}
