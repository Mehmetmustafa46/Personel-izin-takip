using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace PersonelIzinTakip
{
    public partial class PersonelForm : Form
    {
        private readonly Personel personel;
        private readonly Database db;
        private List<Izin> izinler;

        public PersonelForm(Personel personel)
        {
            InitializeComponent();
            this.personel = personel;
            db = new Database();
            InitializeUI();
            LoadIzinler();
        }

        private void InitializeUI()
        {
            // Form ayarları
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = $"Personel İzin Takip - {personel.AdSoyad}";
            this.Size = new Size(1000, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Başlık
            Label lblTitle = new Label
            {
                Text = $"Hoş Geldiniz, {personel.AdSoyad}",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };
            this.Controls.Add(lblTitle);

            // Ana panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            // Bilgi paneli
            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(230, 240, 250),
                Padding = new Padding(0, 0, 0, 0),
                Margin = new Padding(0, 0, 0, 0)
            };
            mainPanel.Controls.Add(infoPanel);
            infoPanel.BringToFront();

            // Bilgileri düzenli göstermek için TableLayoutPanel kullan
            TableLayoutPanel infoTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                AutoSize = false,
                Padding = new Padding(20, 10, 20, 10),
            };
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34f));

            // Departman
            Label lblDepartman = new Label
            {
                Text = $"Departman: {personel.Departman}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            // Pozisyon
            Label lblPozisyon = new Label
            {
                Text = $"Pozisyon: {personel.Pozisyon}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            // Kalan İzin Günü
            Label lblIzin = new Label
            {
                Text = $"Kalan İzin Günü: {personel.KalanIzinGunu}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(231, 76, 60),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            infoTable.Controls.Add(lblDepartman, 0, 0);
            infoTable.Controls.Add(lblPozisyon, 1, 0);
            infoTable.Controls.Add(lblIzin, 2, 0);
            infoPanel.Controls.Add(infoTable);

            // DataGridView
            DataGridView dgvIzinler = new DataGridView
            {
                Name = "dgvIzinler",
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(44, 62, 80),
                    Font = new Font("Segoe UI", 10),
                    SelectionBackColor = Color.FromArgb(46, 204, 113),
                    SelectionForeColor = Color.White
                },
                GridColor = Color.FromArgb(236, 240, 241)
            };

            // Sütunları ekleme
            dgvIzinler.Columns.Add("Id", "ID");
            dgvIzinler.Columns.Add("IzinTuru", "İzin Türü");
            dgvIzinler.Columns.Add("BaslangicTarihi", "Başlangıç");
            dgvIzinler.Columns.Add("BitisTarihi", "Bitiş");
            dgvIzinler.Columns.Add("IzinGunuSayisi", "Gün Sayısı");
            dgvIzinler.Columns.Add("Aciklama", "Açıklama");
            dgvIzinler.Columns.Add("Durum", "Durum");
            dgvIzinler.Columns.Add("TalepTarihi", "Talep Tarihi");
            dgvIzinler.Columns.Add("OnaylayanKisi", "Onaylayan");
            dgvIzinler.Columns.Add("OnayTarihi", "Onay Tarihi");

            // ToolStrip
            ToolStrip toolStrip = new ToolStrip
            {
                BackColor = Color.FromArgb(52, 73, 94),
                GripStyle = ToolStripGripStyle.Hidden,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                RenderMode = ToolStripRenderMode.Professional
            };

            ToolStripButton btnYenile = new ToolStripButton("Yenile")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            ToolStripButton btnYeniIzin = new ToolStripButton("Yeni İzin Talebi")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            ToolStripButton btnCikis = new ToolStripButton("Çıkış")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            ToolStripButton btnUstMenu = new ToolStripButton("Üst Menüye Dön")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(155, 89, 182),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnYenile.MouseEnter += (s, e) => btnYenile.BackColor = Color.FromArgb(41, 128, 185);
            btnYenile.MouseLeave += (s, e) => btnYenile.BackColor = Color.FromArgb(52, 152, 219);
            btnYeniIzin.MouseEnter += (s, e) => btnYeniIzin.BackColor = Color.FromArgb(39, 174, 96);
            btnYeniIzin.MouseLeave += (s, e) => btnYeniIzin.BackColor = Color.FromArgb(46, 204, 113);
            btnCikis.MouseEnter += (s, e) => btnCikis.BackColor = Color.FromArgb(192, 57, 43);
            btnCikis.MouseLeave += (s, e) => btnCikis.BackColor = Color.FromArgb(231, 76, 60);
            btnUstMenu.MouseEnter += (s, e) => btnUstMenu.BackColor = Color.FromArgb(142, 68, 173);
            btnUstMenu.MouseLeave += (s, e) => btnUstMenu.BackColor = Color.FromArgb(155, 89, 182);

            btnYenile.Click += (s, e) => LoadIzinler();
            btnYeniIzin.Click += (s, e) => YeniIzinTalebi();
            btnCikis.Click += (s, e) => CikisYap();
            btnUstMenu.Click += (s, e) => UstMenuDon();

            toolStrip.Items.AddRange(new ToolStripItem[] { btnYenile, btnYeniIzin, btnUstMenu, btnCikis });

            mainPanel.Controls.Add(toolStrip);
            mainPanel.Controls.Add(dgvIzinler);
        }

        private void LoadIzinler()
        {
            try
            {
                izinler = db.GetIzinler().Where(i => i.PersonelId == personel.Id).ToList();

                var dgv = this.Controls.OfType<Panel>().First()
                    .Controls.OfType<DataGridView>().First();
                dgv.Rows.Clear();

                foreach (var izin in izinler)
                {
                    dgv.Rows.Add(
                        izin.Id,
                        izin.IzinTuru,
                        izin.BaslangicTarihi.ToShortDateString(),
                        izin.BitisTarihi.ToShortDateString(),
                        (izin.BitisTarihi - izin.BaslangicTarihi).Days + 1,
                        izin.Aciklama ?? "-",
                        izin.Durum,
                        izin.TalepTarihi.ToShortDateString(),
                        izin.OnaylayanKisi ?? "-",
                        izin.OnayTarihi?.ToShortDateString() ?? "-"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İzin listesi yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void YeniIzinTalebi()
        {
            using (var form = new IzinTalebiForm(personel))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadIzinler();
                }
            }
        }

        private void CikisYap()
        {
            var result = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                using (var form = new GirisForm())
                {
                    form.ShowDialog();
                }
                this.Close();
            }
        }

        private void UstMenuDon()
        {
            var result = MessageBox.Show("Üst menüye dönmek istediğinizden emin misiniz?", "Üst Menüye Dön", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                using (var form = new Form1())
                {
                    form.ShowDialog();
                }
                this.Close();
            }
        }
    }
} 