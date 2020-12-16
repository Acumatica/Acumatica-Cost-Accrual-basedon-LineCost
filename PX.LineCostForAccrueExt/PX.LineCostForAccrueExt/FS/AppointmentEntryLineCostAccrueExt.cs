using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.FS;

namespace PX.LineCostForAccrueExt
{
    public class AppointmentEntryLineCostAccrueExt : PXGraphExtension<AppointmentEntry>
    {
        public override void Initialize()
        {
            PXUIFieldAttribute.SetDisplayName<FSAppointmentDet.curyUnitCost>(Base.AppointmentDetails.Cache, "Unit Cost");
        }

        protected virtual void _(Events.RowSelected<FSAppointmentDet> e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }

            FSAppointmentDet fsoline = e.Row;
            if (fsoline == null) { return; }

            FSAppointmentDetCostAccrueExt fsolineExt = PXCache<FSAppointmentDet>.GetExtension<FSAppointmentDetCostAccrueExt>(fsoline);

            if (fsolineExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
            {
                PXUIFieldAttribute.SetEnabled<FSSODet.curyUnitCost>(Base.AppointmentDetails.Cache, fsoline, true);
            }
        }

        protected virtual void _(Events.FieldDefaulting<FSAppointmentDet.curyUnitCost> e, PXFieldDefaulting BaseInvoke)
        {            
            //CuryUnitCost
            FSAppointmentDet fsoAppDetline = (FSAppointmentDet)e.Row;
            PXCache sender = e.Cache;

            if (sender != null && fsoAppDetline != null && fsoAppDetline.SODetID.HasValue)
            {
                InventoryItem item = (InventoryItem)PXSelectorAttribute.Select<FSAppointmentDet.inventoryID>(Base.Caches[typeof(FSAppointmentDet)], fsoAppDetline);
                if (item != null)
                {
                    InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
                    if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
                    {
                        if (sender.GetStatus(fsoAppDetline) == PXEntryStatus.Inserted)
                        {
                            FSSODet data = PXSelect<FSSODet, Where<FSSODet.sODetID, Equal<Required<FSSODet.sODetID>>>>.Select(Base, fsoAppDetline.SODetID);
                            if (data != null)
                            {
                                e.NewValue = data.CuryUnitCost;
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            e.NewValue = fsoAppDetline.CuryUnitCost;
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }
        }

        //Complete Appointment action is invoking AppointmentEntry.CalculateCosts which is raising defaulting for 
        //unitCost instead of CuryUnitCost. FS team is notified to correct this.
        //AC-176927
        protected virtual void _(Events.FieldDefaulting<FSAppointmentDet.unitCost> e, PXFieldDefaulting BaseInvoke)
        {
            //UnitCost
            FSAppointmentDet fsoAppDetline = (FSAppointmentDet)e.Row;
            PXCache sender = e.Cache;

            if (sender != null && fsoAppDetline != null)
            {
                InventoryItem item = (InventoryItem)PXSelectorAttribute.Select<FSAppointmentDet.inventoryID>(Base.Caches[typeof(FSAppointmentDet)], fsoAppDetline);
                if (item != null)
                {
                    InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
                    if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
                    {
                        e.NewValue = fsoAppDetline.UnitCost;
                        e.Cancel = true;
                        return;
                    }
                }
            }
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }
        }
    }
}