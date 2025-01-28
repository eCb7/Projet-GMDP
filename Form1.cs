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

namespace Projet___Gestionnaire_MDP
{
    public partial class Form1 : Form
    {

        // Methode Cryptage
        private string Encrypt(string clearText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(clearText);
            return Convert.ToBase64String(bytes);
        }





        // Methode Decryptage
        private string Decrypt(string encryptedText)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(bytes);
        }





        // Methode pour sauvegarder les mdp
        private void SavePasswords()
        {
            using (StreamWriter sw = new StreamWriter("passwords.txt"))
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        string line = $"{row.Cells[0].Value};{row.Cells[1].Value};{row.Cells[2].Value}";
                        sw.WriteLine(line);
                    }
                }
            }
        }




        // Methode pour charger les mdp sauvegardés dans la methode SavePassword
        private void LoadPasswords()
        {
            if (File.Exists("passwords.txt"))
            {
                using (StreamReader sr = new StreamReader("passwords.txt"))
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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePasswords();
        }
        

        public Form1()
        {
            InitializeComponent();
        }        



        private void btnAdd_Click(object sender, EventArgs e)
        {
            string app = Microsoft.VisualBasic.Interaction.InputBox("Nom de l'application/site :", "Ajouter");
            string username = Microsoft.VisualBasic.Interaction.InputBox("Nom d'utilisateur :", "Ajouter");
            string password = Microsoft.VisualBasic.Interaction.InputBox("Mot de passe :", "Ajouter");

            if (!string.IsNullOrWhiteSpace(app) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                string encryptedPassword = Encrypt(password);
                dataGridView.Rows.Add(app, username, encryptedPassword);
            }
            else
            {
                MessageBox.Show("Tous les champs sont obligatoires.", "Erreur");
            }
        }


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

                     
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPasswords();
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
    }
}
