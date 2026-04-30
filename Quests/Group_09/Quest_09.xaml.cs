namespace FindShaulsTreasure.Quests.Group_09;

public partial class Quest_09 : BaseQuestView
{
    
    private string[] words =
    {
        "Apple", "Banana", "Orange", "Grape",
        "Dog", "Cat", "Horse", "Cow",
        "Red", "Blue", "Green", "Yellow",
        "Car", "Bus", "Train", "Bike"
    };

    private string[,] groups =
    {
        { "Apple", "Banana", "Orange", "Grape" },
        { "Dog", "Cat", "Horse", "Cow" },
        { "Red", "Blue", "Green", "Yellow" },
        { "Car", "Bus", "Train", "Bike" }d
    };

    private Button[] buttons = new Button[16];
    private Button[] selectedButtons = new Button[4];
    private int selectedCount = 0;

    private bool[] groupSolved = new bool[4];

    public Quest_09(int teamID) : base(teamID)
    {
        Data = new Models.QuestInfo("Example Quest", "Example Hint", Models.QuestType.Automatic);

        InitializeComponent();
        CreateGrid();
    }

    private void CreateGrid()
    {
        WordsGrid.RowDefinitions.Clear();
        WordsGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < 4; i++)
        {
            WordsGrid.RowDefinitions.Add(new RowDefinition());
            WordsGrid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        string[] shuffled = Shuffle(words);

        int index = 0;

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                Button btn = new Button();
                btn.Text = shuffled[index];
                btn.BackgroundColor = Colors.LightGray;
                btn.TextColor = Colors.Black;

                btn.Clicked += OnWordClicked;

                buttons[index] = btn;
                WordsGrid.Add(btn, col, row);

                index++;
            }
        }
    }

    private string[] Shuffle(string[] array)
    {
        string[] result = new string[array.Length];
        array.CopyTo(result, 0);

        Random rnd = new Random();

        for (int i = 0; i < result.Length; i++)
        {
            int j = rnd.Next(result.Length);

            string temp = result[i];
            result[i] = result[j];
            result[j] = temp;
        }

        return result;
    }

    private void OnWordClicked(object sender, EventArgs e)
    {
        Button btn = (Button)sender;

        int i;

        // Check if already selected
        for (i = 0; i < selectedCount; i++)
        {
            if (selectedButtons[i] == btn)
            {
                selectedButtons[i].BackgroundColor = Colors.LightGray;

                // shift left
                int j;
                for (j = i; j < selectedCount - 1; j++)
                {
                    selectedButtons[j] = selectedButtons[j + 1];
                }

                selectedCount--;
                return;
            }
        }

        // Add selection
        if (selectedCount < 4)
        {
            selectedButtons[selectedCount] = btn;
            btn.BackgroundColor = Colors.Orange;
            selectedCount++;
        }
    }

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        if (selectedCount != 4)
        {
            StatusLabel.Text = "Select 4 words!";
            return;
        }

        string[] selectedWords = new string[4];

        for (int i = 0; i < 4; i++)
        {
            selectedWords[i] = selectedButtons[i].Text;
        }

        int groupIndex;

        for (groupIndex = 0; groupIndex < 4; groupIndex++)
        {
            if (groupSolved[groupIndex])
                continue;

            if (IsMatch(groupIndex, selectedWords))
            {
                StatusLabel.Text = "Correct group!";

                for (int i = 0; i < 4; i++)
                {
                    selectedButtons[i].IsVisible = false;
                }

                groupSolved[groupIndex] = true;
                selectedCount = 0;

                if (AllGroupsSolved())
                {
                    StatusLabel.Text = "You won!";
                }

                return;
            }
        }

        StatusLabel.Text = "Wrong group!";
        ClearSelection();
    }

    private bool IsMatch(int groupIndex, string[] selected)
    {
        int matchCount = 0;

        for (int i = 0; i < 4; i++)
        {
            int j;
            for (j = 0; j < 4; j++)
            {
                if (groups[groupIndex, i] == selected[j])
                {
                    matchCount++;
                    break;
                }
            }
        }

        return matchCount == 4;
    }

    private void ClearSelection()
    {
        for (int i = 0; i < selectedCount; i++)
        {
            selectedButtons[i].BackgroundColor = Colors.LightGray;
        }

        selectedCount = 0;
    }

    private bool AllGroupsSolved()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!groupSolved[i])
                return false;
        }

        return true;
    }
}
