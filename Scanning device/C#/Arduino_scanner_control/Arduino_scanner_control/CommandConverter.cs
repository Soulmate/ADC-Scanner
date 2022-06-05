using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Text.RegularExpressions;

namespace Arduino_scanner_control
{
    class CommandConverter
    {
        //public static string Extract_package(ref string buffer) //вытащаить из строки содержимое одного пакета с самого начала (удаляет его из строки)
        //{
        //    if (String.IsNullOrEmpty(buffer))
        //        return null;


        //    char mark_1 = '<';
        //    char mark_2 = '>';
        //    int ci_1 = buffer.IndexOf(mark_1);
        //    if (ci_1 == -1)
        //    {
        //        return null;
        //    }
        //    buffer = buffer.Remove(0, ci_1 + 1);
        //    int ci_2 = buffer.IndexOf(mark_2);
        //    if (ci_2 == -1)
        //    {
        //        return null;
        //    }
        //    if (ci_2 > 0) // если не сразу наткнулись на закрывающий символ
        //    {
        //        string output = buffer.Substring(0, ci_2);
        //        buffer = buffer.Remove(0, ci_2 + 1);
        //        return output;
        //    }
        //    else
        //        return null;
        //}

        public static List<string> Extract_all_packages(ref string buffer) //вытащаить из строки все пакеты (удаляет их из строки)
        {
            List<string> output = new List<string>();

            if (String.IsNullOrEmpty(buffer))
                return output;

            MatchCollection m_arr = Regex.Matches(buffer, "<(.*?)>");
            foreach (Match m in m_arr)
            {
                output.Add(m.Value.Substring(1, m.Value.Length - 2));
                //buffer.Remove(m.Index, m.Length);
            }
            buffer = buffer.Remove(0, buffer.LastIndexOf('>') + 1);

            //while (true)
            //{
            //    string str = Extract_package(ref buffer);
            //    if (str != null)
            //        output.Add(str);
            //    else
            //        break;
            //}
            return output;
        }
    }
}
