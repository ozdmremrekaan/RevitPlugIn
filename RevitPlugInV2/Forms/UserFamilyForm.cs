using Autodesk.Revit.DB;

using System.Data;

using RevitPlugInV2.Dtos;
using RevitPlugInV2.Utils;
using System.IO;

namespace RevitPlugInV2.Forms
{
    public partial class UserFamilyForm : System.Windows.Forms.Form
    {
        private Document _doc;
        
        public UserFamilyForm(Document doc)
        {
            _doc = doc;

            LoadFamiliesIntoComboBox();
            InitializeComponent();
            
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

                // Mevcut satırları temizleyin
                dataGridView1.Rows.Clear();

                // Yüklenebilir aileleri DataGridView'e ekle
                foreach (FamilyDto family in loadableFamilies)
                {
                    // Aynı isme sahip ailelerden sadece birini ekleyin
                    if (!dataGridView1.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["Name"].Value?.ToString() == family.Name))
                    {
                        dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading families: " + ex.Message);
            }
        }

        private async void LoadFamiliesIntoComboBox()
        {
            try
            {
                List<FamilyDto> families = await APIUtils.GetAllFamiliesAsync();

                if (families == null || families.Count == 0)
                {
                    MessageBox.Show("No families found.");
                    return;
                }

                // ComboBox'ı doldur
                comboBox1.Items.Clear();
                foreach (var family in families)
                {
                    comboBox1.Items.Add(new ComboBoxItem { Text = family.Name, Value = family });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading families: " + ex.Message);
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text)) // Eğer hiçbir şey girilmediyse
                {
                    // API üzerinden Blob Storage'dan tüm aileleri al
                    List<FamilyDto> allFamilies = await APIUtils.GetAllFamiliesAsync();

                    // Eğer hiçbir aile bulunamazsa mesaj göster ve işlemi sonlandır
                    if (allFamilies == null || allFamilies.Count == 0)
                    {
                        MessageBox.Show("No families found.");
                        return;
                    }

                    // DataGridView'in sütunlarını oluşturun (ilk kez yükleniyorsa)
                    if (dataGridView1.Columns.Count == 0)
                    {
                        dataGridView1.Columns.Add("Id", "Id");
                        dataGridView1.Columns.Add("Name", "Name");
                        dataGridView1.Columns.Add("FileUrl", "File URL");
                    }

                    // Mevcut satırları temizleyin
                    dataGridView1.Rows.Clear();

                    // Tüm aileleri DataGridView'e ekle
                    foreach (FamilyDto family in allFamilies)
                    {
                        dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                    }
                }
                else // Eğer bir Id girildiyse
                {
                    int familyId;
                    if (!int.TryParse(textBox1.Text, out familyId)) // Girilen değer bir tamsayıya dönüştürülemezse
                    {
                        MessageBox.Show("Please enter a valid integer for Family Id.");
                        return;
                    }

                    // API üzerinden veritabanında belirtilen Id'ye sahip aileyi al
                    FamilyDto family = await APIUtils.GetFamilyAsync(familyId);

                    if (family == null) // Belirtilen Id'ye sahip bir aile bulunamazsa
                    {
                        MessageBox.Show("No family found with the specified Id.");
                        return;
                    }

                    // DataGridView'e sadece bu aileyi ekle
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while fetching or filtering families: " + ex.Message);
            }
        }





        private async void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a family.");
                return;
            }

            var selectedFamily = (comboBox1.SelectedItem as ComboBoxItem)?.Value as FamilyDto;
            if (selectedFamily == null)
            {
                MessageBox.Show("Selected family is not valid.");
                return;
            }

            try
            {




                // Family dosyasını indir
                var familyBytes = await APIUtils.DownloadFamilyFromBlobAsync(selectedFamily.FileUrl);
                var tempFilePath = Path.Combine(Path.GetTempPath(), selectedFamily.Name + ".rfa");

                File.WriteAllBytes(tempFilePath, familyBytes);

                // Family dosyasını projeye yükle
                using (Transaction transaction = new Transaction(_doc, "Load Family"))
                {
                    transaction.Start();


                    if (!CopyFamilyToProject(tempFilePath))
                    {
                        MessageBox.Show("Failed to load family into the project.");
                        return;
                    }


                    transaction.Commit();
                }

                MessageBox.Show("Family loaded into the project successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading family into the project: " + ex.Message);
            }
        }

        private bool CopyFamilyToProject(string filePath)
        {
            try
            {
                // Projenin dosya yolunu al
                string projectPath = _doc.PathName;

                // Proje dosyasının yolunu alamazsak işlemi iptal et
                if (string.IsNullOrEmpty(projectPath))
                {
                    MessageBox.Show("Project file path could not be determined.");
                    return false;
                }

                // Dosyayı projeye kopyala
                string destinationFilePath = Path.Combine(Path.GetDirectoryName(projectPath), Path.GetFileName(filePath));
                File.Copy(filePath, destinationFilePath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while copying family to the project: " + ex.Message);
                return false;
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

    }
}
