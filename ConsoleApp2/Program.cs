using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;


namespace ConsoleApp2
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult);

        private static readonly IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        private const int WM_SETTINGCHANGE = 0x1a;
        private const int SMTO_ABORTIFHUNG = 0x0002;

        private const string Filename = @"InputPic1.jpeg";
        static void Main(string[] args)
        {
            Event[][] myEvents = new Event[31][];
            Event myEvent = new Event("3:00", "11:30", "Work at Arby's", 27, 7);
            myEvents.SetValue(new Event[3], myEvent.EventDate);
            myEvents[myEvent.EventDate].SetValue(myEvent, 1);
            DateTime rn = DateTime.Now;
            Bitmap _otherBitmap = new Bitmap(Filename);
            int x = _otherBitmap.Width;
            int y = _otherBitmap.Height;
            Color c = Color.White;
            DrawCalendar(_otherBitmap, c);
            Graphics _myCalendar = WriteDates(_otherBitmap, Color.White, getFirstOfMonth(rn), DateTime.DaysInMonth(rn.Year, rn.Month));
            _myCalendar.DrawImage(_otherBitmap, 0, 0);
            WriteEvents(myEvents, _otherBitmap, getFirstOfMonth(rn));
            string oFileName = rn.Day.ToString() + ".jpeg";
            _otherBitmap.Save(oFileName);
            Registry.SetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "WallPaper", "C:\\Users\\Tyler\\source\\repos\\ConsoleApp2\\ConsoleApp2\\bin\\Debug\\netcoreapp3.1\\" + oFileName);

            SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero, null, SMTO_ABORTIFHUNG, 100, IntPtr.Zero);
            


        }

        static int getFirstOfMonth(DateTime rn)
        {
            int fom;
            int dow = (int) rn.DayOfWeek;
            int date = rn.Day;
            while (date > 1)
            {
                dow--;
                if (dow == -1)
                {
                    dow += 7;
                }
                date--;
            }
            fom = dow;
            return fom;
        }

        static void DrawCalendar(Bitmap b, Color c)
        {
            int x = b.Height / 5;
            int y = b.Width / 7;
            HorizontalLine(x, b, c);
            HorizontalLine(2 * x, b, c);
            HorizontalLine(3 * x, b, c);
            HorizontalLine(4 * x, b, c);
            VerticalLine(y, b, c);
            VerticalLine(y * 2, b, c);
            VerticalLine(y * 3, b, c);
            VerticalLine(y * 4, b, c);
            VerticalLine(y * 5, b, c);
            VerticalLine(y * 6, b, c);

        }

        static void HorizontalLine(int row, Bitmap b, Color c)
        {
            int x = 0;
            while (x < b.Width)
            {
                b.SetPixel(x, row, c);
                x++;
            }
        }

        static void VerticalLine(int column, Bitmap b, Color c)
        {
            int x = 0;
            while (x < b.Height)
            {
                b.SetPixel(column, x, c);
                x++;
            }
        }

        static Graphics WriteDates(Bitmap b, Color c, int firstOfMonth, int maxDate)
        {
            int dx = b.Width / 7;
            int dy = b.Height / 5;
            Graphics g = Graphics.FromImage(b);
            int x;
            int y;
            int date = 0;
            for (int row = 0; row < 5; row++)
            {
                
                y = row * dy;
                y += 5;
                for (int column = 0; column < 7; column++)
                {

                    // Skips the boxes for days that aren't in this month
                    if (row == 0 && column < firstOfMonth)
                    {
                        continue;
                    }
                    // Increments the number to be put in the boxes, sets the point to put the number, and draws the number
                    date++;
                    x = column * dx;
                    x += 5;
                    if (date > maxDate)
                    {
                        return g;
                    }
                    g.DrawString(date.ToString(), new Font("Tahoma", 14), Brushes.White, new PointF(x, y));
                }
            }
            return g;
        }

        static Boolean WriteEvents(Event[][] myEvents, Bitmap b, int firstOfMonth)
        {
            try
            {
                int dx = b.Width / 7;
                int dy = b.Height / 5;
                int x;
                int y;
                int date = 0;
                Graphics g = Graphics.FromImage(b);
                for (int row = 0; row < 5; row++)
                {
                    y = dy * row;
                    y += 10;
                    for (int column = 0; column < 7; column++)
                    {
                        if (row == 0 && column < firstOfMonth)
                        {
                            continue;
                        }
                        x = column * dx;
                        x += 40;
                        for (int i = 0; i < myEvents[date].Length; i++)
                        {
                            try
                            {
                                Event each = myEvents[date][i];
                                g.DrawString(each.ToString(), new Font("Tahoma", 14), Brushes.White, new PointF(x, y));
                            }
                            catch { continue; }
                        }
                        date++;
                    }
                }

                g.DrawImage(b, 0, 0);


                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
