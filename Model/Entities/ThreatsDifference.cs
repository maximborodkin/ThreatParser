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

            List<string> diffs = new List<string>();
            if (OldThreat.Name != NewThreat.Name) diffs.Add(Threat.NameAttrName);
            if (OldThreat.Description != NewThreat.Description) diffs.Add(Threat.DescriptionAttrName);
            if (OldThreat.Source != NewThreat.Source) diffs.Add(Threat.SourceAttrName);
            if (OldThreat.InteractionObject != NewThreat.InteractionObject) diffs.Add(Threat.InteractionObjectAttrName);
            if (OldThreat.ConfidentialityBreach != NewThreat.ConfidentialityBreach) diffs.Add(Threat.ConfidentialityBreachAttrName);
            if (OldThreat.IntegrityBreach != NewThreat.IntegrityBreach) diffs.Add(Threat.IntegrityBreachAttrName);
            if (OldThreat.AvailabilityBreach != NewThreat.AvailabilityBreach) diffs.Add(Threat.AvailabilityBreachAttrName);

            return string.Join(", ", diffs);
        }
    }
}
