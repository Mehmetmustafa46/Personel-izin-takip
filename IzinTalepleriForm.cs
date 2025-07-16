using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace PersonelIzinTakip
{
    public partial class IzinTalepleriForm : Form
    {
        private List<Izin> izinler;
        private readonly Database db;
        private bool isAdmin;
        private Personel aktifPersonel;

        public IzinTalepleriForm(bool isAdmin = false, Personel aktifPersonel = null)
        {
            InitializeComponent();
            db = new Database();
            this.isAdmin = isAdmin;
            this.aktifPersonel = aktifPersonel;
            InitializeUI();
            LoadIzinler();
        }

        private void InitializeUI()
        {
            // Form arka planı
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Text = "İzin Talepleri";
            this.Size = new Size(1100, 650);
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
            dgvIzinler.Columns.Add("PersonelAdSoyad", "Personel");
            dgvIzinler.Columns.Add("IzinTuru", "İzin Türü");
            dgvIzinler.Columns.Add("BaslangicTarihi", "Başlangıç");
            dgvIzinler.Columns.Add("BitisTarihi", "Bitiş");
            dgvIzinler.Columns.Add("IzinGunuSayisi", "Gün Sayısı");
            dgvIzinler.Columns.Add("Aciklama", "Açıklama");
            dgvIzinler.Columns.Add("Durum", "Durum");
            dgvIzinler.Columns.Add("TalepTarihi", "Talep Tarihi");
            dgvIzinler.Columns.Add("OnaylayanKisi", "Onaylayan");
            dgvIzinler.Columns.Add("OnayTarihi", "Onay Tarihi");

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
            ToolStripButton btnOnayla = new ToolStripButton("Onayla")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(46, 204, 113),
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Enabled = isAdmin
            };
            ToolStripButton btnReddet = new ToolStripButton("Reddet")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(231, 76, 60),
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Enabled = isAdmin
            };

            ToolStripButton btnUstMenu = new ToolStripButton("Üst Menüye Dön")
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(155, 89, 182),
                DisplayStyle = ToolStripItemDisplayStyle.Text
            };

            btnYenile.MouseEnter += (s, e) => btnYenile.BackColor = Color.FromArgb(41, 128, 185);
            btnYenile.MouseLeave += (s, e) => btnYenile.BackColor = Color.FromArgb(52, 152, 219);
            btnOnayla.MouseEnter += (s, e) => btnOnayla.BackColor = Color.FromArgb(39, 174, 96);
            btnOnayla.MouseLeave += (s, e) => btnOnayla.BackColor = Color.FromArgb(46, 204, 113);
            btnReddet.MouseEnter += (s, e) => btnReddet.BackColor = Color.FromArgb(192, 57, 43);
            btnReddet.MouseLeave += (s, e) => btnReddet.BackColor = Color.FromArgb(231, 76, 60);
            btnUstMenu.MouseEnter += (s, e) => btnUstMenu.BackColor = Color.FromArgb(142, 68, 173);
            btnUstMenu.MouseLeave += (s, e) => btnUstMenu.BackColor = Color.FromArgb(155, 89, 182);

            btnYenile.Click += (s, e) => LoadIzinler();
            btnOnayla.Click += (s, e) => IzinDurumGuncelle(dgvIzinler, "Onaylandı");
            btnReddet.Click += (s, e) => IzinDurumGuncelle(dgvIzinler, "Reddedildi");
            btnUstMenu.Click += (s, e) => UstMenuDon();

            toolStrip.Items.AddRange(new ToolStripItem[] { btnYenile, btnOnayla, btnReddet, btnUstMenu });

            // Kontrolleri panele ekleme
            mainPanel.Controls.Add(toolStrip);
            mainPanel.Controls.Add(dgvIzinler);
        }

        private void LoadIzinler()
        {
            try
            {
                izinler = db.GetIzinler();

                // Paneli bul
                var mainPanel = this.Controls.OfType<Panel>().FirstOrDefault();
                if (mainPanel == null) return;
                // DataGridView'i bul
                var dgv = mainPanel.Controls.OfType<DataGridView>().FirstOrDefault();
                if (dgv == null) return;
                dgv.Rows.Clear();
                foreach (var izin in izinler)
                {
                    dgv.Rows.Add(
                        izin.Id,
                        $"{izin.PersonelAd} {izin.PersonelSoyad}",
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

        private void IzinDurumGuncelle(DataGridView dgv, string yeniDurum)
        {
            if (dgv.SelectedRows.Count == 0) return;

            int izinId = Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);
            var izin = izinler.Find(i => i.Id == izinId);

            if (izin == null) return;

            if (izin.Durum != "Beklemede")
            {
                MessageBox.Show("Sadece beklemedeki izin talepleri onaylanabilir veya reddedilebilir.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"İzin talebini {yeniDurum.ToLower()} olarak işaretlemek istediğinizden emin misiniz?",
                "İzin Durumu Güncelleme",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    izin.Durum = yeniDurum;
                    izin.OnaylayanKisi = "Admin"; // TODO: Giriş yapan kullanıcı bilgisi eklenecek
                    izin.OnayTarihi = DateTime.Now;

                    // İzin onaylandıysa personelin kalan izin gününü güncelle
                    if (yeniDurum == "Onaylandı")
                    {
                        var personeller = db.GetPersoneller();
                        var personel = personeller.Find(p => p.Id == izin.PersonelId);
                        if (personel != null)
                        {
                            personel.KalanIzinGunu -= izin.IzinGunuSayisi;
                            db.PersonelGuncelle(personel);
                        }
                    }

                    db.IzinGuncelle(izin);
                    LoadIzinler();
                    MessageBox.Show($"İzin talebi {yeniDurum.ToLower()} olarak işaretlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İzin durumu güncellenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UstMenuDon()
        {
            var result = MessageBox.Show("Üst menüye dönmek istediğinizden emin misiniz?", "Üst Menüye Dön", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                /*using (var form = new Form1())
                {
                    form.ShowDialog();
                }*/
                this.Close();
            }
        }
    }
} 