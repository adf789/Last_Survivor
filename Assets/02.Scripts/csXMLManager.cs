using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class csXMLManager{
	private static XmlNodeList xmlNodeList;
	private static bool isLoaded = false;

	public static XmlNodeList XmlNodeList(string nodeName){
		if (!isLoaded) {
			LoadXML (nodeName);
			isLoaded = true;
		}
		for (int i = 0; i < xmlNodeList.Count; i++) {
			XmlNode node = xmlNodeList.Item (i);
			if (node.Name.Equals (nodeName) && node.HasChildNodes) {
				return node.ChildNodes;
			}
		}
		return null;
	}

	private static void LoadXML(string nodeName){
		string fileName = "Schema";
		TextAsset txtAsset = (TextAsset)Resources.Load ("XML/" + fileName);
		XmlDocument xmlDoc = new XmlDocument ();
		xmlDoc.LoadXml (txtAsset.text);

		// xml파일의 root의 노드 리스트를 선택한다.
		xmlNodeList = xmlDoc.SelectNodes("root/" + nodeName);
	}
}
