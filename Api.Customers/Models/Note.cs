using Newtonsoft.Json;

namespace Api.Customers.Models
{
	public class Note : BaseModel
	{
		[JsonIgnore] public string RowId => EntityId + "-" + NoteId;

		public string EntityId { get; set; }
		public string NoteId { get; set; }
		public string Text { get; set; }
	}
}