using System.Linq;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.FS;
using PX.Objects.IN;

namespace PX.LineCostForAccrueExt
{
    public class SM_OpportunityMaint_DBoxLineCostAccrueExt : PXGraphExtension<SM_OpportunityMaint_DBox, OpportunityMaint>
    {
        public static bool IsActive() => PXAccess.FeatureInstalled<FeaturesSet.serviceManagementModule>();

        public delegate FSServiceOrder CreateDocumentBase(
            ServiceOrderEntry srvOrdGraph,
            AppointmentEntry apptGraph,
            string sourceDocumentEntity,
            string sourceDocType,
            string sourceDocRefNbr,
            int? sourceDocID,
            PXCache headerCache,
            PXCache linesCache,
            DBoxHeader header,
            List<DBoxDetails> details,
            bool createAppointment);

        [PXOverride]
        public FSServiceOrder CreateDocument(
            ServiceOrderEntry srvOrdGraph,
            AppointmentEntry apptGraph,
            string sourceDocumentEntity,
            string sourceDocType,
            string sourceDocRefNbr,
            int? sourceDocID,
            PXCache headerCache,
            PXCache linesCache,
            DBoxHeader header,
            List<DBoxDetails> details,
            bool createAppointment, CreateDocumentBase BaseInvoke)
        {
            srvOrdGraph.RowUpdated.AddHandler<FSSODet>((cache, eArgs) =>
            {
                FSSODet fsoLine = (FSSODet)eArgs.Row;
                InventoryItem item = InventoryItem.PK.Find(cache.Graph, fsoLine?.InventoryID);
                if (item != null)
                {
                    InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
                    if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault())
                    {
                        DBoxDetails oppline = details.Where(x => x?.SourceNoteID == fsoLine.SourceNoteID)?.FirstOrDefault();
                        cache.SetValueExt<FSSODet.curyUnitCost>(fsoLine, oppline.CuryUnitCost);
                    }
                }
            });

            apptGraph.RowUpdated.AddHandler<FSAppointmentDet>((cache, eArgs) =>
            {
                var fsoALine = (FSAppointmentDet)eArgs.Row;
                InventoryItem item = InventoryItem.PK.Find(cache.Graph, fsoALine.InventoryID);
                if (item != null)
                {
                    InventoryItemLineCostAccrueExt itemExt = PXCache<InventoryItem>.GetExtension<InventoryItemLineCostAccrueExt>(item);
                    if (itemExt.UsrUseLineCostForAccrue.GetValueOrDefault())
                    {
                        DBoxDetails oppline = details.Where(x => x?.SourceNoteID == fsoALine?.FSSODetRow?.SourceNoteID)?.FirstOrDefault();
                        cache.SetValueExt<FSAppointmentDet.curyUnitCost>(fsoALine, oppline.CuryUnitCost);
                    }
                }
            });

            return BaseInvoke(srvOrdGraph, apptGraph, sourceDocumentEntity, sourceDocType,
                              sourceDocRefNbr, sourceDocID, headerCache, linesCache, header, details, createAppointment);
        }
    }
}
