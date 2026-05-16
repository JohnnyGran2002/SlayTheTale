using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

#if true

namespace Editor
{
    public class CardDatabase : EditorWindow
    {
        // CardsTab in Tool UI
        private static List<CardData> cardDatabase = new List<CardData>();
        private VisualElement _cardsTab;
        private static VisualTreeAsset cardRowTemplate;
        private ListView _cardListView;
        // AttributesTab in Tool UI
        private ScrollView _detailSection;
        private CardData _activeCard; 
        private readonly float _itemHeight = 40;
        // CardView in Tool UI
        private VisualElement _cardViewContainer;
        private CardView _cardInstance;
        private VisualTreeAsset _cardUxml;
        // Live updates
        private SerializedObject _serializedObject;
        //Scene view
        private VisualElement _sceneViewTab;
        private VisualTreeAsset _sceneView;
        private Scene _previewScene;
        private AttackPreviewController  _previewController;
        
        [MenuItem("Tools/CardDatabase")]
        public static void Init()
        {
            var window = GetWindow<CardDatabase>();
            window.titleContent = new GUIContent("CardDatabase");

            var size = new Vector2(1920, 1080);
            window.maxSize = size;
        }
        public void CreateGUI()
        {
            var visualCard = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/Tool UI template.uxml");
            VisualElement rootFromUXML = visualCard.Instantiate();
            rootVisualElement.Add(rootFromUXML);
        
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>                        
                ("Assets/UI Toolkit/CardTool/CardToolStyles.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
            
            cardRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/CardRowTemplate.uxml");
            _sceneView = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/SceneViewTemplate.uxml");
            //Test {
            // var tabView = rootVisualElement.Q<TabView>();
            // var contentContainer =
            //     tabView.Q<VisualElement>(
            //         className: "unity-tab-view__content-container");
            //
            // contentContainer.style.justifyContent = Justify.Center;
            // contentContainer.style.alignItems = Align.Center;
            // }
            
            _cardViewContainer = rootVisualElement.Q<VisualElement>("CardView");
            _sceneViewTab =  rootVisualElement.Q<VisualElement>("SceneView");
            
            _cardUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Card UI/CardUIDocument.uxml");
            
            LoadAllCards();
            _cardsTab = rootVisualElement.Q<VisualElement>("CardsTab");
            GenerateListView();

            rootVisualElement.Q<Button>("Btn_AddCard").clicked += AddCard_OnClick;
            rootVisualElement.Q<Button>("Btn_DeleteCard").clicked += DeleteCard_OnClick;
            _detailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");
            _detailSection.style.visibility = Visibility.Hidden;

            _previewScene = EditorSceneManager.OpenScene("Assets/Scenes/PreviewScenes/AttackPreviewScene.unity",
                OpenSceneMode.Additive);
            
            SetupPreview();
            
        }
        // Test {
        
        private T FindInPreviewScene<T>() where T : Component
        {
            foreach (GameObject root in _previewScene.GetRootGameObjects())
            {
                T result = root.GetComponentInChildren<T>(true);

                if (result != null)
                    return result;
            }

            return null;
        }

        private void SetupPreview()
        {
            // _previewController = FindInPreviewScene<AttackPreviewController>();
            // if (_previewController == null) return;
            // _previewController.Initialize();
            // Image previewImage = new Image();
            // previewImage.image = _previewController.RenderTexture;
            // previewImage.scaleMode = ScaleMode.ScaleToFit;
            //
            // previewImage.style.flexGrow = 1;
            // previewImage.style.width = Length.Percent(100);
            // previewImage.style.height = Length.Percent(100);
            var root = _sceneView.Instantiate();
            _sceneViewTab.Add(root);
        }
        // }

        private void RefreshUI()
        {
            if (_activeCard == null) return;
            
            UpdateCardPreview(_activeCard);
            _previewController.PreviewCard(_activeCard);
            _cardListView.RefreshItems();
        }
        private void ShowCard(CardData cardData)
        {
            if (_cardInstance != null)
                _cardInstance.RemoveFromHierarchy();
            var card = new Card(cardData);
            _cardInstance = new CardView(_cardUxml);
            _cardInstance.Bind(card);
            _cardViewContainer.Clear();
            _cardViewContainer.Add(_cardInstance);
            
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
                var card = cardDatabase[i];
                
                if (card == null) return;
                //Ska korten ha en icon i listan?
                //e.Q<VisualElement>("Icon").style.backgroundImage = cardDatabase[i] == null ? _defaultCardIcon.texture :  cardDatabase[i].icon.texture;
                e.Q<Label>("Name").text = card.CardName;
            };

            //Create the listview and set various properties
            _cardListView = new ListView(cardDatabase, _itemHeight, makeItem, bindItem);
            _cardListView.selectionType = SelectionType.Single;
            _cardListView.style.height = cardDatabase.Count * _itemHeight + 5;
            _cardsTab.Add(_cardListView);

            _cardListView.selectionChanged += ListView_selectionChanged;
        }

        private void ListView_selectionChanged(IEnumerable<object> selectedCards)
        {
            //safety check
            var card = selectedCards.FirstOrDefault();
            if (card == null) return;
            
            _activeCard = (CardData)card;
            
            _serializedObject = new SerializedObject(_activeCard);
            
            _detailSection.Unbind();
            _detailSection.Bind(_serializedObject);
            
            _detailSection.TrackSerializedObjectValue(_serializedObject, (obj) =>
            {
                Undo.RecordObject(_activeCard, "Card Change");
                
                obj.ApplyModifiedProperties();
                RefreshUI();
            });
            
            _detailSection.style.visibility = Visibility.Visible;
            ShowCard(_activeCard);
        }

        private void UpdateCardPreview(CardData cardData)
        {
            if (_cardInstance == null) return;
            var card = new Card(cardData);
            _cardInstance.Bind(card);
        }

        private void LoadAllCards()
        {
            cardDatabase.Clear();
            
            var guids = AssetDatabase.FindAssets("t:CardData");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var card =  AssetDatabase.LoadAssetAtPath<CardData>(path);
                if (card != null)
                    cardDatabase.Add(card);
                //string [] allPaths = Directory.GetFiles("Assets/Data/CardsData","*.asset", SearchOption.AllDirectories);
                // string cleanedPath = path.Replace("\\", "/");
                // cardDatabase.Add((CardData)AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(CardData)));
            }
        }

        private void AddCard_OnClick()
        {
            var newCard = CreateInstance<CardData>();
            
            AssetDatabase.CreateAsset(newCard, $"Assets/Data/CardsData/{newCard.Id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            cardDatabase.Add(newCard);

            _cardListView.Rebuild();
            _cardListView.style.flexGrow = 1;
        }

        private void DeleteCard_OnClick()
        {
            if (_activeCard == null) return;
            var path = AssetDatabase.GetAssetPath(_activeCard);
            AssetDatabase.DeleteAsset(path);
            cardDatabase.Remove(_activeCard);
            _activeCard = null;
            _cardViewContainer.Clear();
            _detailSection.Unbind();
            _detailSection.style.visibility = Visibility.Hidden;
            _cardListView.Rebuild();
        }
    }
}
#endif