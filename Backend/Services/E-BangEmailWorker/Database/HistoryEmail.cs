namespace E_BangEmailWorker.Database;

public partial class HistoryEmail
{
    public int EmailId { get; set; }

    public string Recipient { get; set; } = null!;

    public DateTime SentDate { get; set; }

    public DateTime HistoryProcessedDate { get; set; }
}
