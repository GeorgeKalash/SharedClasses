using System;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public static class DateTools
    {

        public class DateRange
        {
            DateTime startDate;
            DateTime endDate;
            public short yearDays, monthDays;
            public int delY, delM, delD;

            public static short eom(short _year, short _month)
            {
                if (_month == 1 || _month == 3 || _month == 5 || _month == 7 || _month == 8 || _month == 10 || _month == 12)
                    return 31;

                if (_month == 4 || _month == 6 || _month == 9 || _month == 11)
                    return 30;

                if (_year % 4 == 0)
                    return 29;

                return 28;

            }

            private void setDeltas()
            {
                int Y2, M2;
                M2 = endDate.Month;
                Y2 = endDate.Year;
                delD = endDate.Day - startDate.Day;

                if (delD < 0)
                {
                    delD = monthDays + delD;
                    M2 = M2 - 1;
                }

                delM = M2 - startDate.Month;

                if (delM < 0)
                {
                    delM = 12 + delM;
                    Y2 = Y2 - 1;
                }

                delY = Y2 - startDate.Year;
            }

            public DateRange()
            {
                startDate = endDate = DateTime.Now;
            }

            public DateRange(DateTime _startDate, DateTime _endDate, short _yearDays = 360, short _monthDays = 30)
            {
                startDate = _startDate;
                endDate = _endDate;
                yearDays = (short)_yearDays;
                monthDays = (short)_monthDays;
                setDeltas();
            }

            public void setCalDays(short _yearDays, short _monthDays)
            {
                yearDays = _yearDays;
                monthDays = _monthDays;
            }

            public int periodDays(DateTime? _startDate = null, DateTime? _endDate = null)
            {
                if (_startDate != null)
                    startDate = (DateTime)_startDate;
                if (_endDate != null)
                    endDate = (DateTime)_endDate;
                setDeltas();
                return delY * yearDays + delM * monthDays + delD;
            }

            public string formatPeriod()
            {
                string periodText = "";

                if (delY != 0)
                    periodText = String.Format("{0}y", delY);
                if (delM != 0)
                    periodText += String.Format(" {0}m", delM);
                if (delD != 0)
                    periodText += String.Format(" {0}d", delD);
                return periodText.TrimStart(' ');
            }

        }

        public struct TimeRange
        {
            public string from, to;
            public short duration;
        }

        public const string midnight = "00:00";

        public static string defaultValue(string value)
        {
            switch (value)
            {
                case "@@today":
                    return DateTime.Now.ToString("yyyyMMdd");
                case "@@yesterday":
                    return DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                case "@@bow":
                    return weekStart(DateTime.Now).ToString("yyyyMMdd");
                case "@@eow":
                    return eow(DateTime.Now).ToString("yyyyMMdd");
                case "@@bom":
                    return bom(DateTime.Now).ToString("yyyyMMdd");
                case "@@boq":
                    return boq(DateTime.Now).ToString("yyyyMMdd");
                case "@@bod":
                    {
                        DateTime? beginningOfTheDay = bod(DateTime.Now);
                        return beginningOfTheDay == null ? string.Empty : ((DateTime)beginningOfTheDay).ToString("yyyyMMdd hh:mm");
                    }
                case "@@eod":
                    {
                        DateTime? eod = endOfDay(DateTime.Now);
                        return eod == null ? string.Empty : ((DateTime)eod).ToString("yyyyMMdd hh:mm");
                    }
                case "@@eom":
                    return endOfMonth(DateTime.Now).ToString("yyyyMMdd");
                case "@@eoq":
                    return endOfQuarter(DateTime.Now).ToString("yyyyMMdd");
            }
            return value;
        }

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
            if (days == -1)
                days = 6;
            return date.AddDays(-days);
        }
        public static DateTime eow(DateTime date)
        {                        
            return weekStart(date).AddDays(6);
        }
        public static DateTime endOfHalfMonth(DateTime date)
        {
            return date.Day < 15 ? new DateTime(date.Year, date.Month, 15) : endOfMonth(date);
        }

        public static DateTime boq(DateTime date)
        {
            decimal quarter = date.Month / 3;
            int month = (int)(3 * Math.Floor(quarter));
            return new DateTime(date.Year, month, 1);
        }
        public static DateTime endOfSemiYear(DateTime date)
        {
            int day = date.Month <= 6 ? 30 : 31;
            int month = date.Month <= 6 ? 6 : 12;
            return new DateTime(date.Year, month, day);
        }
        public static DateTime endOfQuarter(DateTime date)
        {
            decimal quarter = date.Month / 3;
            int month = (int)(3 * Math.Ceiling(quarter));
            return endOfMonth(new DateTime(date.Year, month, 1));
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
        public static DateTime? endOfDate(string _key)
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
