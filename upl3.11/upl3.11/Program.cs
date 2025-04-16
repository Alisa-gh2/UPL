using System;
using System.Windows.Forms;

namespace SeriesCalculation
{
    public partial class MainForm : Form
    {
        private TrackBar trackBar;
        private Label lblN;
        private Label lblIterativeResult;
        private Label lblFormulaResult;

        public MainForm()
        {
            //InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Вычисление ряда −1 + 2 − 3 + ⋯ + (−1)^n * n";
            this.ClientSize = new System.Drawing.Size(400, 200);

            // TrackBar для выбора n
            trackBar = new TrackBar
            {
                Minimum = 1,
                Maximum = 100,
                Value = 1,
                TickFrequency = 5,
                LargeChange = 5,
                SmallChange = 1,
                Location = new System.Drawing.Point(20, 20),
                Width = 360
            };
            trackBar.Scroll += TrackBar_Scroll;
            this.Controls.Add(trackBar);

            // Метка для отображения текущего n
            lblN = new Label
            {
                Text = $"n = {trackBar.Value}",
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };
            this.Controls.Add(lblN);

            // Метка для итеративного результата
            lblIterativeResult = new Label
            {
                Text = "Цикл: " + CalculateIterative(trackBar.Value),
                Location = new System.Drawing.Point(20, 90),
                AutoSize = true
            };
            this.Controls.Add(lblIterativeResult);

            // Метка для результата по формуле
            lblFormulaResult = new Label
            {
                Text = "Формула: " + CalculateByFormula(trackBar.Value),
                Location = new System.Drawing.Point(20, 120),
                AutoSize = true
            };
            this.Controls.Add(lblFormulaResult);
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            int n = trackBar.Value;
            lblN.Text = $"n = {n}";
            lblIterativeResult.Text = "Цикл: " + CalculateIterative(n);
            lblFormulaResult.Text = "Формула: " + CalculateByFormula(n);
        }

        // Вычисление суммы итеративно (в цикле)
        private int CalculateIterative(int n)
        {
            int sum = 0;
            for (int i = 1; i <= n; i++)
            {
                sum += (int)(Math.Pow(-1, i) * i);
            }
            return sum;
        }

        // Вычисление суммы по формуле (−1)^n * ⌊(n + 1)/2⌋
        private int CalculateByFormula(int n)
        {
            int sign = (n % 2 == 0) ? 1 : -1;
            int floorValue = (n + 1) / 2;
            return sign * floorValue;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}