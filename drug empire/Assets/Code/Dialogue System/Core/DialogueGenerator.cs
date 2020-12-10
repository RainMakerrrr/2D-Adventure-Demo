using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

namespace DialogueSystem
{
	public class DialogueGenerator : MonoBehaviour
	{

		public string _fileName = "Example"; 
		string _folder = "Russian"; 
		public DialogueNode[] _node;

		public void Generate()
		{
			string path = Application.dataPath + "/Resources/" + _folder + "/" + _fileName + ".xml";

			XmlNode userNode;
			XmlElement element;

			XmlDocument xmlDoc = new XmlDocument();
			XmlNode rootNode = xmlDoc.CreateElement("dialogue");
			XmlAttribute attribute = xmlDoc.CreateAttribute("name");
			attribute.Value = _fileName;
			rootNode.Attributes.Append(attribute);
			xmlDoc.AppendChild(rootNode);

			for (int j = 0; j < _node.Length; j++)
			{
				userNode = xmlDoc.CreateElement("_node");
				attribute = xmlDoc.CreateAttribute("id");
				attribute.Value = j.ToString();
				userNode.Attributes.Append(attribute);
				attribute = xmlDoc.CreateAttribute("npcText");
				attribute.Value = _node[j].npcText;
				userNode.Attributes.Append(attribute);

				for (int i = 0; i < _node[j].playerAnswer.Length; i++)
				{
					element = xmlDoc.CreateElement("answer");
					element.SetAttribute("text", _node[j].playerAnswer[i].text);
					if (_node[j].playerAnswer[i].toNode > 0)
						element.SetAttribute("toNode", _node[j].playerAnswer[i].toNode.ToString());
					if (_node[j].playerAnswer[i].exit)
						element.SetAttribute("exit", _node[j].playerAnswer[i].exit.ToString());
					userNode.AppendChild(element);
				}

				rootNode.AppendChild(userNode);
			}

			xmlDoc.Save(path);
			Debug.Log(this + " Создан XML файл диалога [ " + _fileName + " ] по адресу: " + path);
		}
	}

	[System.Serializable]
	public class DialogueNode
	{
		public string npcText;
		public PlayerAnswer[] playerAnswer;
	}


	[System.Serializable]
	public class PlayerAnswer
	{
		public string text;
		public int toNode;
		public bool exit;
		
	}
}