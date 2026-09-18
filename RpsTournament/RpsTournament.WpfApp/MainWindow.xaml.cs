// Connects tools for working with lists and data collections
using System.Collections.Generic;

// Main WPF graphical interface library. It handles the application window itself, standard buttons, message boxes, and basic commands
using System.Windows;

// Additional library for UI controls. Provides access to elements like text boxes, buttons, tables, and text blocks
using System.Windows.Controls;

// Library for working with graphics, colors, brushes, and fonts (e.g., changing button colors)
using System.Windows.Media;

// This module contains the core game rules and logic
using RpsTournament.Core;

namespace RpsTournament.WpfApp
{
    // Window — declaration of the main MainWindow class
    // 'partial' means the window's code is split between this C# file and the XAML markup
    // The colon (:) means inheritance: our window inherits all base functionality from the standard WPF Window system class
    public partial class MainWindow : Window
    {
        // List to store all played rounds of the game - used to display history in the DataGrid table
        private List<GameRound> _rounds = new List<GameRound>();

        // Counters for tracking the player's wins, losses, and draws
        private int _wins = 0;
        private int _losses = 0;
        private int _draws = 0;

        // Variable that remembers the player's current choice (Rock, Paper, or Scissors)
        // The question mark (?) means the data type can be empty (null) if the player hasn't clicked any move button yet
        private Move? _selectedMove = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Event handler that triggers automatically every time the user changes text in the player name field (PlayerNameTextBox)
        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Checks the length of the entered text. The .Trim() function removes accidental whitespace at the edges
            if (PlayerNameTextBox.Text.Trim().Length >= 2)
            {
                // Internal check: if the red error message stating that the name is required is currently displayed on the screen
                if (StatusTextBlock.Text == "Nimi on kohustuslik!")
                {
                    // ...then the program automatically clears this error (string.Empty makes the text empty), since the player has started fixing their name and entered a sufficient number of characters
                    StatusTextBlock.Text = string.Empty;
                }
            }
        }

        // Click event handler shared across all three move selection buttons.
        // The sender parameter points to the specific button that the user just clicked.
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Safe check: ensuring the object that triggered the event is indeed a Button, and its Tag property (containing "Rock", "Paper", or "Scissors") holds a string text.
            if (sender is Button button && button.Tag is string moveStr)
            {
                // System.Enum.TryParse<Move> is a built-in .NET framework method for working with enumerations (Enums).
                // <Move> specifies the data type the method should attempt to convert the text into.
                // moveStr is the input parameter, the exact text extracted from the button's tag (e.g., "Rock" or "Paper").
                // out Move parsedMove is a special parameter using the 'out' keyword - if text conversion succeeds, a new variable named parsedMove is created on the fly and the result is stored there.
                if (System.Enum.TryParse<Move>(moveStr, out Move parsedMove))
                {
                    // Saves the selected move into the global _selectedMove variable
                    _selectedMove = parsedMove;

                    // Reset colors of all buttons:
                    // Before highlighting the selected button, the code first restores all three buttons to their standard blue background and white text colors.
                    RockButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1D4ED8")!;
                    PaperButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2563EB")!;
                    ScissorsButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3B82F6")!;

                    // Highlighting the clicked button:
                    RockButton.Foreground = Brushes.White;
                    PaperButton.Foreground = Brushes.White;
                    ScissorsButton.Foreground = Brushes.White;

                    button.Background = new SolidColorBrush(Colors.Gold);
                    button.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void PlayRoundButton_Click(object sender, RoutedEventArgs e)
        {
            // Reads what the user typed in the name field
            string playerName = PlayerNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(playerName) || playerName.Length < 2)
            {
                StatusTextBlock.Text = "Nimi on kohustuslik!";
                return;
            }
            // Checks if the player has selected a move. If the variable is null, it means the button was not clicked.
            if (_selectedMove == null)
            {
                StatusTextBlock.Text = "Vali käik!";
                return;
            }

            // If the name and move are selected correctly, all past error messages are cleared from the screen.
            StatusTextBlock.Text = string.Empty;

            // The specific move value of the player is retrieved from our "wrapper" (_selectedMove.Value).
            Move playerMove = _selectedMove.Value;

            // Access our external core (GameLogic) so the computer randomly chooses its move.
            Move computerMove = GameLogic.GetComputerMove();

            // Pass both moves to the core, which calculates the result based on the game rules
            RoundResult result = GameLogic.GetResult(playerMove, computerMove);

            // The number of the current round (total played plus one), player's move, computer's move, and result are recorded.
            GameRound round = new GameRound
            {
                Number = _rounds.Count + 1,
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Result = result
            };

            // This new round is added to the general game history list.
            _rounds.Add(round);

            // Access the interface table so it updates and shows the fresh row with the newly played round.
            RoundsDataGrid.ItemsSource = null;
            RoundsDataGrid.ItemsSource = _rounds;

            // Score calculation
            if (result == RoundResult.Win) _wins++;
            else if (result == RoundResult.Loss) _losses++;
            else _draws++;

            ScoreTextBlock.Text = $"Võidud: {_wins} | Kaotused: {_losses} | Viigid: {_draws}";

            // Tournament completion check (5 rounds)
            if (_rounds.Count >= 5)
            {
                string winnerMessage = _wins > _losses ? $"Mängija {playerName} võitis turniiri!" :
                                       _wins < _losses ? "Arvuti võitis turniiri!" : "Turniir lõppes viigiga!";

                MessageBox.Show(winnerMessage, "Turniiri tulemus", MessageBoxButton.OK, MessageBoxImage.Information);

                // Automatically calls the game reset method to start a new tournament with a clean slate.
                NewTournamentButton_Click(sender, e);
            }
        }

        // Declaration of the event handler that runs when the button to start a new tournament is clicked
        private void NewTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            // Clears the list of all rounds (_rounds), removing the entire game history.
            _rounds.Clear();

            // Resets the player's win, loss, and draw counters back to zero.
            _wins = 0;
            _losses = 0;
            _draws = 0;

            // Resets the player's move selection to an empty (null) state so that the old selection does not carry over to the new tournament.
            _selectedMove = null;

            // Resets the rounds history table data source to clear its visual display on the screen.
            RoundsDataGrid.ItemsSource = null;

            // Returns the score block text to its initial state (all metrics at zero).
            ScoreTextBlock.Text = "Võidud: 0 | Kaotused: 0 | Viigid: 0";

            // Clears any error or status texts on the screen.
            StatusTextBlock.Text = string.Empty;

            // Makes the round start button (PlayRoundButton) active
            PlayRoundButton.IsEnabled = true;

            // Reset background colors of the move selection buttons:
            RockButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1D4ED8")!;
            PaperButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2563EB")!;
            ScissorsButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3B82F6")!;

            // Reset text colors of the move selection buttons:
            RockButton.Foreground = Brushes.White;
            PaperButton.Foreground = Brushes.White;
            ScissorsButton.Foreground = Brushes.White;
        }

        // An event that automatically triggers for each column of the DataGrid table when it is being created or updated.
        // The e parameter contains information about the current column. (GameTypes.cs)
        private void RoundsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // If the system property name of the column is "Number" (round number), the header of this column on the screen is changed to the Estonian word "Voor" (Round).
            if (e.PropertyName == "Number") e.Column.Header = "Voor";

            // If the property is named "PlayerMove" (player's move), the header is changed to "Mängija käik".
            else if (e.PropertyName == "PlayerMove") e.Column.Header = "Mängija käik";

            // If the property is named "ComputerMove" (computer's move), the header is changed to "Arvuti käik".
            else if (e.PropertyName == "ComputerMove") e.Column.Header = "Arvuti käik";

            // If the property is named "Result" (round result), the header is changed to "Tulemus" (Result).
            else if (e.PropertyName == "Result") e.Column.Header = "Tulemus";
        }
    }
}