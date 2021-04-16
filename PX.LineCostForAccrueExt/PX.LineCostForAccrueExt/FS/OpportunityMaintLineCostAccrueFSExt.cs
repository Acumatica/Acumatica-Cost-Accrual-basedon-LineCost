using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.FS;

namespace PX.LineCostForAccrueExt
{
    public class OpportunityMaintLineCostAccrueFSExt : PXGraphExtension<SM_OpportunityMaint, OpportunityMaint>
    {
        public static bool IsActive() => PXAccess.FeatureInstalled<FeaturesSet.serviceManagementModule>();

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
    }
}
