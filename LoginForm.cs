using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using BCrypt.Net;

namespace Projet___Gestionnaire_MDP
{
    // Formulaire de connexion/inscription pour le gestionnaire de mots de passe
    public class LoginForm : Form
    {
        // Chaîne de connexion à la base de données SQLite
        private readonly string connectionString;

        // Contrôles du formulaire
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblUsername;
        private Label lblPassword;

        // Propriétés pour stocker les identifiants (utilisées après authentification réussie)
        public string Username { get; private set; }
        public string Password { get; private set; } // Ajout de la propriété Password manquante

        // Constructeur prenant la chaîne de connexion en paramètre
        public LoginForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeForm(); // Initialise l'interface graphique
        }

        // Méthode d'initialisation de l'interface graphique
        private void InitializeForm()
        {
            // Configuration de la fenêtre
            this.Text = "Connexion - Gestionnaire MDP";
            this.ClientSize = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Label Nom d'utilisateur
            lblUsername = new Label
            {
                Text = "Nom d'utilisateur:",
                Location = new Point(30, 30),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            // Champ Nom d'utilisateur
            txtUsername = new TextBox
            {
                Location = new Point(140, 30),
                Size = new Size(180, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            // Label Mot de passe
            lblPassword = new Label
            {
                Text = "Mot de passe:",
                Location = new Point(30, 70),
                Size = new Size(100, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            // Champ Mot de passe
            txtPassword = new TextBox
            {
                Location = new Point(140, 70),
                Size = new Size(180, 20),
                PasswordChar = '*', // Masque les caractères saisis
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            // Bouton Connexion
            btnLogin = new Button
            {
                Text = "Connexion",
                Location = new Point(140, 120),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += btnLogin_Click; // Gestionnaire d'événement pour le clic

            // Bouton S'inscrire
            btnRegister = new Button
            {
                Text = "S'inscrire",
                Location = new Point(240, 120),
                Size = new Size(90, 30),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += btnRegister_Click; // Gestionnaire d'événement pour le clic

            // Ajout des contrôles au formulaire
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
        }

        // Gestionnaire d'événement pour le bouton de connexion
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Validation des entrées
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez saisir un nom d'utilisateur et un mot de passe", "Erreur",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authentification de l'utilisateur
            if (AuthenticateUser(username, password))
            {
                // Stockage des identifiants pour une utilisation ultérieure
                Username = username;
                Password = password; // Stockage du mot de passe pour le déchiffrement
                DialogResult = DialogResult.OK;
                Close(); // Ferme le formulaire avec un résultat OK
            }
            else
            {
                MessageBox.Show("Identifiants incorrects", "Erreur d'authentification",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear(); // Efface le mot de passe saisi
                txtPassword.Focus(); // Remet le focus sur le champ mot de passe
            }
        }

        // Méthode pour authentifier un utilisateur avec la base de données
        private bool AuthenticateUser(string username, string password)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT password_hash FROM Users WHERE username = @username";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // Vérification du mot de passe hashé avec BCrypt
                        string storedHash = result.ToString();
                        return BCrypt.Net.BCrypt.Verify(password, storedHash);
                    }
                }
            }
            return false;
        }

        // Gestionnaire d'événement pour le bouton d'inscription
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Validation des entrées
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez saisir un nom d'utilisateur et un mot de passe", "Erreur",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Vérification de la longueur du mot de passe
            if (password.Length < 8)
            {
                MessageBox.Show("Le mot de passe doit contenir au moins 8 caractères", "Erreur",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Vérification de l'existence de l'utilisateur
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Ce nom d'utilisateur est déjà pris", "Erreur",
                                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Création du nouvel utilisateur avec mot de passe hashé
                    string insertQuery = "INSERT INTO Users (username, password_hash) VALUES (@username, @password_hash)";
                    using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        // Hashage du mot de passe avec BCrypt avant stockage
                        insertCmd.Parameters.AddWithValue("@password_hash", BCrypt.Net.BCrypt.HashPassword(password));
                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Compte créé avec succès ! Vous pouvez maintenant vous connecter.", "Succès",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte : {ex.Message}", "Erreur",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}