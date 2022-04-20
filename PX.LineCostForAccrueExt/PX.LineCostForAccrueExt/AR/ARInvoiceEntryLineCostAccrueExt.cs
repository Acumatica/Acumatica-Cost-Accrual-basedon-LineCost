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
		public static bool IsActive() => true;

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
							FSARTran arDataExt = FSARTran.PK.Find(Base, row.TranType, row.RefNbr, row.LineNbr);
							if (arDataExt != null && !String.IsNullOrEmpty(arDataExt.SrvOrdType) && !String.IsNullOrEmpty(arDataExt.ServiceOrderRefNbr))
							{
								PXResult<POLine, FSSODet> datainfo = (PXResult<POLine, FSSODet>)
																	 PXSelectJoin<POLine, InnerJoin<FSSODet, On<FSSODet.poLineNbr, Equal<POLine.lineNbr>,
																							And<FSSODet.poType, Equal<POLine.orderType>,
																							And<FSSODet.poNbr, Equal<POLine.orderNbr>>>>>,
																						  Where<FSSODet.lineNbr, Equal<Required<FSSODet.lineNbr>>,
																							And<FSSODet.pOSource, Equal<INReplenishmentSource.purchaseToOrder>,
																							And<FSSODet.srvOrdType, Equal<Required<FSSODet.srvOrdType>>,
																							And<FSSODet.refNbr, Equal<Required<FSSODet.refNbr>>>>>>>.
																							Select(Base, arDataExt.ServiceOrderLineNbr,
																								   arDataExt.SrvOrdType, arDataExt.ServiceOrderRefNbr);
								POLine poData = datainfo;
								if (poData != null)
								{
									sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * poData.UnitCost);
									sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
								}
								else
								{
									if (arDataExt.AppointmentLineNbr.HasValue && !String.IsNullOrEmpty(arDataExt.AppointmentRefNbr))
									{
										FSAppointmentDet fsApptLine = PXSelect<FSAppointmentDet, Where<FSAppointmentDet.refNbr, Equal<Required<FSAppointmentDet.refNbr>>,
																			And<FSAppointmentDet.lineNbr, Equal<Required<FSAppointmentDet.lineNbr>>>>>.
																			Select(Base, arDataExt.AppointmentRefNbr, arDataExt.AppointmentLineNbr);
										if (fsApptLine != null)
										{
											sender.SetValueExt<ARTran.accruedCost>(row, row.BaseQty * fsApptLine.CuryUnitCost);
											sender.SetValueExt<ARTranLineCostAccrueExt.usrLineCostForAccrue>(row, true);
										}
									}
									else
									{
										FSSODet fsOLine = PXSelect<FSSODet, Where<FSSODet.srvOrdType, Equal<Required<FSSODet.srvOrdType>>,
																			And<FSSODet.refNbr, Equal<Required<FSSODet.refNbr>>,
																			And<FSSODet.lineNbr, Equal<Required<FSSODet.lineNbr>>>>>>.
																			Select(Base, arDataExt.SrvOrdType, arDataExt.ServiceOrderRefNbr, arDataExt.ServiceOrderLineNbr);
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
	}
}