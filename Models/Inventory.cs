using Google.Cloud.Firestore;

namespace HopeWorldWide.Models
{
    [FirestoreData]
    public class Inventory
    {
        [FirestoreProperty]
        public string productId { get; set; } // Unique identifier for the product

        [FirestoreProperty]
        public int currentQuantity { get; set; } // Current inventory quantity
    }
}
