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
    public partial class Form2 : Form
    {

        public Form1 fr1;
        public Form2()
        {
            InitializeComponent();
        }
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Otel.mdb");

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            cmbodaduzenle.Items.Clear();
            fr1.OdaDurumu();
            Yenileme();
            
        }

        private void cmbodaduzenle_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("Select * from odabilgileri where oda='" +cmbodaduzenle.SelectedItem + "'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                textKat.Text = read["kat"].ToString();
                textBanyoSayisi.Text = read["banyosayisi"].ToString();
                textYatakSayisi.Text = read["yataksayisi"].ToString();
                textCephe.Text = read["cephe"].ToString();
                textBox6.Text = read["gucret"].ToString();

            }
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("update odabilgileri set kat='" + textKat.Text+ "', yataksayisi='"+textYatakSayisi.Text+"', banyosayisi='"+textBanyoSayisi.Text+"', cephe='"+textCephe.Text+"', gucret='"+textBox6.Text+"'where oda='"+cmbodaduzenle.SelectedItem+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Oda kaydı güncellendi", "Güncellendi");

            

            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is TextBox)
                {
                    groupBox1.Controls[i].Text = "";
                }
            }
            cmbodaduzenle.Text = "";
            comboBox1.Items.Clear();
            cmbodaduzenle.Items.Clear();
            fr1.cmbOdalar.Items.Clear();
            fr1.OdaDurumu();
            Yenileme();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("insert into odabilgileri(oda,kat,yataksayisi,banyosayisi,cephe,gucret,durumu) values('" + comboBox1.Text + "','" + textBox4.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','" + textBox1.Text + "','" + textBox5.Text + "', 'BOŞ')", baglanti);
            komut.ExecuteNonQuery();

           
            baglanti.Close();
            fr1.OdaDurumu();
            MessageBox.Show("Oda Kaydı Oluşturuldu.", "Oda Kayıt");
            comboBox1.Text = "";
            //cmbOdemeTuru.Text = "";
          

            for (int i = 0; i < groupBox1.Controls.Count; i++)
            {
                if (groupBox1.Controls[i] is TextBox)
                {
                    groupBox1.Controls[i].Text = "";
                }
            }
            
            fr1.cmbOdalar.Items.Clear();
            cmbodaduzenle.Items.Clear();
            comboBox1.Items.Clear();
            fr1.OdaDurumu();
            Yenileme();
            
        }
        public void Yenileme()
        {
            foreach (Control item in fr1.Controls)
            {
                if (item is Button)
                {
                    if (item.BackColor == Color.White)
                    {
                        comboBox1.Items.Add(item.Text);
                    }
                }
            }
        }
    }
}
