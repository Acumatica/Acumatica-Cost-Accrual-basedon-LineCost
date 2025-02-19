using System.Collections.Generic;
using PX.Data;
using PX.Objects.AR;
using PX.Objects.CS;
using PX.Objects.FS;
using PX.Objects.SO;
using PX.Objects.IN;

namespace PX.LineCostForAccrueExt
{
    public class SM_SOInvoiceEntryLineCostAccrueExt : PXGraphExtension<SM_SOInvoiceEntry, SOInvoiceEntry>
    {
        public static bool IsActive() => PXAccess.FeatureInstalled<FeaturesSet.serviceManagementModule>();

        public delegate FSCreatedDoc PressSaveBase(int batchID, List<DocLineExt> docLines, BeforeSaveDelegate beforeSave);

        [PXOverride]
        public FSCreatedDoc PressSave(int batchID, List<DocLineExt> docLines, BeforeSaveDelegate beforeSave, PressSaveBase BaseInvoke)
        {
            FSCreatedDoc fsCreatedDocRow = BaseInvoke(batchID, docLines, beforeSave);

            ARInvoice arInvoice = Base.Document.Current;

            if (arInvoice != null)
            {
                foreach(ARTran tranline in Base.Transactions.Select())
                {
                    InventoryItem item = ARTran.FK.InventoryItem.FindParent(Base, tranline);
                    InventoryItemLineCostAccrueExt itemExt = item?.GetExtension<InventoryItemLineCostAccrueExt>();
                    if (itemExt?.UsrUseLineCostForAccrue == true)
                    {
                        Base.CalculateAccruedCost(Base.Transactions.Cache, tranline);
                        Base.Transactions.Cache.MarkUpdated(tranline);
                        Base.Transactions.Cache.IsDirty = true;
                    }
                }

                if (Base.Transactions.Cache.IsDirty)
                {
                    Base.Save.Press();
                }
            }

            return fsCreatedDocRow;
        }
    }
}