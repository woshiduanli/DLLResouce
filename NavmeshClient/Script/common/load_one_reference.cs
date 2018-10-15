using System; 

public partial class MapReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        this.FileName = resource_reader.GetValueByCol( "FileName" );
        int.TryParse( resource_reader.GetValueByCol( "Width" ), out this.Width );
        int.TryParse( resource_reader.GetValueByCol( "Height" ), out this.Height );
    }
} 