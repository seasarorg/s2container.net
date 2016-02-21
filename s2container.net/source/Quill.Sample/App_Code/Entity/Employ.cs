using Quill.Attr;

namespace Quill.Sample.App_Code.Entity {
    public class Employ {
        [Column("EMPNO")]
        public decimal Id { get; set; }

        [Column("ENAME")]
        public string Name { get; set; }

        public string Job { get; set; }
    }
}
