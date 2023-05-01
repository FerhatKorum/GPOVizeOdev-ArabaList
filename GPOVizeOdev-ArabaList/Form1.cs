using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Xml.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace GPOVizeOdev_ArabaList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                cars = System.Text.Json.JsonSerializer.Deserialize<List<Car>>(jsondata);
            }


            ShowList();
        }

        public List<Car> cars = new List<Car>()
        {
            new Car()
            {
                Marka = "Hyundai",
                Seri = "i20",
                Model = "1.4 MPI Jump",
                Yıl = "2023",
                Yakıt = "Benzin",
                Vites = "Manuel",
                AraçDurumu = "İkinci El",
                KM = "32.400",
                KasaTipi = "Hatcback 5 kapı",
                MotorGücü = "100hp",
                MotorHacmi = "1368",
                Çekiş = "Önden Çekiş",
                Renk = "Kırmızı",
                Plaka = "53 ZZ 256",
            },
             new Car()
            {
                Marka = "Audi",
                Seri = "A6",
                Model = "A6 Sedan 2.0 TDI ",
                Yıl = "2012",
                Yakıt = "Dizel",
                Vites = "Otomatik",
                AraçDurumu = "İkinci El",
                KM = "206.000",
                KasaTipi = "Sedan",
                MotorGücü = "177 hp",
                MotorHacmi = "1968 cc",
                Çekiş = "Önden Çekiş",
                Renk = "Beyaz",
                Plaka = "53 ZZ 256",
             }
        };

        public void ShowList()
        {
            listView1.Items.Clear();
            foreach (Car car in cars)
            {
                AddCartoListView(car);
            }
        }

        public void AddCartoListView(Car car)
        {
            ListViewItem item = new ListViewItem(new string[]
                   {
                        car.Marka,
                        car.Seri,
                        car.Model,
                        car.Yıl,
                        car.Yakıt,
                        car.Vites,
                        car.AraçDurumu,
                        car.KM,
                        car.KasaTipi,
                        car.MotorGücü,
                        car.MotorHacmi,
                        car.Çekiş,
                        car.Renk,
                        car.Plaka
                   }); ;

            item.Tag = car;
            listView1.Items.Add(item);

        }

        void EditCarOnListView(ListViewItem cItem, Car car)
        {
            cItem.SubItems[0].Text = car.Marka;
            cItem.SubItems[1].Text = car.Seri;
            cItem.SubItems[2].Text = car.Model;
            cItem.SubItems[3].Text = car.Yıl;
            cItem.SubItems[4].Text = car.Yakıt;
            cItem.SubItems[5].Text = car.Vites;
            cItem.SubItems[6].Text = car.AraçDurumu;
            cItem.SubItems[7].Text = car.KM;
            cItem.SubItems[8].Text = car.KasaTipi;
            cItem.SubItems[9].Text = car.MotorGücü;
            cItem.SubItems[10].Text = car.MotorHacmi;
            cItem.SubItems[11].Text = car.Çekiş;
            cItem.SubItems[12].Text = car.Renk;
            cItem.SubItems[13].Text = car.Plaka;

            cItem.Tag = car;
        }

        private void AddCommand(object sender, EventArgs e)
        {
            FrmCar frm = new FrmCar()
            {
                Text = "Araba Ekle",
                StartPosition = FormStartPosition.CenterParent,
                car = new Car()
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                cars.Add(frm.car);
                AddCartoListView(frm.car);
            }

        }

        private void EditCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem cItem = listView1.SelectedItems[0];

            Car secili = cItem.Tag as Car;


            FrmCar frm = new FrmCar()
            {
                Text = "Araba Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                car = Clone(secili),
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.car;
                EditCarOnListView(cItem, secili);
            }
        }

        Car Clone(Car car)
        {
            return new Car()
            {
                id = car.id,
                Marka = car.Marka,
                Seri = car.Seri,
                Model = car.Model,
                Yıl = car.Yıl,
                Yakıt = car.Yakıt,
                Vites = car.Vites,
                AraçDurumu = car.AraçDurumu,
                KM = car.KM,
                KasaTipi = car.KasaTipi,
                MotorGücü = car.MotorGücü,
                MotorHacmi = car.MotorHacmi,
                Çekiş = car.Çekiş,
                Renk = car.Renk,
                Plaka = car.Plaka,
            };
        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem cItem = listView1.SelectedItems[0];

            Car secili = cItem.Tag as Car;

            var sonuc = MessageBox.Show($"Seçili Araba Silinsin mi ? \n\n {secili.Marka}", "Silmeyi Onayla",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                cars.Remove(secili);
                listView1.Items.Remove(cItem);
            }
        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.json|Xml Formatı|*.xml",
            };

            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (sf.FileName.EndsWith("json"))
                {
                    string data = System.Text.Json.JsonSerializer.Serialize(cars);
                    File.WriteAllText(sf.FileName, data);
                }
                else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
                    serializer.Serialize(sw, cars);
                    sw.Close();
                }
            }
        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json , Xml Formatları|*.json;*.xml",
            };

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    cars = System.Text.Json.JsonSerializer.Deserialize<List<Car>>(jsondata);
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                    StreamReader sr = new StreamReader(of.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Car>));
                    cars = (List<Car>)serializer.Deserialize(sr);
                    sr.Close();
                }
                ShowList();
            }

        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");
        protected override void OnClosing(CancelEventArgs e)
        {
            string data = System.Text.Json.JsonSerializer.Serialize(cars);
            File.WriteAllText(temp, data);

            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }

    [Serializable]
    public class Car
    {


        public string id;
        [Browsable(false)]
        public string ID
        {
            get
            {
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }
        }

        [Category("Araç"), DisplayName("Marka")]
        public string Marka { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Seri")]

        public string Seri { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Model")]

        public string Model { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Yıl")]

        public string Yıl { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Yakıt"), Description("Lütfen Benzin - Dizel - Mazot olarak giriniz.")]

        public string Yakıt { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Vites")]

        public string Vites { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Araç Durumu")]

        public string AraçDurumu { get; set; }
        [Category("Araç Özellikleri"), DisplayName("KM")]

        public string KM { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Kasa Tipi")]

        public string KasaTipi { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Motor Gücü")]

        public string MotorGücü { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Motor Hacmi")]

        public string MotorHacmi { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Çekiş")]

        public string Çekiş { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Renk")]

        public string Renk { get; set; }
        [Category("Araç Özellikleri"), DisplayName("Plaka")]

        public string Plaka { get; set; }
     

    }
}
