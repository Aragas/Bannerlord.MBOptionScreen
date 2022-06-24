﻿using System;
using System.Xml.Serialization;

namespace MCM.Abstractions.Settings.Definitions.Xml
{
    [Serializable]
    [XmlType("PropertyFloatingInteger")]
    public class PropertyFloatingIntegerXmlModel : PropertyBaseXmlModel, IPropertyDefinitionWithMinMax
    {
        [XmlAttribute("MinValue")]
        public decimal MinValue { get; set; }

        [XmlAttribute("MaxValue")]
        public decimal MaxValue { get; set; }
    }
}