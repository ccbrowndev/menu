namespace menu.Models
{
    public class AzureListData
    {
        public string Name { get; set; }
        public List<AzureListItem> AzureListItems { get; set; }

        public AzureListData(string name, List<AzureListItem> azureListItems)
        {
            Name = name;
            AzureListItems = azureListItems;
        }
    }
}
