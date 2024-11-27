using Google.Cloud.Firestore;

    namespace HopeWorldWide.Models
    {
        [FirestoreData]
        public class Intake
        {
            [FirestoreProperty]
            public string poNumber { get; set; } // Purchase Order Number

            [FirestoreProperty]
            public string company { get; set; } // Company

            [FirestoreProperty]
            public int inQuantity { get; set; } // Intake Quantity
        }
    }


