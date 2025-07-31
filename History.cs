public class History
{
    public int HistoryId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public double PlaytimeHours { get; set; }

    public int UserId { get; set; }
    public int GameId { get; set; }
}