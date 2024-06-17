using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using PatternRecognition.FingerprintRecognition.Core;
using PatternRecognition.FingerprintRecognition.FeatureDisplay;
using PatternRecognition.FingerprintRecognition.ResourceProviders;

namespace Finger_Print_Based_Voting_System
{
    public partial class Election : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["votecon"].ConnectionString);
        private bool biometricMatched = false;
        public Election()
        {
            InitializeComponent();

            /////////////////////////////////////
            var providerByFeatType = new Dictionary<Type, List<Type>>();
            var mtiaListExtractors = new List<Type>();
            var orImgExtractors = new List<Type>();
            var skImgExtractors = new List<Type>();
            var experiments = new List<Type>();

            Assembly thisAss = Assembly.GetExecutingAssembly();
            string dir = Path.GetDirectoryName(thisAss.Location);
            foreach (string fileName in Directory.GetFiles(dir))
                try
                {
                    Assembly currAssembly = Assembly.LoadFile(fileName);
                    foreach (Type type in currAssembly.GetExportedTypes())
                        if (type.IsClass && !type.IsAbstract)
                        {
                            var currInterface = type.GetInterface("IFeatureExtractor`1");
                            if (currInterface != null)
                            {
                                var featType = currInterface.GetGenericArguments()[0];
                                if (featType == typeof(List<Minutia>))
                                {
                                    mtiaListExtractors.Add(type);
                                    continue;
                                }

                                if (featType == typeof(OrientationImage))
                                {
                                    orImgExtractors.Add(type);
                                    continue;
                                }

                                if (featType == typeof(SkeletonImage))
                                {
                                    skImgExtractors.Add(type);
                                    continue;
                                }
                            }

                            currInterface = type.GetInterface("IMatchingExperiment");
                            if (currInterface != null)
                            {
                                experiments.Add(type);
                                continue;
                            }

                            currInterface = type.GetInterface("IResourceProvider`1");
                            if (currInterface != null)
                            {
                                var featType = currInterface.GetGenericArguments()[0];
                                if (!providerByFeatType.ContainsKey(featType))
                                    providerByFeatType.Add(featType, new List<Type>());
                                providerByFeatType[featType].Add(type);
                                continue;
                            }

                            currInterface = type.GetInterface("IMatcher`1");
                            if (currInterface != null && !providersByMatcher.ContainsKey(type))
                                providersByMatcher.Add(type, new List<Type>());
                        }
                }
                catch
                {
                }
            foreach (var pair in providersByMatcher)
            {
                var featType = pair.Key.GetInterface("IMatcher`1").GetGenericArguments()[0];
                foreach (var provider1 in providerByFeatType[featType])
                    pair.Value.Add(provider1);
            }
            // Populating cbxMinutiaExtractor
            cbxExperiment.DataSource = experiments;
            cbxExperiment.DisplayMember = "Name";
            cbxExperiment.ValueMember = "Name";

            // Populating cbxMinutiaExtractor
            cbxMinutiaExtractor.DataSource = mtiaListExtractors;
            cbxMinutiaExtractor.DisplayMember = "Name";
            cbxMinutiaExtractor.ValueMember = "Name";

            // Populating cbxMinutiaExtractor
            cbxOrientationImageExtractor.DataSource = orImgExtractors;
            cbxOrientationImageExtractor.DisplayMember = "Name";
            cbxOrientationImageExtractor.ValueMember = "Name";

            // Populating cbxMinutiaExtractor
            cbxSkeletonImageExtractor.DataSource = skImgExtractors;
            cbxSkeletonImageExtractor.DisplayMember = "Name";
            cbxSkeletonImageExtractor.ValueMember = "Name";

            // Populating cbxMatcher
            cbxMatcher.DataSource = new List<Type>(providersByMatcher.Keys);
            cbxMatcher.DisplayMember = "Name";
            cbxMatcher.ValueMember = "Name";

            ////////////////////////////////////////////////////
        }

        #region private fields

        private Bitmap qImage, tImage;

        private IResourceProvider provider;

        private ResourceRepository repository;

        private string resourcePath;

        private IMatcher matcher;

        private object qFeatures, tFeatures;

        private MinutiaListProvider mtiaListProvider = new MinutiaListProvider();

        private OrientationImageProvider orImgProvider = new OrientationImageProvider();

        private SkeletonImageProvider skImgProvider = new SkeletonImageProvider();

        private readonly Dictionary<Type, List<Type>> providersByMatcher = new Dictionary<Type, List<Type>>();

        private IMatchingExperiment experiment;

        #endregion

        public static Bitmap ByteToImage(byte[] blob)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                mStream.Write(blob, 0, blob.Length);
                mStream.Seek(0, SeekOrigin.Begin);

                Bitmap bm = new Bitmap(mStream);
                return bm;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!biometricMatched)
            {
                MessageBox.Show("Biometric not matched. Unable to vote.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Enter the Election ID", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (comboBox2.Text == "")
            {
                MessageBox.Show("Choose Candidate", "Field Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string today = DateTime.Now.ToString("dd/MM/yyyy");

                if (label12.Text == today)
                {
                    con.Open();
                    SqlCommand cc = new SqlCommand("select CID from Vote where VoterID='" + textBox1.Text + "' AND ElectionID='" + comboBox1.Text + "'", con);
                    string candidate = Convert.ToString(cc.ExecuteScalar());
                    if (candidate == "")
                    {
                        SqlCommand cmd = new SqlCommand("insert into Vote values('" + textBox1.Text + "','" + comboBox1.Text + "','" + comboBox2.Text + "')", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Vote Casted!");
                    }
                    else
                    {
                        MessageBox.Show("Already Voted!");
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("The Election Date is: " + label12.Text);
                }

            }
        }

        private void ShowResults(double matchingScore, List<MinutiaPair> matchingMtiae)
        {
            if (matchingScore == 0 || matchingMtiae == null)
            {
                //MessageBox.Show(string.Format("Similarity: {0}.", matchingScore));
            }
            else
            {
                List<Minutia> qMtiae = new List<Minutia>();
                List<Minutia> tMtiae = new List<Minutia>();
                foreach (MinutiaPair mPair in matchingMtiae)
                {
                    qMtiae.Add(mPair.QueryMtia);
                    tMtiae.Add(mPair.TemplateMtia);
                }
                IFeatureDisplay<List<Minutia>> display = new MinutiaeDisplay();

                pictureBox1.Image = qImage.Clone() as Bitmap;
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                display.Show(qMtiae, g);
                pictureBox1.Invalidate();

                pictureBox2.Image = tImage.Clone() as Bitmap;
                g = Graphics.FromImage(pictureBox2.Image);
                display.Show(tMtiae, g);
                pictureBox2.Invalidate();

                //MessageBox.Show(string.Format("Similarity: {0}. Matching minutiae: {1}.", matchingScore, matchingMtiae.Count));
            }
        }

        public void ShowBlueMinutiae(List<Minutia> features, Graphics g)
        {
            int mtiaRadius = 6;
            int lineLength = 18;
            Pen pen = new Pen(Brushes.Blue) { Width = 3 };
            pen.Color = Color.LightBlue;

            Pen whitePen = new Pen(Brushes.Blue) { Width = 5 };
            whitePen.Color = Color.White;

            int i = 0;
            foreach (Minutia mtia in (IList<Minutia>)features)
            {
                g.DrawEllipse(whitePen, mtia.X - mtiaRadius, mtia.Y - mtiaRadius, 2 * mtiaRadius + 1, 2 * mtiaRadius + 1);
                g.DrawLine(whitePen, mtia.X, mtia.Y, Convert.ToInt32(mtia.X + lineLength * Math.Cos(mtia.Angle)), Convert.ToInt32(mtia.Y + lineLength * Math.Sin(mtia.Angle)));

                pen.Color = Color.LightBlue;

                g.DrawEllipse(pen, mtia.X - mtiaRadius, mtia.Y - mtiaRadius, 2 * mtiaRadius + 1, 2 * mtiaRadius + 1);
                g.DrawLine(pen, mtia.X, mtia.Y, Convert.ToInt32(mtia.X + lineLength * Math.Cos(mtia.Angle)), Convert.ToInt32(mtia.Y + lineLength * Math.Sin(mtia.Angle)));
                i++;
            }

            Minutia lastMtia = ((IList<Minutia>)features)[((IList<Minutia>)features).Count - 1];
            pen.Color = Color.LightBlue;
            g.DrawEllipse(pen, lastMtia.X - mtiaRadius, lastMtia.Y - mtiaRadius, 2 * mtiaRadius + 1, 2 * mtiaRadius + 1);
            g.DrawLine(pen, lastMtia.X, lastMtia.Y, Convert.ToInt32(lastMtia.X + lineLength * Math.Cos(lastMtia.Angle)), Convert.ToInt32(lastMtia.Y + lineLength * Math.Sin(lastMtia.Angle)));
        }

        private void cbxMatcher_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type matcherType = (Type)selectedValue;
                matcher = Activator.CreateInstance(matcherType) as IMatcher;

                cbxFeatureProvider.DataSource = providersByMatcher[matcherType];
                cbxFeatureProvider.DisplayMember = "Name";
                cbxFeatureProvider.ValueMember = "Name";
            }
        }

        private void cbxFeatureProvider_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type providerType = (Type)selectedValue;
                provider = Activator.CreateInstance(providerType) as IResourceProvider;

            }
        }

        private void cbxMinutiaExtractor_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type extractorType = (Type)selectedValue;
                mtiaListProvider.MinutiaListExtractor = Activator.CreateInstance(extractorType) as IFeatureExtractor<List<Minutia>>;
                //cbxMinutiaExtractor_Enter(sender, e);
            }
        }

        private void cbxOrientationImageExtractor_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type extractorType = (Type)selectedValue;
                orImgProvider.OrientationImageExtractor = Activator.CreateInstance(extractorType) as IFeatureExtractor<OrientationImage>;
                // cbxOrientationImageExtractor_Enter(sender, e);
            }
        }

        private void cbxSkeletonImageExtractor_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type extractorType = (Type)selectedValue;
                skImgProvider.SkeletonImageExtractor = Activator.CreateInstance(extractorType) as IFeatureExtractor<SkeletonImage>;
                //cbxSkeletonImageExtractor_Enter(sender, e);
            }
        }

        private void cbxExperiment_SelectedValueChanged(object sender, EventArgs e)
        {
            object selectedValue = ((ComboBox)sender).SelectedItem;
            if (selectedValue != null)
            {
                Type experimentType = (Type)selectedValue;
                experiment = Activator.CreateInstance(experimentType) as IMatchingExperiment;
            }
        }

        private void Election_Load(object sender, EventArgs e)
        {
            electionid();
        }

        private void electionid()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("select ElectionID from Election", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            comboBox1.Items.Insert(0, "Please choose an item");
            comboBox1.ValueMember = "ElectionID";
            comboBox1.DisplayMember = "ElectionID";
            comboBox1.DataSource = dt;

            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.tif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.tif";
            ofd.ShowDialog();
            label8.Text = ofd.FileName;

            if (ofd.FileName != "")
            {
                qImage = ImageLoader.LoadImage(ofd.FileName);
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                pictureBox1.Image = Image.FromStream(fs);
                fs.Flush();
                fs.Dispose();
                fs.Close();
            }

            con.Open();
            SqlCommand cmm = new SqlCommand("Select * from Voters", con);
            SqlDataReader dr = cmm.ExecuteReader();

            while (dr.Read())
            {
                byte[] buffer = (byte[])dr["FingerPrint"];
                string imgname = dr.GetString(6).ToString();

                Image img = Image.FromStream(new MemoryStream(buffer));
                tImage = new Bitmap(new Bitmap(img));
                pictureBox2.Image = tImage;

                Type resourceType = provider.GetType();
                foreach (PropertyInfo propertyInfo in resourceType.GetProperties())
                {
                    var currInterface = propertyInfo.PropertyType.GetInterface("IResourceProvider`1");
                    if (currInterface != null)
                    {
                        var featType = currInterface.GetGenericArguments()[0];
                        if (featType == typeof(OrientationImage))
                        {
                            resourceType.InvokeMember(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, provider, new object[] { orImgProvider });
                            continue;
                        }
                        if (featType == typeof(List<Minutia>))
                        {
                            resourceType.InvokeMember(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, provider, new object[] { mtiaListProvider });
                            continue;
                        }
                        if (featType == typeof(SkeletonImage))
                        {
                            resourceType.InvokeMember(propertyInfo.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, provider, new object[] { skImgProvider });
                            continue;
                        }
                    }

                    if (propertyInfo.CanWrite)
                    {
                        currInterface = propertyInfo.PropertyType;
                        if (currInterface.Name == "IFeatureExtractor`1")
                        {
                            var featType = currInterface.GetGenericArguments()[0];
                            if (featType == typeof(OrientationImage))
                            {
                                provider = orImgProvider;
                                continue;
                            }
                            if (featType == typeof(List<Minutia>))
                            {
                                provider = mtiaListProvider;
                                continue;
                            }
                            if (featType == typeof(SkeletonImage))
                            {
                                provider = skImgProvider;
                                continue;
                            }
                        }
                    }
                }

                repository = new ResourceRepository(Path.GetDirectoryName(label8.Text));
                qFeatures = provider.GetResource(Path.GetFileNameWithoutExtension(label8.Text), repository);
                tFeatures = provider.GetResource(imgname, repository);

                // Matching features
                List<MinutiaPair> matchingMtiae = null;
                double score;
                IMinutiaMatcher minutiaMatcher = matcher as IMinutiaMatcher;
                if (minutiaMatcher != null)
                {
                    score = minutiaMatcher.Match(qFeatures, tFeatures, out matchingMtiae);

                    if (qFeatures is List<Minutia> && tFeatures is List<Minutia>)
                    {
                        pictureBox1.Image = qImage.Clone() as Bitmap;
                        Graphics g1 = Graphics.FromImage(pictureBox1.Image);
                        ShowBlueMinutiae(qFeatures as List<Minutia>, g1);

                        pictureBox2.Image = tImage.Clone() as Bitmap;
                        Graphics g2 = Graphics.FromImage(pictureBox2.Image);
                        ShowBlueMinutiae(tFeatures as List<Minutia>, g2);

                        if (score == 0 || matchingMtiae == null)
                        {
                            //MessageBox.Show(string.Format("Similarity: {0}.", score));
                        }
                        else
                        {
                            List<Minutia> qMtiae = new List<Minutia>();
                            List<Minutia> tMtiae = new List<Minutia>();
                            foreach (MinutiaPair mPair in matchingMtiae)
                            {
                                qMtiae.Add(mPair.QueryMtia);
                                tMtiae.Add(mPair.TemplateMtia);
                            }
                            IFeatureDisplay<List<Minutia>> display = new MinutiaeDisplay();

                            display.Show(qMtiae, g1);
                            pictureBox1.Invalidate();

                            display.Show(tMtiae, g2);
                            pictureBox2.Invalidate();
                        }
                    }
                    else
                        ShowResults(score, matchingMtiae);
                }
                else
                    score = matcher.Match(qFeatures, tFeatures);

                if (score > 98)
                {
                    biometricMatched = true;
                    button1.Enabled = true;
                    textBox1.Text = dr.GetValue(0).ToString();
                    textBox2.Text = dr.GetString(1).ToString();
                    byte[] photo = (byte[])dr["Photo"];

                    Image photoimg = Image.FromStream(new MemoryStream(photo));
                    Bitmap pImage = new Bitmap(new Bitmap(photoimg));

                    pictureBox3.Image = pImage;
                    label4.ForeColor = Color.Green;
                    label4.Text = "Biometric Matched!";
                    groupBox2.Enabled = true;
                    break;
                }
                else
                {
                    biometricMatched = false;
                    button1.Enabled = false;
                    label4.ForeColor = Color.Red;
                    label4.Text = "Biometric Not Matched!";
                }
            }

            dr.Close();
            con.Close();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select CID from candidate where Election = '" + comboBox1.Text + "'", con);
                SqlCommand cmd1 = new SqlCommand("select ElectionName from Election where ElectionID = '" + comboBox1.Text + "'", con);
                label10.Text = "Election : " + Convert.ToString(cmd1.ExecuteScalar());

                SqlCommand cmd2 = new SqlCommand("select Date from Election where ElectionID = '" + comboBox1.Text + "'", con);
                label12.Text = Convert.ToString(cmd2.ExecuteScalar());
                con.Close();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBox2.ValueMember = "CID";
                comboBox2.DisplayMember = "CID";
                comboBox2.DataSource = dt;


                linkLabel1.Enabled = true;

            }
            catch
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select Name from candidate where CID = '" + comboBox2.Text + "'", con);
                label11.Text = "Participant : " + Convert.ToString(cmd1.ExecuteScalar());
                SqlCommand cmd2 = new SqlCommand("select Photo from candidate where CID = '" + comboBox2.Text + "'", con);
                byte[] buffer = (byte[])cmd2.ExecuteScalar();
                con.Close();
                Image img = Image.FromStream(new MemoryStream(buffer));
                Bitmap tImg = new Bitmap(new Bitmap(img));
                pictureBox4.Image = tImg;

            }
            catch { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Participants can = new Participants(comboBox1.Text);
            can.Show();
        }

    }
}


