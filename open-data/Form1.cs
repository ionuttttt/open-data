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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace open_data
{

    public partial class Form1 : Form
    {
        private List<macchine> listMacchine;        // Variabile che contiene la lista di oggetti macchine  
        private string fileName = "catalogo.csv";        // Nome del file CSV da cui verranno caricati i dati
        // Variabili booleane che tengono traccia di quale colonna è ordinata
        bool C_anno = false;
        bool C_prezzo = false;
        bool C_km = false;

        public Form1()
        {
            InitializeComponent();
            // Eventi che scattano al click sulle colonne o all'attivazione di un elemento della listView
            listView1.ColumnClick += listView1_ColumnClick;
            listView1.ItemActivate += listView1_ItemActivate;
            listView1.FullRowSelect=true;
            listMacchine = new List<macchine>();            // Inizializzazione della lista di macchine




            // Caricamento dei dati dal file CSV nella lista di macchine e nella listView
            CaricaMacchineDaCSV();
            CaricaMacchinenellalistview();
        }

        // Metodo per caricare i dati delle macchine da un file CSV
        private void CaricaMacchineDaCSV()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))        // Lettura del file CSV
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = sr.ReadLine()) != null)    // Ciclo per leggere ogni riga del CSV

                    {
                        // Salta la prima riga
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        string[] fields = line.Split(',');  // Split dei dati separati da virgole
                        // Creazione di un nuovo oggetto macchine e aggiunta alla lista
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
        // Metodo per caricare i dati nella ListView
        private void CaricaMacchinenellalistview()
        {
            listView1.View = View.Details;
            listView1.Columns.Clear(); 
            listView1.Columns.Add("Numero", 50);
            listView1.Columns.Add("Marca", 250);
            listView1.Columns.Add("Anno", 50);
            listView1.Columns.Add("Prezzo", 100);
            listView1.Columns.Add("Km", 60);
            listView1.Columns.Add("Carburante", 100);
            listView1.Columns.Add("Venditore", 100);
            listView1.Columns.Add("Trasmissione", 100);
            listView1.Columns.Add("Proprietari", 150);
            listView1.Items.Clear();
            // Aggiunta dei dati alla ListView per ogni macchina nella lista
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

                listView1.Items.Add(item);// Aggiunta dell'elemento alla listView
            }
        }
        // Gestione dell'evento di click sulle colonne della listView
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs ordinaeriordinacolonna)
        {
            // Ordinamento in base alla colonna "Anno"
            if (ordinaeriordinacolonna.Column == 2) 
            {
                if (C_anno == false)
                {
                    C_anno = true;
                    listMacchine.Sort((x, y) => string.Compare(x.Anno, y.Anno));
                }
                else if (C_anno == true)
                {
                    C_anno = false;
                    ordina();// Metodo che ordina la lista in modo predefinito
                }
            }
            // Ordinamento in base alla colonna "Prezzo"
            else if (ordinaeriordinacolonna.Column == 3) 
            {
                if (C_prezzo == false)
                {
                    C_prezzo = true;
                    listMacchine.Sort((x, y) =>
                    {
                        int prezzoX = ConvertiPrezzo(x.Prezzo);
                        int prezzoY = ConvertiPrezzo(y.Prezzo);
                        return prezzoX.CompareTo(prezzoY);
                    });
                }
                else if (C_prezzo == true)
                {
                    C_prezzo = false;
                    ordina();
                }
            }
            // Ordinamento in base alla colonna "Km"
            else if (ordinaeriordinacolonna.Column == 4) 
            {
                if (C_km == false)
                {
                    C_km = true;
                    listMacchine.Sort((x, y) =>
                    {
                        int kmX = ConvertiPrezzo(x.Km);
                        int kmY = ConvertiPrezzo(y.Km);
                        return kmX.CompareTo(kmY);
                    });
                }
                else if (C_km == true)
                {
                    C_km = false;
                    ordina();
                }
            }

            controlla();// Controlla se ci sono altri ordinamenti attivi
            CaricaMacchinenellalistview();// Ricarica la lista dopo l'ordinamento

        }
        // Metodo per convertire i prezzi (da stringa a intero)
        private int ConvertiPrezzo(string prezzo)
        {
            int.TryParse(prezzo, out int prezzoNumerico);
            return prezzoNumerico;
        }
        // Classe "macchine"
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
            
        }
        //Ordinamento in base al campo "petrol"
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "diesel"
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox2.Checked)
            {
                listMacchine = listMacchine.OrderByDescending(m => m.Carburante == "Diesel").ToList();//llll
            }
            else
            {
                ordina();
                controlla();
            }
            CaricaMacchinenellalistview();
        }
        //Ordinamento in base al campo "cng"
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "manual"
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "automatic"
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Third Owner"
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "First Owner"
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Second Owner"
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Fourth & Above Owner"
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Individual"
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Dealer"
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {

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
        //Ordinamento in base al campo "Trustmark Dealer"
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

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
        // Metodo che ordina la lista in modo predefinito
        private void ordina()
        {
            listMacchine.Sort((x, y) =>
            {
                int NumeroelencoX = ConvertiPrezzo(x.Numeroelenco);
                int NumeroelencoY = ConvertiPrezzo(y.Numeroelenco);
                return NumeroelencoX.CompareTo(NumeroelencoY);
            });
        }
        // metodo che controlla se ci sono altri ordinamenti attivi (in caso affermativo gli applica)
        private void controlla()
        {
            if (C_anno == true)
            {
                listMacchine.Sort((x, y) => string.Compare(x.Anno, y.Anno));
            }
            if (C_prezzo == true)
            {
                listMacchine.Sort((x, y) =>
                {
                    int prezzoX = ConvertiPrezzo(x.Prezzo);
                    int prezzoY = ConvertiPrezzo(y.Prezzo);
                    return prezzoX.CompareTo(prezzoY);
                });

            }
            if (C_km == true)
            {
                listMacchine.Sort((x, y) =>
                {
                    int kmX = ConvertiPrezzo(x.Km);
                    int kmY = ConvertiPrezzo(y.Km);
                    return kmX.CompareTo(kmY);
                });
            }
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
        // Metodo per il pulsante "reset" che azzera tutti i filtri e ricarica la lista
        private void button1_Click(object sender, EventArgs e)
        {
            C_anno = false;
            C_prezzo = false;
            C_km = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox11.Checked = false;
            checkBox12.Checked = false;
            ordina();
            CaricaMacchinenellalistview();
        }
        // Evento per l'attivazione di un elemento nella listView (quando l'utente fa doppio clic)
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)    // Controlla se c'è almeno un elemento selezionato nella ListView

            {

                var selectedItem = listView1.SelectedItems[0];        // Recupera l'elemento selezionato
                string marca = selectedItem.SubItems[1].Text;       // Estrae il testo della colonna "Marca" (seconda colonna) dall'elemento selezionato


                string Url = $"https://www.google.com/search?q={marca}";        // Crea un URL di ricerca di Google utilizzando il valore della marca selezionata
              // Usa Process.Start per aprire il link nel browser predefinito
                Process.Start(new ProcessStartInfo
                {
                    FileName = Url,
                    UseShellExecute = true 
                });
            }
        }
    }
}
