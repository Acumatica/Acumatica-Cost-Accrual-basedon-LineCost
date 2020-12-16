using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.SO;

namespace PX.LineCostForAccrueExt
{
    public sealed class SOLineLineCostAccrueExt : PXCacheExtension<SOLine>
    {
        #region UsrUseLineCostForAccrue
        public abstract class usrUseLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrUseLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(typeof(Search<InventoryItemLineCostAccrueExt.usrUseLineCostForAccrue, 
                            Where<InventoryItem.inventoryID, Equal<Current<SOLine.inventoryID>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<SOLine.inventoryID>))]
        [PXUIField(DisplayName = "Use Line Cost for Accrue", Enabled = false, IsReadOnly = true)]
        public bool? UsrUseLineCostForAccrue { get; set; }
        #endregion
    }
}