
using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using System.Text.RegularExpressions;


namespace UtilityNet
{
    public class DateTimeUtil
    {
        /// <summary>
        /// 时间单位
        /// </summary>
        public enum TimeUnit
        {
            /// <summary>
            /// 秒
            /// </summary>
            Second = 0,
            /// <summary>
            /// 分钟
            /// </summary>
            Minute = 1,
            /// <summary>
            /// 小时
            /// </summary>
            Hour = 2,
            /// <summary>
            /// 天
            /// </summary>
            Day = 3
        }

        /// <summary>
        /// 系统当前时间（与数据库时间同步）
        /// </summary>
        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// SQL最小的时间
        /// </summary>
        public static DateTime SQLMinValue
        {
            get
            {
                return new DateTime(1753, 1, 1, 0, 00, 00);
            }
        }


        /// <summary>
        /// 获取类似“几分钟前” 的时间格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetFriendlyDate(DateTime dateTime)
        {
            float timeDiff = 0.0f;
            //获取当前用户和服务器时间的时差
            //timeDiff = UserBO.Instance.GetUserTimeDiffrence(user);

            return GetFriendlyDateTime(dateTime, timeDiff, false);
        }

        /// <summary>
        /// 输出友好时间
        /// </summary>
        /// <param name="dateTime">服务器时间</param>
        /// <param name="TimeDiffrence">用户所在时区和服务器时区时差</param>
        /// <param name="outputTime">是否包含时间</param>
        /// <returns></returns>
        private static string GetFriendlyDateTime(DateTime dateTime, float TimeDiffrence, bool outputTime)
        {

            TimeSpan t = (Now - dateTime);

            if (SQLMinValue > dateTime || (DateTime.MaxValue - dateTime).TotalHours < TimeDiffrence || dateTime.AddHours(TimeDiffrence) < SQLMinValue)
                return string.Empty;

            dateTime = dateTime.AddHours(TimeDiffrence);

            string timeString = dateTime.ToString("HH:mm");
            if (t.TotalSeconds < 10)
            {
                return "刚刚";
            }
            else if (t.TotalSeconds < 60)
            {
                return string.Format("{0}秒前", (int)t.TotalSeconds);
            }
            else if (t.TotalMinutes < 60)
            {
                return string.Format("{0}分钟前", (int)t.TotalMinutes);
            }
            else if (t.TotalHours < 24)
            {
                return string.Format("{0}小时前", (int)t.TotalHours);
            }
            else if (t.Days == 1)
            {
                 return "昨天" + timeString;
            }
            else if (t.Days == 2)
            {
                return "前天" + timeString;
            }
            else if (t.Days > 2 && t.Days < 30)
            {
                return (int)t.Days + "天前";
            }

            return outputTime ? FormatDateTime(dateTime,false) : FormatDate(dateTime);

        }

        /// <summary>
        /// 如果是整数的天 或者 小时 或者 分钟 将转换成对应的单位: 如:"3天"
        /// 0返回 "无限期"
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="timeUnit"></param>
        /// <returns></returns>
        public static string FormatSecond(long seconds, TimeUnit timeUnit)
        {

            if (seconds == 0)
                return "久远";

            int secondOfDay = 24 * 3600;
            int secondOfHour = 3600;
            int secondOfMinute = 60;

            StringBuffer buffer = new StringBuffer();
            if (seconds > secondOfDay)
            {
                int day = (int)(seconds / secondOfDay);
                buffer += day + "天";
                seconds -= day * secondOfDay;
            }

            if (timeUnit == TimeUnit.Day && buffer.Length > 0) return buffer.ToString();

            if (seconds > secondOfHour)
            {
                int hour;
                hour = (int)seconds / secondOfHour;
                buffer += hour + "小时";
                seconds -= hour * secondOfHour;
            }

            if (timeUnit == TimeUnit.Hour && buffer.Length > 0) return buffer.ToString();

            if (seconds > secondOfMinute)
            {
                int minute;
                minute = (int)seconds / secondOfMinute;
                buffer += minute + "分钟";
                seconds -= minute * secondOfMinute;
            }

            if (timeUnit == TimeUnit.Minute && buffer.Length > 0) return buffer.ToString();

            if (seconds > 0)
                buffer += seconds + "秒";

            return buffer.ToString();
        }
        /// <summary>
        /// 格式化秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string FormatSecond(long second)
        {
            return FormatSecond(second, TimeUnit.Second);
        }

        /// <summary>
        /// 检查日期的有效性（那些年月日分开的日期， 有时后面的Day会超出当月最大值）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime CheckDateTime(int year, int month, int day)
        {
            if (month <= 0 && month > 12) month = 1;
            if (day <= 0) day = 1;
            if (year < DateTimeUtil.SQLMinValue.Year) year = DateTimeUtil.SQLMinValue.Year;

            if (year > 9999) year = 9999;

            if (month > 0 && month < 13)//天数检查
            {
                int temp = DateTime.DaysInMonth(year <= 0 || year > 9999 ? 2000 : year, month);
                if (day > temp)
                {
                    day = (short)temp;
                }
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// 如果是整数的天 或者 小时 或者 分钟 将转换成对应的单位
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="timeUnit"></param>
        /// <returns></returns>
        public static long FormatSecond(long seconds, out TimeUnit timeUnit)
        {
            if (seconds == 0)
            {
                timeUnit = TimeUnit.Second;
                return 0;
            }
            else if (seconds % (60 * 60 * 24) == 0)
            {
                timeUnit = TimeUnit.Day;
                return seconds / (60 * 60 * 24);
            }
            else if (seconds % (60 * 60) == 0)
            {
                timeUnit = TimeUnit.Hour;
                return seconds / (60 * 60);
            }
            else if (seconds % 60 == 0)
            {
                timeUnit = TimeUnit.Minute;
                return seconds / 60;
            }
            else
            {
                timeUnit = TimeUnit.Second;
                return seconds;
            }
        }


        /// <summary>
        /// 返回分钟数到 时间的转换
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string FormatMinute(int minute)
        {
            if (minute == 0) return "0";
            return FormatSecond(minute * 60);
        }

        public static string FormatMinute(int minute, TimeUnit unit)
        {
            if (minute == 0) return "0";
            return FormatSecond(minute * 60, unit);
        }


        public static string FormatDateTime(DateTime time)
        {
            return FormatDateTime(time, true);
        }

        /// <summary>
        /// 输出日期和时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime time,bool outSecond)
        {
            if (time.Year == 9999)
                return "久远";
            else if (time.Year < 1800)
                return "";
            if (outSecond)
                return time.ToString( string.Concat("yyyy-MM-dd", " " , "HH:mm:ss"));
            else
                return time.ToString(string.Concat("yyyy-MM-dd", " ", "HH:mm"));
        }



        /// <summary>
        /// 输出日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(DateTime date)
        {
            if (date.Year == 9999)
                return "久远";
            else if (date.Year < 1800)
                return "";

            return date.ToString("yyyy-MM-dd");
        }


        public static long GetSeconds(long time, TimeUnit timeUnit)
        {
            switch (timeUnit)
            {
                case TimeUnit.Second: return time;
                case TimeUnit.Minute: return time * 60;
                case TimeUnit.Hour: return time * 60 * 60;
                default: return time * 60 * 60 * 24;
            }
        }

        /// <summary>
        /// 获取本周1的日期  时间为0点
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonday()
        {
            int dayOfWeek = (int)Now.DayOfWeek;
            DateTime monday;
            if(dayOfWeek == 0)
                monday = Now.AddDays(-6);
            else
                monday = Now.AddDays(1 - dayOfWeek);

            monday = new DateTime(monday.Year, monday.Month, monday.Day);

            return monday;
        }
    }
}