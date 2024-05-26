using System;
using System.Windows.Forms;

namespace TrafficLightApp
{
    public partial class MainForm : Form
    {
        private enum State
        {
            Red,
            Green
        }

        private enum PedestrianSignal
        {
            Walk,
            DontWalk
        }

        private State currentState;
        private PedestrianSignal currentPedestrianSignal;
        private PictureBox trafficLightPictureBox;
        private PictureBox pedestrianSignalPictureBox;
        private PictureBox diagramPictureBox;
        private Timer mainTimer;
        private Timer pedestrianTimer;
        private bool pedestrianButtonPressed;
        private TextBox trafficLightStateTextBox;
        private TextBox pedestrianSignalStateTextBox;

        public MainForm()
        {
            InitializeComponent();
            InitializeTimers();
            InitializeUI();
            SetState(State.Red);
            SetPedestrianSignal(PedestrianSignal.Walk);
        }

        private void InitializeTimers()
        {
            mainTimer = new Timer();
            mainTimer.Interval = 5000; // Initial interval for Red state
            mainTimer.Tick += MainTimer_Tick;

            pedestrianTimer = new Timer();
            pedestrianTimer.Interval = 3000; // Pedestrian signal duration
            pedestrianTimer.Tick += PedestrianTimer_Tick;
        }

        private void InitializeUI()
        {
            this.Size = new System.Drawing.Size(800, 450); // Adjusted to fit the new PictureBox
            this.Text = "Traffic Light";
            this.StartPosition = FormStartPosition.CenterScreen;

            Button pedestrianButton = new Button();
            pedestrianButton.Text = "Pedestrian Press";
            pedestrianButton.Size = new System.Drawing.Size(150, 50);
            pedestrianButton.Location = new System.Drawing.Point(25, 300);
            pedestrianButton.Click += PedestrianButton_Click;

            this.Controls.Add(pedestrianButton);

            trafficLightPictureBox = new PictureBox();
            trafficLightPictureBox.Size = new System.Drawing.Size(150, 150);
            trafficLightPictureBox.Location = new System.Drawing.Point(25, 25);
            trafficLightPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            trafficLightPictureBox.Image = Properties.Resources.red_light;

            this.Controls.Add(trafficLightPictureBox);

            pedestrianSignalPictureBox = new PictureBox();
            pedestrianSignalPictureBox.Size = new System.Drawing.Size(150, 150);
            pedestrianSignalPictureBox.Location = new System.Drawing.Point(25, 200);
            pedestrianSignalPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pedestrianSignalPictureBox.Image = Properties.Resources.dont_walk_signal;

            this.Controls.Add(pedestrianSignalPictureBox);

            trafficLightStateTextBox = new TextBox();
            trafficLightStateTextBox.Location = new System.Drawing.Point(25, 350);
            trafficLightStateTextBox.Size = new System.Drawing.Size(150, 20);
            trafficLightStateTextBox.ReadOnly = true;
            trafficLightStateTextBox.Text = "Traffic Light: Red";

            this.Controls.Add(trafficLightStateTextBox);

            pedestrianSignalStateTextBox = new TextBox();
            pedestrianSignalStateTextBox.Location = new System.Drawing.Point(25, 375);
            pedestrianSignalStateTextBox.Size = new System.Drawing.Size(150, 20);
            pedestrianSignalStateTextBox.ReadOnly = true;
            pedestrianSignalStateTextBox.Text = "Pedestrian: Walk";

            this.Controls.Add(pedestrianSignalStateTextBox);

            // Add the new PictureBox for the diagram
            diagramPictureBox = new PictureBox();
            diagramPictureBox.Size = new System.Drawing.Size(550, 403);
            diagramPictureBox.Location = new System.Drawing.Point(220, 25); // Positioned to the right of existing controls
            diagramPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            diagramPictureBox.Image = Properties.Resources.diagram;

            this.Controls.Add(diagramPictureBox);
        }

        private void PedestrianButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Pedestrian button pressed.");
            pedestrianButtonPressed = true;
            SetPedestrianSignal(PedestrianSignal.Walk);
            pedestrianTimer.Start();
            mainTimer.Stop();
            SetState(State.Red);
            Console.WriteLine("Pedestrian Walk signal started.");
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (!pedestrianButtonPressed)
            {
                if (currentState == State.Red)
                {
                    SetPedestrianSignal(PedestrianSignal.DontWalk);
                    SetState(State.Green);
                    trafficLightStateTextBox.Text = "Traffic Light: Green";
                    pedestrianSignalStateTextBox.Text = "Pedestrian: Don't Walk";
                }
                else if (currentState == State.Green)
                {
                    SetPedestrianSignal(PedestrianSignal.Walk);
                    SetState(State.Red);
                    trafficLightStateTextBox.Text = "Traffic Light: Red";
                    pedestrianSignalStateTextBox.Text = "Pedestrian: Walk";
                }
            }
        }

        private void PedestrianTimer_Tick(object sender, EventArgs e)
        {
            pedestrianTimer.Stop();
            SetPedestrianSignal(PedestrianSignal.DontWalk);
            Console.WriteLine("Pedestrian Don't Walk signal started.");
            pedestrianButtonPressed = false;

            if (currentState == State.Red)
            {
                SetState(State.Green);
            }
            mainTimer.Start();
        }

        private void SetState(State newState)
        {
            currentState = newState;
            Console.WriteLine($"State changed to: {newState}");
            switch (currentState)
            {
                case State.Red:
                    trafficLightPictureBox.Image = Properties.Resources.red_light;
                    mainTimer.Interval = 5000; // Red state duration: 5 seconds
                    mainTimer.Start();
                    break;
                case State.Green:
                    trafficLightPictureBox.Image = Properties.Resources.green_light;
                    mainTimer.Interval = 7000; // Green state duration: 7 seconds
                    mainTimer.Start();
                    break;
            }
        }

        private void SetPedestrianSignal(PedestrianSignal signal)
        {
            currentPedestrianSignal = signal;
            Console.WriteLine($"Pedestrian signal changed to: {signal}");
            switch (signal)
            {
                case PedestrianSignal.Walk:
                    pedestrianSignalPictureBox.Image = Properties.Resources.walk_signal;
                    break;
                case PedestrianSignal.DontWalk:
                    pedestrianSignalPictureBox.Image = Properties.Resources.dont_walk_signal;
                    break;
            }
        }
    }
}