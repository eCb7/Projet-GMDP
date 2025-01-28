namespace Projet___Gestionnaire_MDP
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importerLesMotsDePassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exporterLesMotsDePassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aProposToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colApplication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importerDepuisTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exporterVersTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colApplication,
            this.colUsername,
            this.colPassword});
            this.dataGridView.Location = new System.Drawing.Point(12, 98);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(984, 427);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(156, 32);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(141, 60);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Supprimer";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 32);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(138, 60);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Ajouter";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.aProposToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip2.TabIndex = 4;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importerLesMotsDePassesToolStripMenuItem,
            this.exporterLesMotsDePassesToolStripMenuItem,
            this.quitterToolStripMenuItem});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "Fichier";
            this.fichierToolStripMenuItem.Click += new System.EventHandler(this.fichierToolStripMenuItem_Click);
            // 
            // importerLesMotsDePassesToolStripMenuItem
            // 
            this.importerLesMotsDePassesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importerDepuisTXTToolStripMenuItem});
            this.importerLesMotsDePassesToolStripMenuItem.Name = "importerLesMotsDePassesToolStripMenuItem";
            this.importerLesMotsDePassesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.importerLesMotsDePassesToolStripMenuItem.Text = "Importer les mots de passes";
            this.importerLesMotsDePassesToolStripMenuItem.Click += new System.EventHandler(this.importerLesMotsDePassesToolStripMenuItem_Click);
            // 
            // exporterLesMotsDePassesToolStripMenuItem
            // 
            this.exporterLesMotsDePassesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exporterVersTXTToolStripMenuItem});
            this.exporterLesMotsDePassesToolStripMenuItem.Name = "exporterLesMotsDePassesToolStripMenuItem";
            this.exporterLesMotsDePassesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.exporterLesMotsDePassesToolStripMenuItem.Text = "Exporter les mots de passes";
            this.exporterLesMotsDePassesToolStripMenuItem.Click += new System.EventHandler(this.exporterLesMotsDePassesToolStripMenuItem_Click);
            // 
            // quitterToolStripMenuItem
            // 
            this.quitterToolStripMenuItem.Name = "quitterToolStripMenuItem";
            this.quitterToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.quitterToolStripMenuItem.Text = "Quitter";
            this.quitterToolStripMenuItem.Click += new System.EventHandler(this.quitterToolStripMenuItem_Click);
            // 
            // aProposToolStripMenuItem
            // 
            this.aProposToolStripMenuItem.Name = "aProposToolStripMenuItem";
            this.aProposToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.aProposToolStripMenuItem.Text = "A propos";
            this.aProposToolStripMenuItem.Click += new System.EventHandler(this.aProposToolStripMenuItem_Click);
            // 
            // colApplication
            // 
            dataGridViewCellStyle25.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle25.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(95)))), ((int)(((byte)(41)))));
            this.colApplication.DefaultCellStyle = dataGridViewCellStyle25;
            this.colApplication.HeaderText = "Application / Site";
            this.colApplication.Name = "colApplication";
            this.colApplication.Width = 313;
            // 
            // colUsername
            // 
            dataGridViewCellStyle26.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.colUsername.DefaultCellStyle = dataGridViewCellStyle26;
            this.colUsername.HeaderText = "Username";
            this.colUsername.Name = "colUsername";
            this.colUsername.Width = 313;
            // 
            // colPassword
            // 
            dataGridViewCellStyle27.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(45)))), ((int)(((byte)(123)))));
            this.colPassword.DefaultCellStyle = dataGridViewCellStyle27;
            this.colPassword.HeaderText = "Mot de passe";
            this.colPassword.Name = "colPassword";
            this.colPassword.Width = 315;
            // 
            // importerDepuisTXTToolStripMenuItem
            // 
            this.importerDepuisTXTToolStripMenuItem.Name = "importerDepuisTXTToolStripMenuItem";
            this.importerDepuisTXTToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importerDepuisTXTToolStripMenuItem.Text = "Importer depuis TXT";
            this.importerDepuisTXTToolStripMenuItem.Click += new System.EventHandler(this.importerDepuisTXTToolStripMenuItem_Click);
            // 
            // exporterVersTXTToolStripMenuItem
            // 
            this.exporterVersTXTToolStripMenuItem.Name = "exporterVersTXTToolStripMenuItem";
            this.exporterVersTXTToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exporterVersTXTToolStripMenuItem.Text = "Exporter vers TXT";
            this.exporterVersTXTToolStripMenuItem.Click += new System.EventHandler(this.exporterVersTXTToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(131)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(1008, 537);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importerLesMotsDePassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exporterLesMotsDePassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aProposToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApplication;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassword;
        private System.Windows.Forms.ToolStripMenuItem importerDepuisTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exporterVersTXTToolStripMenuItem;
    }
}

