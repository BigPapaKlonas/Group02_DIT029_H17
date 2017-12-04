using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{
    public GameObject signUpPanel;
    public GameObject loginPanel;

    public Text invalid;

    public Button registerBtn;
    public InputField username;
    public InputField password1;
    public InputField password2;

    public Button backBtn;

    private GameObject startCanvas;

    private bool existingUsername;

    /*
     * Adding listeners to our buttons and finding the Canvas.
     */
    private void Start()
    {
        registerBtn.onClick.AddListener(OnSignUpClick);
        backBtn.onClick.AddListener(OnBackClick);

        startCanvas = GameObject.Find("StartCanvas");
    }

    void OnBackClick()
    {
        GameObject login = Instantiate(loginPanel);
        login.transform.SetParent(startCanvas.transform, false);
        Destroy(gameObject);
    }

    private void OnSignUpClick()
    {
        StartCoroutine(SignUpAuth());
    }

    /*
    * Sign up with username, and 2 password fields. 
    * Checking if the username exists, and if the passwords max and >= 6 characters.
    */
    IEnumerator SignUpAuth()
    {

        existingUsername = ConnectionManager.R.Db("root").Table("instructors").GetField("name")
            .Contains(username.text.ToLower()).Run(ConnectionManager.conn);

        yield return existingUsername;

        if (password1.text == password2.text && existingUsername != true && username.text != "" && password1.text.Length >= 6)
        {
            // Hashing passwords using SHA256.
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password1.text);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            string hash = System.Text.Encoding.ASCII.GetString(data);

            // Inserting a new Instructor.
            var insert =
                ConnectionManager.R.Db("root")
                .Table("instructors").Insert(ConnectionManager.R.Array(
                    ConnectionManager.R.HashMap("name", username.text.ToLower())
                    .With("password", hash)
                    .With("diagrams", ConnectionManager.R.Array())
                )
                )
                .Run(ConnectionManager.conn);
            yield return insert;
            if (insert.ToString() == "failure")
            {
                Debug.LogError("Error at Insert.");
            }
            else
            {
                // Starting an authicated session and setting the current instructor to username.
                ConnectionManager.auth = true;
                ConnectionManager.coordinator.SetInstructor(username.text.ToLower());
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Destroy(gameObject);
            }

        }
        else
        {
            StartCoroutine (ShowInvalidText());
        }

    }

    /*
    * Listening for invalidity in passwords or username.
    */
    IEnumerator ShowInvalidText()
    {
        Text invalidPassword = Instantiate(invalid);
        invalidPassword.transform.SetParent(startCanvas.transform, false);
        invalidPassword.enabled = true;
        if (existingUsername == true)
        {
            Debug.LogError("Username already in use.");
            invalidPassword.text = "Username already in use.";
        }
        else if (password1.text != password2.text)
        {
            Debug.LogError("Passwords does not match.");
            invalidPassword.text = "Passwords does not match.";
        }
        else if (password1.text.Length <= 6)
        {
            Debug.LogError("Password have to have atleast 6 characters.");
            invalidPassword.text = "Password have to have atleast 6 characters.";
        }
        
        yield return new WaitForSeconds(3f);
        invalidPassword.enabled = false;
    }
}

