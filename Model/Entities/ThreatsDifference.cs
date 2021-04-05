using System.Collections.Generic;
using System.Text;

namespace ThreatParser.Model
{
    public class ThreatsDifference
    {
        public DifferenceType DifferenceType { get; set; }
        public Threat OldThreat { get; set; }
        public Threat NewThreat { get; set; }
        public string DifferentFieldsNames { get; private set; }

        public ThreatsDifference(DifferenceType differenceType, Threat oldThreat, Threat newThreat)
        {
            DifferenceType = differenceType;
            OldThreat = oldThreat;
            NewThreat = newThreat;
            DifferentFieldsNames = GetDifferentFieldsNames();
        }

        private string GetDifferentFieldsNames()
        {
            if (OldThreat == null || NewThreat == null) return "Все";

            StringBuilder result = new StringBuilder();
            if (OldThreat.Name != NewThreat.Name) result.Append("Наименование УБИ, ");
            if (OldThreat.Description != NewThreat.Description) result.Append("Описание, ");
            if (OldThreat.Source != NewThreat.Source) result.Append("Источник угрозы, ");
            if (OldThreat.InteractionObject != NewThreat.InteractionObject) result.Append("Объект воздействия, ");
            if (OldThreat.ConfidentialityBreach != NewThreat.ConfidentialityBreach) result.Append("Нарушение конфиденциальности, ");
            if (OldThreat.IntegrityBreach != NewThreat.IntegrityBreach) result.Append("Нарушение целостности, ");
            if (OldThreat.AvailabilityBreach != NewThreat.AvailabilityBreach) result.Append("Нарушение доступности, ");

            return result.ToString().Trim(',', ' ');
        }
    }
}
