using Google.Cloud.Firestore;

namespace HopeWorldWide.Models
{
    [FirestoreData]
    public class Asset
    {


        [FirestoreProperty]
        public string assetCode { get; set; }

        [FirestoreProperty]
        public string assetMake { get; set; }

        [FirestoreProperty]
        public string assetModel { get; set; }

        [FirestoreProperty]
        public string location { get; set; }

        [FirestoreProperty]
        public string site { get; set; }

        [FirestoreProperty]
        public string department { get; set; }

        [FirestoreProperty]
        public string assetCategory { get; set; }

        [FirestoreProperty]
        public string EmployeeId { get; set; }

        [FirestoreProperty]
        public double currentValue { get; set; }

        [FirestoreProperty]
        public double insuredValue { get; set; }

        [FirestoreProperty]
        public bool policyCover { get; set; }

        [FirestoreProperty]
        public bool allRiskCover { get; set; }
    }
}