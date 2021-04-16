using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.IN;

namespace PX.LineCostForAccrueExt
{
    public sealed class CROpportunityProductsLineCostAccrueExt : PXCacheExtension<CROpportunityProducts>
    {
        #region UsrLineCostForAccrue
        public abstract class usrLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(typeof(Search<InventoryItemLineCostAccrueExt.usrUseLineCostForAccrue,
                            Where<InventoryItem.inventoryID, Equal<Current<CROpportunityProducts.inventoryID>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<CROpportunityProducts.inventoryID>))]
        [PXUIField(DisplayName = "Use Line Cost for Accrue", Enabled = false, IsReadOnly = true)]
        public bool? UsrLineCostForAccrue { get; set; }
        #endregion
    }
}