using PX.Data;
using PX.Objects.IN;

namespace PX.LineCostForAccrueExt
{
    public sealed class InventoryItemLineCostAccrueExt : PXCacheExtension<InventoryItem>
    {
        public static bool IsActive() => true;

        #region UsrUseLineCostForAccrue
        public abstract class usrUseLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrUseLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIEnabled(typeof(Where<InventoryItem.postToExpenseAccount, Equal<InventoryItem.postToExpenseAccount.sales>>))]
        [PXFormula(typeof(Default<InventoryItem.postToExpenseAccount>))]
        [PXUIField(DisplayName = "Use Line Cost")]
        public bool? UsrUseLineCostForAccrue { get; set; }
        #endregion
    }
}