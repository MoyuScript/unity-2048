using UnityEngine;
using UnityEngine.UIElements;
public class Ui {
    private static UIDocument _uiDocument;
    public static UIDocument UiDocument {
        get {
            if (_uiDocument == null) {
                _uiDocument = GameObject.Find("UIDocument").GetComponent<UIDocument>();
            }
            return _uiDocument;
        }
    }
}