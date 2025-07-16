using System;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace PersonelIzinTakip;

public partial class Form1 : Form
{
    private Personel aktifPersonel;

    public Form1(Personel personel = null)
    {
        aktifPersonel = personel;
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Arka plan rengi
        this.BackColor = Color.FromArgb(245, 247, 250);
        this.Text = "Personel İzin Takip Sistemi";
        this.Size = new Size(900, 650);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        // Başlık
        Label lblTitle = new Label
        {
            Text = "Personel İzin Takip Sistemi",
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = Color.FromArgb(44, 62, 80),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 80
        };
        this.Controls.Add(lblTitle);

        // Menü oluşturma
        MenuStrip menuStrip = new MenuStrip
        {
            BackColor = Color.FromArgb(52, 73, 94),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };

        // Personel menüsü
        ToolStripMenuItem personelMenu = new ToolStripMenuItem("Personel İşlemleri");
        personelMenu.DropDownItems.Add("Personel Ekle", null, PersonelEkle_Click);
        personelMenu.DropDownItems.Add("Personel Listesi", null, PersonelListesi_Click);

        // İzin menüsü
        ToolStripMenuItem izinMenu = new ToolStripMenuItem("İzin İşlemleri");
        var izinTalebiMenu = izinMenu.DropDownItems.Add("İzin Talebi Oluştur", null, IzinTalebi_Click);
        var izinTalepleriMenu = izinMenu.DropDownItems.Add("İzin Talepleri", null, IzinTalepleri_Click);

        // Çıkış menüsü
        ToolStripMenuItem cikisMenu = new ToolStripMenuItem("Çıkış");
        cikisMenu.Click += Cikis_Click;

        menuStrip.Items.Add(personelMenu);
        menuStrip.Items.Add(izinMenu);
        menuStrip.Items.Add(cikisMenu);
        this.MainMenuStrip = menuStrip;
        this.Controls.Add(menuStrip);
        menuStrip.BringToFront();

        // Personel girişi ise sadece izin talebi ve çıkış menülerini göster
        if (aktifPersonel != null)
        {
            personelMenu.Visible = false;
            izinTalepleriMenu.Visible = false;
        }

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

        // Hoş geldiniz kutusu
        Panel welcomePanel = new Panel
        {
            Size = new Size(500, 200),
            Location = new Point((mainPanel.Width - 500) / 2, 100),
            BackColor = Color.FromArgb(236, 240, 241),
            BorderStyle = BorderStyle.FixedSingle,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        mainPanel.Controls.Add(welcomePanel);
        welcomePanel.BringToFront();

        Label lblWelcome = new Label
        {
            Text = "Hoş geldiniz!\nPersonel izin işlemlerinizi kolayca yönetin.",
            Font = new Font("Segoe UI", 15, FontStyle.Regular),
            ForeColor = Color.FromArgb(52, 73, 94),
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };
        welcomePanel.Controls.Add(lblWelcome);

        // Başlangıçta menüyü göster
        menuStrip.Visible = true;
    }

    private void PersonelEkle_Click(object sender, EventArgs e)
    {
        using (var form = new PersonelEkleForm())
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Personel eklendi, gerekirse ana formu güncelle
            }
        }
    }

    private void PersonelListesi_Click(object sender, EventArgs e)
    {
        using (var form = new PersonelListesiForm())
        {
            form.ShowDialog();
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
        using (var form = new IzinTalepleriForm(isAdmin, isAdmin ? null : aktifPersonel))
        {
            form.ShowDialog();
        }
    }

    private void Cikis_Click(object sender, EventArgs e)
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

    private void PersonelGiris_Click(object sender, EventArgs e)
    {
        using (var form = new GirisForm())
        {
            if (form.ShowDialog() == DialogResult.OK)
            {
                // var personel = form.GirisYapanPersonel; 
                // if (personel != null)
                // {
                //     
                // }
            }
        }
    }
}
