using Newtonsoft.Json;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel; //新

namespace menu.Models
{
    [Table("user_lists")]
    public class UserList : ObservableObject //新
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }
        [Column("deadline")]
        public DateTime Deadline { get; set; }
        [MaxLength(50)]
        [Column("share_code")]
        public string ShareCode { get; set; }

        [Column("is_in_trash")]
        public bool IsInTrash { get; set; } = false;
        [Indexed]
        [Column("user_pid")]
        public int pid { get; set; }
        [Ignore]
        public List<ListItem> ListItems { get; set; }

        public string ToAzureString()
        {
            AzureListData azureListData = new AzureListData(this.Name, this.ListItems.Select(item => item.ToAzureListItem()).ToList());

            return JsonConvert.SerializeObject(azureListData).ToString();
        }

        private record AzureListData(string Name, List<AzureListItem> AzureListItems);
    }
}
