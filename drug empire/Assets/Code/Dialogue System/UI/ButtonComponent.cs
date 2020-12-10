using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DialogueSystem
{
	public class ButtonComponent : MonoBehaviour
	{

		[SerializeField] private Button _button;
		[SerializeField] private Text _text;
		[SerializeField] private RectTransform _rect;
		[SerializeField] private RectTransform _background;

		public Button Button => _button;

		public Text Text => _text;

		public RectTransform Rect => _rect;

		public RectTransform Background
		{
			get => _background;
			set => _background = value;
		}
	}
}