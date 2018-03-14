using System.IO;
using System.Linq;

namespace project_vega.Core.Models
{
    public class PhotoSettings
    {
        public int MaxBytes { get; set; }
        public string[] AcceptedFileTypes { get; set; }

        public bool IsSupported(string fileName)
        {
            return AcceptedFileTypes.Any(f => f == Path.GetExtension(fileName).ToLower());
        }
    }
}