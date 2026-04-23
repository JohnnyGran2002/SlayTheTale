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
        
        [UnityEditor.MenuItem("Tools/CardDataTest")]
        public static void Init()
        {
            CardDataTest window = GetWindow<CardDataTest>();
            window.titleContent = new GUIContent("CardDataTest");

            Vector2 size = new Vector2(1920, 1080);
            window.minSize = size;
            window.maxSize = size;
        }

        private static List<Test> _cardDatabase = new List<Test>();
        private VisualElement _cardsTab;
        private static VisualTreeAsset cardRowTemplate;
        private ListView _cardListView;
        private float _cardHeight = 40;
        public void OnGUI()
        {
            var visualCard = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Card UI/CardUIDocument.uxml");
            VisualElement rootFromUXML = visualCard.Instantiate();
            rootVisualElement.Add(rootFromUXML);
        
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>                        
                ("Assets/UI Toolkit/Card UI/CardUIDocument.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
            
            _defaultCardIcon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI Toolkit/UnityThemes/UnityDefaultRuntimeTheme.tss", typeof(Sprite));
            
            LoadAllCards();
            _cardsTab = rootVisualElement.Q<VisualElement>("CardsTab");
            GenerateListView();
        }

        private void GenerateListView()
        {
            Func<VisualElement> makeCard = () => cardRowTemplate.CloneTree();

            Action<VisualElement, int> bindCard = (e, i) =>
            {
                e.Q<VisualElement>("Icon").style.backgroundImage = 
                    _cardDatabase[i] == null ? _defaultCardIcon.texture : 
                    _cardDatabase[i].icon.texture;
                e.Q<Label>("Name").text = _cardDatabase[i].name;
            };
            
            _cardListView = new ListView(_cardDatabase, 35, makeCard, bindCard)
            {
                selectionType = SelectionType.Single,
                style =
                {
                    height = _cardDatabase.Count * _cardHeight
                }
            }; 
            _cardsTab.Add(_cardListView);
        }

        private void LoadAllCards()
        {
            _cardDatabase.Clear();
            
            string [] allPaths = Directory.GetFiles("Assets/Data/CardsData","*.asset", SearchOption.AllDirectories);

            foreach (string path in allPaths)
            {
                string cleanedPath = path.Replace("\\", "/");
                _cardDatabase.Add((Test)AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(Test)));
            }
        }
    }
}
