using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class MailData
{
    [FirestoreDocumentId] public string Id { get; set; }
    [FirestoreProperty] public string Title { get; set; }
    [FirestoreProperty] public string Message { get; set; }
    [FirestoreProperty] public int RewardScore { get; set; }
    [FirestoreProperty] public bool IsRead { get; set; }
    [FirestoreProperty] public bool IsClaimed { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    [FirestoreProperty] public Timestamp ExpiresAt { get; set; }

    public bool HasReward => RewardScore > 0;
}
