using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace TicTac.ViewModels
{
    /// <summary>
    /// Maintains playerturn, winning condition, resetting game
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        bool player1Turn = true;
        bool GameEnded = false;
        int turns = 0;

        #region Game Move Logic

        private DelegateCommand<string> _playerMove;
        public DelegateCommand<string> PlayerMove =>
            _playerMove ?? (_playerMove = new DelegateCommand<string>(ExecutePlayerMove, CanExecutePlayerMove));

        void ExecutePlayerMove(string parameter)
        {
            int index = Convert.ToInt16(parameter);
            PrintMoveOnButton(index);

            GameEnded = CheckGameWonByCurrentPlayer();
            if (GameEnded == true)
            {
                PrintWinningMessage();
                return;
            }
            player1Turn = !player1Turn;
            checkMatchDrawn();

        } 
        #endregion


        //Choosing Mark on button depending on which player is playing
        //On the gameboard player 1 move will be shown as X and for player 2 O
        private void PrintMoveOnButton(int index)
        {
            if (player1Turn)
                ButtonContent[index] = "X";
            else
                ButtonContent[index] = "O";
        }

        //Draw condition, if turns has become 9 than all the game block are filled 
        //and yet no result so draw
        private void checkMatchDrawn()
        {
            turns += 1;
            if (turns == 9)
            {
                GameEnded = true;
                WinningMessage = "Game Drawn!!!";
            }
        }

        //In the textblock printing message will be shown
        void PrintWinningMessage()
        {
            string winner = "1";
            if (!player1Turn)
            {
                winner = "2";
            }
            WinningMessage = string.Format("Player {0} has won the Match", winner);
        }

        private string _winningMessage;
        public string WinningMessage
        {
            get { return _winningMessage; }
            set 
            { 
                SetProperty(ref _winningMessage, value);
                PlayerMove.RaiseCanExecuteChanged();
            }
        }

        //takine three blocks of game and checking if they have same non-empty content
        private bool AllThreeElementSame(int i, int j, int k)
        {

            if ((ButtonContent[i] == ButtonContent[j] && ButtonContent[j] == ButtonContent[k]) && (ButtonContent[i] == "X" || ButtonContent[i] == "O"))
            {
                return true;
            }
            return false;
        }

        //Checking winning game condition
        private bool CheckGameWonByCurrentPlayer()
        {
            //Checking Any row having winning Combination
            if (AllThreeElementSame(0, 1, 2) || AllThreeElementSame(3, 4, 5) || AllThreeElementSame(6, 7, 8))
                return true;

            //Checking Any column having winning Combination
            if (AllThreeElementSame(0, 3, 6) || AllThreeElementSame(1, 4, 7) || AllThreeElementSame(2, 5, 8))
                return true;

            //Checking Any DIagonal having winning Combination
            if (AllThreeElementSame(0, 4, 8) || AllThreeElementSame(2, 4, 6))
                return true;

            return false;
        }

        //If there game is ended or block has been played firstly (has X or O)
        //then that move is not allowed

        bool CanExecutePlayerMove(string parameter)
        {
            int index = Convert.ToInt16(parameter);
            if (GameEnded == true || ButtonContent[index] != "")
                return false;

            return true;
        }

        #region Close Application
        private DelegateCommand<Window> _exitApplication;
        public DelegateCommand<Window> ExitApplication =>
            _exitApplication ?? (_exitApplication = new DelegateCommand<Window>(ExecuteExitApplication));

        void ExecuteExitApplication(Window window)
        {
            if (window != null)
                window.Close();
        }
        #endregion


        #region Reset Game
        private DelegateCommand _resetGame;
        public DelegateCommand ResetGame =>
            _resetGame ?? (_resetGame = new DelegateCommand(ExecuteResetGame));

        void ExecuteResetGame()
        {
            ButtonContent.Clear();
            populateButtonContent();
            WinningMessage = "";
            turns = 0;
            GameEnded = false;
            player1Turn = true;
        } 
        #endregion


        // This collection will directly map to 9 blocks (or button) in the game

        private ObservableCollection<string> _buttonContent;
        public ObservableCollection<string> ButtonContent
        {
            get { return _buttonContent; }
            set 
            { 
                SetProperty(ref _buttonContent, value);
                PlayerMove.RaiseCanExecuteChanged();
            }
        }

//Since it is beginnning of the game, Intially all button/block will have no content
        private void populateButtonContent()
        {
            for (int i = 0; i < 9; i++)
            {
                ButtonContent.Add("");
            }
        }


        public MainWindowViewModel()
        {
            ButtonContent = new ObservableCollection<string>();
            populateButtonContent();

        }
    }
}
