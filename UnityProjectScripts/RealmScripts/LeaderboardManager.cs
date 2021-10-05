using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Realms;
using System.Linq;
using Realms.Sync;
using System.Threading.Tasks;
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    private Realm realm;
    private VisualElement root;
    private ListView listView;
    private Label displayTitle;
    private string username;
    private bool isLeaderboardUICreated = false;
    private List<Stat> topStats;
    private IDisposable listenerToken;  // (Part 2 Sync): listenerToken is the token for registering a change listener on all Stat objects

    #region PublicMethods
    // SetLoggedInUser() opens a realm, calls the CreateLeaderboardUI() method
    // to create the LeaderboardUI and adds it to the Root Component
    // SetLoggedInUser()  takes a userInput, representing a username, as a
    // parameter
    public void SetLoggedInUser(string userInput)

    {
        username = userInput;

        realm = Realm.GetInstance();

        // only create the leaderboard on the first run, consecutive
        // restarts/reruns will already have a leaderboard created
        if (isLeaderboardUICreated == false)
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            CreateLeaderboardUI();
            root.Add(displayTitle);
            root.Add(listView);
            isLeaderboardUICreated = true;
        }
    }
    #endregion

    #region PrivateMethods
    // CreateLeaderboardUI() creates a Leaderboard title for the UI and calls
    // CreateTopStatListView() to create a list of Stat objects with high scores
    private void CreateLeaderboardUI()
    {
        // create leaderboard title
        displayTitle = new Label();
        displayTitle.text = "Leaderboard:";
        displayTitle.AddToClassList("display-title");

        topStats = realm.All<Stat>().OrderByDescending(s => s.Score).ToList();
        CreateTopStatListView();
    }

    // CreateTopStatListView() creates a set of Labels containing high stats
    private void CreateTopStatListView()
    {
        int maximumAmountOfTopStats;
        // set the maximumAmountOfTopStats to 5 or less
        if (topStats.Count > 4)
        {
            maximumAmountOfTopStats = 5;
        }
        else
        {
            maximumAmountOfTopStats = topStats.Count;
        }


        var topStatsListItems = new List<string>();

        topStatsListItems.Add("Your top points: " + GetRealmPlayerTopStat());


        for (int i = 0; i < maximumAmountOfTopStats; i++)
        {
            // only display the top stats if they are greater than 0, and show
            // no top stats if there are none greater than 0
            if (topStats[i].Score > 1)
            {
                topStatsListItems.Add($"{topStats[i].StatOwner.Name}: {topStats[i].Score} points");
            }
        };
        // Create a new label for each top score
        var label = new Label();
        label.AddToClassList("list-item-game-name-label");
        Func<VisualElement> makeItem = () => new Label();

        // Bind Stats to the UI
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            (e as Label).text = topStatsListItems[i];
            (e as Label).AddToClassList("list-item-game-name-label");
        };

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 5;

        listView = new ListView(topStatsListItems, itemHeight, makeItem, bindItem);
        listView.AddToClassList("list-view");
    }

    // GetRealmPlayerTopStat() queries a realm for the player's Stat object with
    // the highest score
    private int GetRealmPlayerTopStat()
    {
        var realmPlayer = realm.All<Player>().Where(p => p.Name == username).First();
        var realmPlayerTopStat = realmPlayer.Stats.OrderByDescending(s => s.Score).First().Score;
        return realmPlayer.Stats.OrderByDescending(s => s.Score).First().Score;
    }



    #endregion

    #region UnityLifecycleMethods
    private void Awake()
    {
        Instance = this;
    }
    private void OnDisable()
    {
    }

    #endregion










}