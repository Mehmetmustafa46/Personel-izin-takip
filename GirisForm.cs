using System;
using System.Windows.Forms;
using System.Drawing;

namespace PersonelIzinTakip
{
    public partial class GirisForm : Form
    {
        private readonly Database db;
        private Personel aktifPersonel;

        public GirisForm()
        {
            InitializeComponent();
            db = new Database();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Form ayarları
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = "Personel İzin Takip - Giriş";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Başlık
            Label lblTitle = new Label
            {
                Text = "Personel İzin Takip",
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
                Padding = new Padding(30),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront();

            int y = 20;
            int labelWidth = 100;
            int inputWidth = 200;
            int inputHeight = 32;
            int gap = 40;
            Font labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font inputFont = new Font("Segoe UI", 10);

            // Kullanıcı Tipi Seçimi
            Label lblKullaniciTipi = new Label
            {
                Text = "Giriş Tipi:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            ComboBox cmbKullaniciTipi = new ComboBox
            {
                Name = "cmbKullaniciTipi",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbKullaniciTipi.Items.AddRange(new string[] { "Personel", "Admin" });
            cmbKullaniciTipi.SelectedIndex = -1;
            mainPanel.Controls.Add(lblKullaniciTipi);
            mainPanel.Controls.Add(cmbKullaniciTipi);
            y += gap;

            // Sicil No / Kullanıcı Adı
            Label lblKullaniciAdi = new Label
            {
                Text = "Sicil No:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            TextBox txtKullaniciAdi = new TextBox
            {
                Name = "txtKullaniciAdi",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                BackColor = Color.FromArgb(245, 247, 250),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            mainPanel.Controls.Add(lblKullaniciAdi);
            mainPanel.Controls.Add(txtKullaniciAdi);
            y += gap;

            // Şifre
            Label lblSifre = new Label
            {
                Text = "Şifre:",
                Location = new Point(0, y + 6),
                Size = new Size(labelWidth, 24),
                Font = labelFont,
                ForeColor = Color.FromArgb(52, 73, 94),
                TextAlign = ContentAlignment.MiddleRight
            };
            TextBox txtSifre = new TextBox
            {
                Name = "txtSifre",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, inputHeight),
                Font = inputFont,
                BackColor = Color.FromArgb(245, 247, 250),
                ForeColor = Color.FromArgb(44, 62, 80),
                PasswordChar = '•'
            };
            mainPanel.Controls.Add(lblSifre);
            mainPanel.Controls.Add(txtSifre);
            y += gap + 20;

            // Giriş Butonu
            Button btnGiris = new Button
            {
                Text = "Giriş Yap",
                Location = new Point(labelWidth + 10, y),
                Size = new Size(inputWidth, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnGiris.FlatAppearance.BorderSize = 0;
            btnGiris.MouseEnter += (s, e) => btnGiris.BackColor = Color.FromArgb(39, 174, 96);
            btnGiris.MouseLeave += (s, e) => btnGiris.BackColor = Color.FromArgb(46, 204, 113);
            btnGiris.Click += (s, e) => GirisYap(cmbKullaniciTipi.Text, txtKullaniciAdi.Text, txtSifre.Text);
            mainPanel.Controls.Add(btnGiris);

            // Kullanıcı tipi değiştiğinde etiketleri güncelle
            cmbKullaniciTipi.SelectedIndexChanged += (s, e) =>
            {
                lblKullaniciAdi.Text = cmbKullaniciTipi.Text == "Admin" ? "Kullanıcı Adı:" : "Sicil No:";
            };
        }

        private void GirisYap(string kullaniciTipi, string kullaniciAdi, string sifre)
        {
            if (string.IsNullOrWhiteSpace(kullaniciTipi))
            {
                MessageBox.Show("Lütfen giriş tipini seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (kullaniciTipi == "Admin")
                {
                    // Admin girişi kontrolü
                    if (kullaniciAdi == "admin" && sifre == "admin123")
                    {
                        this.Hide();
                        using (var form = new Form1())
                        {
                            form.ShowDialog();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Personel girişi kontrolü
                    var personeller = db.GetPersoneller();
                    var personel = personeller.Find(p => p.SicilNo == kullaniciAdi);

                    if (personel == null)
                    {
                        MessageBox.Show("Sicil numarası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (personel.Sifre != sifre)
                    {
                        MessageBox.Show("Şifre hatalı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Hide();
                    using (var form = new Form1(personel))
                    {
                        form.ShowDialog();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş yapılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IzinTalebi_Click(object sender, EventArgs e)
        {
            if (aktifPersonel == null)
            {
                MessageBox.Show("Sadece personel girişi ile izin talebi oluşturabilirsiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var form = new IzinTalebiForm(aktifPersonel))
            {
                form.ShowDialog();
            }
        }

        private void IzinTalepleri_Click(object sender, EventArgs e)
        {
            bool isAdmin = aktifPersonel == null;
            using (var form = new IzinTalepleriForm(isAdmin, aktifPersonel))
            {
                form.ShowDialog();
            }
        }
    }
} 