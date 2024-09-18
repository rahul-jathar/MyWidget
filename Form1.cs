using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MyWidgetApp
{
    public partial class Form1 : Form
    {
        private static readonly Color ButtonColor = ColorTranslator.FromHtml("#E5E4E2");
        private ContextMenuStrip contextMenu;
        private static readonly HttpClient client = new();
        private static int brightnessValue = 150;
        public Form1()
        {
            InitializeComponent();
            InitializeWidget();
            CreateContextMenu();
            this.ContextMenuStrip = contextMenu;
        }

        private void InitializeWidget()
        {
            // Set the form properties to make it widget-like
            this.FormBorderStyle = FormBorderStyle.None;
            //this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.Magenta; // Set a color to be fully transparent
            this.TransparencyKey = Color.Magenta; // Make this color transparent
            this.ShowInTaskbar = false; // Hide the widget from the taskbar

            // Set the opacity (0.0 to 1.0, where 1.0 is fully opaque)
            this.Opacity = 0.9; // Adjust as needed

            // Create buttons
            Button button1 = new Button
            {
                Text = "Spot Light",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(100, 50),
                BackColor = ButtonColor
            };
            button1.Click += async (sender, e) =>
            {
                string url = "http://pg_bed.local/json/state";
                var requestBody = new WledBrightnessControlModel()
                {
                    on = true,
                    bri = 255,
                    seg = new List<Segments>()
                    {
                        new Segments()
                        {
                            id = 0,
                            on = brightnessValue == 0 ? false : true,
                            bri = brightnessValue,
                            col = new List<List<int>>(){new List<int>() { 255, 255, 255, 255} }
                        }
                    }
                };

                try
                {
                    // Convert the request body to JSON
                    var jsonContent = new StringContent(
                        JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(url, jsonContent);

                    // Check if the response was successful
                    response.EnsureSuccessStatusCode();

                    // Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show("Request error: " + httpEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            };

            Button button2 = new Button
            {
                Text = "Spread Light",
                Location = new System.Drawing.Point(10, 70),
                Size = new System.Drawing.Size(100, 50),
                BackColor = ButtonColor
            };
            button2.Click += async (sender, e) =>
            {
                string url = "http://pg_bed.local/json/state";
                var requestBody = new WledBrightnessControlModel()
                {
                    on = true,
                    bri = 255,
                    seg = new List<Segments>()
                    {
                        new Segments()
                        {
                            id = 1,
                            on = brightnessValue == 0 ? false : true,
                            bri = brightnessValue,
                            col = new List<List<int>>(){new List<int>() { 255, 255, 255, 255} }
                        }
                    }
                };

                try
                {
                    // Convert the request body to JSON
                    var jsonContent = new StringContent(
                        JsonConvert.SerializeObject(requestBody),
                        Encoding.UTF8,
                        "application/json"
                    );

                    // Send POST request
                    HttpResponseMessage response = await client.PostAsync(url, jsonContent);

                    // Check if the response was successful
                    response.EnsureSuccessStatusCode();

                    // Read and display the response content
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(responseBody);
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show("Request error: " + httpEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            };

            TrackBar trackBar = new()
            {
                Location = new System.Drawing.Point(10, 130),
                Size = new System.Drawing.Size(100, 45),
                Minimum = 0,
                Maximum = 255,
                TickFrequency = 10,
                Value = brightnessValue // Set an initial value
            };
            trackBar.Scroll += (sender, e) =>
            {
                // Handle track bar value change
                brightnessValue = trackBar.Value;
                Console.WriteLine($"TrackBar Value: {trackBar.Value}");
            };

            // Add buttons to the form
            this.Controls.Add(button1);
            this.Controls.Add(button2);
            this.Controls.Add(trackBar);

            // Calculate the position for the widget
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            var screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            // Set the size and position of the form (widget)
            this.Size = new System.Drawing.Size(120, 150);
            this.Location = new System.Drawing.Point(screenWidth - this.Width, 0); // Top-right corner
        }
        private void CreateContextMenu()
        {
            contextMenu = new ContextMenuStrip();

            // Create the "Close" menu item
            ToolStripMenuItem closeMenuItem = new ToolStripMenuItem
            {
                Text = "Close"
            };
            closeMenuItem.Click += (sender, e) => this.Close();

            // Add the "Close" menu item to the context menu
            contextMenu.Items.Add(closeMenuItem);
        }
    }
    public class WledBrightnessControlModel
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public List<Segments> seg { get; set; }
    }
    public class Segments
    {
        public int id { get; set; }
        public bool on { get; set; }
        public int bri { get; set; }
        public int cct { get; set; }
        public int set { get; set; }
        public string n { get; set; }
        public List<List<int>> col { get; set; }
    }
}
