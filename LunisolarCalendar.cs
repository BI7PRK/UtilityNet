using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UtilityNet
{
    /// <summary>
    /// 农历
    /// </summary>
    public static class LunisolarCalendar
    {
        private static ChineseLunisolarCalendar cnCalender;

        /// <summary>
        /// 天干
        /// </summary>
        private static string[] FormerlyChar = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        /// <summary>
        /// 地支
        /// </summary>
        private static string[] FortuneChar = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };
        /// <summary>
        /// 属相
        /// </summary>
        private static string[] AnimalNameChar = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        /// <summary>
        /// 农历月份
        /// </summary>
        private static string[] LunarMonths = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "腊" };

        /// <summary>
        /// 日前缀
        /// </summary>
        private static string[] DaysPreChars = { "初", "十", "廿", "卅" };
        /// <summary>
        /// 日
        /// </summary>
        private static string[] DaysChars = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };


        private static DateTime MinSupported;
        private static DateTime MaxSupported;

        static LunisolarCalendar()
        {
            cnCalender = new ChineseLunisolarCalendar();

            MinSupported = cnCalender.MinSupportedDateTime;
            MaxSupported = cnCalender.MaxSupportedDateTime;
        }

        /// <summary>
        /// 阳历返回农历的月日部分
        /// </summary>
        /// <param name="_now"></param>
        /// <returns></returns>
        public static string GetLunarDateNoYear(this DateTime _now)
        {
            var dt = ToLunarDate(_now);
            return FormatLunarString(dt.Substring(5));
        }

        /// <summary>
        /// 将一个日期格式的转换成农历形式（不带年份）
        /// </summary>
        /// <param name="str">例如：07-04</param>
        /// <returns></returns>
        public static string FormatLunarString(string str)
        {
            try
            {
                var arr = str.Split('-');
                int m = int.Parse(arr[0]);
                int d = int.Parse(arr[1]);
                var day = "";
                if (d != 10 && d != 20 && d != 30)
                {
                    day = string.Concat(DaysPreChars[d / 10], DaysChars[d % 10]);
                }
                else
                {
                    int i = d <= 10 ? 0 : d / 10;
                    day = string.Concat(DaysPreChars[i], DaysChars[10]);
                }

                return LunarMonths[m - 1] + "月" +  day;
            }
            catch
            {

                throw new Exception("日期格式不正确");
            }
        }

        /// <summary>
        /// 返回时间格式的农历
        /// </summary>
        /// <param name="_now"></param>
        /// <returns></returns>
        public static string ToLunarDate(this DateTime _now)
        {
            int year = cnCalender.GetYear(_now);
            int month = cnCalender.GetMonth(_now);
            int day = cnCalender.GetDayOfMonth(_now);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = cnCalender.GetLeapMonth(year);
            if (leapMonth > 0 && leapMonth <= month)
            {
                month--;
            }

            return string.Format("{0}-{1}-{2}", new object[]
            {
                year, 
                month < 10 ? "0" + month : month + "", 
                day < 10 ? "0" + day : day + "", 
            });
        }
        /// <summary>
        /// 当前月份是否为闰月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool GetLeapMonth(int year, int month)
        {
            int leapMonth = cnCalender.GetLeapMonth(year);
            return leapMonth > 0 && leapMonth == month -1;
        }

        /// <summary>
        /// 农历转成阳历
        /// </summary>
        /// <param name="lunar">农历</param>
        /// <param name="isLeap">是否为闰月</param>
        /// <returns></returns>
        public static DateTime ToSolarDate(int year, int month, int day)
        {
            var isLeap = GetLeapMonth(year, month);
            if (year < MinSupported.Year || year > MaxSupported.Year)
                throw new Exception("年份超出范围");
            if (month < 1 || month > 12)
                throw new Exception("表示月份的数字必须在1～12之间");

            if (day < 1 || day > cnCalender.GetDaysInMonth(year, month))
                throw new Exception("农历日期输入有误");

            int num1 = 0, num2 = 0;
            int leapMonth = cnCalender.GetLeapMonth(year);
            if (((leapMonth == month + 1) && isLeap) || (leapMonth > 0 && leapMonth <= month))
                num2 = month;
            else
                num2 = month - 1;
            while (num2 > 0)
            {
                num1 += cnCalender.GetDaysInMonth(year, num2--);
            }

            DateTime dt = GetLunarNewYearDate(year);
            return dt.AddDays(num1 + day - 1);
        }

        /// <summary>
        /// 获取指定年份春节当日（正月初一）的阳历日期
        /// </summary>
        /// <param name="year">指定的年份</param>
        private static DateTime GetLunarNewYearDate(int year)
        {
            DateTime dt = new DateTime(year, 1, 1);
            int cnYear = cnCalender.GetYear(dt);
            int cnMonth = cnCalender.GetMonth(dt);

            int num1 = 0;
            int num2 = cnCalender.IsLeapYear(cnYear) ? 13 : 12;

            while (num2 >= cnMonth)
            {
                num1 += cnCalender.GetDaysInMonth(cnYear, num2--);
            }

            num1 = num1 - cnCalender.GetDayOfMonth(dt) + 1;
            return dt.AddDays(num1);
        }

        /// <summary>
        /// 获取完整的农历
        /// </summary>
        /// <param name="_now"></param>
        /// <returns></returns>
        public static string ToLunisolarString(this DateTime _now)
        {
            int year = cnCalender.GetYear(_now);
            int month = cnCalender.GetMonth(_now);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = cnCalender.GetLeapMonth(year);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }
            return string.Format("{0}年{1}{2}月{3}", new object[] 
            { 
               GetYearString(year),
                isleap ? "(闰)" : "",
                LunarMonths[month - 1],
                GetDayString(_now)
            });
        }

        private static string GetYearString(int year)
        {
            var str = "";
            foreach (var item in year.ToString())
            {
                str += DaysChars[int.Parse(item.ToString())];
            }
            return str;
        }

        private static string GetMonthString(DateTime _now)
        {
            int lunyear = cnCalender.GetYear(_now);
            int lunmonth = cnCalender.GetMonth(_now);
            //获取闰月， 0 则表示没有闰月
            int leapMonth = cnCalender.GetLeapMonth(lunyear);
            bool isleap = false;
            if (leapMonth > 0)
            {
                if (leapMonth == lunmonth)
                {
                    //闰月
                    isleap = true;
                    lunmonth--;
                }
                else if (lunmonth > leapMonth)
                {
                    lunmonth--;
                }
            }

            return (isleap ? "(闰)" : "") + LunarMonths[lunmonth - 1] + "月";
        }
        private static string GetDayString(DateTime _now)
        {
            int d = cnCalender.GetDayOfMonth(_now);
            var day = "";
            if (d != 10 && d != 20 && d != 30)
            {
                day = string.Concat(DaysPreChars[d / 10], DaysChars[d % 10]);
            }
            else
            {
                int i = d <= 10 ? 0 : d / 10;
                day = string.Concat(DaysPreChars[i], DaysChars[10]);
            }
            return day;
        }
    }
}
