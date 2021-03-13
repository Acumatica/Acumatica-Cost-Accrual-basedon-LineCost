using System;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects.FS;

namespace PX.LineCostForAccrueExt
{
	public class ARInvoiceEntryLineCostAccrueExt : PXGraphExtension<ARInvoiceEntry>
	{
		public delegate void BaseCalculateAccruedCost(PXCache sender, ARTran row);

		[PXOverride]
		public virtual void CalculateAccruedCost(PXCache sender, ARTran row, BaseCalculateAccruedCost BaseInvoke)
		{
			BaseInvoke(sender, row);
			if (row != null && (row.LineType == SOLineType.NonInventory || row.LineType == SOLineType.MiscCharge)
							&& row.AccrueCost.GetValueOrDefault(false))
			{
				InventoryItem item = (InventoryItem)PXSelectorAttribute.Select<ARTran.inventoryID>(Base.Caches[typeof(ARTran)], row);
				if (item != null)
				{
					InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
					if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault(false))
					{
						if (row.SOOrderLineNbr.HasValue && !String.IsNullOrEmpty(row.SOOrderType) && !String.IsNullOrEmpty(row.SOOrderNbr))
                        {
							POLine poData = PXSelectJoin<POLine, InnerJoin<POOrderEntry.SOLineSplit3, On<POOrderEntry.SOLineSplit3.pOLineNbr, Equal<POLine.lineNbr>,
																	And<POOrderEntry.SOLineSplit3.pOType, Equal<POLine.orderType>,
																	And<POOrderEntry.SOLineSplit3.pONbr, Equal<POLine.orderNbr>>>>>,
																 Where<POOrderEntry.SOLineSplit3.lineNbr, Equal<Required<POOrderEntry.SOLineSplit3.lineNbr>>,
																	And<POOrderEntry.SOLineSplit3.orderType, Equal<Required<POOrderEntry.SOLineSplit3.orderType>>,
																	And<POOrderEntry.SOLineSplit3.orderNbr, Equal<Required<POOrderEntry.SOLineSplit3.orderNbr>>>>>>.
																	Select(Base, row.SOOrderLineNbr, row.SOOrderType, row.SOOrderNbr);
							if (poData != null)
							{
								sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * poData.UnitCost);
								sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
							}
							else
                            {
								SOLine sOLine = PXSelect<SOLine, Where<SOLine.lineNbr, Equal<Required<SOLine.lineNbr>>,
																	And<SOLine.orderType, Equal<Required<SOLine.orderType>>,
																	And<SOLine.orderNbr, Equal<Required<SOLine.orderNbr>>>>>>.
																	Select(Base, row.SOOrderLineNbr, row.SOOrderType, row.SOOrderNbr);
								if (sOLine != null)
								{
									sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * sOLine.UnitCost);
									sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
								}
							}
						}
						else
						{
							FSxARTran arDataExt = PXCache<ARTran>.GetExtension<FSxARTran>(row);
                            if (arDataExt != null && arDataExt.SOID.HasValue && arDataExt.SODetID.HasValue)
                            {
                                PXResult<POLine, FSSODet> datainfo = (PXResult<POLine, FSSODet>)
                                                                     PXSelectJoin<POLine, InnerJoin<FSSODet, On<FSSODet.poLineNbr, Equal<POLine.lineNbr>,
                                                                                            And<FSSODet.poType, Equal<POLine.orderType>,
                                                                                            And<FSSODet.poNbr, Equal<POLine.orderNbr>>>>>,
                                                                                          Where<FSSODet.sODetID, Equal<Required<FSSODet.sODetID>>,
                                                                                            And<FSSODet.pOSource, Equal<INReplenishmentSource.purchaseToOrder>,
                                                                                            And<FSSODet.sOID, Equal<Required<FSSODet.sOID>>>>>>.
                                                                                            Select(Base, arDataExt.SODetID, arDataExt.SOID);
                                POLine poData = datainfo;
                                if (poData != null)
                                {
                                    sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * poData.UnitCost);
                                    sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
                                }
								else
								{
									if (arDataExt.AppointmentID.HasValue && arDataExt.AppDetID.HasValue)
									{
										FSAppointmentDet fsApptLine = PXSelect<FSAppointmentDet, Where<FSAppointmentDet.appointmentID, Equal<Required<FSAppointmentDet.appointmentID>>,
																			And<FSAppointmentDet.appDetID, Equal<Required<FSAppointmentDet.appDetID>>>>>.
																			Select(Base, arDataExt.AppointmentID, arDataExt.AppDetID);
										if (fsApptLine != null)
										{
											sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * fsApptLine.CuryUnitCost);
											sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
										}
									}
									else
									{
										FSSODet fsOLine = PXSelect<FSSODet, Where<FSSODet.sODetID, Equal<Required<FSSODet.sODetID>>,
																			And<FSSODet.sOID, Equal<Required<FSSODet.sOID>>>>>.
																			Select(Base, arDataExt.SODetID, arDataExt.SOID);
										if (fsOLine != null)
										{
											sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * fsOLine.CuryUnitCost);
											sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
										}
									}
								}
							}
                        }
					}
				}
			}
		}

		public void _(Events.RowUpdated<ARTran> e, PXRowUpdated BaseInvoke)
		{
			if (BaseInvoke != null) { BaseInvoke(e.Cache, e.Args); }

			if (e.Row == null || e.OldRow == null) { return; }
			PXCache sender = e.Cache;

			if (!sender.ObjectsEqual<FSxARTran.sODetID, FSxARTran.sOID>(e.Row, e.OldRow))
			{
				Base.CalculateAccruedCost(e.Cache, e.Row);
			}
		}
	}
}