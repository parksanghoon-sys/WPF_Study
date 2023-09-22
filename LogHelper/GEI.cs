namespace LogHelper
{
    internal class GEI : Attribute
    {
        private object[] _objs;

        public GEI(params object[] pParams) => this._objs = pParams;

        public object GetParameter(int index) => this._objs[index];
    }
}
