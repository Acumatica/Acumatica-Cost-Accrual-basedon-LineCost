using PX.Data;
using PX.Objects.AR;

namespace PX.LineCostForAccrueExt
{
    public sealed class ARTranLineCostAccrueExt : PXCacheExtension<ARTran>
    {
        public static bool IsActive() => true;

        #region UsrLineCostForAccrue
        public abstract class usrLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Line Cost Used for Accrue", Enabled = false, IsReadOnly = true)]
        public bool? UsrLineCostForAccrue { get; set; }        
        #endregion
    }
}