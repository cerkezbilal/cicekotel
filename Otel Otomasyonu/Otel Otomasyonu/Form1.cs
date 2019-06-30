using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Otel_Otomasyonu
{
    public partial class Form1 : Form
    {

        public Form2 fr2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            fr2.fr1 = this;
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Otel.mdb");
        public void OdaDurumu()
        {
            int sayac = 101;
            foreach (Control btn in Controls)
            {
                if (btn is Button)
                {
                    if (btn.Name != "btnOdaDuzenle" && btn.Name != "btnEkle")
                    {
                        btn.BackColor = Color.White;
                        btn.Text = "ODA-" + sayac.ToString();
                        sayac++;
                    }
                }
            }

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Select * from odabilgileri ",baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                foreach (Control oda in Controls)
                {
                    if(oda is Button)
                    {
                        if (read["oda"].ToString() == oda.Text && read["durumu"].ToString()=="BOŞ")
                        {
                            oda.BackColor = Color.Green;
                            cmbOdalar.Items.Add(read["oda"].ToString());
                            fr2.cmbodaduzenle.Items.Add(read["oda"].ToString());
                        }

                        if (read["oda"].ToString() == oda.Text && read["durumu"].ToString() == "DOLU" )     {
                            oda.BackColor = Color.Red;
                        }
                    }

                }
            }
            baglanti.Close();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            OdaDurumu();
            foreach (Control cikis in Controls)
            {
                if (cikis is Button)
                {
                    
                        cikis.Click += Cikis_Click;
                    
                }
            }

            
        }

        private void Cikis_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b.BackColor == Color.Red)
            {
                DialogResult dialog = MessageBox.Show("Oda çıkışı yapılsın mı ?", "Çıkış", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (dialog==DialogResult.Yes)
                {
                    baglanti.Open();
                    OleDbCommand komut = new OleDbCommand("delete * from kayitbilgileri where oda='" + b.Text + "' ", baglanti);
                    komut.ExecuteNonQuery();
                    OleDbCommand komut2 = new OleDbCommand("update odabilgileri set durumu= 'BOŞ' where oda='" + b.Text + "'", baglanti);
                    komut2.ExecuteNonQuery();

                    baglanti.Close();

                    MessageBox.Show("Oda çıkışı yapıldı", "Çıkış");
                    cmbOdalar.Items.Clear();
                    fr2.cmbodaduzenle.Items.Clear();
                    fr2.comboBox1.Items.Clear();
                    OdaDurumu();
                }
                
            }
            
        }

        private void cmbOdalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Select * from odabilgileri where oda='" + cmbOdalar.SelectedItem+"'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read()) 
            {
                textKat.Text = read["kat"].ToString();
                textBanyoSayisi.Text = read["banyosayisi"].ToString();
                textYatakSayisi.Text = read["yataksayisi"].ToString();
                textCephe.Text = read["cephe"].ToString();
                textGunlukUcret.Text = read["gucret"].ToString();

            }
            baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into kayitbilgileri(ad,soyad,adres,telefon,mail,oda,kat,gtarihi,ctarihi,gun,gucret,tutar,odemeturu,aciklama) values('"+textAd.Text+"','"+textSoyad+"','"+textAdres.Text+"','"+textTel.Text+"','"+textMail.Text+"','"+cmbOdalar.Text+"','"+textKat.Text+"','"+dateTimePicker1.Text+"','"+dateTimePicker2.Text+"','"+textGun.Text+"','"+textGunlukUcret.Text+"','"+textTutar.Text+"','"+cmbOdemeTuru.Text+"','"+textTutar.Text+"') ", baglanti);
            komut.ExecuteNonQuery();

            OleDbCommand komut2 = new OleDbCommand("update odabilgileri set durumu='DOLU' where oda = '"+cmbOdalar.SelectedItem+"'",baglanti);
            komut2.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Eklendi.", "Kayıt");
            cmbOdalar.Text = "";
            cmbOdemeTuru.Text = "";
            cmbOdalar.Items.Clear();
            fr2.comboBox1.Items.Clear();
            fr2.cmbodaduzenle.Items.Clear();
            OdaDurumu();

            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is TextBox)
                {
                    groupBox1.Controls[i].Text = "";
                }
            }
        }

        private void btnOdaDuzenle_Click(object sender, EventArgs e)
        {
          

          
            fr2.ShowDialog();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            
            TimeSpan gun = new TimeSpan();
            gun = DateTime.Parse(dateTimePicker2.Text) - DateTime.Parse(dateTimePicker1.Text);
            textGun.Text = gun.TotalDays.ToString();

            textTutar.Text = (double.Parse(textGun.Text) * double.Parse(textGunlukUcret.Text)).ToString();

        }
    }
}
