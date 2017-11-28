using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{
    public GameObject signUpPanel;
    public GameObject loginPanel;

    public Button registerBtn;
    public InputField username;
    public InputField password1;
    public InputField password2;

    public Button backBtn;

    private void Start()
    {
        registerBtn.onClick.AddListener(OnSignUpClick);
        backBtn.onClick.AddListener(OnBackClick);
    }

    void OnBackClick()
    {
        loginPanel.SetActive(true);
        Destroy(gameObject);
    }

    private void OnSignUpClick()
    {
        StartCoroutine(SignUpAuth());
    }

    IEnumerator SignUpAuth()
    {

        bool existingUsername = ConnectionManager.R.Db("root").Table("instructors").GetField("name")
            .Contains(username.text).Run(ConnectionManager.conn);

        yield return existingUsername;

        if (password1.text == password2.text && existingUsername != true)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password1.text);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hash = System.Text.Encoding.ASCII.GetString(data);

            var insert =
                ConnectionManager.R.Db("root")
                .Table("instructors").Insert(ConnectionManager.R.Array(
                    ConnectionManager.R.HashMap("name", username.text)
                    .With("password", hash)
                    .With("diagrams", ConnectionManager.R.Array())
                )
                )
                .Run(ConnectionManager.conn);
            yield return insert;
            if (insert.ToString() == "")
            {
                Debug.LogError("Error at Insert.");
            }
            else
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Destroy(gameObject);
            }

        }

    }
}

