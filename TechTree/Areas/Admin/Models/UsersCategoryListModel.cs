namespace TechTree.Areas.Admin.Models
{
    public class UsersCategoryListModel
    {
        public int CategoryId { get; set; }
        public ICollection<UserViewModel> Users { get; set; }       //will contain collections all users registered in system will be implemented on ui as checkboxes
        public ICollection<UserViewModel> UsersSelected { get; set; }  //it contains a collections of users that have been saved with particular category 
    }
}
