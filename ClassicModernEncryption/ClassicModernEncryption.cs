using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace CeasarShiftCipherCSharp
{
    public partial class CeasarShiftCipherForm : Form
    {

        private CeasarShiftCipherEngine objCSCipher = new CeasarShiftCipherEngine();

        public CeasarShiftCipherForm()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string plainText = txtPlaintext.Text.Trim();
            string keyText = txtKey.Text.Trim();

            if (plainText == "" || keyText == "")
                MessageBox.Show("Silahkan isi terlebih dahulu plain teks dan kuncinya", "Data tidak komplit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else 
            {
                try
                {
                    txtCiphertext.Text = objCSCipher.Encrypt(plainText, Convert.ToInt32(keyText));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string cipherText = txtCiphertext.Text.Trim();
            string keyText = txtKey.Text.Trim();

            if (cipherText == "" || keyText == "")
                MessageBox.Show("Silahkan isi terlebih dahulu data cipher teks and kuncinya", "Data tidak komplit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    txtPlaintext.Text = objCSCipher.Decrypt(cipherText, Convert.ToInt32(keyText));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #region rijndael algorithm

        internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";
        #region NewRijndaelManaged
        /// <summary>
        /// Membuat kelas RijndaelManaged
        /// </summary>
        /// <param name="salt" />Pasword salt
        /// <returns></returns>
        private static RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox1.Text == "")
                MessageBox.Show("Salt key minimal 8 byte", "Data tidak komplit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    textBox2.Text = EncryptRijndael(textBox1.Text, textBox3.Text);
                    textBox1.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Salt key minimal 8 byte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox2.Text == "")
                MessageBox.Show("Salt key minimal 8 byte", "Data tidak komplit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    textBox1.Text = DecryptRijndael(textBox2.Text, textBox3.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Salt key minimal 8 byte", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Encrypt plain teks dan memberikan hasil sebuah byte array (BASE64 string)
        /// </summary>
        /// <param name="teks" />teks = plainteks yang akan diencrypt
        /// <param name="salt" />pasword salt
        /// <returns>teks yang sudah terenkripsi</returns>
        public static string EncryptRijndael(string teks, string salt)
        {
            if (string.IsNullOrEmpty(teks))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(teks);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// melakukan cek apakah string termasuk base64 encoded
        /// </summary>
        /// <param name="base64String" />base64 encoded string
        /// <returns>masih mengembalikan Base64 encoded string</returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        /// <summary>
        /// Decrypts teks yang telah di encrypt
        /// </summary>
        /// <param name="cipherText" />BASE64 yang telah dienkripsi
        /// <param name="salt" />Pasword salt
        /// <returns>Plainteks asli</returns>
        public static string DecryptRijndael(string cipherText, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }

        #endregion


    }
}