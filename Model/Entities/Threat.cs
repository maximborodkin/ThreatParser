using System;
using LinqToExcel.Attributes;

namespace ThreatParser.Model
{
    public class Threat
    {
        [ExcelColumn("Идентификатор УБИ")]
        public int Id { get; set; }
        
        [ExcelColumn("Наименование УБИ")]
        public string Name { get; set; }

        [ExcelColumn("Описание")]
        public string Description { get; set; }

        [ExcelColumn("Источник угрозы (характеристика и потенциал нарушителя)")]
        public string Source { get; set; }

        [ExcelColumn("Объект воздействия")]
        public string InteractionObject { get; set; }

        [ExcelColumn("Нарушение конфиденциальности")]
        public bool ConfidentialityBreach { get; set; }

        [ExcelColumn("Нарушение целостности")]
        public bool IntegrityBreach { get; set; }

        [ExcelColumn("Нарушение доступности")]
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

        public static string GetCSVHeader()
        {
            return $"{nameof(Id)},{nameof(Name)},{nameof(Description)},{nameof(Source)},{nameof(InteractionObject)},{nameof(ConfidentialityBreach)},{nameof(IntegrityBreach)},{nameof(AvailabilityBreach)}";
        }

        public string ToCSVString()
        {
            return $"{Id},{Name},{Description},{Source},{InteractionObject},{ConfidentialityBreach},{IntegrityBreach},{AvailabilityBreach}";
        }
    }
}
