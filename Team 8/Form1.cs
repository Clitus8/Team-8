using System.Text.RegularExpressions;

namespace Team_8
{
    public partial class Form1 : Form
    {
        private Queue<string> queue = new Queue<string>();
        int linenum = -1;
        bool playing = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void TimeEvent(object sender, EventArgs e)
        {
            if (playing == true && linenum < listBox1.Items.Count - 1)
            {

                linenum++;
                listBox1.SetSelected(linenum, true);
            }
            else
            {
                linenum = -1;
                playing = false;
                timer.Stop();
                queue.Dequeue();
                btnStart.Text = "Start";
            }
        }

        private void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Browse Html Files Only";
            openFile.Filter = "Html Files Only (*.html) | *.html";
            openFile.DefaultExt = "html";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();

                // load the lines to the list box
                var fileLocation = File.ReadAllLines(openFile.FileName);
                List<string> lines = new List<string>(fileLocation);

                for (int i = 0; i < lines.Count; i++)
                {
                    listBox1.Items.Add(lines[i]);
                }
            }
        }

        private void Start(object sender, EventArgs e)
        {
            if (playing == false)
            {
                if (listBox1.Items.Count > 0)
                {
                    playing = true;
                    timer.Start();
                    btnStart.Text = "Stop";
                }
                else
                {
                    MessageBox.Show("Select a text file first! ");
                }
            }
            else
            {
                timer.Stop();
                queue.Dequeue();
                btnStart.Text = "Start";
                playing = false;
            }
        }

        private void Check_Click(object sender, EventArgs e)
        {
            label1.Text = checkErrorsTag(label1.Text);
        }

        private void ChangeLabel(object sender, EventArgs e)
        {
            label1.Text = listBox1.SelectedItem.ToString();
        }

        public string checkErrorsTag(string value)
        {

            Regex regex = new Regex("<(\"[^\"]*\"|'[^']*'|[^'\">])*>");

            Match match = regex.Match(value);
            if (match.Success)
            {
                return label1.Text = RemoveHTMLTags(label1.Text);
            }
            else
            {
                MessageBox.Show("Errors");
                return "";
            }
        }

        public string RemoveHTMLTags(string value)
        {
            Regex regex = new Regex("\\<[^\\>]*\\>");
            value = regex.Replace(value, String.Empty);
            queue.Enqueue(value);
            return value;


        }

    }
}