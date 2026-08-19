using Firebase.Firestore;

[FirestoreData]
public class RankingData
{
    [FirestoreProperty] public string Nickname {  get; set; }
    [FirestoreProperty] public int Bestscore { get; set; }
    [FirestoreProperty] public Timestamp UpdatedAt { get; set; }

}
