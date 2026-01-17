using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace rainbow_calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double _firstNumber = 0;
        private string _operator = string.Empty;
        private bool _isNewInput = true;
        private string _onDsiplay = "";

        public MainWindow()
        {
            InitializeComponent();
            Display.Text = "0";

            // Ensure window gets keyboard focus
            Loaded += (_, __) => Keyboard.Focus(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;

            if (_isNewInput)
            {
                _onDsiplay = button.Content.ToString();
                Display.Text = _onDsiplay;
                _isNewInput = false;
            }
            else
            {
                _onDsiplay += button.Content.ToString();
                Display.Text = _onDsiplay;
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;


            if (_onDsiplay == "" || Display.Text == "0")
            {
                return;
            }
            else if (_onDsiplay.Contains('+') || _onDsiplay.Contains('-') ||
                _onDsiplay.Contains('x') || _onDsiplay.Contains('/'))
            {
                Equals_Click(sender, e);
            }
            else
            {
                _onDsiplay += " " + button.Content.ToString() + " ";
                Display.Text = _onDsiplay;
            }
            _operator = button.Content.ToString();
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            // check if all nums are entered 
            if (_onDsiplay == "0" || !_onDsiplay.Contains(' '))
                return;

            string[] allItems = _onDsiplay.Split(" ");
            if (allItems.Length != 3)
                return;

            
            if (!double.TryParse(allItems[2], out double secondNumber))
                return;

            double result;
            double.TryParse(allItems[0], out _firstNumber);
            

            switch (_operator)
            {
                case "+":
                    result = _firstNumber + secondNumber;
                    break;
                case "−":
                    result = _firstNumber - secondNumber;
                    break;
                case "x":
                    result = _firstNumber * secondNumber;
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("Cannot divide by zero", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        Clear();
                        return;
                    }
                    result = _firstNumber / secondNumber;
                    break;
                default:
                    return;
            }

            var currPress = sender as Button;
            if (currPress == null) return;
            
            // use operator as equal
            if (currPress.Content.ToString() == "=")
            {
                Display.Text = result.ToString();
                _onDsiplay = result.ToString();
            }
            else
            {
                _onDsiplay = result.ToString() + ' ' + currPress.Content.ToString();
                Display.Text = _onDsiplay;
            }
                
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            _firstNumber = 0;
            _operator = string.Empty;
            _isNewInput = true;
            _onDsiplay = "";
            Display.Text = "0";
        }

        // =========================
        // Keyboard Support
        // =========================
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Numbers (top row & numpad)
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                HandleDigit((char)('0' + (e.Key - Key.D0)));
                return;
            }

            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                HandleDigit((char)('0' + (e.Key - Key.NumPad0)));
                return;
            }

            // Operators
            switch (e.Key)
            {
                case Key.Add:
                case Key.OemPlus when Keyboard.Modifiers == ModifierKeys.None:
                    SetOperator("+");
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    SetOperator("−");
                    break;

                case Key.Multiply:
                    SetOperator("×");
                    break;

                case Key.Divide:
                case Key.Oem2: // '/'
                    SetOperator("÷");
                    break;
                case Key.Enter:
                    Equals_Click(this, new RoutedEventArgs());
                    break;

                case Key.Escape:
                    Clear();
                    break;

                case Key.Back:
                    Backspace();
                    break;
            }
        }

        private void HandleDigit(char digit)
        {
            if (_isNewInput)
            {
                Display.Text = digit.ToString();
                _isNewInput = false;
            }
            else
            {
                Display.Text += digit;
            }
        }

        private void SetOperator(string op)
        {
            if (double.TryParse(Display.Text, out double number))
            {
                _firstNumber = number;
                _operator = op;
                _isNewInput = true;
            }
        }

        private void Backspace()
        {
            if (string.IsNullOrEmpty(_onDsiplay))
            {
                Display.Text = "0";
                _isNewInput = true;
                return;
            }

            // remove trailing space
            if (_onDsiplay.EndsWith(" "))
            {
                _onDsiplay = _onDsiplay[..^1];
            }

            // handle operator removal
            if (!string.IsNullOrEmpty(_operator))
            {
                if (_onDsiplay.EndsWith(_operator))
                {
                    _onDsiplay = _onDsiplay[..^1];
                    _operator = string.Empty;
                }
                else
                {
                    _onDsiplay = _onDsiplay[..^1];
                }
                Display.Text = _onDsiplay;
            }
            else if (_isNewInput || Display.Text.Length <= 1)
            {
                Display.Text = "0";
                _isNewInput = true;
                _onDsiplay = "";
            }
            else
            {
                _onDsiplay = _onDsiplay[..^1];
                Display.Text = _onDsiplay;
            }
        }

    }

}
