using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggergatesModel
{
    public class ProjectProperty : ValueObject
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int ProjectId{ get; set; }

        public ProjectProperty()
        {

        }
        public ProjectProperty(string key, string text, string value)
        {
            this.Key = key;
            this.Text = Text;
            this.Value = value;

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Text;
            yield return Value;
        }
    }
}
