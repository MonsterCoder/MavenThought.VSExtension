namespace GeorgeChen.MavenThought_VSExtension.model
{
    /// <summary>
    /// Class Item
    /// </summary>
    public class ClassItem : CodeItem
    {
        /// <summary>
        /// Constructs a class item instance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="codeNamespace"></param>
        /// <param name="fullname"></param>
        public ClassItem(string name, string codeNamespace, string fullname) : base(name)
        {
            CodeNamespace = codeNamespace;
            Fullname = fullname;
        }

        /// <summary>
        /// Name space
        /// </summary>
        public string CodeNamespace { get; set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string Fullname { get; set; }
    }
}