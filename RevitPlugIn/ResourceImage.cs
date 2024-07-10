using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace RevitPlugin
{
    /// <summary>
    /// Gets the embedded resource image from the cbb.res assembly based on user provided file name with extension.
    /// Helper methods.
    /// </summary>
    public static class ResourceImage
    {
        #region public methods

        /// <summary>
        /// Gets the icon image from resource assembly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static BitmapImage GetIcon(string name)
        {
            // Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Construct the full resource name
            var resourceName = $"{assembly.GetName().Name}.Images.{name}";

            // Create the resource reader stream
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    MessageBox.Show($"Resource '{resourceName}' not found.");
                    return null;
                }

                var image = new BitmapImage();

                // Ensure the stream is properly initialized before setting it as StreamSource
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad; // Ensure the stream is closed after loading
                image.EndInit();

                // Return constructed BitmapImage
                return image;
            }
        }

        /// <summary>
        /// Gets the icon image from resource assembly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Image GetImage(string name)
        {
            // Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Construct the full resource name
            var resourceName = $"{assembly.GetName().Name}.Images.{name}";

            // Create the resource reader stream
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    MessageBox.Show($"Resource '{resourceName}' not found.");
                    return null;
                }

                return Image.FromStream(stream);
            }
        }

        #endregion
    }
}
