using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Directory = System.IO.Directory;

#if true

namespace Tools
{
    public class CardDatabase : EditorWindow
    {
        // CardsTab in Tool UI
        private static List<CardData> cardDatabase = new List<CardData>();
        private VisualElement cardsTab;
        private static VisualTreeAsset cardRowTemplate;
        private ListView cardListView;
        // AttributesTab in Tool UI
        private ScrollView detailSection;
        private CardData activeCard; 
        private readonly float itemHeight = 40;
        // CardView in Tool UI
        private VisualElement cardViewContainer;
        private VisualElement cardInstance;
        private VisualTreeAsset cardUXML;
        // Live updates
        private SerializedObject serializedObject;
        
        [MenuItem("Tools/CardDatabase")]
        public static void Init()
        {
            CardDatabase window = GetWindow<CardDatabase>();
            window.titleContent = new GUIContent("CardDatabase");

            Vector2 size = new Vector2(1920, 1080);
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
            
            cardViewContainer = rootVisualElement.Q<VisualElement>("CardView");
            
            cardUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/Card UI/CardUIDocument.uxml");
            
            LoadAllCards();
            cardsTab = rootVisualElement.Q<VisualElement>("CardsTab");
            GenerateListView();

            rootVisualElement.Q<Button>("Btn_AddCard").clicked += AddCard_OnClick;
            rootVisualElement.Q<Button>("Btn_DeleteCard").clicked += DeleteCard_OnClick;
            detailSection = rootVisualElement.Q<ScrollView>("ScrollView_Details");
            detailSection.style.visibility = Visibility.Hidden;
        }

        private void RefreshUI()
        {
            if (activeCard == null) return;
            
            UpdateCardPreview(activeCard);
            cardListView.RefreshItems();
        }
        private void ShowCard(CardData card)
        {
            if (cardInstance != null)
                cardInstance.RemoveFromHierarchy();

            cardInstance = cardUXML.CloneTree();

            // Bind data manuellt
            cardInstance.Q<Label>("Card_Name_Label").text = card.CardName;
            cardInstance.Q<Label>("Card_Description_Label").text = card.Description;
            cardInstance.Q<Label>("Card_Cost_Label").text = card.Cost.ToString();
            cardInstance.Q<Label>("Card_Type_Label").text = card.CardType.ToString();

            var art = cardInstance.Q<VisualElement>("Artwork");
            if (card.Artwork != null) art.style.backgroundImage = new StyleBackground(card.Artwork);
            else art.style.backgroundImage = null;
            
            cardViewContainer.Clear();
            cardViewContainer.Add(cardInstance);
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
            cardListView = new ListView(cardDatabase, itemHeight, makeItem, bindItem);
            cardListView.selectionType = SelectionType.Single;
            cardListView.style.height = cardDatabase.Count * itemHeight + 5;
            cardsTab.Add(cardListView);

            cardListView.selectionChanged += ListView_selectionChanged;
        }

        private void ListView_selectionChanged(IEnumerable<object> selectedCards)
        {
            //safety check
            var card = selectedCards.FirstOrDefault();
            if (card == null) return;
            
            activeCard = (CardData)card;
            
            serializedObject = new SerializedObject(activeCard);
            
            detailSection.Unbind();
            detailSection.Bind(serializedObject);
            
            detailSection.TrackSerializedObjectValue(serializedObject, (obj) =>
            {
                Undo.RecordObject(activeCard, "Card Change");
                
                obj.ApplyModifiedProperties();
                RefreshUI();
            });
            
            detailSection.style.visibility = Visibility.Visible;
            ShowCard(activeCard);
        }

        private void UpdateCardPreview(CardData card)
        {
            if (cardInstance == null) return;
            
            cardInstance.Q<Label>("Card_Name_Label").text = card.CardName;
            cardInstance.Q<Label>("Card_Description_Label").text = card.Description;
            cardInstance.Q<Label>("Card_Cost_Label").text = card.Cost.ToString();
            cardInstance.Q<Label>("Card_Type_Label").text = card.CardType.ToString();
            
            var art = cardInstance.Q<VisualElement>("Artwork");

            if (card.Artwork != null)
            {
                art.style.backgroundImage =  new StyleBackground(card.Artwork);
            }
        }

        private void LoadAllCards()
        {
            cardDatabase.Clear();
            
            string [] guids = AssetDatabase.FindAssets("t:CardData");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
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
            CardData newCard = CreateInstance<CardData>();
            
            AssetDatabase.CreateAsset(newCard, $"Assets/Data/CardsData/{newCard.Id}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            cardDatabase.Add(newCard);

            cardListView.Rebuild();
            cardListView.style.flexGrow = 1;
        }

        private void DeleteCard_OnClick()
        {
            if (activeCard == null) return;
            string path = AssetDatabase.GetAssetPath(activeCard);
            AssetDatabase.DeleteAsset(path);
            cardDatabase.Remove(activeCard);
            activeCard = null;
            cardViewContainer.Clear();
            detailSection.Unbind();
            detailSection.style.visibility = Visibility.Hidden;
            cardListView.Rebuild();
        }
    }
}
#endif