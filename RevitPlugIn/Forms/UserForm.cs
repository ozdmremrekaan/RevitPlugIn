using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using RevitPlugIn.Dtos;
using RevitPlugIn.Utils;

namespace RevitPlugIn.Forms
{
    public partial class UserForm : System.Windows.Forms.Form
    {
        private Document _doc;

        public UserForm(Document doc)
        {
            _doc = doc;
            InitializeComponent();
            LoadFamiliesIntoComboBox();
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            // Kullanıcıyı çıkış yap ve login formuna yönlendir
            UserUtils.Logout();
            this.Hide(); // Mevcut formu gizle
            
            // Login formunu göster
            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Kullanıcı tekrar giriş yaptıysa kullanıcı tipine göre formu göster
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
            if (!int.TryParse(textBox1.Text, out int id))
            {
                MessageBox.Show("Please enter a valid Id.");
                return;
            }

            try
            {
                var family = await APIUtils.GetFamilyAsync(id);

                if (family != null)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();

                    dataGridView1.Columns.Add("Id", "Id");
                    dataGridView1.Columns.Add("Name", "Name");
                    dataGridView1.Columns.Add("FileUrl", "File URL");

                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
                }
                else
                {
                    MessageBox.Show($"Family with Id {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while getting family: " + ex.Message);
            }
        }

        private async void LoadFamilies_Click(object sender, EventArgs e)
        {
            try
            {
                List<FamilyDto> families = await APIUtils.GetAllFamiliesAsync();

                if (families == null || families.Count == 0)
                {
                    MessageBox.Show("No families found.");
                    return;
                }

                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                dataGridView1.Columns.Add("Id", "Id");
                dataGridView1.Columns.Add("Name", "Name");
                dataGridView1.Columns.Add("FileUrl", "File URL");

                foreach (var family in families)
                {
                    dataGridView1.Rows.Add(family.Id, family.Name, family.FileUrl);
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

        private async void LoadFamilyIntoProject_Click(object sender, EventArgs e)
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
