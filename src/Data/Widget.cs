using System;
using NPoco;

namespace Data
{
    [TableName("Widgets"), PrimaryKey("id", AutoIncrement = false)]
    public class Widget : IEquatable<Widget>
    {
        [Column(Name = "id")]
        public long Id { get; set; }

        public bool Equals(Widget other)
        {
            return Id == other.Id;
        }
    }
}
