using UnityEngine;
using System;
using System.Collections.Generic;

public class EventDelegate
{
    public float shotInterval = 0;
    private float nextShotTime = 0;

    public delegate void Callback(params object[] args);
    private Callback mCachedCallback;

    public void Clear()
    {
        mCachedCallback = null;
    }

	public EventDelegate ()
    { 

    }
    public EventDelegate(Callback call)
    { 
       this.mCachedCallback = call; 
    }


    public override bool Equals(object obj)
    {
        if (obj == null) 
            return false;

        if (obj is Callback)
        {
            Callback callback = obj as Callback;
            return callback.Equals(mCachedCallback);
        }

        if (obj is EventDelegate)
        {
            EventDelegate del = obj as EventDelegate;
            if (del.mCachedCallback != null)
                return del.mCachedCallback.Equals(mCachedCallback);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool Execute(params object[] args)
    {
        if (shotInterval > 0.00001F)
        {
            if (nextShotTime >= Time.realtimeSinceStartup)
                return false;

            nextShotTime = Time.realtimeSinceStartup + shotInterval;
        }
        if (mCachedCallback != null)
        {
            mCachedCallback(args);
            return true;
        }
        return false;
	}


    static public void Execute(List<EventDelegate> list, params object[] args)
	{
		if (list != null)
		{
            for (int i = 0; i < list.Count; i++) 
			{
				EventDelegate del = list[i];
				if (del != null)
                    del.Execute(args);
			}
		}
	}

	static public EventDelegate Set (List<EventDelegate> list, Callback callback, float shotInterval = 0f)
	{
		if (list != null)
		{
			EventDelegate del = new EventDelegate(callback);
            if ( shotInterval != 0f ) 
                del.shotInterval = shotInterval;
			list.Clear();
			list.Add(del);
			return del;
		}
		return null;
	}


    static public EventDelegate Add( List<EventDelegate> list, Callback callback, float shotInterval = 0f) 
    {
        if ( list != null ) 
        {
            for ( int i = 0 ; i < list.Count; i++ ) 
            {
                EventDelegate del = list[i];
                if ( del != null && del.Equals( callback ) )
                    return del;
            }

            EventDelegate ed = new EventDelegate( callback );
            ed.shotInterval = shotInterval;
            list.Add( ed );
            return ed;
        }
        Debug.LogWarning( "Attempting to add a callback to a list that's null" );
        return null;
    }

    static public EventDelegate Add(List<EventDelegate> list, EventDelegate ed, float shotInterval = 0f)
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++) 
            {
                EventDelegate del = list[i];
                if (del != null && del.Equals(ed))
                    return del;
            }

            list.Add(ed);
            return ed;
        }
        Debug.LogWarning("Attempting to add a callback to a list that's null");
        return null;
    }

    static public bool Remove (List<EventDelegate> list, Callback callback)
	{
		if (list != null)
		{
            for (int i = 0; i < list.Count; i++) 
			{
				EventDelegate del = list[i];
				
				if (del != null && del.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
			}
		}
		return false;
	}
}
