﻿using System;

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
            public DateTime startDate;
            public DateTime endDate;
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
                    M2 -= 1;
                }

                delM = M2 - startDate.Month;

                if (delM < 0)
                {
                    delM = 12 + delM;
                    Y2 -= 1;
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
                        return beginningOfTheDay == null ? string.Empty : ((DateTime)beginningOfTheDay).ToString("yyyyMMdd HH:mm");
                    }
                case "@@eod":
                    {
                        DateTime? eod = endOfDay(DateTime.Now);
                        return eod == null ? string.Empty : ((DateTime)eod).ToString("yyyyMMdd HH:mm");
                    }
                case "@@eom":
                    return endOfMonth(DateTime.Now).ToString("yyyyMMdd");
                case "@@eoq":
                    return endOfQuarter(DateTime.Now).ToString("yyyyMMdd");
            }
            return value;
        }

        public static string MinutesToHHmm(int totalMinutes)
        {
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            return $"{hours}:{minutes:D2}";
        }


        public static short? hour(string _time)
        {
            if (short.TryParse(_time.Substring(0, 2), out short hour) == false)
                return null;
            return hour;
        }
        public static short? minutes(string _time)
        {
            if (short.TryParse(_time.Substring(3, 2), out short minutes) == false)
                return null;
            return minutes;
        }
        public static short? timeLapse(string _time1, string _time2, bool _signed)
        {
            if (_time1 == _time2 && _signed == false)
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
        public static DateTime nextAnniversary(DateTime baseDate, DateTime trxDate)
        {
            if (trxDate.Month == baseDate.Month && trxDate.Day == baseDate.Day)
                return trxDate.AddYears(1);
            if ((baseDate.Month < trxDate.Month) || (baseDate.Month == trxDate.Month && baseDate.Day < trxDate.Day))
                return new DateTime(trxDate.Year + 1, baseDate.Month, baseDate.Day);
            else
                return new DateTime(trxDate.Year, baseDate.Month, baseDate.Day);
        }

        public static short dow(string _dayId)
        {
            DateTime d = date(_dayId);
            return d.DayOfWeek == DayOfWeek.Sunday ? (short)7 : (short) d.DayOfWeek;
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
            double quarter = ((double) date.Month) / 3;
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
        public static DateTime? endOfDay(DateTime? _date)
        {
            return _date == null ? (DateTime ?) null : dateTime(dayId(_date), "23:59");
        }
        public static DateTime? endOfDate(string _key)
        {
            DateTime? dt = date(_key);
            return dt != null ? endOfDay(dt) : (DateTime?)null;
        }
        public static string toString(DateTime _date)
        {
            return _date.ToString("yyyyMMdd HH:mm:ss");
        }
        public static DateTime boy(DateTime _date)
        {
            return new DateTime(_date.Year, 1, 1);
        }
        public static DateTime eoy(DateTime _date)
        {
            return new DateTime(_date.Year, 12, 31);
        }
        public static DateTime bom(DateTime _date)
        {
            return new DateTime(_date.Year, _date.Month, 1);
        }
        public static DateTime? endOfDay(string _dayId)
        {
            return dateTime(_dayId, "23:59");
        }
        public static DateTime dateTime(string _dayId, string _time)
        {
                string dt = string.Format("{0} {1}", _dayId, _time);
                return DateTime.ParseExact(dt, "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
        }
        public static DateTime date(string _dayId)
        {
            return DateTime.ParseExact(_dayId, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
        public static string addDays(string _dayId, short _days)
        {
            DateTime dt = DateTime.ParseExact(_dayId, "yyyyMMdd", CultureInfo.InvariantCulture);
            dt = dt.AddDays(_days);
            return dt.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }
        public static string addMinutes(string _time, short _minutes, bool _reset = true)
        {
            short hours = Convert.ToInt16(_time.Substring(0, 2));
            short minutes = Convert.ToInt16(_time.Substring(3, 2));

            minutes += _minutes;

            if (minutes >= 60)
            {
                hours += (short)(minutes / 60);
                minutes = (short)(minutes % 60);
                if (_reset)
                    hours = (short)(hours % 24);
            }

            string leadingZeroHours = hours < 10 ? "0" : string.Empty;
            string leadingZeroMinutes = minutes < 10 ? "0" : string.Empty;

            return string.Format("{0}{1}:{2}{3}", leadingZeroHours, hours, leadingZeroMinutes, minutes);
        }

        public static double totalYears(DateTime _d1, DateTime _d2)
        {
            return (_d2 - _d1).TotalDays / 365.2425;
        }
    }
}
