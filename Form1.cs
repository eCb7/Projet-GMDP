using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projet___Gestionnaire_MDP
{
    public partial class Form1 : Form
    {
        private readonly string filePath = "passwords.dat"; // Fichier local pour sauvegarder les mots de passe
        private readonly string encryptionKey = "MySuperSecureKey123!"; // Clé de chiffrement AES (doit faire 16 ou 32 caractères)

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPasswords();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePasswords(); 
        }

        // Méthode pour sauvegarder les mots de passe
        private void SavePasswords()
        {
            try
            {
                var passwordData = new StringBuilder();

                
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string app = row.Cells["colApplication"].Value?.ToString() ?? "";
                        string username = row.Cells["colUsername"].Value?.ToString() ?? "";
                        string password = row.Cells["colPassword"].Value?.ToString() ?? "";

                        
                        passwordData.AppendLine($"{app};{username};{password}");
                    }  
                }

                
                string encryptedData = Encrypt(passwordData.ToString(), encryptionKey);
                File.WriteAllText(filePath, encryptedData);

                MessageBox.Show("Mots de passe sauvegardés avec succès !", "Sauvegarde");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur");
            }
        }

        // Méthode pour charger les mots de passe
        private void LoadPasswords()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    
                    string encryptedData = File.ReadAllText(filePath);
                    string decryptedData = Decrypt(encryptedData, encryptionKey);

                    
                    string[] lines = decryptedData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 3)
                        {
                            dataGridView.Rows.Add(parts[0], parts[1], parts[2]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur");
            }
        }

        // Méthodes pour chiffrer et déchiffrer les données (AES)
        private string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // IV par défaut (zéro)
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string Decrypt(string cipherText, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // IV par défaut (zéro)
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

         // Bouton pour ajouter un mot de passe
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string app = Microsoft.VisualBasic.Interaction.InputBox("Nom de l'application/site :", "Ajouter");
            string username = Microsoft.VisualBasic.Interaction.InputBox("Nom d'utilisateur :", "Ajouter");
            string password = Microsoft.VisualBasic.Interaction.InputBox("Mot de passe :", "Ajouter");

            if (!string.IsNullOrWhiteSpace(app) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                string encryptedPassword = Encrypt(password, encryptionKey);
                dataGridView.Rows.Add(app, username, encryptedPassword);
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Erreur");
            }
        }

        // Methode export mdp depuis fichier texte
        private void ExportToTextFile()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Fichier texte (*.txt)|*.txt";
                sfd.Title = "Exporter vers un fichier texte";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string app = row.Cells["colApplication"].Value?.ToString() ?? string.Empty;
                                string username = row.Cells["colUsername"].Value?.ToString() ?? string.Empty;
                                string password = row.Cells["colPassword"].Value?.ToString() ?? string.Empty;

                                sw.WriteLine($"{app};{username};{password}");
                            }
                        }
                    }
                    MessageBox.Show("Données exportées avec succès.", "Exportation");
                }
            }
        }





        // Methode import mdp depuis fichier texte
        private void ImportFromTextFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Fichier texte (*.txt)|*.txt";
                ofd.Title = "Importer depuis un fichier texte";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(ofd.FileName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(';');
                            if (parts.Length == 3)
                            {
                                dataGridView.Rows.Add(parts[0], parts[1], parts[2]);
                            }
                        }
                    }
                    MessageBox.Show("Données importées avec succès.", "Importation");
                }
            }
        }





        // a partir de la c'est les methodes liées a la partie graphique (boutons) 



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                dataGridView.Rows.RemoveAt(dataGridView.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Sélectionnez une ligne à supprimer.", "Erreur");
            }
        }



        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.ShowDialog(); // Affiche le formulaire À propos
        }


        private void importerDepuisTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromTextFile();
        }


        private void exporterVersTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToTextFile();
        }


        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); // Ferme l'application
        }


        
        
        
        
        
        
        // Apres c'est des metohdes "inutiles" mais que je peux pas retirer pour des raisons mystiques
        private void fichierToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importerLesMotsDePassesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exporterLesMotsDePassesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
