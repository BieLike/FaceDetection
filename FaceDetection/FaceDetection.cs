﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace FaceDetection
{
    public partial class FaceDetection : Form
    {
//list
        CascadeClassifier faceDetected, EyeDetected;   //Variable to detect eyes and face
        Image<Bgr, byte> frame;   //Variable to get colour image
        VideoCapture cam;   //Variable to use the Webcam
        Image<Gray, byte> trainedFace = null;   //Variable to save face
        Image<Gray, byte> grayFace = null;   //Variable to make gray face picture
        Image<Gray, byte> result;  //Variable to get grayFace picture
        List<Image<Gray, byte>> trainingImg = new List<Image<Gray, byte>>();   //Variable to get face and name
        List<string> label = new List<string>();   //Variable to get name
        int count, numLabels;   //Variable to get number of face in folder
        private Dictionary<int, string> labelMap = new Dictionary<int, string>();   //Map integer to name(string/label)
        double confidenceThreshold = 2000.0;   //Threshold to make picture more accuracy
        DataTable table = new DataTable();   //To make table list of checkin

        public FaceDetection()
        {
            InitializeComponent();
//HaarCascade,haar.xml
            string haarcascadePath = Path.Combine(Application.StartupPath, "haarcascade_frontalface_default.xml");
            string haarEyes = Path.Combine(Application.StartupPath, "haarcascade_eye.xml");
            EyeDetected = new CascadeClassifier(haarEyes);
            faceDetected = new CascadeClassifier(haarcascadePath);
            //LoadTrainingData();
            
            try
            {
                string labelsInfoPath = Path.Combine(Application.StartupPath, "Faces", "Faces.txt");
                string Labelsinf = File.ReadAllText(labelsInfoPath);
                string[] label2 = Labelsinf.Split(',');
                numLabels = Convert.ToInt16(label2[0]);
                count = numLabels;
                string faceLoad;
                for (int i=1; i<numLabels+1; i++)
                {
                    faceLoad = $"face{i}.bmp";
                    string faceLoadPath = Path.Combine(Application.StartupPath, "Faces", faceLoad);
                    trainingImg.Add(new Image<Gray, byte>(faceLoadPath));
                    label.Add(label2[i]);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nothing in the database");
            }

            table.Columns.Add("Name",typeof(string));
            table.Columns.Add("Date",typeof(string));
            dgvChecked.DataSource = table;
            dgvChecked.Columns[0].HeaderText = "Name";
            dgvChecked.Columns[1].HeaderText = "Time";
            dgvChecked.Columns[0].Width = 165;
            dgvChecked.Columns[1].Width = 165;
            dgvChecked.Refresh();
        }

        /*private void LoadTrainingData()
        {
            try
            {
                string labelsInfo = File.ReadAllText(Path.Combine(Application.StartupPath, "Faces", "Faces.txt"));
                string[] labels = labelsInfo.Split(',');

                count = Convert.ToInt32(labels[0]);
                for (int i = 1; i <= count; i++)
                {
                    string faceFile = Path.Combine(Application.StartupPath, "Faces", $"face{i}.bmp");
                    trainingImg.Add(new Image<Gray, byte>(faceFile));
                    label.Add(labels[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No data in the database or error in loading training data: " + ex.Message);
            }
        }*/

        private void imgbWebcam_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                count++;
                Mat matFrame = cam.QueryFrame();
                var frame = matFrame.ToImage<Bgr, byte>().Resize(320, 240, Inter.Cubic);
                grayFace = frame.Convert<Gray, byte>();
                var detectedFaces = faceDetected.DetectMultiScale(grayFace, 1.2, 10, new Size(20, 20), Size.Empty);

                foreach (var f in detectedFaces)
                {
                    trainedFace = frame.Copy(f).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                    break;
                }

                if (trainedFace != null)
                {
                    trainingImg.Add(trainedFace);
                    label.Add(txtName.Text);

                    string facesPath = Path.Combine(Application.StartupPath, "Faces");
                    Directory.CreateDirectory(facesPath);
                    File.WriteAllText(Path.Combine(facesPath, "Faces.txt"), trainingImg.Count.ToString() + ",");

                    for (int i = 1; i <= trainingImg.Count; i++)
                    {
                        string faceFilePath = Path.Combine(facesPath, $"face{i}.bmp");
                        trainingImg[i - 1].Save(faceFilePath);
                        File.AppendAllText(Path.Combine(facesPath, "Faces.txt"), label[i - 1] + ",");
                    }
                    imgbShow.Image = trainedFace;
                    imgbShow.Refresh();
                    MessageBox.Show(txtName.Text + " Added Successfully");

                    trainedFace = null;
                }
                else
                {
                    MessageBox.Show("No face detected. Please try again.");
                    imgbShow.Image = null;
                    imgbShow.Refresh();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    }

        private void btnStart_Click(object sender, EventArgs e)
        {
            cam = new VideoCapture();
            cam.QueryFrame();
//EventHandler
            Application.Idle += new EventHandler(FrameProcedure);
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            if (txtThreshold.Text == "")
            {
                txtThreshold.Text = ""+2;
                txtThreshold.SelectAll();
            }
            else
            {
                confidenceThreshold = (double.Parse(txtThreshold.Text) * 1000.0);

            }
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            int Same = 0, rc = dgvChecked.RowCount, i;
            //wtf all this
            string name = "";
            Mat matFrame = cam.QueryFrame();
            frame = matFrame.ToImage<Bgr, byte>().Resize(320, 240, Inter.Cubic);
            grayFace = frame.Convert<Gray, byte>();
            //var facesDetectedNow = faceDetected.DetectMultiScale(grayFace, 1.3, 15, new Size(20, 20), DefaultSize);
            var facesDetectedNow = faceDetected.DetectMultiScale(grayFace, 1.1, 11, new Size(20, 20), Size.Empty);
            foreach (var f in facesDetectedNow)
            {
                grayFace.ROI = f;
                result = frame.Copy(f).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                frame.Draw(f, new Bgr(Color.Green), 2);
                Rectangle[] eyes = EyeDetected.DetectMultiScale(grayFace, 1.1, 11);
                foreach (var eye in eyes)
                {
                    var ayy = eye;
                    ayy.X += f.X;
                    ayy.Y += f.Y;
                    frame.Draw(ayy, new Bgr(0, 0, 255), 1);
                }
                if (trainingImg.Count != 0)
                {
                    
                    var termCriteria = new MCvTermCriteria(count, 0.001);
                    var recognizer = new EigenFaceRecognizer(trainingImg.Count, double.PositiveInfinity);
                    recognizer.Train(trainingImg.ToArray(), ConvertLabelsToInts(label).ToArray());
                    var predictionResult = recognizer.Predict(result);
                    var predictedLabel = predictionResult.Label;
                    //name = labelMap.ContainsKey(predictedLabel) ? labelMap[predictedLabel] : "Unknown";
                        if (predictionResult.Distance < confidenceThreshold)
                        {
                            if (labelMap.ContainsKey(predictedLabel))
                            {
                                name = labelMap[predictedLabel];
                            }
                            else
                            {
                                name = "";
                            }
                    }
                    for (i=0; i<rc; i++)
                    {
                        //string sname = dgvChecked.Rows[i].Cells[0].Value.ToString();
                        if (!dgvChecked.Rows[i].IsNewRow)
                        {
                            var cellValue = dgvChecked.Rows[i].Cells[0].Value;

                            // Check if the cell value is not null
                            if (cellValue != null && name == cellValue.ToString())
                            {
                                if (rc == 1)
                                {
                                    Same = 0;
                                }
                                else if (name == dgvChecked.Rows[i].Cells[0].Value.ToString())
                                {
                                    Same = 1;
                                }
                                else
                                {
                                    Same = 0;
                                }
                            }
                        }
                    }
                    if (name == "")
                    {
                        dgvChecked.Refresh();
                    }
                    else if (Same == 1)
                    {
                        dgvChecked.Refresh();
                    }
                    else if(Same == 0)
                    {
                        table.Rows.Add(name, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                        dgvChecked.DataSource = table;
                        dgvChecked.Refresh();
                    }
                    CvInvoke.PutText(frame, name, new Point(f.X - 2, f.Y - 2), FontFace.HersheyTriplex, 0.6, new MCvScalar(0, 0, 255), 1);

                }
                else
                {
                    name = "";
                    CvInvoke.PutText(frame, name, new Point(f.X - 2, f.Y - 2), FontFace.HersheyTriplex, 0.6, new MCvScalar(0, 0, 255), 1);
                }
                //CvInvoke.PutText(frame, name, new Point(f.X - 2, f.Y - 2), FontFace.HersheyTriplex, 0.6, new MCvScalar(0, 0, 255), 1);
            }
            imgbWebcam.Image = frame;
            name = "";
            Same = 0;

        }


        private List<int> ConvertLabelsToInts(List<string> labels)
        {
            var labelMapReverse = new Dictionary<string, int>();
            var intLabels = new List<int>();

            int currentLabel = 0;
            foreach (var label in labels)
            {
                if (!labelMapReverse.ContainsKey(label))
                {
                    labelMapReverse[label] = currentLabel;
                    labelMap[currentLabel] = label; // Map integer to name
                    currentLabel++;
                }
                intLabels.Add(labelMapReverse[label]);
            }

            return intLabels;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
