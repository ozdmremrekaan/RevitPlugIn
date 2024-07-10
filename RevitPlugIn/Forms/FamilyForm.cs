using Autodesk.Revit.DB;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using RevitPlugIn.Dtos;
using RevitPlugIn.Utils;

namespace RevitPlugIn.Forms
{

    public partial class FamilyForm : System.Windows.Forms.Form
    {

        private Document _doc;
        private DataGridView dataGridView;
       

        public FamilyForm(Document doc)
        {
            InitializeComponent();
            _doc = doc;
            FillComboBoxWithFamilies();
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill; // Dock the dataGridView to fill the form
            Controls.Add(dataGridView);
            
        }


        private void FillComboBoxWithFamilies()
        {
            try
            {
                List<FamilySymbol> loadableFamilySymbols = new FilteredElementCollector(_doc)
                    .OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>()
                    .Where(fs => !_doc.IsFamilyDocument && !fs.Family.IsInPlace && fs.Family.IsEditable) 
                    .ToList();

                foreach (FamilySymbol familySymbol in loadableFamilySymbols)
                {
                    comboBox1.Items.Add(familySymbol.Family.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while fetching loadable families: " + ex.Message);
            }
        }



        private async void AddToLibraryButton_Click(object sender, EventArgs e)
        {
            // ComboBox'ta seçili olan family adını al
            string selectedFamilyName = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedFamilyName))
            {
                MessageBox.Show("Please select a family.");
                return;
            }

            try
            {
                // Seçilen family adına göre dosyayı bul
                FamilySymbol selectedFamily = new FilteredElementCollector(_doc)
                    .OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>()
                    .FirstOrDefault(f => f.Family.Name == selectedFamilyName);

                if (selectedFamily == null)
                {
                    MessageBox.Show("Selected family not found.");
                    return;
                }

                // Family dosyasının yolunu geçici bir konuma kaydet
                string tempFilePath = Path.Combine(Path.GetTempPath(), selectedFamilyName + ".rfa");
                var familyDoc =_doc.EditFamily(selectedFamily.Family);
                familyDoc.SaveAs(tempFilePath);
                familyDoc.Close();
                // Family dosyasını oku
                byte[] familyFileBytes = File.ReadAllBytes(tempFilePath);

                // Dosyayı Azure Blob Storage'a yükle
                await APIUtils.UploadFamilyToBlobAsync(selectedFamilyName, familyFileBytes);

                // Geçici dosyayı sil
                File.Delete(tempFilePath);

                MessageBox.Show("Family uploaded to Azure Blob Storage successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while uploading family: " + ex.Message);
            }
        }

        private async void LoadFamilies_Click(object sender, EventArgs e)
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
                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while loading families: " + ex.Message);
            }
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            
            UserUtils.Logout();
            this.Hide(); 

           
            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    
                    var userInfo = UserUtils.LoggedInUser;
                    if (userInfo != null)
                    {
                        if (userInfo.Role == UserRole.ADMIN)
                        {
                            FamilyForm familyForm = new FamilyForm(_doc); 
                            familyForm.ShowDialog();
                        }
                        else if (userInfo.Role == UserRole.USER)
                        {
                            UserForm userForm = new UserForm(_doc);
                            userForm.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Access Denied", "You do not have the required permissions to use this feature.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error", "User information not found. Please log in again.");
                    }
                }
                else
                {
                    // Kullanıcı giriş yapmadıysa formu kapat
                    this.Close();
                }
            }
        }

        private async void Apply_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int familyId))
            {
                try
                {
                    bool result = await APIUtils.DeleteFamilyAsync(familyId);

                    if (result)
                    {
                        MessageBox.Show("Family deleted successfully.");
                        // Delete the row from the DataGridView
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if ((int)row.Cells["Id"].Value == familyId)
                            {
                                dataGridView.Rows.Remove(row);
                                break;
                            }
                        }
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

        private async void FilterByLetter_Click(object sender, EventArgs e)
        {
            // TextBox'tan alınan harfi al
            string letter = textBox2.Text;

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
