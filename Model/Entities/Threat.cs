using System;
using LinqToExcel.Attributes;

namespace ThreatParser.Model
{
    public class Threat
    {
        public const string IdAttrName = "Идентификатор УБИ";
        [ExcelColumn(columnName: IdAttrName)]
        public int Id { get; set; }

        public const string NameAttrName = "Наименование УБИ";
        [ExcelColumn(columnName: NameAttrName)]
        public string Name { get; set; }

        public const string DescriptionAttrName = "Описание";
        [ExcelColumn(columnName: DescriptionAttrName)]
        public string Description { get; set; }

        public const string SourceAttrName = "Источник угрозы (характеристика и потенциал нарушителя)";
        [ExcelColumn(columnName: SourceAttrName)]
        public string Source { get; set; }

        public const string InteractionObjectAttrName = "Объект воздействия";
        [ExcelColumn(columnName: InteractionObjectAttrName)]
        public string InteractionObject { get; set; }

        public const string ConfidentialityBreachAttrName = "Нарушение конфиденциальности";
        [ExcelColumn(columnName: ConfidentialityBreachAttrName)]
        public bool ConfidentialityBreach { get; set; }

        public const string IntegrityBreachAttrName = "Нарушение целостности";
        [ExcelColumn(columnName: IntegrityBreachAttrName)]
        public bool IntegrityBreach { get; set; }

        public const string AvailabilityBreachAttrName = "Нарушение доступности";
        [ExcelColumn(columnName: AvailabilityBreachAttrName)]
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
