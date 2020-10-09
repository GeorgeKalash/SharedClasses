using System;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public class DateTools
    {
        public struct TimeRange
        {
            public string from, to;
            public short duration;
        }

        public const string midnight = "00:00";
        public static short? hour(string _time)
        {
            short hour;
            if (Int16.TryParse(_time.Substring(0, 2), out hour) == false)
                return null;
                // throw new Octopus.OctopusException((int)Globals.ErrorId.INVALID_TIME_RANGE, _message: _time);
            return hour;
        }
        public static short? minutes(string _time)
        {
            short minutes;
            if (Int16.TryParse(_time.Substring(3, 2), out minutes) == false)
                return null;
                // throw new Octopus.OctopusException((int)Globals.ErrorId.INVALID_TIME_RANGE, _message: _time);
            return minutes;
        }
        public static short? timeLapse(string _time1, string _time2, bool _signed)
        {
            if (_time1 == _time2 && _time1 == midnight && _signed == false)
                return 24 * 60;

            if ((_time1 == null) || (_time2 == null) || (_time1 == "") || (_time2 == "") || (_time1 == _time2))
                return 0;

            short? startingHour = hour(_time1), endingHour = hour(_time2), startingMinutes = minutes(_time1), endingMinutes = minutes(_time2);

            if (startingHour == null || endingHour == null || startingMinutes == null || endingMinutes == null)
                return null;

            startingHour %= 24;
            endingHour %= 24;

            bool dayChange = false;

            if (_signed == false && startingHour > endingHour)
            {
                dayChange = true;
                endingHour += 24;
            }

            short deltaHours = (short)(endingHour - startingHour);

            if (_signed == false && deltaHours > 24 && dayChange == true)
            {
                deltaHours = (short)(deltaHours % 24);

            }

            return (short)(deltaHours * 60 + (endingMinutes - startingMinutes));

        }
        public static DateTime endOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
        public static DateTime nextAnniversary(DateTime birthDate, DateTime trxDate)
        {
            if (trxDate.Month == birthDate.Month && trxDate.Day == birthDate.Day)
                return trxDate;
            if ((birthDate.Month < trxDate.Month) || (birthDate.Month == trxDate.Month && birthDate.Day < trxDate.Day))
                return new DateTime(trxDate.Year + 1, birthDate.Month, birthDate.Day);
            else
                return new DateTime(trxDate.Year, birthDate.Month, birthDate.Day);
        }
        public static DateTime nextHalfMonth(DateTime date)
        {
            if (date.Day == 1)
                return new DateTime(date.Year, date.Month, 15);
            if (date.Day == 15)
                return endOfMonth(date);

            throw new Exception("cannot add half month, day should be 1 or 15");
        }
        public static DateTime weekStart(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            return date.AddDays(-days);
        }
        public static DateTime endOfHalfMonth(DateTime date)
        {
            return date.Day < 15 ? new DateTime(date.Year, date.Month, 15) : endOfMonth(date);
        }
        public static DateTime endOfQuarter(DateTime date)
        {
            decimal quarter = date.Month / 3;
            int month = (int)(3 * Math.Ceiling(quarter));
            return endOfMonth(date);
        }
        public static string time(int _duration)
        {
            short hours = (short) (_duration / 60);
            short minutes = (short)(_duration % 60);
            TimeSpan ts = new TimeSpan(hours, minutes, 0);
            return ts.ToString().Substring(0,5);
        }
        public static short deltaDOY(DateTime _date1, DateTime _date2)
        {
            const short fullYear = 365;
            short delta = (short)(_date2.DayOfYear - _date1.DayOfYear);
            return delta > 0 ? delta : (short)(fullYear + delta);
        }

        public static string timePart(DateTime? _date)
        {
            if (_date == null)
                return null;
            return ((DateTime)_date).ToString("HH:mm", CultureInfo.InvariantCulture);
        }
        public static string dayId(DateTime? _date)
        {
            if (_date == null)
                return null;
            return ((DateTime)_date).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }
        public static DateTime? bod(DateTime _date)
        {
            return dateTime(dayId(_date), "00:00");
        }
        public static DateTime? endOfDay(DateTime _date)
        {
            return dateTime(dayId(_date), "23:59");
        }
        public DateTime? endOfDate(string _key)
        {
            DateTime? dt = date(_key);
            return dt != null ? endOfDay((DateTime)dt) : (DateTime?)null;
        }
        public static string toString(DateTime _date)
        {
            return _date.ToString("yyyyMMdd HH:mm:ss");
        }
        public static DateTime boy(DateTime _date)
        {
            return new DateTime(_date.Year, 1, 1);
        }
        public static DateTime bom(DateTime _date)
        {
            return new DateTime(_date.Year, _date.Month, 1);
        }
        public static DateTime? endOfDay(string _dayId)
        {
            return dateTime(_dayId, "23:59");
        }
        public static DateTime? dateTime(string _dayId, string _time)
        {
            try
            {
                string dt = string.Format("{0} {1}", _dayId, _time);
                return DateTime.ParseExact(dt, "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? date(string _dayId)
        {
            if (_dayId == null)
                return null;
            try
            {
                return DateTime.ParseExact(_dayId, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
