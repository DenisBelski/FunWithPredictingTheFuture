using System.Threading;
using Newtonsoft.Json;

namespace FunWithPredictingTheFuture
{
    public partial class MainWindow : Form
    {
        private const string AppName = "FUTURE PREDICTOR";
        private readonly string PredictionsConfigPath = $"{Environment.CurrentDirectory}\\predictionsConfig.json";

        private string[] predictions;
        private Random randomAnswer = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void bPredict_Click(object sender, EventArgs e)
        {
            buttonPredict.Enabled = false;
            await Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdateProgressBar(i);
                        this.Text = $"{i}%";
                    }));

                    Thread.Sleep(10);
                }
            });

            var randomIndex = randomAnswer.Next(predictions.Length);
            var prediction = predictions[randomIndex];
            MessageBox.Show($"{prediction}!");

            progressBar.Value = 0;
            this.Text = AppName;
            buttonPredict.Enabled = true;
        }

        private void UpdateProgressBar(int i)
        {
            if (i == progressBar.Maximum)
            {
                progressBar.Maximum = i + 1;
                progressBar.Value = i + 1;
                progressBar.Maximum = i;
            }
            else
            {
                progressBar.Value = i + 1;
            }

            progressBar.Value = i;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = AppName;

            try
            {
                var predictionData = File.ReadAllText(PredictionsConfigPath);

                predictions = JsonConvert.DeserializeObject<string[]>(predictionData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (predictions == null)
                {
                    Close();
                }
                else if (predictions.Length == 0)
                {
                    MessageBox.Show("Predictions are over!");
                    Close();
                }
            }
        }
    }
}