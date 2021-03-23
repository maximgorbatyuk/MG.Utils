namespace MG.Utils.AspNetCore.Attributes
{
    public class StandardAllowedFileExtensionsAttribute : AllowedExtensionsAttribute
    {
        private static readonly string[] _extensions =
        {
            ".pdf", ".docx", "xlsx", ".zip"
        };

        public StandardAllowedFileExtensionsAttribute()
            : base(_extensions)
        {
        }
    }
}