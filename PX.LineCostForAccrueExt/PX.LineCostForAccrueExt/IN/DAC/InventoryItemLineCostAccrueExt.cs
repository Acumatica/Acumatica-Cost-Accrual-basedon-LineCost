using System;
using PX.Data;
using PX.Objects.IN;

namespace PX.LineCostForAccrueExt
{
    public sealed class InventoryItemLineCostAccrueExt : PXCacheExtension<InventoryItem>
    {
        #region UsrUseLineCostForAccrue
        public abstract class usrUseLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrUseLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<InventoryItem.accrueCost, Equal<True>>))]
        [PXFormula(typeof(Default<InventoryItem.accrueCost>))]
        [PXUIField(DisplayName = "Use Line Cost")]
        public bool? UsrUseLineCostForAccrue { get; set; }
        #endregion
    }
}