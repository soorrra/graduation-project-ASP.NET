using System.ComponentModel;

namespace Petsitter.ViewModels
{
    public class ChatVM
    {
        public int SitterId { get; set; }

        [DisplayName("Sitter Name")]
        public string SitterName { get; set; }
    }
}
