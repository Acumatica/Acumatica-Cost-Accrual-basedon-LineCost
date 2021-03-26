using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.FS;
using System.Linq;
using System.Collections.Generic;

namespace PX.LineCostForAccrueExt
{
    public class ServiceOrderEntryLineCostAccrue : PXGraphExtension<ServiceOrderEntry>
    {
        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXFormula(typeof(AppointmentExists<FSSODet.sODetID>))]
        public void _(Events.CacheAttached<FSSODetCostAccrueExt.usrAppointmentExist> e) { }

        protected virtual void _(Events.RowSelected<FSSODet> e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }

            FSSODet fsoline = e.Row;
            if (fsoline == null) { return; }

            FSSODetCostAccrueExt fsolineExt = PXCache<FSSODet>.GetExtension<FSSODetCostAccrueExt>(fsoline);

            if (fsolineExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
            {
                PXUIFieldAttribute.SetEnabled<FSSODet.curyUnitCost>(Base.ServiceOrderDetails.Cache, fsoline,
                                                                   (String.IsNullOrEmpty(fsoline.PONbr) && !fsolineExt.UsrAppointmentExist.GetValueOrDefault(false)));
            }
        }

        protected virtual void _(Events.FieldDefaulting<FSSODet.curyUnitCost> e, PXFieldDefaulting BaseInvoke)
        {
            FSSODet fsoline = (FSSODet)e.Row;
            if (fsoline != null)
            {
                FSSODetCostAccrueExt fsolineExt = PXCache<FSSODet>.GetExtension<FSSODetCostAccrueExt>(fsoline);
                if (fsolineExt.UsrUseLineCostForAccrue.GetValueOrDefault(false) ||
                    fsolineExt.UsrAppointmentExist.GetValueOrDefault(false))
                {
                    e.NewValue = fsoline.CuryUnitCost;
                    e.Cancel = true;
                    return;
                }
            }
            BaseInvoke?.Invoke(e.Cache, e.Args);
        }

        [PXOverride]
        public virtual void Persist(Action BasePersist)
        {
            //Stuff CuryUnitCost for new.
            if (Base.GraphAppointmentEntryCaller != null)
            {
                foreach (FSAppointmentDet appointmentDet in Base.GraphAppointmentEntryCaller.AppointmentDetails.Select())
                {
                    if (Base.GraphAppointmentEntryCaller.AppointmentDetails.Cache.GetStatus(appointmentDet) == PXEntryStatus.Inserted)
                    {
                        FSAppointmentDetCostAccrueExt fsolineExt = PXCache<FSAppointmentDet>.GetExtension<FSAppointmentDetCostAccrueExt>(appointmentDet);
                        if (fsolineExt.UsrUseLineCostForAccrue.GetValueOrDefault(false) && appointmentDet.FSSODetRow != null)
                        {
                            appointmentDet.FSSODetRow.CuryUnitCost = appointmentDet.CuryUnitCost;
                        }
                    }
                }
            }
            BasePersist();
        }
    }
}