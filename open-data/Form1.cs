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

namespace open_data
{
    public partial class Form1 : Form
    {
        private List<macchine> listMacchine;
        private string fileName = "catalogo.csv";
        public Form1()
        {
            InitializeComponent();
            listView1.ColumnClick += listView1_ColumnClick;
            listMacchine = new List<macchine>();



            CaricaMacchineDaCSV();
            CaricaMacchinenellalistview();
        }


        private void CaricaMacchineDaCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        string[] fields = line.Split(',');
                        macchine g = new macchine(
                            fields[0],
                            fields[1],
                            fields[2],
                            fields[3],
                            fields[4],
                            fields[5],
                            fields[6],
                            fields[7],
                            fields[8]
                        );

                        listMacchine.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nella lettura del file CSV: " + ex.Message);
            }
        }

        private void CaricaMacchinenellalistview()
        {
            listView1.View = View.Details;
            listView1.Columns.Clear(); // Azzera le colonne esistenti
            listView1.Columns.Add("Numero", 50);
            listView1.Columns.Add("Marca", 250);
            listView1.Columns.Add("Anno", 50);
            listView1.Columns.Add("Prezzo", 100);
            listView1.Columns.Add("Km", 60);
            listView1.Columns.Add("Carburante", 100);
            listView1.Columns.Add("Venditore", 100);
            listView1.Columns.Add("Trasmissione", 100);
            listView1.Columns.Add("Proprietari", 150);
            listView1.Items.Clear(); // Azzera gli item esistenti

            foreach (var macchina in listMacchine)
            {
                ListViewItem item = new ListViewItem(macchina.Numeroelenco.ToString());
                item.SubItems.Add(macchina.Marca);
                item.SubItems.Add(macchina.Anno);
                item.SubItems.Add(macchina.Prezzo);
                item.SubItems.Add(macchina.Km);
                item.SubItems.Add(macchina.Carburante);
                item.SubItems.Add(macchina.Venditore.ToString());
                item.SubItems.Add(macchina.Trasmissione.ToString());
                item.SubItems.Add(macchina.Proprietari.ToString());

                listView1.Items.Add(item);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs ordinaeriordinacolonna)
        {
            if (ordinaeriordinacolonna.Column == 2) // Indice della colonna "Anno"
            {
                listMacchine.Sort((x, y) => string.Compare(x.Anno, y.Anno));

            }
            else if (ordinaeriordinacolonna.Column == 3) // Indice della colonna "Prezzo"
            {
                // Ordinamento in base al prezzo, dopo averlo convertito in formato numerico
                listMacchine.Sort((x, y) =>
                {
                    int prezzoX = ConvertiPrezzo(x.Prezzo);
                    int prezzoY = ConvertiPrezzo(y.Prezzo);
                    return prezzoX.CompareTo(prezzoY);
                });
            }
           else if (ordinaeriordinacolonna.Column == 4) // Indice della colonna "Km"
            {
                listMacchine.Sort((x, y) =>
                {
                    int kmX = ConvertiPrezzo(x.Km);
                    int kmY = ConvertiPrezzo(y.Km);
                    return kmX.CompareTo(kmY);
                });
            }

            controlla();
            CaricaMacchinenellalistview();

        }

        private int ConvertiPrezzo(string prezzo)
        {
            int.TryParse(prezzo, out int prezzoNumerico);
            return prezzoNumerico;
        }

        class macchine
        {


            public macchine(
                string numeroelenco,
                string marca,
                string anno,
                string prezzo,
                string km,
                string carburante,
                string venditore,
                string trasmissione,
                string proprietari)
            {
                Numeroelenco = numeroelenco;
                Marca = marca;
                Anno = anno;
                Prezzo = prezzo;
                Km = km;
                Carburante = carburante;
                Venditore = venditore;
                Trasmissione = trasmissione;
                Proprietari = proprietari;

            }

            public string Numeroelenco { get; set; }
            public string Marca { get; set; }
            public string Anno { get; set; }
            public string Prezzo { get; set; }
            public string Km { get; set; }
            public string Carburante { get; set; }
            public string Venditore { get; set; }
            public string Trasmissione { get; set; }
            public string Proprietari { get; set; }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox2.Checked = false;
            checkBox3.Checked = false;

            // Filtra e ordina le macchine a petrol in cima
            if (checkBox1.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "Petrol").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox1.Checked = false;
            checkBox3.Checked = false;

            // Filtra e ordina le macchine a diesel in cima
            if (checkBox2.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "Diesel").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox1.Checked = false;
            checkBox2.Checked = false;

            // Filtra e ordina le macchine a CNG in cima
            if (checkBox3.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "CNG").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox4.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox5.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Trasmissione == "Manual").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox5.Checked = false;

            // Filtra e ordina le macchine a automatic in cima
            if (checkBox4.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Trasmissione == "Automatic").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox9.Checked = false;
            checkBox8.Checked = false;
            checkBox6.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox7.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Third Owner").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox6.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox9.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "First Owner").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox9.Checked = false;
            checkBox7.Checked = false;
            checkBox6.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox8.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Second Owner").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox9.Checked = false;
            checkBox8.Checked = false;
            checkBox7.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox6.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Fourth & Above Owner").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox11.Checked = false;
            checkBox10.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox12.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Individual").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox12.Checked = false;
            checkBox10.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox11.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Dealer").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            // Deseleziona le altre checkbox
            checkBox11.Checked = false;
            checkBox12.Checked = false;

            // Filtra e ordina le macchine a manual in cima
            if (checkBox10.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Trustmark Dealer").ToList();
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }

        private void ordina()
        {
            listMacchine.Sort((x, y) =>
            {
                int NumeroelencoX = ConvertiPrezzo(x.Numeroelenco);
                int NumeroelencoY = ConvertiPrezzo(y.Numeroelenco);
                return NumeroelencoX.CompareTo(NumeroelencoY);
            });
        }

        private void controlla()
        {
            if (checkBox1.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "Petrol").ToList();
            }
            if (checkBox2.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "Diesel").ToList();
            }
            if (checkBox3.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "CNG").ToList();
            }
            if (checkBox5.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Trasmissione == "Manual").ToList();
            }
            if (checkBox4.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Trasmissione == "Automatic").ToList();
            }
            if (checkBox6.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Fourth & Above Owner").ToList();
            }
            if (checkBox7.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Third Owner").ToList();
            }
            if (checkBox8.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "Second Owner").ToList();
            }
            if (checkBox9.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Proprietari == "First Owner").ToList();
            }
            if (checkBox10.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Trustmark Dealer").ToList();
            }
            if (checkBox11.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Dealer").ToList();
            }
            if (checkBox12.Checked == true)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Venditore == "Individual").ToList();
            }
        }
    }
}
