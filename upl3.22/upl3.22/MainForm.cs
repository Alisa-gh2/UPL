using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// MainForm.cs
public partial class MainForm : Form
{
    private TextBox txtSearch = new TextBox();
    private Button btnGenerate = new Button();
    // ... остальные элементы

    public MainForm()
    {
        InitializeComponentsManually();
    }

    private void InitializeComponentsManually()
    {
        // Настройка txtSearch
        this.txtSearch.Location = new Point(20, 20);
        this.txtSearch.Name = "txtSearch";
        this.Controls.Add(this.txtSearch);

        // Аналогично для других элементов...
    }
}

namespace MarshRoutesApp
{
    public partial class MainForm : Form
    {
        private List<MARSH> marshes = new List<MARSH>();
        private TextBox textBox1;
        private Button button1;
        private Button button2;
        private Button button3;
        private ListBox listBox1;
        private Button btnGenerate;
        private Button btnLoad;
        private Button btnSearch;
        private ListBox lstResults;
        private TextBox txtSearch;
        private readonly string filePath = "marshes.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            marshes = GenerateMarshes(10);
            SaveMarshesToFile(marshes, filePath);
            MessageBox.Show("Данные успешно сгенерированы и сохранены в файл!", "Успех",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            marshes = ReadMarshesFromFile(filePath);
            DisplayMarshes(marshes);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (marshes.Count == 0)
            {
                MessageBox.Show("Сначала загрузите данные!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string searchTerm = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Введите название пункта для поиска!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var foundMarshes = marshes.Where(m =>
                m.StartPoint.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.EndPoint.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.RouteNumber)
                .ToList();

            if (foundMarshes.Count == 0)
            {
                lstResults.Items.Clear();
                lstResults.Items.Add($"Нет маршрутов, начинающихся или заканчивающихся в пункте '{searchTerm}'");
            }
            else
            {
                DisplayMarshes(foundMarshes);
            }
        }

        private void DisplayMarshes(List<MARSH> marshesToDisplay)
        {
            lstResults.Items.Clear();
            foreach (var marsh in marshesToDisplay)
            {
                lstResults.Items.Add(marsh.ToString());
            }
        }

        private List<MARSH> GenerateMarshes(int count)
        {
            var points = new List<string>
            {
                "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань",
                "Нижний Новгород", "Челябинск", "Самара", "Омск", "Ростов-на-Дону"
            };

            var random = new Random();
            var result = new List<MARSH>();

            for (int i = 0; i < count; i++)
            {
                int startIndex = random.Next(points.Count);
                int endIndex;
                do
                {
                    endIndex = random.Next(points.Count);
                } while (endIndex == startIndex);

                result.Add(new MARSH
                {
                    StartPoint = points[startIndex],
                    EndPoint = points[endIndex],
                    RouteNumber = random.Next(1, 1000)
                });
            }

            return result;
        }

        private void SaveMarshesToFile(List<MARSH> marshes, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                foreach (var marsh in marshes)
                {
                    writer.WriteLine($"{marsh.StartPoint},{marsh.EndPoint},{marsh.RouteNumber}");
                }
            }
        }

        private void InitializeComponent()
        {
            btnGenerate = new Button();
            btnLoad = new Button();
            btnSearch = new Button();
            lstResults = new ListBox();
            txtSearch = new TextBox();
            SuspendLayout();
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(20, 50);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(150, 29);
            btnGenerate.TabIndex = 0;
            btnGenerate.Text = "Генерация данных";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click_1;
            // 
            // btnLoad
            // 
            btnLoad.Location = new Point(20, 80);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(150, 29);
            btnLoad.TabIndex = 1;
            btnLoad.Text = "Загрузка данных";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(20, 110);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(150, 29);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Поиск маршрутов";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // lstResults
            // 
            lstResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstResults.FormattingEnabled = true;
            lstResults.Location = new Point(200, 20);
            lstResults.Name = "lstResults";
            lstResults.Size = new Size(300, 184);
            lstResults.TabIndex = 3;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(20, 20);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(125, 27);
            txtSearch.TabIndex = 4;
            // 
            // MainForm
            // 
            ClientSize = new Size(525, 275);
            Controls.Add(txtSearch);
            Controls.Add(lstResults);
            Controls.Add(btnSearch);
            Controls.Add(btnLoad);
            Controls.Add(btnGenerate);
            Name = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        private List<MARSH> ReadMarshesFromFile(string path)
        {
            var result = new List<MARSH>();

            if (!File.Exists(path))
            {
                MessageBox.Show("Файл с данными не найден! Сначала сгенерируйте данные.", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        result.Add(new MARSH
                        {
                            StartPoint = parts[0],
                            EndPoint = parts[1],
                            RouteNumber = int.Parse(parts[2])
                        });
                    }
                }
            }

            return result;
        }

        private void btnGenerate_Click_1(object sender, EventArgs e)
        {
            marshes = GenerateMarshes(10);
            SaveMarshesToFile(marshes, filePath);

            // Добавьте эту строку:
            string fullPath = Path.GetFullPath(filePath);
            MessageBox.Show($"Файл сохранен по пути:\n{fullPath}", "Успех",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}