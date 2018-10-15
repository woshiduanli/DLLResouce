using UnityEngine;
using UnityEditor;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(Action))]
public class ActionEditor : CustomEditorBase
{
    private Action Action_ { get { return this.serializedObject.targetObject as Action; } }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        this.serializedObject.Update();

        for (int i = 0; i < Action_.ActionEvents.Length; i++)
        {
            Action.ActionEvent ae = Action_.ActionEvents[i];
            if (string.IsNullOrEmpty(ae.ActionEventName))
            {
                if (ae.EventType == ActionEventType.ANIMATION)
                    ae.ActionEventName = ae.State.ToString();
                else if (!string.IsNullOrEmpty(ae.SourcePath))
                    ae.ActionEventName = Path.GetFileName(ae.SourcePath);
                else if (!string.IsNullOrEmpty(ae.AudioPath))
                    ae.ActionEventName = Path.GetFileName(ae.AudioPath);
                else
                    ae.ActionEventName = ae.EventType.ToString().ToLower();
            }
        }

        #region PlayAction

        if (GUILayout.Button("PlayAction"))
        {
            PlayAction(true);
        }
        if (GUILayout.Button("PlayLowAction"))
        {
            PlayAction(false);
        }
        #endregion

    }

    private void PlayAction(bool high)
    {
        for (int i = 0; i < Action_.ActionEvents.Length; i++)
        {
            Action.ActionEvent ae = Action_.ActionEvents[i];
            if (ae.EventType == ActionEventType.ANIMATION && ae.RigorTime == 0)
            {
                EditorUtility.DisplayDialog("错误", "请填写动作不可打断时间", "OK");
                return;
            }
        }
        if (!EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Error", "需要在运行状态下才能进行该操作", "OK");
            return;
        }
        if (!ModelLoader.self)
            return;
        ModelLoader.self.ResetTime();
        ModelLoader.self.transform.position = Vector3.zero;
        SkillContext context = new SkillContext();
        context.Wave = 1;

        for (int i = 0; i < 10; i++)
        {
            SkillContext.FireBoll fireBoll = new SkillContext.FireBoll();
            fireBoll.limit = 3;

            if (ModelLoader.targetList.Count > 0)
                fireBoll.ro = ModelLoader.targetList[Random.Range(0, ModelLoader.targetList.Count - 1)];
            context.targets.Add(fireBoll);
        }

        bool hasbuff = false;
        for (int i = 0; i < Action_.ActionEvents.Length; i++)
        {
            Action.ActionEvent ae = Action_.ActionEvents[i];
            if (ae.EventPart == EventPart.BUFF)
            {
                hasbuff = true;
                break;
            }
        }

        if (hasbuff)
        {
            ModelLoader.self.mActionPerformer.StartAction(Action_, EventPart.BUFF, high, null);
            ModelLoader.self.mActionPerformer.StartCoroutine(PlayAction(context, 2, high));
        }
        else
            ModelLoader.self.mActionPerformer.StartCoroutine(PlayAction(context, 0, high));
    }

    IEnumerator PlayAction(SkillContext context,float time,bool high)
    {
        yield return new WaitForSeconds(time);
        ModelLoader.self.mActionPerformer.StartAction(Action_, EventPart.FIRE, high, context);
        GameObject[] targets = GameObject.FindGameObjectsWithTag("target");
        for (int i = 0; i < targets.Length; i++)
        {
            RoleObject target = targets[i].GetComponent<RoleObject>();
            if (target)
                target.mActionPerformer.StartAction(Action_, EventPart.HIT, high);
        }
    }
}