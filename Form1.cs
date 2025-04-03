using System;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Projet___Gestionnaire_MDP
{
    public partial class Form1 : Form
    {
        // Chaîne de connexion à la base de données SQLite
        private readonly string connectionString = "Data Source=passwords.db;Version=3;";
        // Stocke l'utilisateur actuellement connecté
        private string currentUser = "";
        // Stocke le mot de passe maître de l'utilisateur actuel
        private string currentMasterPassword = "";

        // Constructeur du formulaire principal
        public Form1()
        {
            InitializeComponent();
            // Ajoute un gestionnaire d'événement pour quand le formulaire est fermé
            this.FormClosed += Form1_FormClosed;
        }

        // Événement déclenché au chargement du formulaire
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDatabase(); // Initialise la base de données
            ShowLoginForm();       // Affiche le formulaire de connexion
        }

        // Événement déclenché à la fermeture du formulaire
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Sauvegarde les mots de passe de l'utilisateur avant de fermer
            SaveUserPasswords(currentUser, currentMasterPassword);
        }

        #region Méthodes de sécurité
        // Hash un mot de passe avec BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Vérifie si un mot de passe correspond à un hash
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        // Génère un mot de passe aléatoire sécurisé
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

        // Chiffre un mot de passe avec AES en utilisant le mot de passe maître comme clé
        private string EncryptPassword(string password, string masterPassword)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(masterPassword));
                aes.IV = new byte[16];

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(password);
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        // Déchiffre un mot de passe avec AES
        private string DecryptPassword(string encryptedPassword, string masterPassword)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(masterPassword));
                aes.IV = new byte[16];

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion

        #region Gestion de la base de données
        // Initialise la base de données et crée les tables si elles n'existent pas
        private void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Création de la table Users
                string createUsersTable = @"
                CREATE TABLE IF NOT EXISTS Users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT UNIQUE NOT NULL,
                    password_hash TEXT NOT NULL)";

                // Création de la table Passwords
                string createPasswordsTable = @"
                CREATE TABLE IF NOT EXISTS Passwords (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL,
                    application TEXT NOT NULL,
                    username TEXT,
                    encrypted_password TEXT NOT NULL,
                    FOREIGN KEY(user_id) REFERENCES Users(id))";

                using (SQLiteCommand command = new SQLiteCommand(createUsersTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(createPasswordsTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Affiche le formulaire de connexion et récupère les infos de l'utilisateur
        private void ShowLoginForm()
        {
            using (LoginForm loginForm = new LoginForm(connectionString))
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit(); // Quitte si l'utilisateur annule
                    return;
                }
                currentUser = loginForm.Username;
                currentMasterPassword = loginForm.Password;
            }
            // Charge les mots de passe de l'utilisateur après connexion
            LoadUserPasswords(currentUser, currentMasterPassword);
        }

        // Charge les mots de passe de l'utilisateur depuis la base de données
        private void LoadUserPasswords(string username, string masterPassword)
        {
            try
            {
                dataGridView.Rows.Clear(); // Vide le DataGridView

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Récupère l'ID de l'utilisateur
                    string userIdQuery = "SELECT id FROM Users WHERE username = @username";
                    int userId;

                    using (SQLiteCommand command = new SQLiteCommand(userIdQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        userId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Récupère les mots de passe associés à cet utilisateur
                    string passwordsQuery = @"
                    SELECT application, username, encrypted_password 
                    FROM Passwords 
                    WHERE user_id = @user_id";

                    using (SQLiteCommand command = new SQLiteCommand(passwordsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Déchiffre chaque mot de passe et l'ajoute au DataGridView
                                string encryptedPassword = reader["encrypted_password"].ToString();
                                string decryptedPassword = DecryptPassword(encryptedPassword, masterPassword);

                                dataGridView.Rows.Add(
                                    reader["application"].ToString(),
                                    reader["username"].ToString(),
                                    decryptedPassword);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement: {ex.Message}");
            }
        }

        // Sauvegarde les mots de passe de l'utilisateur dans la base de données
        private void SaveUserPasswords(string username, string masterPassword)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Récupère l'ID de l'utilisateur
                    string userIdQuery = "SELECT id FROM Users WHERE username = @username";
                    int userId;

                    using (SQLiteCommand command = new SQLiteCommand(userIdQuery, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        userId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // Supprime les anciens mots de passe
                    string deleteQuery = "DELETE FROM Passwords WHERE user_id = @user_id";
                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.ExecuteNonQuery();
                    }

                    // Sauvegarde chaque mot de passe du DataGridView
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["colApplication"].Value != null)
                        {
                            string app = row.Cells["colApplication"].Value.ToString();
                            string user = row.Cells["colUsername"].Value?.ToString() ?? "";
                            string password = row.Cells["colPassword"].Value?.ToString() ?? "";

                            // Chiffre le mot de passe avant sauvegarde
                            string encrypted = EncryptPassword(password, masterPassword);

                            string insertQuery = @"
                            INSERT INTO Passwords 
                            (user_id, application, username, encrypted_password) 
                            VALUES (@user_id, @app, @user, @encrypted)";

                            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@user_id", userId);
                                command.Parameters.AddWithValue("@app", app);
                                command.Parameters.AddWithValue("@user", user);
                                command.Parameters.AddWithValue("@encrypted", encrypted);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde: {ex.Message}");
            }
        }
        #endregion

        #region Gestion de l'interface
        // Générateur de mot de passe
        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            string generatedPassword = GeneratePassword();

            if (MessageBox.Show($"Mot de passe généré : {generatedPassword}\n\nCopier dans le presse-papier ?",
                               "Générateur de mot de passe",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Clipboard.SetText(generatedPassword);
            }
        }

        // Ajoute une nouvelle entrée de mot de passe
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string app = Microsoft.VisualBasic.Interaction.InputBox("Nom de l'application/site :", "Ajouter");
            string username = Microsoft.VisualBasic.Interaction.InputBox("Nom d'utilisateur :", "Ajouter");

            if (string.IsNullOrWhiteSpace(app) || string.IsNullOrWhiteSpace(username))
                return;

            string password;
            if (MessageBox.Show("Voulez-vous générer un mot de passe sécurisé ?",
                               "Génération",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                password = GeneratePassword();
                MessageBox.Show($"Votre nouveau mot de passe : {password}", "Mot de passe généré");
            }
            else
            {
                password = Microsoft.VisualBasic.Interaction.InputBox("Mot de passe :", "Ajouter");
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                dataGridView.Rows.Add(app, username, password);
            }
        }

        // Supprime une entrée sélectionnée
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

        // Exporte les mots de passe vers un fichier texte
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

        // Importe des mots de passe depuis un fichier texte
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
        #endregion

        #region Menu actions
        // Affiche la fenêtre "À propos"
        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }

        // Importe depuis un fichier texte (menu)
        private void importerDepuisTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFromTextFile();
        }

        // Exporte vers un fichier texte (menu)
        private void exporterVersTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToTextFile();
        }

        // Quitte l'application
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Méthodes inutilisées (nécessaires pour le designer)
        // Méthodes vides nécessaires pour le designer mais non utilisées
        private void fichierToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void importerLesMotsDePassesToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void exporterLesMotsDePassesToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        #endregion
    }
}