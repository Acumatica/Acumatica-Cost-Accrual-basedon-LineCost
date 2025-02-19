﻿using PX.Data;
using PX.Objects.CS;
using PX.Objects.SO;

namespace PX.LineCostForAccrueExt
{
    public class SOOrderEntryCostPXExt : PXGraphExtension<SOOrderEntry>
    {
        public static bool IsActive() => PXAccess.FeatureInstalled<FeaturesSet.distributionModule>();

        public override void Initialize()
        {
            PXUIFieldAttribute.SetVisible<SOLine.curyUnitCost>(Base.Transactions.Cache, null, true);
        }

        protected virtual void _(Events.RowSelected<SOLine> e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }

            SOLine soline = e.Row;
            if (soline == null) { return; }

            SOLineLineCostAccrueExt solineExt = PXCache<SOLine>.GetExtension<SOLineLineCostAccrueExt>(soline);

            if (solineExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
            {
                PXUIFieldAttribute.SetEnabled<SOLine.curyUnitCost>(Base.Transactions.Cache, soline, true);
            }
        }
    }
}