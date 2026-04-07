using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace krakenScripts
{
	public class KrakenControl : MonoBehaviour
	{

		public static int score;  // Puntaje global accesible desde otros scripts
		GameObject kraken;
		public GameObject score_value;
		bool buttonLeft_pressed;
		bool buttonRight_pressed;
		public Animator touchAnimator;

		void Start()
		{
			kraken = GameObject.Find("Kraken");
			score = 0;
			score_value.GetComponent<Text>().text = score.ToString();
#if UNITY_ANDROID && !UNITY_EDITOR
        touchAnimator.enabled = true;  // Activa animaciones táctiles solo en Android
#else
			touchAnimator.enabled = false;
#endif
		}

		// Procesa input continuo: botones táctiles o teclas A/D para rotación
		void Update()
		{
			if (buttonLeft_pressed)
			{
				RotateLeft();
			}
			else if (buttonRight_pressed)
			{
				RotateRight();
			}
			else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				RotateLeft();
			}
			else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				RotateRight();
			}
			score_value.GetComponent<Text>().text = score.ToString();  // Actualiza UI score cada frame
		}

		// Detecta presión botón izquierdo (down event)
		public void ButtonLeftDown()
		{
			buttonLeft_pressed = true;
		}

		// Detecta presión botón derecho (down event)
		public void ButtonRightDown()
		{
			buttonRight_pressed = true;
		}

		// Detecta liberación botón izquierdo (up event)
		public void ButtonLeftUp()
		{
			buttonLeft_pressed = false;
		}

		// Detecta liberación botón derecho (up event)
		public void ButtonRightUp()
		{
			buttonRight_pressed = false;
		}

		// Rota Kraken 200°/s a la izquierda (eje Z positivo)
		void RotateLeft()
		{
			if (kraken != null)
				kraken.gameObject.transform.Rotate(Vector3.forward * 200f * Time.deltaTime);
		}

		// Rota Kraken 200°/s a la derecha (eje Z negativo)
		void RotateRight()
		{
			if (kraken != null)
				kraken.gameObject.transform.Rotate(Vector3.back * 200f * Time.deltaTime);
		}
	}
}