using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace PersonelIzinTakip
{
    public partial class PersonelEkleForm : Form
    {
        private readonly Database db;
        private readonly Personel personel;

        public PersonelEkleForm(Personel personel = null)
        {
            InitializeComponent();
            db = new Database();
            this.personel = personel ?? new Personel();
            InitializeUI();
            if (personel != null)
            {
                LoadPersonelData();
            }
        }

        private void InitializeUI()
        {
            // Form arka planı
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = personel.Id == 0 ? "Yeni Personel Ekle" : "Personel Düzenle";
            this.Size = new Size(420, 540);
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
                Padding = new Padding(30, 10, 30, 10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            int y = 20;
            int labelWidth = 110;
            int inputWidth = 210;
            int inputHeight = 32;
            int gap = 40;
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10);

            // Label ve TextBox helper
            void AddLabelAndControl(string labelText, Control input, int tabIndex)
            {
                Label lbl = new Label
                {
                    Text = labelText,
                    Location = new Point(0, y + 6),
                    Size = new Size(labelWidth, 24),
                    Font = labelFont,
                    ForeColor = Color.FromArgb(52, 73, 94),
                    TextAlign = ContentAlignment.MiddleRight
                };
                input.Location = new Point(labelWidth + 10, y);
                input.Size = new Size(inputWidth, inputHeight);
                input.Font = inputFont;
                input.TabIndex = tabIndex;
                input.BackColor = Color.FromArgb(245, 247, 250);
                input.ForeColor = Color.FromArgb(44, 62, 80);
                mainPanel.Controls.Add(lbl);
                mainPanel.Controls.Add(input);
                y += gap;
            }

            // Ad
            TextBox txtAd = new TextBox { Name = "txtAd", BorderStyle = BorderStyle.FixedSingle };
            AddLabelAndControl("Ad:", txtAd, 0);
            // Soyad
            TextBox txtSoyad = new TextBox { Name = "txtSoyad", BorderStyle = BorderStyle.FixedSingle };
            AddLabelAndControl("Soyad:", txtSoyad, 1);
            // Sicil No
            TextBox txtSicilNo = new TextBox { Name = "txtSicilNo", BorderStyle = BorderStyle.FixedSingle };
            AddLabelAndControl("Sicil No:", txtSicilNo, 2);
            // Departman
            TextBox txtDepartman = new TextBox { Name = "txtDepartman", BorderStyle = BorderStyle.FixedSingle };
            AddLabelAndControl("Departman:", txtDepartman, 3);
            // Pozisyon
            TextBox txtPozisyon = new TextBox { Name = "txtPozisyon", BorderStyle = BorderStyle.FixedSingle };
            AddLabelAndControl("Pozisyon:", txtPozisyon, 4);
            // İşe Giriş Tarihi
            DateTimePicker dtpIseGirisTarihi = new DateTimePicker
            {
                Name = "dtpIseGirisTarihi",
                Format = DateTimePickerFormat.Short,
                CalendarMonthBackground = Color.FromArgb(245, 247, 250),
                CalendarForeColor = Color.FromArgb(44, 62, 80)
            };
            AddLabelAndControl("İşe Giriş Tarihi:", dtpIseGirisTarihi, 5);
            // Kalan İzin Günü
            NumericUpDown nudKalanIzinGunu = new NumericUpDown
            {
                Name = "nudKalanIzinGunu",
                Minimum = 0,
                Maximum = 365,
                BorderStyle = BorderStyle.FixedSingle
            };
            AddLabelAndControl("Kalan İzin Günü:", nudKalanIzinGunu, 6);

            // Butonlar
            Button btnKaydet = new Button
            {
                Text = "Kaydet",
                Location = new Point(labelWidth + 10, y + 10),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TabIndex = 7
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
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TabIndex = 8
            };
            btnIptal.FlatAppearance.BorderSize = 0;
            btnIptal.MouseEnter += (s, e) => btnIptal.BackColor = Color.FromArgb(192, 57, 43);
            btnIptal.MouseLeave += (s, e) => btnIptal.BackColor = Color.FromArgb(231, 76, 60);

            btnKaydet.Click += (s, e) => KaydetPersonel(
                txtAd, txtSoyad, txtSicilNo, txtDepartman, txtPozisyon,
                dtpIseGirisTarihi, nudKalanIzinGunu);
            btnIptal.Click += (s, e) => this.Close();

            mainPanel.Controls.Add(btnKaydet);
            mainPanel.Controls.Add(btnIptal);
        }

        private void LoadPersonelData()
        {
            // Paneli bul
            var mainPanel = this.Controls.OfType<Panel>().FirstOrDefault();
            if (mainPanel == null) return;

            var txtAd = mainPanel.Controls["txtAd"] as TextBox;
            var txtSoyad = mainPanel.Controls["txtSoyad"] as TextBox;
            var txtSicilNo = mainPanel.Controls["txtSicilNo"] as TextBox;
            var txtDepartman = mainPanel.Controls["txtDepartman"] as TextBox;
            var txtPozisyon = mainPanel.Controls["txtPozisyon"] as TextBox;
            var dtpIseGirisTarihi = mainPanel.Controls["dtpIseGirisTarihi"] as DateTimePicker;
            var nudKalanIzinGunu = mainPanel.Controls["nudKalanIzinGunu"] as NumericUpDown;

            if (txtAd != null) txtAd.Text = personel.Ad;
            if (txtSoyad != null) txtSoyad.Text = personel.Soyad;
            if (txtSicilNo != null) txtSicilNo.Text = personel.SicilNo;
            if (txtDepartman != null) txtDepartman.Text = personel.Departman;
            if (txtPozisyon != null) txtPozisyon.Text = personel.Pozisyon;
            if (dtpIseGirisTarihi != null) dtpIseGirisTarihi.Value = personel.IseGirisTarihi;
            if (nudKalanIzinGunu != null) nudKalanIzinGunu.Value = personel.KalanIzinGunu;
        }

        private void KaydetPersonel(TextBox txtAd, TextBox txtSoyad, TextBox txtSicilNo,
            TextBox txtDepartman, TextBox txtPozisyon, DateTimePicker dtpIseGirisTarihi,
            NumericUpDown nudKalanIzinGunu)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text) ||
                string.IsNullOrWhiteSpace(txtSicilNo.Text) || string.IsNullOrWhiteSpace(txtDepartman.Text) ||
                string.IsNullOrWhiteSpace(txtPozisyon.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            personel.Ad = txtAd.Text;
            personel.Soyad = txtSoyad.Text;
            personel.SicilNo = txtSicilNo.Text;
            personel.Departman = txtDepartman.Text;
            personel.Pozisyon = txtPozisyon.Text;
            personel.IseGirisTarihi = dtpIseGirisTarihi.Value;
            personel.KalanIzinGunu = (int)nudKalanIzinGunu.Value;

            try
            {
                if (personel.Id == 0)
                {
                    db.PersonelEkle(personel);
                }
                else
                {
                    db.PersonelGuncelle(personel);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Personel kaydedilirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 