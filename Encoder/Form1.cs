using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Encoder
{
    public partial class Form1 : Form
    {    
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        Encoding encoding = Encoding.UTF8;        
        FileStream FS;
        byte[] EncryptedData;
        byte[] DecryptedData;
        byte[] data = new byte[1024];
        public static RSAParameters publicKey, privateKey;
        public string CD = Environment.CurrentDirectory;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {                
                data = encoding.GetBytes(textBox1.Text);
                EncryptedData = RSA.Encrypt(data, false);
                textBox2.Text = Convert.ToBase64String(EncryptedData);
                textBox1.Clear();
            } else { MessageBox.Show("Введите текст!"); textBox1.Focus(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                DecryptedData = RSA.Decrypt(EncryptedData, false);
                textBox1.Text = encoding.GetString(DecryptedData);
                textBox2.Clear();
            }
            else { MessageBox.Show("Введите текст!"); textBox2.Focus(); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(CD+"\\publicKey.xml") == true && File.Exists(CD+"\\privateKey.xml") == true)
            {
                using (StreamReader sr = new StreamReader(CD + "\\publicKey.xml"))
                { RSA.FromXmlString(sr.ReadToEnd()); sr.Close(); }
                using (StreamReader sr = new StreamReader(CD + "\\privateKey.xml"))
                { RSA.FromXmlString(sr.ReadToEnd()); sr.Close(); }
            }
            else
            {
                FS = File.Create(CD + "\\publicKey.xml"); FS.Close();
                FS = File.Create(CD + "\\privateKey.xml"); FS.Close();
                using (StreamWriter sw = new StreamWriter(CD + "\\publicKey.xml", false, encoding))
                {sw.WriteLine(RSA.ToXmlString(false)); sw.Close();}
                using (StreamWriter sw = new StreamWriter(CD + "\\privateKey.xml", false, encoding))
                { sw.WriteLine(RSA.ToXmlString(true)); sw.Close();}
                using (StreamReader sr = new StreamReader(CD + "\\publicKey.xml"))
                {RSA.FromXmlString(sr.ReadToEnd()); sr.Close();}
                using (StreamReader sr = new StreamReader(CD + "\\privateKey.xml"))
                {RSA.FromXmlString(sr.ReadToEnd()); sr.Close();}
            }
        }
    }
}
