namespace GameVault.BLL.ModelVM.Review
{
    public class ReviewDTO
    {
        public int Review_Id { get; set; }
        public int Player_Id { get; set; }
        public int Game_Id { get; set; }
        public string Comment { get; set; }
        public float Ratinga { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public float Rating { get;  set; }
    }
}
