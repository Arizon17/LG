using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authorization : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Button createButton;
    [SerializeField] private GameObject lobby;
    [SerializeField] private InputField inputField;
    [SerializeField] private GameObject errorNickNameLengthText;

    void Init()
    {
        createButton.onClick.AddListener((() =>
        {
            if (inputField.text.Length > 14)
            {
                StartCoroutine(ErrorNickLength());
                return;
            }
            lobby.GetComponent<LobbyManager>().Username = inputField.text.ToLower();
            lobby.gameObject.SetActive(true); 
            gameObject.SetActive(false);
        } ));
    }

    IEnumerator ErrorNickLength()
    {
        errorNickNameLengthText.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorNickNameLengthText.SetActive(false);
    }
    void Start()
    {
       Init(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
