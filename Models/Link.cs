using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace LinkShortener.Models
{
    public class Link
    {
        public int Id { get; set; }
        private string? _longUrl;
        [Required(ErrorMessage = "The field LongUrl is required")]
        [RegularExpression(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", ErrorMessage = "Invalid URL")]
        public string? LongUrl
        {
            get => _longUrl;
            set
            {
                _longUrl = value;
                GenerateShortUrl();
            }
        }
        public string? ShortUrl { get; set; }
        public DateTime CreationDate { get;set; }
        public int Clicks { get; set; }
        public Link()
        {
            Clicks = 0;
            CreationDate = DateTime.Now;
        }
        private void GenerateShortUrl()
        {
            Guid random = Guid.NewGuid();
            string randomResult = LongUrl + random.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(randomResult);
            byte[] digest = SHA256.HashData(buffer);
            ShortUrl = Convert.ToHexString(digest).ToLower().Substring(0, 8);
        }

    }
}
