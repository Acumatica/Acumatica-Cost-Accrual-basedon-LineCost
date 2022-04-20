using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;

namespace PX.LineCostForAccrueExt
{
    public class OpportunityMaintLineCostAccrueExt : PXGraphExtension<OpportunityMaint.CRCreateSalesOrderExt, OpportunityMaint>
    {
        public static bool IsActive() => PXAccess.FeatureInstalled<FeaturesSet.distributionModule>();

        protected virtual void _(Events.RowSelected<CROpportunity> e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }
            PXUIFieldAttribute.SetVisibility<CROpportunityProducts.curyUnitCost>(Base.Products.Cache, null, PXUIVisibility.Visible);
            PXUIFieldAttribute.SetVisible<CROpportunityProducts.curyUnitCost>(Base.Products.Cache, null, true);
        }

        protected virtual void _(Events.RowSelected<CROpportunityProducts> e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }

            CROpportunityProducts oppline = e.Row;
            if (oppline == null) { return; }

            CROpportunityProductsLineCostAccrueExt opplineExt = PXCache<CROpportunityProducts>.GetExtension<CROpportunityProductsLineCostAccrueExt>(oppline);

            if (opplineExt.UsrLineCostForAccrue.GetValueOrDefault(false))
            {                
                PXUIFieldAttribute.SetEnabled<CROpportunityProducts.curyUnitCost>(e.Cache, oppline, true);
            }
        }

        public delegate void BaseDoCreateSalesOrder();

        [PXOverride]
        public virtual void DoCreateSalesOrder(BaseDoCreateSalesOrder BaseInvoke)
        {
            PXGraph.InstanceCreated.AddHandler<SOOrderEntry>((graph) =>
            {
                graph.RowUpdated.AddHandler<SOLine>((cache, eArgs) =>
                {
                    var soLine = (SOLine)eArgs.Row;
                    InventoryItem item = InventoryItem.PK.Find(cache.Graph, soLine.InventoryID);
                    if (item != null)
                    {
                        InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
                        if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault())
                        {
                            CROpportunityProducts oppLine = PXResult<CROpportunityProducts>.Current;
                            cache.SetValueExt<SOLine.curyUnitCost>(soLine, oppLine.CuryUnitCost);
                        }
                    }
                });
            });

            BaseInvoke();
        }
    }
}
