using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace Projet___Gestionnaire_MDP
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = "Data Source=passwords.db;Version=3;";
        private string currentUser = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDatabase();
            ShowLoginForm();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePasswords(); 
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string GeneratePassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                res.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return res.ToString();
        }


        private void ShowLoginForm()
        {
            using (LoginForm loginForm = new LoginForm(connectionString))
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
                currentUser = loginForm.Username;
            }
            LoadPasswords();
        }

        private void InitializeDatabase()
        {
            try
            {
                // Vérifiez si le fichier de base de données existe
                string dbFilePath = "passwords.db";
                if (!File.Exists(dbFilePath))
                {
                    SQLiteConnection.CreateFile(dbFilePath);
                }

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Création des tables avec vérification
                    string[] createTables = {
                "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, username TEXT UNIQUE, password_hash TEXT)",
                "CREATE TABLE IF NOT EXISTS Passwords (id INTEGER PRIMARY KEY, user_id INTEGER, application TEXT, username TEXT, password_hash TEXT, FOREIGN KEY(user_id) REFERENCES Users(id))"
            };

                    foreach (string query in createTables)
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur d'initialisation de la base: {ex.Message}", "Erreur critique");
                Application.Exit(); // Ferme l'application si la base ne peut pas être initialisée
            }
        }
        

        // Méthode pour sauvegarder les mots de passe
        private void LoadPasswords()
        {
            try
            {
                dataGridView.Rows.Clear();
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT p.application, p.username 
                            FROM Passwords p 
                            JOIN Users u ON p.user_id = u.id 
                            WHERE u.username = @username";

                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@username", currentUser);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // N'affichez pas le mot de passe réel
                            dataGridView.Rows.Add(reader["application"].ToString(),
                                                 reader["username"].ToString(),
                                                 "********");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement : {ex.Message}", "Erreur");
            }
        }

        private void SavePasswords()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Récupérer l'ID de l'utilisateur
                    string getUserIdQuery = "SELECT id FROM Users WHERE username = @username";
                    SQLiteCommand getUserIdCmd = new SQLiteCommand(getUserIdQuery, connection);
                    getUserIdCmd.Parameters.AddWithValue("@username", currentUser);
                    int userId = Convert.ToInt32(getUserIdCmd.ExecuteScalar());

                    // Supprimer les anciennes entrées
                    string deleteQuery = "DELETE FROM Passwords WHERE user_id = @user_id";
                    SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@user_id", userId);
                    deleteCmd.ExecuteNonQuery();

                    // Ajouter les nouvelles entrées
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string app = row.Cells["colApplication"].Value?.ToString() ?? "";
                            string username = row.Cells["colUsername"].Value?.ToString() ?? "";
                            string password = row.Cells["colPassword"].Value?.ToString() ?? "";

                            string insertQuery = "INSERT INTO Passwords (user_id, application, username, password_hash) VALUES (@user_id, @app, @username, @password)";
                            SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection);
                            insertCmd.Parameters.AddWithValue("@user_id", userId);
                            insertCmd.Parameters.AddWithValue("@app", app);
                            insertCmd.Parameters.AddWithValue("@username", username);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}", "Erreur");
            }
        }

        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            string generatedPassword = GeneratePassword();

            // Afficher le mot de passe généré et proposer de le copier
            if (MessageBox.Show($"Mot de passe généré : {generatedPassword}\n\nCopier dans le presse-papier ?",
                               "Générateur de mot de passe",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Clipboard.SetText(generatedPassword);
            }
        }

        // Bouton pour ajouter un mot de passe
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string app = Microsoft.VisualBasic.Interaction.InputBox("Nom de l'application/site :", "Ajouter");
            string username = Microsoft.VisualBasic.Interaction.InputBox("Nom d'utilisateur :", "Ajouter");

            // Proposer de générer un mot de passe
            if (MessageBox.Show("Voulez-vous générer un mot de passe sécurisé ?",
                               "Génération",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string password = GeneratePassword();
                string hashedPassword = HashPassword(password);
                dataGridView.Rows.Add(app, username, "********"); // Masquer le vrai mot de passe
                                                                  // Stocker le hash dans un Tag ou autre propriété si nécessaire
            }
            else
            {
                string password = Microsoft.VisualBasic.Interaction.InputBox("Mot de passe :", "Ajouter");
                if (!string.IsNullOrWhiteSpace(app) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                {
                    string hashedPassword = HashPassword(password);
                    dataGridView.Rows.Add(app, username, "********");
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
            SavePasswords(); 
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
