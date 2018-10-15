using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using UnityEngine;

#region Reference管理器
partial class ReferenceManager<T> : IEnumerable<KeyValuePair<int, T>> where T : PropDefine {
    
	protected Dictionary<int, T> container_ = new Dictionary<int, T>();
    protected static FieldInfo[] Fields = typeof( T ).GetFields();

	public IEnumerator<KeyValuePair<int, T>> GetEnumerator() {
		return container_.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}

    public T GetReference( int id ) {
        if ( id > 0 ) {
            T result;
            if ( container_.TryGetValue( id, out result ) ) {
                return result;
            }
        }
        return null;
    }
    public T this[ int id ] {
        get { return GetReference( id ); }
    }

    public void ForEach( Action<T> action ) {
        foreach ( T t in container_.Values ) {
            action( t );
        }
    }
    //public void ForEach( Action<T> action, Predicate<T> predicate ) {
    //    foreach ( T t in container_.Values ) {
    //        if ( predicate( t ) ) {
    //            action( t );
    //        }
    //    }
    //}
    public T Find( Predicate<T> pred ) {
        foreach ( T t in container_.Values ) {
            if ( pred( t ) ) {
                return t;
            }
        }
        return null;
    }

    public T[] ToArray() {
        T[] ary = new T[ container_.Count ];
        container_.Values.CopyTo( ary, 0 );
        return ary;
    }

    #region 从text resources加载
    public AsyncDelegate AsyncReloadDataFromTextResources( string path, int editionType, bool crypto  ){
        AsyncDelegate ad = new AsyncDelegate(
            delegate(){
                ReloadDataFromFile( path, editionType, crypto );
            }
            );
        ad.Start();
        return ad;
    }

    public void ReloadDataFromFile( string path, int editionType, bool crypto ){
        this.container_.Clear();
            using (ResourceUtilReader reader = new ResourceUtilReader(path, typeof(T).Name.Replace("Reference", ""), editionType, crypto))
            {
                while (reader.GetNextLine())
                {
                    if (!ReadOneReference(reader))
                        break;
                }
            }
       
       
        OnAfterReload();
    }
    //通过reader读取一条记录并添加到表里
    private bool ReadOneReference( ResourceUtilReader reader ) {
        //判断ID
        T reference = null;
        int refId = reader.GetIntValueByCol( "id" );
        if( !this.container_.TryGetValue( refId, out reference ) ){
            reference = Activator.CreateInstance<T>();
        }
        reference.LoadReference( reader );

        this.container_[refId] = reference;
        return true;
    }
    #endregion

    protected virtual void OnAfterReload() { }
    public int Count {
        get { return container_.Count; }
    }
}
#endregion

#region 道具reference管理器

internal class ItemReferenceManager : ReferenceManager<ItemReference>
{
    private readonly HashSet<int> listBreakOnMove_ = new HashSet<int>(); // 哪些道具应该打断自动寻路
    private readonly HashSet<int> listDecKillPoint_ = new HashSet<int>(); // 哪些道具是减恶名值的

    protected override void OnAfterReload()
    {
        // 找到哪些道具使用后，应该立刻坐下，并且效果被会移动打断的
        listBreakOnMove_.Clear();
        listDecKillPoint_.Clear();
        ForEach(delegate(ItemReference ir)
        {
            if (!ir.CanUse)
            {
                return;
            }
            if (ir.ItemFunParam == null || ir.ItemFunParam.Length == 0)
            {
                return;
            }
            foreach (FunItemConf fic in ir.ItemFunParam)
            {
                if (fic.FuncID == ItemFunType.DecKillPoint)
                {
                    listDecKillPoint_.Add(ir.ID);
                }
            }
        });
    }

    /// <summary>
    ///     判断给定的ItemID，是不是这样一种道具：使用后的效果，会在移动时被打断。
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool IsItemEffectBreakWhenMove(int itemId)
    {
        return listBreakOnMove_.Contains(itemId);
    }

    public bool IsItemEffectDecKillPoint(int itemId)
    {
        return listDecKillPoint_.Contains(itemId);
    }
}

#endregion

#region 地图reference管理器

internal class MapReferenceManager : ReferenceManager<MapReference>
{
    private readonly Dictionary<int, int> dictMapOwner_ = new Dictionary<int, int>(32);

    public int GetMapOwnerCamp(int mapId)
    {
        int camp;
        if (dictMapOwner_.TryGetValue(mapId, out camp))
        {
            return camp;
        }
        else
        {
            return -1;
        }
    }



    public IEnumerable<KeyValuePair<int, int>> GetMapOwnerCampList()
    {
        return dictMapOwner_;
    }

}

#endregion

#region MapObjectReference 管理器

internal class MapObjectReferenceManager : ReferenceManager<MapObjectReference>
{
    /// <summary>
    ///     查找从哪个MapObject能采集到指定Item
    /// </summary>
    /// <param name="targetItemId"></param>
    /// <returns></returns>
    public int FindByCollectTargetId(int targetItemId)
    {
        foreach (var kvp in this)
        {
            MapObjectReference mor = kvp.Value;
            if (mor.EF1 == MapObjectEffectType.DropItem || mor.EF1 == MapObjectEffectType.Collect)
            {
                DropTableConfig[] dtcList = mor.Data;
                if (dtcList != null)
                {
                    foreach (DropTableConfig dtc in dtcList)
                    {
                        if (dtc.item_ref == targetItemId)
                        {
                            return mor.ID;
                        }
                    }
                }
            }
        }
        return -1;
    }

    
}

#endregion
