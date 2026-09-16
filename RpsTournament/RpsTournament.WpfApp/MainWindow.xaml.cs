using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RpsTournament.Core;

namespace RpsTournament.WpfApp
{
    public partial class MainWindow : Window
    {
        private List<GameRound> _rounds = new List<GameRound>();
        private int _wins = 0;
        private int _losses = 0;
        private int _draws = 0;

        private Move? _selectedMove = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PlayerNameTextBox.Text.Trim().Length >= 2)
            {
                if (StatusTextBlock.Text == "Nimi on kohustuslik!")
                {
                    StatusTextBlock.Text = string.Empty;
                }
            }
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string moveStr)
            {
                if (System.Enum.TryParse<Move>(moveStr, out Move parsedMove))
                {
                    _selectedMove = parsedMove;

                    RockButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1D4ED8")!;
                    PaperButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2563EB")!;
                    ScissorsButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3B82F6")!;

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
            string playerName = PlayerNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(playerName) || playerName.Length < 2)
            {
                StatusTextBlock.Text = "Nimi on kohustuslik!";
                return;
            }

            if (_selectedMove == null)
            {
                StatusTextBlock.Text = "Vali käik!";
                return;
            }

            StatusTextBlock.Text = string.Empty;

            Move playerMove = _selectedMove.Value;
            Move computerMove = GameLogic.GetComputerMove();
            RoundResult result = GameLogic.GetResult(playerMove, computerMove);

            GameRound round = new GameRound
            {
                Number = _rounds.Count + 1,
                PlayerMove = playerMove,
                ComputerMove = computerMove,
                Result = result
            };

            _rounds.Add(round);
            RoundsDataGrid.ItemsSource = null;
            RoundsDataGrid.ItemsSource = _rounds;

            if (result == RoundResult.Win) _wins++;
            else if (result == RoundResult.Loss) _losses++;
            else _draws++;

            ScoreTextBlock.Text = $"Võidud: {_wins} | Kaotused: {_losses} | Viigid: {_draws}";

            if (_rounds.Count >= 5)
            {
                string winnerMessage = _wins > _losses ? $"Mängija {playerName} võitis turniiri!" :
                                       _wins < _losses ? "Arvuti võitis turniiri!" : "Turniir lõppes viigiga!";

                MessageBox.Show(winnerMessage, "Turniiri tulemus", MessageBoxButton.OK, MessageBoxImage.Information);

                NewTournamentButton_Click(sender, e);
            }
        }

        private void NewTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            _rounds.Clear();
            _wins = 0;
            _losses = 0;
            _draws = 0;
            _selectedMove = null;

            RoundsDataGrid.ItemsSource = null;
            ScoreTextBlock.Text = "Võidud: 0 | Kaotused: 0 | Viigid: 0";
            StatusTextBlock.Text = string.Empty;
            PlayRoundButton.IsEnabled = true;

            RockButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1D4ED8")!;
            PaperButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2563EB")!;
            ScissorsButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3B82F6")!;

            RockButton.Foreground = Brushes.White;
            PaperButton.Foreground = Brushes.White;
            ScissorsButton.Foreground = Brushes.White;
        }

        private void RoundsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Number") e.Column.Header = "Voor";
            else if (e.PropertyName == "PlayerMove") e.Column.Header = "Mängija käik";
            else if (e.PropertyName == "ComputerMove") e.Column.Header = "Arvuti käik";
            else if (e.PropertyName == "Result") e.Column.Header = "Tulemus";
        }
    }
}