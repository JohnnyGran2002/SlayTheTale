using System;
using System.Collections.Generic;
using System.IO;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

namespace Tools
{
    public class CardDataTest : EditorWindow //Later name - CardDatabase
    {
        private Sprite _defaultCardIcon;
        private static List<Test> _cardDatabase = new List<Test>();
        private VisualElement _cardsTab;
        private static VisualTreeAsset cardRowTemplate;
        private ListView _cardListView;
        private float _cardHeight = 40;
        
        [UnityEditor.MenuItem("Tools/CardDataTest")]
        public static void Init()
        {
            CardDataTest window = GetWindow<CardDataTest>();
            window.titleContent = new GUIContent("CardDataTest");

            Vector2 size = new Vector2(1920, 1080);
            // window.minSize = size;
            window.maxSize = size;
        }
        
        public void CreateGUI()
        {
            var visualCard = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Tool UI.uxml");
            VisualElement rootFromUXML = visualCard.Instantiate();
            rootVisualElement.Add(rootFromUXML);
        
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>                        
                ("Assets/UI Toolkit/CardToolStyles.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
            
            cardRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Card UI/CardRowTemplate.uxml");
            _defaultCardIcon = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/com.unity.dt.app-ui/PackageResources/Icons/Regular/AddLayer.png", typeof(Sprite));
            
            LoadAllCards();
            Debug.Log(_cardDatabase.Count);
            _cardsTab = rootVisualElement.Q<VisualElement>("CardsTab");
            GenerateListView();
        }

        private void GenerateListView()
        {
            //Defining what each item will visually look like. In this case, the makeItem function is creating a clone of the ItemRowTemplate.
            Func<VisualElement> makeItem = () => cardRowTemplate.CloneTree();

            //Define the binding of each individual Item that is created. Specifically, 
            //it binds the Icon visual element to the scriptable object’s Icon property and the 
            //Name label to the FriendlyName property.
            Action<VisualElement, int> bindItem = (e, i) =>
            {
                e.Q<VisualElement>("Icon").style.backgroundImage = _cardDatabase[i] == null ? _defaultCardIcon.texture :  _cardDatabase[i].icon.texture;
                e.Q<Label>("Name").text = _cardDatabase[i].Name;
            };

            //Create the listview and set various properties
            _cardListView = new ListView(_cardDatabase, _cardHeight, makeItem, bindItem);
            _cardListView.selectionType = SelectionType.Single;
            _cardListView.style.height = _cardDatabase.Count * _cardHeight + 5;
            _cardsTab.Add(_cardListView);
        }

        private void LoadAllCards()
        {
            _cardDatabase.Clear();
            
            string [] allPaths = Directory.GetFiles("Assets/Data/CardsData/Test","*.asset", SearchOption.AllDirectories);

            foreach (string path in allPaths)
            {
                string cleanedPath = path.Replace("\\", "/");
                _cardDatabase.Add((Test)AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(Test)));
            }
        }
    }
}
