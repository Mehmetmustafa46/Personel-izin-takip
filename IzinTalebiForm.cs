using System;
using System.Windows.Forms;
using System.Drawing;

namespace PersonelIzinTakip
{
    public partial class IzinTalebiForm : Form
    {
        private readonly Personel personel;
        private readonly Database db;

        public IzinTalebiForm(Personel personel)
        {
            InitializeComponent();
            this.personel = personel;
            db = new Database();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Form ayarları
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = "İzin Talebi Oluştur";
            this.Size = new Size(520, 480);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Başlık
            Label lblTitle = new Label
            {
                Text = "İzin Talebi Oluştur",
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
                Padding = new Padding(30, 10, 30, 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            int y = 20;
            int labelWidth = 110;
            int inputWidth = 300;
            int inputHeight = 32;
            int gap = 40;
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10);

            // Personel bilgileri
            Label lblPersonel = new Label
            {
                Text = $"Personel: {personel.AdSoyad}",
                Location = new Point(0, y),
                Size = new Size(labelWidth + inputWidth + 10, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            mainPanel.Controls.Add(lblPersonel);
            y += gap;

            // İzin türü
            Label lblIzinTuru = new Label
            {
                Text = "İzin Türü:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            ComboBox cmbIzinTuru = new ComboBox
            {
                Name = "cmbIzinTuru",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbIzinTuru.Items.AddRange(new string[] { "Yıllık İzin", "Hastalık İzni", "Mazeret İzni" });
            mainPanel.Controls.Add(lblIzinTuru);
            mainPanel.Controls.Add(cmbIzinTuru);
            y += gap;

            // Başlangıç tarihi
            Label lblBaslangic = new Label
            {
                Text = "Başlangıç:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            DateTimePicker dtpBaslangic = new DateTimePicker
            {
                Name = "dtpBaslangic",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                Format = DateTimePickerFormat.Short,
                CalendarMonthBackground = Color.FromArgb(245, 247, 250),
                CalendarForeColor = Color.FromArgb(44, 62, 80)
            };
            mainPanel.Controls.Add(lblBaslangic);
            mainPanel.Controls.Add(dtpBaslangic);
            y += gap;

            // Bitiş tarihi
            Label lblBitis = new Label
            {
                Text = "Bitiş:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            DateTimePicker dtpBitis = new DateTimePicker
            {
                Name = "dtpBitis",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                Format = DateTimePickerFormat.Short,
                CalendarMonthBackground = Color.FromArgb(245, 247, 250),
                CalendarForeColor = Color.FromArgb(44, 62, 80)
            };
            mainPanel.Controls.Add(lblBitis);
            mainPanel.Controls.Add(dtpBitis);
            y += gap;

            // Açıklama
            Label lblAciklama = new Label
            {
                Text = "Açıklama:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            TextBox txtAciklama = new TextBox
            {
                Name = "txtAciklama",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, 60),
                Font = inputFont,
                Multiline = true,
                BackColor = Color.FromArgb(245, 247, 250),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            mainPanel.Controls.Add(lblAciklama);
            mainPanel.Controls.Add(txtAciklama);
            y += gap + 20;

            // Butonlar
            Button btnKaydet = new Button
            {
                Text = "Kaydet",
                Location = new Point(labelWidth + 10, y + 10),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.MouseEnter += (s, e) => btnKaydet.BackColor = Color.FromArgb(39, 174, 96);
            btnKaydet.MouseLeave += (s, e) => btnKaydet.BackColor = Color.FromArgb(46, 204, 113);

            Button btnIptal = new Button
            {
                Text = "İptal",
                Location = new Point(labelWidth + 120, y + 10),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.MouseEnter += (s, e) => btnIptal.BackColor = Color.FromArgb(192, 57, 43);
            btnIptal.MouseLeave += (s, e) => btnIptal.BackColor = Color.FromArgb(231, 76, 60);

            btnKaydet.Click += (s, e) => KaydetIzinTalebi(cmbIzinTuru, dtpBaslangic, dtpBitis, txtAciklama);
            btnIptal.Click += (s, e) => this.Close();

            mainPanel.Controls.Add(btnKaydet);
            mainPanel.Controls.Add(btnIptal);
        }

        private void KaydetIzinTalebi(ComboBox cmbIzinTuru, DateTimePicker dtpBaslangic, DateTimePicker dtpBitis, TextBox txtAciklama)
        {
            if (string.IsNullOrEmpty(cmbIzinTuru.Text))
            {
                MessageBox.Show("Lütfen izin türü seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpBaslangic.Value > dtpBitis.Value)
            {
                MessageBox.Show("Başlangıç tarihi bitiş tarihinden büyük olamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var izinGunuSayisi = (dtpBitis.Value - dtpBaslangic.Value).Days + 1;

            if (izinGunuSayisi > personel.KalanIzinGunu)
            {
                MessageBox.Show($"Kalan izin gününüz ({personel.KalanIzinGunu}) yetersiz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var izin = new Izin
                {
                    PersonelId = personel.Id,
                    BaslangicTarihi = dtpBaslangic.Value,
                    BitisTarihi = dtpBitis.Value,
                    IzinTuru = cmbIzinTuru.Text,
                    Aciklama = txtAciklama.Text,
                    Durum = "Beklemede",
                    TalepTarihi = DateTime.Now
                };

                db.IzinEkle(izin);
                MessageBox.Show("İzin talebi başarıyla oluşturuldu.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İzin talebi kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 