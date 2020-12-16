using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.FS;
using System.Collections.Generic;

namespace PX.LineCostForAccrueExt
{
    public sealed class FSSODetCostAccrueExt : PXCacheExtension<FSSODet>
    {
        #region UsrUseLineCostForAccrue
        public abstract class usrUseLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrUseLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(typeof(Search<InventoryItemLineCostAccrueExt.usrUseLineCostForAccrue,
                            Where<InventoryItem.inventoryID, Equal<Current<FSSODet.inventoryID>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<FSSODet.inventoryID>))]
        [PXUIField(DisplayName = "Use Line Cost for Accrue", Enabled = false, IsReadOnly = true)]
        public bool? UsrUseLineCostForAccrue { get; set; }
        #endregion

        #region UsrAppointmentExist
        public abstract class usrAppointmentExist : PX.Data.BQL.BqlBool.Field<usrAppointmentExist> { }

        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(AppointmentExists<FSSODet.sODetID>))]
        [PXUIField(DisplayName = "Appointment Exists", Enabled = false, IsReadOnly = true)]
        public bool? UsrAppointmentExist { get; set; }
        #endregion
    }

    public class AppointmentExists<DetailIDField> : BqlFormulaEvaluator<DetailIDField>, IBqlOperand
            where DetailIDField : IBqlField
    {
        public override object Evaluate(PXCache cache, object item, Dictionary<Type, object> parameters)
        {
            PXFieldState fState = cache.GetStateExt<DetailIDField>(item) as PXFieldState;
            if (fState == null || String.IsNullOrEmpty(Convert.ToString(fState.Value))) return null;
            FSAppointmentDet appointmentDet = PXSelectReadonly<FSAppointmentDet,
                                                                Where<FSAppointmentDet.sODetID, Equal<Required<FSSODet.sODetID>>>>.
                                                                SelectWindowed(cache.Graph, 0, 1, fState.Value);
            return (appointmentDet != null);
        }
    }
}
