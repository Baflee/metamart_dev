using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginMenu : MonoBehaviour
{
	[SerializeField] GameObject welcomePanel;
	[SerializeField] Text user;
	[Space]
	[SerializeField] InputField email;
	[SerializeField] InputField password;

	[SerializeField] Text errorMessages;


	[SerializeField] Button loginButton;

	
	[SerializeField] string url;

	WWWForm form;

	public void OnLoginButtonClicked ()
	{
		loginButton.interactable = false;
		StartCoroutine (Login ());
	}

	IEnumerator Login ()
	{
		form = new WWWForm ();

		form.AddField ("email", email.text);
		form.AddField ("password", password.text);

		WWW w = new WWW (url, form);
		yield return w;

		if (w.error != null) {
			errorMessages.text = "404 not found!";
			Debug.Log("<color=red>"+w.text+"</color>");//error
		} else {
			if (w.isDone) {
				if (w.text.Contains ("error")) {
					errorMessages.text = "invalid email or password!";
					Debug.Log("<color=red>"+w.text+"</color>");//error
				} else {
					//open welcom panel
					welcomePanel.SetActive (true);
					user.text = email.text;
					Debug.Log("<color=green>"+w.text+"</color>");//user exist
				}
			}
		}

		loginButton.interactable = true;

		w.Dispose ();
	}
}