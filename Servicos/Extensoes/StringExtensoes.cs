using System;

namespace Servicos.Extensoes
{
    public static class StringExtensoes
    {
        public static DateTime ConveterParaDateTime(this string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new Exception("A string não é válida como formato de data");

            var date = data.Trim().Substring(0, 10).Split("/");
            var time = data.Trim().Substring(14).Trim().Split(":");
            return new DateTime(year: int.Parse(date[2]),
                                month: int.Parse(date[1]),
                                day: int.Parse(date[0]),
                                hour: int.Parse(time[0]),
                                minute: int.Parse(time[1]),
                                second: 0);
        }
    }
}
