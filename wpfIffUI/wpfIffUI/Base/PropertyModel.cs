using System.Collections.Generic;

namespace wpfIffUI.Control
{
    public class PropertyModel : PropertyModel<object>
    {
        public PropertyModel()
        {
        }

        public PropertyModel(params string[] names)
            : base(names)
        {
        }

        public PropertyModel(List<string> names)
            : base(names)
        {
        }
    }
}
