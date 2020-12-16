﻿using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.FS;
using System.Collections.Generic;

namespace PX.LineCostForAccrueExt
{
    public sealed class FSAppointmentDetCostAccrueExt : PXCacheExtension<FSAppointmentDet>
    {
        #region UsrUseLineCostForAccrue
        public abstract class usrUseLineCostForAccrue : PX.Data.BQL.BqlBool.Field<usrUseLineCostForAccrue> { }

        [PXDBBool]
        [PXDefault(typeof(Search<InventoryItemLineCostAccrueExt.usrUseLineCostForAccrue,
                            Where<InventoryItem.inventoryID, Equal<Current<FSAppointmentDet.inventoryID>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<FSAppointmentDet.inventoryID>))]
        [PXUIField(DisplayName = "Use Line Cost for Accrue", Enabled = false, IsReadOnly = true)]
        public bool? UsrUseLineCostForAccrue { get; set; }
        #endregion
    }
}