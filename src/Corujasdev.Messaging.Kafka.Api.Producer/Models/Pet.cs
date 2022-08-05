using System.ComponentModel.DataAnnotations;

namespace Corujasdev.Messaging.Kafka.Api.Producer.Models
{
    public class Pet
    {
        public Guid Id { get; }

        [Required]
        public string Name { get; set; }

        [Required]
        public EnTypePet Type { get; set; }

        public Pet() => Id = Guid.NewGuid();
    }

    public enum EnTypePet
    {
        Cat = 1, 
        Dog = 2
    }
}
