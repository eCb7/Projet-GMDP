using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projet___Gestionnaire_MDP
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.Text = "À propos";
            this.Width = 400;
            this.Height = 300;

            // Créer un label pour le texte
            Label lblAbout = new Label()
            {
                Left = 10,
                Top = 10,
                Width = 370,
                Height = 200,
                TextAlign = System.Drawing.ContentAlignment.TopLeft,
                Text =
                "Nom : GMDP\n" +
                "Lien GitHub : https://github.com/eCb7/GMDP\n" +
                "Date de création : 2024\n\n" +
                "Technologies utilisées :\n" +
                "- .NET Framework 4.7.2\n" +
                "- C#\n" +
                "- Windows Forms\n\n" +
                "Contexte :\n" +
                "Ce projet a été réalisé dans le cadre d'un BTS SIO, option SLAM.\n\n" +
                "Crédits :\n" +
                "Développé par eCb7.\n" +
                "Merci aux enseignants du BTS SIO pour leur encadrement.\n\n" +
                "Pour plus d'informations, visitez le dépôt GitHub."
            };

            // Ajouter un bouton pour fermer
            Button btnClose = new Button()
            {
                Text = "Fermer",
                Left = 150,
                Width = 100,
                Top = 220
            };
            btnClose.Click += (sender, e) => this.Close();

            // Ajouter les contrôles au formulaire
            this.Controls.Add(lblAbout);
            this.Controls.Add(btnClose);
        }

        private void InitializeComponent()
        {
        
        }
    }
}