namespace LibraryProject.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
