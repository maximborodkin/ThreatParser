using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreatParser.Model.Entities
{
    public class Threat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string InteractionObject { get; set; }
        public bool ConfidentialityBreach { get; set; }
        public bool IntegrityBreach { get; set; }
        public bool AvailabilityBreach { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, Source, InteractionObject, ConfidentialityBreach, IntegrityBreach, AvailabilityBreach);
        }

        public override bool Equals(object obj)
        {
            Threat another = obj as Threat;
            return another != null
                && Id == another.Id
                && Name == another.Name
                && Description == another.Description
                && Source == another.Source
                && InteractionObject == another.InteractionObject
                && ConfidentialityBreach == another.ConfidentialityBreach
                && IntegrityBreach == another.IntegrityBreach
                && AvailabilityBreach == another.AvailabilityBreach;
        }
    }
}
