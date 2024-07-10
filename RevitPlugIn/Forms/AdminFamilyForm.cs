using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using RevitPlugIn.Dtos;
using RevitPlugIn.Utils;

namespace RevitPlugIn.Forms
{
    public partial class AdminFamilyForm : System.Windows.Forms.Form
    {
        private Document _doc;
        private DataGridView dataGridView;

        public AdminFamilyForm(Document doc)
        {
            InitializeComponent();
            _doc = doc;
            dataGridView = new DataGridView();
            LoadFamilies();
        }

        private async void LoadFamilies()
        {
            try
            {
                // API üzerinden Blob Storage'dan aileleri ekle
                await APIUtils.AddFamiliesFromBlobAsync();

                // API üzerinden tüm yüklenebilir aileleri al
                List<FamilyDto> loadableFamilies = await APIUtils.GetAllFamiliesAsync();

                // Eğer yüklenebilir aileler null ise veya hiçbir öğe içermiyorsa
                if (loadableFamilies == null || loadableFamilies.Count == 0)
                {
                    MessageBox.Show("No loadable families found.");
                    return;
                }

                // DataGridView'in sütunlarını oluşturun (ilk kez yükleniyorsa)
                if (dataGridView1.Columns.Count == 0)
                {
                    dataGridView1.Columns.Add("Id", "Id");
                    dataGridView1.Columns.Add("Name", "Name");
                    dataGridView1.Columns.Add("FileUrl", "File URL");
                }
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.Width = 250;
                }

                // Mevcut satırları temizleyin
                dataGridView1.Rows.Clear();

                // Yüklenebilir aileleri DataGridView'e ekle
                foreach (FamilyDto family in loadableFamilies)
                {
                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading families: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int familyId))
            {
                try
                {
                    bool result = await APIUtils.DeleteFamilyAsync(familyId);

                    if (result)
                    {
                        MessageBox.Show("Family deleted successfully.");
                        // Listeyi otomatik olarak yenile
                        LoadFamilies();
                    }
                    else
                    {
                        MessageBox.Show("Family not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while deleting family: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid family ID.");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string letter = textBox1.Text;

            try
            {
                List<FamilyDto> allFamilies = await APIUtils.GetAllFamiliesAsync();
                if (allFamilies == null || allFamilies.Count == 0)
                {
                    MessageBox.Show("No families found.");
                    return;
                }

                // Harfe göre filtrele
                List<FamilyDto> filteredFamilies = allFamilies.Where(family => family.Name.StartsWith(letter, StringComparison.OrdinalIgnoreCase)).ToList();

                // DataGridView'i temizle
                dataGridView1.Rows.Clear();

                // Filtrelenmiş aileleri DataGridView'e ekle
                foreach (var family in filteredFamilies)
                {
                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while filtering families: " + ex.Message);
            }
        }
    }
}
