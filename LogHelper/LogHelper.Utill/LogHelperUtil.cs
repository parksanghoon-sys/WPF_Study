using System.Reflection;
using System.Text;

namespace LogHelper.Utill
{
    internal class LogHelperUtil
    {
        public static string GetMessage(
          bool isNewLine,
          string header,
          string strFormat,
          params object[] listObj)
        {
            StringBuilder stringBuilder = new StringBuilder(CustomFormatAnalysis.Analysis(header) + " : ");
            stringBuilder.AppendFormat(strFormat, listObj);
            if (isNewLine)
                stringBuilder.Append("\r\n");
            return stringBuilder.ToString();
        }

        public static object GetParameter<T>(T pEnum, int index)
        {
            FieldInfo field = typeof(T).GetField(pEnum.ToString());
            return !(field.GetCustomAttributes(false)[0] is GEI customAttribute) ? (field.GetCustomAttributes(false)[0] as GEI).GetParameter(index) : customAttribute.GetParameter(index);
        }

        public static T GetEnum<T>(string pEnum) => (T)Enum.Parse(typeof(T), pEnum);
    }
}
