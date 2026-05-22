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
        private VisualTreeAsset _cardTabContentTemplate;
        private VisualElement _createCardContainer;
        private VisualElement _cardListContainer;
        private Button _createCardButton;
        private static VisualTreeAsset cardRowTemplate;
        private VisualTreeAsset _foldoutTemplate;
        private ListView _cardListView;
        // AttributesTab in Tool UI
        private VisualTreeAsset _attributesTemplate;
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
            // Main tool UI
            var toolTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/Tool UI template.uxml");
            VisualElement rootFromUXML = toolTemplate.Instantiate();
            rootVisualElement.Add(rootFromUXML);
        
            // Styles
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>                        
                ("Assets/UI Toolkit/CardTool/CardToolStyles.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
            
            // Templates
            cardRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/CardRowTemplate.uxml");
            _attributesTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/AttributesTemplate_Details.uxml");
            _foldoutTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/FoldoutTemplate.uxml");
            _sceneView = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/SceneViewTemplate.uxml");
            _cardTabContentTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/CardTool/CardTabContent.uxml");
            // Root containers
            _cardsTab = rootVisualElement.Q<VisualElement>("CardsTab");
            _detailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details"); // Rename _detailSection
            _cardViewContainer = rootVisualElement.Q<VisualElement>("CardView");
            _sceneViewTab =  rootVisualElement.Q<VisualElement>("SceneView");
            
            // Instantiate templates
            if (_attributesTemplate != null)
            {
                var details = _attributesTemplate.Instantiate();
                _detailSection.Add(details);
            }

            //OLD 
            // if (_foldoutTemplate != null)
            // {
            //     var foldout =  _foldoutTemplate.Instantiate();
            //     _cardsTab.Add(foldout);
            // }
            //NEW
            if (_cardTabContentTemplate != null)
            {
                var content = _cardTabContentTemplate.Instantiate();
                _cardsTab.Add(content);

                var createCardRoot = content.Q<VisualElement>("CreateCard");
                var cardListRoot = content.Q<VisualElement>("CardList");

                _createCardContainer =
                    createCardRoot.Q<VisualElement>("ContentContainer");

                _cardListContainer =
                    cardListRoot.Q<VisualElement>("ContentContainer");

                _createCardButton = createCardRoot.Q<Button>();

                _createCardButton.clicked += AddCard_OnClick;
            }
            
            // Card UI
            _cardUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Card UI/CardUIDocument.uxml");
            
            LoadAllCards();
            GenerateFoldoutView();
            //GenerateListView();
            
            //rootVisualElement.Q<Button>("Btn_AddCard").clicked += AddCard_OnClick;
            //rootVisualElement.Q<Button>("Btn_DeleteCard").clicked += DeleteCard_OnClick;
            _detailSection.style.visibility = Visibility.Hidden;

            if (_activeCard == null && cardDatabase.Count > 0)
            {
                SelectCard(cardDatabase[0]);
            }
            
            _previewScene = EditorSceneManager.OpenScene("Assets/Scenes/PreviewScenes/AttackPreviewScene.unity",
                OpenSceneMode.Additive); // Remove?
            
            SetupPreview();
            
        }

        private void SetupPreview()
        {
            var root = _sceneView.Instantiate();
            _sceneViewTab.Add(root);
        }

        private void RefreshUI()
        {
            if (_activeCard == null) return;
            
            UpdateCardPreview(_activeCard);
            if (_previewController != null) _previewController.PreviewCard(_activeCard);
            //_cardListView.RefreshItems();
            GenerateFoldoutView();
        }
        private void ShowCard(CardData cardData)
        {
            if (_cardInstance != null)
                _cardInstance.RemoveFromHierarchy();
            var card = new Card(cardData);
            _cardInstance = new CardView(_cardUxml);
            _cardInstance.Bind(card);
            _cardInstance.style.scale = new Vector3(2, 2, 2);
            _cardViewContainer.Clear();
            _cardViewContainer.Add(_cardInstance);
            
        }

        private void GenerateFoldoutView()
{
    if (_createCardContainer == null ||
        _cardListContainer == null)
        return;

    _createCardContainer.Clear();
    _cardListContainer.Clear();

    GenerateNewCardsFoldout(_createCardContainer);

    foreach (CardType type in Enum.GetValues(typeof(CardType)))
    {
        var typeFoldout = new Foldout
        {
            text = type.ToString(),
            value = true
        };

        foreach (CardElement element in Enum.GetValues(typeof(CardElement)))
        {
            var elementFoldout = new Foldout
            {
                text = element.ToString(),
                value = false
            };

            for (int cost = 1; cost <= 3; cost++)
            {
                var costFoldout = new Foldout
                {
                    text = $"{cost}",
                    value = false
                };

                var cards = cardDatabase
                    .Where(c => !c.isDraft)
                    .Where(c =>
                        c.CardType == type &&
                        c.Element == element &&
                        c.Cost == cost)
                    .OrderBy(c => c.CardName)
                    .ToList();

                foreach (var card in cards)
                {
                    var row = cardRowTemplate.Instantiate();

                    row.Q<Label>("Name").text = card.CardName;

                    row.RegisterCallback<ClickEvent>(_ =>
                    {
                        SelectCard(card);
                    });

                    var deleteButton = row.Q<Button>("Delete");

                    deleteButton.clicked += () =>
                    {
                        DeleteCard(card);
                    };

                    costFoldout.Add(row);
                }

                if (cards.Count > 0)
                    elementFoldout.Add(costFoldout);
            }

            if (elementFoldout.childCount > 0)
                typeFoldout.Add(elementFoldout);
        }

        if (typeFoldout.childCount > 0)
            _cardListContainer.Add(typeFoldout);
    }
}
        /*
        private void GenerateFoldoutView()
        {
            var root = rootVisualElement.Q<VisualElement>("FoldoutRoot");

            if (root == null) return;
            
            root.Clear();

            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                var typeFoldout = new Foldout
                {
                    text =  type.ToString(),
                    value = true
                };

                foreach (CardElement element in Enum.GetValues(typeof(CardElement)))
                {
                    var elementFoldout = new Foldout
                    {
                        text = element.ToString(),
                        value = false
                    };
                    for (int cost = 1; cost <= 3; cost++)
                    {
                        var costFoldout = new Foldout
                        {
                            text = $"{cost}",
                            value = false
                        };

                        var cards = cardDatabase
                            .Where(c => c.Cost > 0)
                            .Where(c =>
                                c.CardType == type &&
                                c.Element == element &&
                                c.Cost == cost)
                            .OrderBy(c => c.CardName)
                            .ToList();

                        foreach (var card in cards)
                        {
                            var row = cardRowTemplate.Instantiate();

                            row.Q<Label>("Name").text = card.CardName;

                            row.RegisterCallback<ClickEvent>(_ =>
                            {
                                SelectCard(card);
                            });

                            var deleteButton = row.Q<Button>("Delete");

                            deleteButton.clicked += () =>
                            {
                                DeleteCard(card);
                            };

                            costFoldout.Add(row);
                        }

                        elementFoldout.Add(costFoldout);
                    }
                    
                    typeFoldout.Add(elementFoldout);
                }
                
                root.Add(typeFoldout);
            }
            GenerateNewCardsFoldout(root);
        }
        */
        private void GenerateNewCardsFoldout(VisualElement root)
        {
            var newCards = cardDatabase
                .Where(c => c.isDraft)
                .OrderBy(c => c.Id)
                .ToList();

            if (newCards.Count == 0)
                return;

            var newFoldout = new Foldout
            {
                text = "New Cards",
                value = true
            };

            foreach (var card in newCards)
            {
                var row = cardRowTemplate.Instantiate();

                row.Q<Label>("Name").text =
                    string.IsNullOrWhiteSpace(card.CardName)
                        ? $"New Card ({card.Id})"
                        : card.CardName;

                row.RegisterCallback<ClickEvent>(_ =>
                {
                    SelectCard(card);
                });

                var deleteButton = row.Q<Button>("Delete");

                deleteButton.clicked += () =>
                {
                    DeleteCard(card);
                };

                newFoldout.Add(row);
            }

            root.Add(newFoldout);
        }
        private void GenerateListView()
        {
            //Defining what each item will visually look like. In this case, the makeItem function is creating a clone of the ItemRowTemplate.
            Func<VisualElement> makeCard = () => cardRowTemplate.CloneTree();

            //Define the binding of each individual Item that is created. Specifically, 
            //it binds the Icon visual element to the scriptable object’s Icon property and the 
            //Name label to the FriendlyName property.
            Action<VisualElement, int> bindCard = (e, i) =>
            {
                var card = cardDatabase[i];
                
                if (card == null) return;
                //Ska korten ha en icon i listan?
                //e.Q<VisualElement>("Icon").style.backgroundImage = cardDatabase[i] == null ? _defaultCardIcon.texture :  cardDatabase[i].icon.texture;
                e.Q<Label>("Name").text = card.CardName;
            };

            //Create the listview and set various properties
            _cardListView = new ListView(cardDatabase, _itemHeight, makeCard, bindCard);
            _cardListView.selectionType = SelectionType.Single;
            _cardListView.style.height = cardDatabase.Count * _itemHeight + 5;
            _cardsTab.Add(_cardListView);

            _cardListView.selectionChanged += ListView_selectionChanged;
        }

        private void SelectCard(CardData card)
        {
            if (card == null)
                return;

            _activeCard = card;

            _serializedObject = new SerializedObject(_activeCard);

            _detailSection.Unbind();
            _detailSection.Bind(_serializedObject);
            
            _detailSection.style.visibility = Visibility.Visible;

            ShowCard(_activeCard);
            
            RegisterCardChangeTracking();
        }
        private void RegisterCardChangeTracking()
        {
            _detailSection.Unbind();
            _detailSection.Bind(_serializedObject);

            _detailSection.TrackSerializedObjectValue(_serializedObject, obj =>
            {
                Undo.RecordObject(_activeCard, "Card Change");

                obj.ApplyModifiedProperties();

                HandleDraftState(_activeCard);

                EditorUtility.SetDirty(_activeCard);

                RefreshUI();
            });
        }
        private void HandleDraftState(CardData card)
        {
            if (!card.isDraft)
                return;

            if (string.IsNullOrWhiteSpace(card.CardName))
                return;

            if (card.Cost <= 0)
                return;

            card.isDraft = false;
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

            newCard.isDraft = true;
            AssetDatabase.CreateAsset(newCard, $"Assets/Data/CardsData/{newCard.Id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            cardDatabase.Add(newCard);

            GenerateFoldoutView();
            
            SelectCard(newCard);
            // _cardListView.Rebuild();
            // _cardListView.style.flexGrow = 1;
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
        private void DeleteCard(CardData card)
        {
            if (card == null)
                return;

            var path = AssetDatabase.GetAssetPath(card);

            AssetDatabase.DeleteAsset(path);

            cardDatabase.Remove(card);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            GenerateFoldoutView();
            RefreshUI();
        }
    }
}
#endif