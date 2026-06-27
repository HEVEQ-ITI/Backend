namespace HEVEQ.Application.Common.Helpers
{
    public static class DayOfWeekDisplayHelper
    {
        public static string GetDayName(int dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Sunday",
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                6 => "Saturday",
                _ => "Unknown"
            };
        }

        public static string GetDayNameAr(int dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "الأحد",
                1 => "الاثنين",
                2 => "الثلاثاء",
                3 => "الأربعاء",
                4 => "الخميس",
                5 => "الجمعة",
                6 => "السبت",
                _ => "غير معروف"
            };
        }
    }
}