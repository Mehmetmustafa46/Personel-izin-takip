using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace PersonelIzinTakip
{
    public partial class PersonelListesiForm : Form
    {
        private List<Personel> personeller;
        private readonly Database db;

        public PersonelListesiForm()
        {
            InitializeComponent();
            db = new Database();
            InitializeUI();
            LoadPersoneller();
        }

        private void InitializeUI()
        {
            // Form arka planı
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = "Personel Listesi";
            this.Size = new Size(950, 600);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Başlık
            Label lblTitle = new Label
            {
                Text = this.Text,
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

            // DataGridView oluşturma
            DataGridView dgvPersoneller = new DataGridView
            {
                Name = "dgvPersoneller",
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
            dgvPersoneller.Columns.Add("Id", "ID");
            dgvPersoneller.Columns.Add("Ad", "Ad");
            dgvPersoneller.Columns.Add("Soyad", "Soyad");
            dgvPersoneller.Columns.Add("SicilNo", "Sicil No");
            dgvPersoneller.Columns.Add("Departman", "Departman");
            dgvPersoneller.Columns.Add("Pozisyon", "Pozisyon");
            dgvPersoneller.Columns.Add("IseGirisTarihi", "İşe Giriş Tarihi");
            dgvPersoneller.Columns.Add("KalanIzinGunu", "Kalan İzin Günü");

            // ToolStrip oluşturma
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
            ToolStripButton btnDuzenle = new ToolStripButton("Düzenle")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };
            ToolStripButton btnSil = new ToolStripButton("Sil")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnYenile.MouseEnter += (s, e) => btnYenile.BackColor = Color.FromArgb(41, 128, 185);
            btnYenile.MouseLeave += (s, e) => btnYenile.BackColor = Color.FromArgb(52, 152, 219);
            btnDuzenle.MouseEnter += (s, e) => btnDuzenle.BackColor = Color.FromArgb(39, 174, 96);
            btnDuzenle.MouseLeave += (s, e) => btnDuzenle.BackColor = Color.FromArgb(46, 204, 113);
            btnSil.MouseEnter += (s, e) => btnSil.BackColor = Color.FromArgb(192, 57, 43);
            btnSil.MouseLeave += (s, e) => btnSil.BackColor = Color.FromArgb(231, 76, 60);

            btnYenile.Click += (s, e) => LoadPersoneller();
            btnDuzenle.Click += (s, e) => DuzenlePersonel(dgvPersoneller);
            btnSil.Click += (s, e) => SilPersonel(dgvPersoneller);

            toolStrip.Items.AddRange(new ToolStripItem[] { btnYenile, btnDuzenle, btnSil });

            // Kontrolleri panele ekleme
            mainPanel.Controls.Add(toolStrip);
            mainPanel.Controls.Add(dgvPersoneller);
        }

        private void LoadPersoneller()
        {
            try
            {
                personeller = db.GetPersoneller();

                // Paneli bul
                var mainPanel = this.Controls.OfType<Panel>().FirstOrDefault();
                if (mainPanel == null) return;
                // DataGridView'i bul
                var dgv = mainPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                if (dgv == null) return;
                dgv.Rows.Clear();
                foreach (var personel in personeller)
                {
                    dgv.Rows.Add(
                        personel.Id,
                        personel.Ad,
                        personel.Soyad,
                        personel.SicilNo,
                        personel.Departman,
                        personel.Pozisyon,
                        personel.IseGirisTarihi.ToShortDateString(),
                        personel.KalanIzinGunu
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Personel listesi yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DuzenlePersonel(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0) return;

            int personelId = Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);
            var personel = personeller.Find(p => p.Id == personelId);

            using (var form = new PersonelEkleForm(personel))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        db.PersonelGuncelle(personel);
                        LoadPersoneller();
                        MessageBox.Show("Personel bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Personel güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SilPersonel(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0) return;

            int personelId = Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);
            var result = MessageBox.Show(
                "Seçili personeli silmek istediğinizden emin misiniz?",
                "Personel Silme",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    db.PersonelSil(personelId);
                    LoadPersoneller();
                    MessageBox.Show("Personel silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Personel silinirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
} 