using System;
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
        double confidenceThreshold = 2000.0;   //Threshold to make picture more accuracy when the light is not the same
        DataTable table = new DataTable();   //To make table list of checkin
        //-Summary : define variable to use in method

        public FaceDetection()
        {
            InitializeComponent(); 
            string haarFacePath = Path.Combine(Application.StartupPath, "haarcascade_frontalface_default.xml");   //Path to Haar face detect
            string haarEyesPath = Path.Combine(Application.StartupPath, "haarcascade_eye.xml");   //Path for Haar eye detect
            EyeDetected = new CascadeClassifier(haarEyesPath);   //Initialize eye detect path to access to the data
            faceDetected = new CascadeClassifier(haarFacePath);   //Initialize face detect path to access to the data
            //-Summary : define the path to access data

            try
            {
                string labelsInfoPath = Path.Combine(Application.StartupPath, "Faces", "Faces.txt");   //Define the path of face's name
                string Labelsinf = File.ReadAllText(labelsInfoPath);   //Store all the name
                string[] label2 = Labelsinf.Split(',');   //Split all the name into an array
                numLabels = Convert.ToInt16(label2[0]);   //Convert string number of face to int
                count = numLabels;   //Store number of face to use in another method
                string faceLoad;   
                for (int i=1; i<numLabels+1; i++)
                {
                    faceLoad = $"face{i}.bmp";   //Create file name to access to the face number i
                    string faceLoadPath = Path.Combine(Application.StartupPath, "Faces", faceLoad);   //Access to face Image
                    trainingImg.Add(new Image<Gray, byte>(faceLoadPath));   //Add gray face to trainingImg list
                    label.Add(label2[i]);   //Add name of face to label list
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nothing in the database");
            }
            //-Summary : prepare name and face contain in the list

            table.Columns.Add("Name",typeof(string));   //Add Name column to table
            table.Columns.Add("Time",typeof(string));   //Add Time column to table
            dgvChecked.DataSource = table;   //Add table to DataGridView
            dgvChecked.Columns[0].Width = 165;   //Set column[0] width
            dgvChecked.Columns[1].Width = 165;   //Set column[1] width
            dgvChecked.Refresh();
            //-Summary : create table for check in
        }

        private void imgbWebcam_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                count++;  //Number of face
                Mat matFrame = cam.QueryFrame();   //Capture image of a frame on Webcam
                var frame = matFrame.ToImage<Bgr, byte>().Resize(320, 240, Inter.Cubic);   //Convert to colour image and resize
                grayFace = frame.Convert<Gray, byte>();   //Convert to gray image
                var detectedFaces = faceDetected.DetectMultiScale(grayFace, 1.2/*How much img reduce*/, 5/*num of face detecting*/, new Size(20, 20), Size.Empty);   //Detect face in the grayscale by Haar
                //-Summary : Define and prepare variable to use

                foreach (var f in detectedFaces)
                {
                    trainedFace = frame.Copy(f).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);   //Convert face image to gray and resize
                    break;   //Ensure that only the first detected face is processeed
                }

                if (trainedFace != null)
                {
                    trainingImg.Add(trainedFace);   //Add trainedFace to trainingImg list
                    label.Add(txtName.Text);   //Add name to label list

                    string facesPath = Path.Combine(Application.StartupPath, "Faces");   //Path to save face
                    Directory.CreateDirectory(facesPath);   //Create dictionary to saving face path because it more effiency
                    File.WriteAllText(Path.Combine(facesPath, "Faces.txt"), trainingImg.Count.ToString() + ",");   //Write number of face in Faces.txt and add ,

                    for (int i = 1; i <= trainingImg.Count; i++)
                    {
                        string faceFilePath = Path.Combine(facesPath, $"face{i}.bmp");   //Path to save face
                        trainingImg[i - 1].Save(faceFilePath);   //Add face of i-1 in trainingImg to Faces folder
                        File.AppendAllText(Path.Combine(facesPath, "Faces.txt"), label[i - 1] + ",");   //Add name of i-1 in label to Faces.txt
                    }
                    imgbShow.Image = trainedFace;   //Show save image
                    imgbShow.Refresh();
                    MessageBox.Show(txtName.Text + " Added Successfully");

                    trainedFace = null;   //Prepare for next face
                }
                else
                {
                    MessageBox.Show("No face detected. Please try again.");
                    imgbShow.Image = null;   //Clear imageBox
                    imgbShow.Refresh();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            //-Summary : Capture face, add face and name to list, add list to folder
    }

        private void btnStart_Click(object sender, EventArgs e)
        {
            cam = new VideoCapture();
            cam.QueryFrame();   //Start Webcam
            Application.Idle += new EventHandler(FrameProcedure);   //Call FrameProcedure method
        }

        private void txtThreshold_TextChanged(object sender, EventArgs e)
        {
            if (txtThreshold.Text == "")
            {
                txtThreshold.Text = ""+2;   //Set default threshold = 2(* 1000)
                txtThreshold.SelectAll();
            }
            else
            {
                confidenceThreshold = (double.Parse(txtThreshold.Text) * 1000.0);   //Set threshold to times 1000

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

                }
                else
                {
                    name = "";
                }
                CvInvoke.PutText(frame, name, new Point(f.X - 2, f.Y - 2), FontFace.HersheyTriplex, 0.6, new MCvScalar(0, 0, 255), 1);
            }
            imgbWebcam.Image = frame;
            name = "";
            Same = 0;

        }


        private List<int> ConvertLabelsToInts(List<string> labels)
        {
            var labelMapReverse = new Dictionary<string, int>();   //Create Dictionary
            var intLabels = new List<int>();   //Create list of integer

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
