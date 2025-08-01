namespace GameVault_BLL.ModelVM.Review
{
    public class UpdateReview
    {
        public int Review_Id { get; set; }
        public int Player_Id { get; set; }
        public int Game_Id { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
