using Firebase.Firestore;

[FirestoreData]
public class GameData
{
    [FirestoreProperty] public int BestScore {  get; set; }

}
