using Firebase.Firestore;

[FirestoreData]
public class PostData
{
    [FirestoreDocumentId] public string postId { get; set; }
    [FirestoreProperty] public string Comment {  get; set; }
    [FirestoreProperty] public string UserId {  get; set; }
    [FirestoreProperty] public string Nickname {  get; set; }
    [FirestoreProperty] public Timestamp CreatedAt {  get; set; }
}
