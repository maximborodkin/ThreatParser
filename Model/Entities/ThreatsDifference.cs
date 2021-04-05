namespace ThreatParser.Model
{
    public class ThreatsDifference
    {
        public DifferenceType DifferenceType { get; set; }
        public Threat OldThreat { get; set; }
        public Threat NewThreat { get; set; }

        public ThreatsDifference(DifferenceType differenceType, Threat oldThreat, Threat newThreat)
        {
            DifferenceType = differenceType;
            OldThreat = oldThreat;
            NewThreat = newThreat;
        }
    }
}
