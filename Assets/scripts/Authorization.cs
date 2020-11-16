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

    void Init()
    {
        createButton.onClick.AddListener((() =>
        {
            lobby.GetComponent<LobbyManager>().Username = inputField.text;
            lobby.transform.parent.gameObject.SetActive(true); 
            gameObject.SetActive(false);
        } ));
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
