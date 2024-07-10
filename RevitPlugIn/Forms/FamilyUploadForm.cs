using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using RevitPlugIn.Utils;
using System.Linq;
using System.IO;

namespace RevitPlugIn.Forms
{
    public partial class FamilyUploadForm : System.Windows.Forms.Form
    {
        private Document _doc;
        

        public FamilyUploadForm(Document doc)
        {
            _doc = doc;
            InitializeComponent();
            LoadFamilies();
        }

        private void LoadFamilies()
        {
            try
            {
                HashSet<string> uniqueFamilyNames = new HashSet<string>(); // Birinci adımlarda aynı isme sahip aileleri takip etmek için bir küme oluştur

                List<FamilySymbol> loadableFamilySymbols = new FilteredElementCollector(_doc)
                    .OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>()
                    .Where(fs => !_doc.IsFamilyDocument && !fs.Family.IsInPlace && fs.Family.IsEditable)
                    .ToList();

                foreach (FamilySymbol familySymbol in loadableFamilySymbols)
                {
                    string familyName = familySymbol.Family.Name;

                    // Eğer bu isim daha önce eklenmediyse, listeye ekle
                    if (!uniqueFamilyNames.Contains(familyName))
                    {
                        familyBox.Items.Add(familyName);
                        uniqueFamilyNames.Add(familyName); // Eklenen ismi kümeye ekle
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while fetching loadable families: " + ex.Message);
            }
        }


        private async void UploadButton_Click(object sender, EventArgs e)
        {
            string selectedFamilyName = familyBox.SelectedItem?.ToString();

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
                var familyDoc = _doc.EditFamily(selectedFamily.Family);
                familyDoc.SaveAs(tempFilePath);
                familyDoc.Close();

                // Family dosyasını oku
                byte[] familyFileBytes = File.ReadAllBytes(tempFilePath);

                // Dosyayı Azure Blob Storage'a yükle
                await APIUtils.UploadFamilyToBlobAsync(selectedFamilyName, familyFileBytes);

                // Blob'dan aileleri ekle
                await APIUtils.AddFamiliesFromBlobAsync();

                // Geçici dosyayı sil
                File.Delete(tempFilePath);

                MessageBox.Show("Family uploaded to Azure Blob Storage and added from Blob successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while uploading family: " + ex.Message);
            }
        }

    }
}
