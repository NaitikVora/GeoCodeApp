using Excel = Microsoft.Office.Interop.Excel;
using GMap.NET;
using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace GeoCodeAppFinal
{
    public partial class Form2 : Form
    {
        private List<PointLatLng> _points;

        public Form2()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
        }
        DataTable dt = new DataTable();
        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtLat.Text) && String.IsNullOrEmpty(txtLng.Text) && (String.IsNullOrEmpty(textBox2.Text)==false)) //laod favourite 
            {
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader("C:\\Coordinates\\location.txt");

                    //Read the first line of text
                    combined = Convert.ToString(sr.ReadLine());

                    //Continue to read until you reach end of file
                    while (combined != null)
                    {

                        String[] com = (combined.Split(','));
                        //write the lie to console window
                        latit = Convert.ToDouble(com[0]);
                        longi = Convert.ToDouble(com[1]);
                        String location = Convert.ToString(com[2]);

                        if (location == textBox2.Text)
                        {
                            label4.Text = com[2];
                            map.Position = new PointLatLng(latit, longi);
                            map.MinZoom = 5;
                            map.MaxZoom = 100;
                            map.Zoom = 10;


                            PointLatLng point = new PointLatLng(latit, longi);
                            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                            GMapOverlay markers = new GMapOverlay("markers");
                            markers.Markers.Add(marker);
                            map.Overlays.Add(markers);
                            combined = sr.ReadLine();
                        }
                    }

                    //close the file
                    sr.Close();

                }
                catch (Exception exx)
                {
                    MessageBox.Show("Exception: " + exx.Message);
                }
            }
            else //load from lat,lng
            {

                double lat = Convert.ToDouble(txtLat.Text);
                double lng = Convert.ToDouble(txtLng.Text);
                map.Position = new PointLatLng(lat, lng);
                map.MinZoom = 5;
                map.MaxZoom = 100;
                map.Zoom = 10;


                PointLatLng point = new PointLatLng(lat, lng);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                map.Overlays.Add(markers);

                try
                {
                    myPath = @"C:\\coordinates\\location.txt";
                    if (File.Exists(myPath))
                    {
                        using (StreamWriter sw = File.AppendText(myPath))
                        {
                            sw.WriteLine(txtLat.Text + "," + txtLng.Text);
                            sw.Close();
                        }
                    }
                    else
                    {
                        throw new Exception("First create a file !");

                    }
                }
                catch (Exception exxx)
                {
                    MessageBox.Show("Exception: " + exxx.Message);
                }

                //

               
                using (System.IO.TextReader tr = File.OpenText(@"C:\\Coordinates\\location.txt"))
                {
                    string line;
                    //add new list of string arrey
                    List<string[]> lststr = new List<string[]>();
                    while ((line = tr.ReadLine()) != null)
                    {

                        string[] items = line.Trim().Split(' ');
                        lststr.Add(items);
                    }
                    int col = lststr.Max(x => x.Length);
                    if (dt.Columns.Count == 0)
                    {
                        // Create the data columns for the data table based on the number of items
                        // on the first line of the file
                        for (int i = 0; i < col; i++)
                            dt.Columns.Add(new DataColumn("Coordinates :", typeof(string)));
                    }
                    // loop the list 
                    foreach (string[] item in lststr)
                    {
                        dt.Rows.Add(item);
                    }


                }
                //show it in gridview 
                this.dataGridView1.DataSource = dt;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 for1 = new Form1();
            for1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _points.Add(new PointLatLng(Convert.ToDouble(txtLat.Text), Convert.ToDouble(txtLng.Text)));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _points.Clear();
            map.Overlays.Clear();
            map.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var route = GoogleMapProvider.Instance.GetRoute(_points[0],_points[1],false,false,14);
            var r = new GMapRoute(route.Points, "My Route")
            {
                Stroke = new Pen(Color.Red, 5)
            };
            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyDh3m3ew1lpyiOC38_eQUxg-PgzWqXqqzo";
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.ShowCenter = false;
           
            
            dataGridView1.ShowEditingIcon=true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var polygon = new GMapPolygon(_points, "My Area")
            {
                Stroke = new Pen(Color.DarkGreen,2),
                Fill = new SolidBrush(Color.BurlyWood)
            };
            var polygons = new GMapOverlay("polygons");
            polygons.Polygons.Add(polygon);
            map.Overlays.Add(polygons);
            map.Refresh();
        }
        double latit;
        double longi;
        String combined;
        String myPath;

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("C:\\Coordinates\\location.txt");

                //Read the first line of text
                combined = Convert.ToString(sr.ReadLine());
                
                //Continue to read until you reach end of file
                while (combined != null)
                {
                    
                    String[] com = (combined.Split(','));
                    //write the lie to console window
                    MessageBox.Show("Latitude = " + com[0]);
                    MessageBox.Show("Longitude = " + com[1]);
                    latit = Convert.ToDouble(com[0]);
                    longi = Convert.ToDouble(com[1]);
                    map.Position = new PointLatLng(latit, longi);
                    map.MinZoom = 5;
                    map.MaxZoom = 100;
                    map.Zoom = 10;


                    PointLatLng point = new PointLatLng(latit, longi);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                    GMapOverlay markers = new GMapOverlay("markers");
                    markers.Markers.Add(marker);
                    map.Overlays.Add(markers);

                    //Read the next line
                    combined = sr.ReadLine();
                }

                //close the file
                sr.Close();

            }
            catch (Exception exx)
            {
                MessageBox.Show("Exception: " + exx.Message);
            }
            /*
            latit = Convert.ToDouble(lat);
            longi = Convert.ToDouble(lng);
            map.Position = new PointLatLng(latit, longi);
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 10;


            PointLatLng point = new PointLatLng(latit, longi);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
            GMapOverlay markers = new GMapOverlay("markers");
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
            */
    }

        private void button8_Click(object sender, EventArgs e)
        {
            

        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream f = new FileStream("C:\\coordinates\\location.txt", FileMode.Create);
                f.Close();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Exception: " + exp.Message);
            }    
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                myPath = @"C:\\coordinates\\location.txt";
                File.Delete(myPath);
            }
            catch (Exception epx)
            {
                MessageBox.Show("Exception: " + epx.Message);
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                myPath = @"C:\\coordinates\\location.txt";
                if (File.Exists(myPath))
                {
                    using (StreamWriter sw = File.AppendText(myPath))
                    {
                        sw.WriteLine(txtLat.Text+","+txtLng.Text+","+textBox1.Text);
                        sw.Close();
                    }
                }
                else
                {
                    throw new Exception("First create a file !");

                }
            }
            catch (Exception exxx)
            {
                MessageBox.Show("Exception: " + exxx.Message);
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
           
          
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form4 fo4 = new Form4();
            fo4.Show();
        }
    }
}
