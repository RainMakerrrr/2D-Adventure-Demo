using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using NPC.Friends;
using PlayerController;
using QuestingSystem;
using UnityEngine.Events;


namespace DialogueSystem
{
	public class DialogueManager : MonoBehaviour
	{
		public event UnityAction<string> OnDialogueEnded;
		public event UnityAction<string> OnAccept;
		public event UnityAction<string> OnDismiss;
		

		[SerializeField] private ButtonComponent questionButton;
		[SerializeField] private ButtonComponent answerButton;
		[SerializeField] private string folder = "Russian";

		private string fileName, lastName;
		private List<Dialogue> node;
		private Dialogue dialogue;
		private Answer answer;
		private List<RectTransform> buttons = new List<RectTransform>();

		public static DialogueManager Instance { get; private set; }

		public void DialogueStart(string name, Player player, HumanQuestReceiver npc)
		{
			if (name == string.Empty) return;
			fileName = name;
			questionButton.Background = npc.DialogueBackground;
			answerButton.Background = player.DialogueBackground;
			Load();
		}
		
		public void DialogueStart(string name, Player player, Paul npc)
		{
			if (name == string.Empty) return;
			fileName = name;
			questionButton.Background = npc.DialogueBackground;
			answerButton.Background = player.DialogueBackground;
			Load();
		}
		
		

		private void Awake()
		{
			Instance = this;

		}

		private void Start()
		{
			if (questionButton != null && answer != null)
			{
				questionButton.gameObject.SetActive(false);
				answerButton.gameObject.SetActive(false);
				
				questionButton.Background.gameObject.SetActive(false);
				answerButton.Background.gameObject.SetActive(false);
			}
		}


		private void Load()
		{

			if (lastName == fileName)
			{
				BuildDialogue(0);
				return;
			}

			node = new List<Dialogue>();

			try
			{
				TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
				XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

				int index = 0;
				while (reader.Read())
				{
					if (reader.IsStartElement("node"))
					{
						dialogue = new Dialogue();
						dialogue.answer = new List<Answer>();
						dialogue.npcText = reader.GetAttribute("npcText");
						node.Add(dialogue);

						XmlReader inner = reader.ReadSubtree();
						while (inner.ReadToFollowing("answer"))
						{
							answer = new Answer();
							answer.text = reader.GetAttribute("text");

							int number;
							if (int.TryParse(reader.GetAttribute("toNode"), out number)) answer.toNode = number;
							else answer.toNode = 0;

							bool result;
							if (bool.TryParse(reader.GetAttribute("exit"), out result)) answer.exit = result;
							else answer.exit = false;

							node[index].answer.Add(answer);
						}

						inner.Close();

						index++;
					}
				}

				lastName = fileName;
				reader.Close();
			}
			catch (System.Exception error)
			{
				Debug.Log(this + " Ошибка чтения файла диалога: " + fileName + ".xml >> Error: " + error.Message);
				lastName = string.Empty;
			}

			BuildDialogue(0);
		}


		private void BuildElement(bool exit, int toNode, string text, bool isActiveButton, ButtonComponent button)
		{
			if(button.Background != null)
				button.Background.gameObject.SetActive(true);

			ButtonComponent clone = Instantiate(button);
			clone.gameObject.SetActive(true);
			clone.Rect.SetParent(button.Background);
			SetElementsPosition(clone.Rect);


			StartCoroutine(TypeSentece(clone, text));


			clone.Button.interactable = isActiveButton;

			if (answer.exit && dialogue.npcText == string.Empty) CloseDialogue();
			if (clone.Text.text == string.Empty && answer.exit == false) BuildDialogue(toNode);
			if (toNode > 0) SetNextDialogue(clone.Button, toNode);
			if (exit) SetExitDialogue(clone.Button);

			buttons.Add(clone.Rect);
		}


		private void SetElementsPosition(RectTransform rect)
		{
			rect.localScale = Vector3.one;

			rect.anchorMin = new Vector2(0f, 0f);
			rect.anchorMax = new Vector2(1f, 1f);
			rect.pivot = new Vector2(0.5f, 0.5f);

			rect.offsetMax = new Vector2(0f, 0f);
			rect.offsetMin = new Vector2(0f, 0f);


		}

		private IEnumerator TypeSentece(ButtonComponent buttonText, string sentence)
		{
			
			buttonText.Text.text = "";

			foreach (var letter in sentence.ToCharArray())
			{
				buttonText.Text.text += letter;
				yield return new WaitForSeconds(0.02f);
			}

			if (buttonText.Text.text == "Да") buttonText.GetComponent<Image>().color = Color.green;
			if (buttonText.Text.text == "Нет") buttonText.GetComponent<Image>().color = Color.red;
			if (buttonText.Text.text == string.Empty) buttonText.Text.text = "<Кликните сюда, чтобы продолжить>";

		}



		private void ClearDialogue()
		{
			foreach (RectTransform b in buttons)
			{
				Destroy(b.gameObject);
			}

			buttons = new List<RectTransform>();
			StopAllCoroutines();
		}


		private void SetNextDialogue(Button button, int id)
		{
			button.onClick.AddListener(() => { BuildDialogue(id); });
		}

		private void SetExitDialogue(Button button)
		{
			button.onClick.AddListener(() =>
			{
				CloseDialogue();
				if(button.GetComponentInChildren<Text>().text == "Да") OnAccept?.Invoke(fileName);
				if(button.GetComponentInChildren<Text>().text == "Нет") OnDismiss?.Invoke(fileName);
				
			});
		}

		private void CloseDialogue()
		{
			questionButton.Background.gameObject.SetActive(false);
			answerButton.Background.gameObject.SetActive(false);
			OnDialogueEnded?.Invoke(fileName);
			ClearDialogue();
		}

		private void BuildDialogue(int current)
		{
			ClearDialogue();

			StartCoroutine(BuildPlayerDialogue(current));
		}

		private IEnumerator BuildNpcDialogue(int current)
		{
			BuildElement(false, 0, node[current].npcText, false, questionButton);
			yield return new WaitForSeconds(1.75f);
		}

		private IEnumerator BuildPlayerDialogue(int current)
		{
			yield return BuildNpcDialogue(current);
			for (int i = 0; i < node[current].answer.Count; i++)
			{
				BuildElement(node[current].answer[i].exit, node[current].answer[i].toNode, node[current].answer[i].text,
					true, answerButton);
			}
		}
		
}

	class Dialogue
	{
		public string npcText;
		public List<Answer> answer;
	}


	class Answer
	{
		public string text;
		public int toNode;
		public bool exit;
	}
}