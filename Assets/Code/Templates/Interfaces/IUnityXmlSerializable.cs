﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

public class IUnityXmlSerializable : MonoBehaviour {

    public virtual Component ReadXml(XmlReader reader, Component workingObj) { return null; }

    public virtual void WriteXml(XmlWriter writer) { }
}
