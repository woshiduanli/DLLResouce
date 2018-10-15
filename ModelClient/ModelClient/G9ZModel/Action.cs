//#define test
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
    //Action����ģʽ
    public enum ActionWrapMode
    {
        ONCE = 1,               //���β��ţ�����������ActionEvent��ֹͣ
        EXT_END = 2,            //�ȴ��ⲿֹ֪ͨͣ
    }

    //Action�¼�����
    public enum ActionEventType
    {
        ANIMATION = 0,//���Ŷ���
        EFFECT,//������Ч
        FIREBALL, //����һ����������Ч
        AUDIO,//��������
        SCREEN_BLACK,//����ѹ��Ч��
        CAMERA_SHACK,  //��������
        RADIALBLUR,//����ֱ��ģ��Ч��
        DEAD,//����Ч��
        SLOW,//������
        HEATDISTORT,//Ť��
        TRANSFORM,//λ��
        HIDING,// ���أ��������ڲ���ʾģ�͵�Ч��
        XPBODYCHANGE,//xp�����¼�
    }

    public enum EventPart
    {
        SING = 0,   //�����׶�
        FIRE,       //ʩ�Ž׶�
        HIT,        //���н׶�
        FIREBALLHIT,//�켣��Ч����
        BUFF,       //BUFF
    }

    public enum PathType
    {
        Linear = 0,
        CatmullRom = 1,
        LeftCatmullRom =2,
        RightCatmullRom =3
    }

    public class Action : ScriptableObject
    {
        [CommonAttribute("Action��")]
        public string ActionName;

        [CommonAttribute("Actionʱ��")]
        public float LifeTime = 5.0f;

        [CommonAttribute("Action�¼�")]
        public ActionEvent[] ActionEvents;

        [System.Serializable()]
        public class ActionEvent
        {
            [EnumPopup("�¼�����", new string[] {"���Ŷ���", "������Ч", "���Ż���", "��������", "ѹ��", "����", "ֱ��ģ��", "����", "������", "Ť��", "λ��", "����", "xp����" })]
            public ActionEventType EventType = ActionEventType.ANIMATION;

            [CommonAttribute("�¼���")]
            public string ActionEventName;

            [CommonAttribute("����ʱ��")]
            public float StartTime;

            [CommonAttribute("����ʱ��"), Tooltip("����ڵ�ǰ�¼��Ŀ�ʼʱ��")]
            public float EndTime;

            [EnumPopup("�¼��׶�", new string[] { "����", "����", "����", "��������", "BUFF" })]
            public EventPart EventPart = EventPart.FIRE;

            [ConditionalHideAttribute("������", "EventType", true, (int)ActionEventType.ANIMATION), Tooltip("�붯���������ҹ�")]
            public MotionState State = MotionState.stand;

            [ConditionalHideAttribute("�������ɴ��ʱ��", "EventType", true, (int)ActionEventType.ANIMATION)]
            public float RigorTime;

            [ConditionalHideAttribute("�Ƿ�����ƶ�", "EventType", true, (int)ActionEventType.ANIMATION)]
            public bool CanMove;

            [ConditionalHideAttribute("�ƶ�����", "EventType", true, (int)ActionEventType.ANIMATION)]
            public List<MoveData> MoveDatas;

            [AssetToFilePath("��Ч·��", ".audio", "EventType", true, (int)ActionEventType.EFFECT , (int)ActionEventType.FIREBALL , (int)ActionEventType.AUDIO)]
            public string AudioPath = string.Empty;

            [AssetToFilePath("��Դ·��", ".go", "EventType", true, (int)ActionEventType.EFFECT , (int)ActionEventType.FIREBALL ,
                                                             (int)ActionEventType.CAMERA_SHACK , (int)ActionEventType.SCREEN_BLACK)]
            public string SourcePath = string.Empty;

            [AssetToFilePath("������Դ·��", ".go", "EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                 (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public string LowSourcePath = string.Empty;

            [ConditionalHideAttribute("���Ʋ���", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK), 
                                                                           Tooltip("��ֹ����¼���ÿ�����������֣���ʱ�ɳ������")]
            public bool IsControlWave = false;

            [ConditionalHideAttribute("��Ŀ���������", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                                (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK), 
                                                                                Tooltip("��Ч�����꽫����Ŀ������������")]
            public bool IsActOnTarget = false;

            [ConditionalHideAttribute( "�󶨹�����", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                             (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public string BindBone = string.Empty;

            [ConditionalHideAttribute("����", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Vector3 Offset;

            [ConditionalHideAttribute("������ƫ������(ֵΪ0-1)", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public bool IsRatioOffset;

            [ConditionalHideAttribute("ƫ�ƾ���", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Distance;

            [ConditionalHideAttribute("��ת", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Vector3 Angle;

            [ConditionalHideAttribute( "�ƶ��ٶ�", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Speed = 1;

            [ConditionalHideAttribute("��֡ǿ��", "SourcePath|EventType", true, (int)ActionEventType.SLOW)]
            public float SlowPower = 0.2f;

            [ConditionalHideAttribute("�������", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Duration;

            [ConditionalHideAttribute("�ٻ�ID", "EventType", true, (int)ActionEventType.XPBODYCHANGE)]
            public int CallApprid;

            [EnumPopup("�켣����", new string[] { "ֱ��", "������", "������", "������" }), ConditionalHideAttribute("�켣����", "EventType", true, (int)ActionEventType.FIREBALL)]
            public PathType PathType = PathType.Linear;

            [ConditionalHideAttribute("������ߵ�ƫ��", "PathType", true, (int)PathType.CatmullRom, (int)PathType.LeftCatmullRom, (int)PathType.RightCatmullRom)]
            public float OffsetMaxY = 1;

            [ConditionalHideAttribute("Child", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                           (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Childs[] Child;
        }

        [System.Serializable()]
        public class Childs
        {
            [CommonAttribute("��ת"), Tooltip("������¼���Դ����ת��������תʱ�������¼��ı������ת")]
            public Vector3 Angle;
            [CommonAttribute("ƫ��"), Tooltip("������¼���Դ��ƫ�ƣ�������תʱ�������¼��ı��������")]
            public Vector3 Offset;
            [CommonAttribute("�������"), Tooltip("������¼���Դ�ļ���ʱ�䣬������תʱ�������¼��ı���ļ���ʱ��")]
            public float Duration;
            [EnumPopup("�켣����", new string[] { "ֱ��", "������", "������", "������" }), ConditionalHideAttribute("�켣����", "EventType", true, (int)ActionEventType.FIREBALL)]
            public PathType PathType = PathType.Linear;
            [ConditionalHideAttribute("������ߵ�ƫ��", "PathType", true, (int)PathType.CatmullRom, (int)PathType.LeftCatmullRom,(int)PathType.RightCatmullRom)]
            public float OffsetMaxY = 1;
        }

        [System.Serializable()]
        public class MoveData
        {
            [CommonAttribute("��ʼʱ��")]
            public float StartTime;
            [CommonAttribute("����")]
            public float Distance;
            [CommonAttribute("����ʱ��")]
            public float EndTime;
        }
    }
}

