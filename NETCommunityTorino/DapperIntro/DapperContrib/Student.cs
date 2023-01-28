//using Dapper.Contrib.Extensions;

namespace DapperIntro.DapperContrib
{
    // It's not mandatory, you can use it when the name of table is different than name of class
    //[Table("Students")]
    public class Student
    {
        // Without it the default is Id
        //[Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
    }
}
