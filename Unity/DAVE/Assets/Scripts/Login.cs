﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject signUpPanel;

    public Button login;
    public Button signUp;

    public InputField username;
    public InputField password;

    public Text invalid;

    public Button backBtn;

    private GameObject startCanvas;

    void Start()
    {

        startCanvas = GameObject.Find("StartCanvas");
        login.onClick.AddListener(OnLoginClick);
        signUp.onClick.AddListener(OnSignUpClick);
        backBtn.onClick.AddListener(OnBackClick);

    }

    void OnBackClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Destroy(gameObject);
    }

    private void OnLoginClick()
    {
        StartCoroutine(LoginAuthentication());
    }

    private void OnSignUpClick()
    {

        loginPanel.SetActive(false);
        GameObject signUp = Instantiate(signUpPanel);
        signUp.transform.SetParent(startCanvas.transform, false);
        
    }

    IEnumerator LoginAuthentication()
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(password.text);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
        string hash = System.Text.Encoding.ASCII.GetString(data);
        
        bool confirmation = ConnectionManager.R.Db("root")
            .Table("instructors")
            .Contains(row => row.G("name").Eq(username.text)
            .And(row.G("password").Eq(hash))).Run(ConnectionManager.conn);
        yield return confirmation;

        if (confirmation == true)
        {
            ConnectionManager.auth = true;
            ConnectionManager.coordinator.SetInstructor(username.text);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ShowForFive());
        }
    }

    IEnumerator ShowForFive()
    {
        Text invalidPassword = Instantiate(invalid);
        invalidPassword.transform.SetParent(startCanvas.transform, false);
        invalidPassword.enabled = true;
        Debug.LogError("Passwords doesn't match or username is already in use.");
        yield return new WaitForSeconds(3f);
        invalidPassword.enabled = false;
    }

}
