using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Menu
{
    public static class ConsoleMenu
    {
        #region Menu con le frecce

        public static int Selection(string[] content, ConsoleColor SelForeground = ConsoleColor.Gray, ConsoleColor SelBackground = ConsoleColor.Blue, string leftSelChar = "", string leftRightChar = "", bool center = false)
        {
            int sel = 0;
            int start = Console.CursorTop;
            ConsoleKey c;
            bool cursor = Console.CursorVisible;//Serve per settare lo stato del cursore a come era prima            
                
            ConsoleColor baseBG = Console.BackgroundColor;
            ConsoleColor baseFG = Console.ForegroundColor;

            Write_S_Menu(sel, content, start, SelForeground, SelBackground, baseFG, baseBG, leftSelChar, leftRightChar, center);
            while (true)
            {
                Console.SetCursorPosition(0, start);

                if (Console.KeyAvailable)
                {

                    c = Console.ReadKey(true).Key;

                    switch (c)
                    {
                        case ConsoleKey.UpArrow:
                            sel--;
                            break;

                        case ConsoleKey.DownArrow:
                            sel++;
                            break;

                        case ConsoleKey.Enter:
                            Console.CursorVisible = cursor;//Resetta lo stato del cursore a prima del menu
                            return sel;
                    }

                    //imposta i limiti
                    if (sel == -1)
                        sel = content.Length - 1;
                    else if (sel == content.Length)
                        sel = 0;
                    Console.SetCursorPosition(0, start);
                    Write_S_Menu(sel, content, start, SelForeground, SelBackground, baseFG, baseBG, leftSelChar, leftRightChar, center);
                }
            }
        }

        private static void Write_S_Menu(int sel, string[] content, int start, ConsoleColor SelForeground, ConsoleColor SelBackground, ConsoleColor baseFG, ConsoleColor baseBG, string leftSelChar, string leftRightChar, bool center)
        {
            Console.CursorVisible = false;
            if (center)
            {
                for (int i = 0; i < content.Length; i++)
                {
                    Console.SetCursorPosition((Console.WindowWidth - content[i].Length) / 2, i + start);
                    Console.WriteLine(content[i]);
                }
                Console.SetCursorPosition((Console.WindowWidth - content[sel].Length) / 2, sel + start);
            }
            else
            {
                foreach (string s in content)
                    Console.WriteLine(s);
                Console.SetCursorPosition(0, sel + start);
            }

            Console.BackgroundColor = SelBackground;
            Console.ForegroundColor = SelForeground;
            Console.Write(content[sel]);
            Console.BackgroundColor = baseBG;
            Console.ForegroundColor = baseFG;
        }

        #endregion

        #region menu inserimento

        public static string[] InsertData(string[] field, ConsoleColor FieldForeground = ConsoleColor.White, ConsoleColor FieldBackground = ConsoleColor.Black, bool setAll = true)
        {
            ConsoleColor baseBG = Console.BackgroundColor;
            ConsoleColor baseFG = Console.ForegroundColor;
            int start = Console.CursorTop;
            int sel = 0;
            ConsoleKeyInfo c;

            Console.TreatControlCAsInput = true;

            //inizializza il vettore dei contenuti inseriti
            string[] content = new string[field.Length];
            for (int i = 0; i < content.Length; i++)
                content[i] = "";

            Write_I_Menu(sel, start, field, content, FieldForeground, FieldBackground, baseBG, baseFG);

            Console.SetCursorPosition(field[0].Length, start);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    c = Console.ReadKey();

                    //controllo lettere-numeri - backSpace - Space
                    if (It_Was_a_Mother_Fucking_Space(c.Key))
                        content[sel] += " ";

                    else if (It_Was_a_BackSpace(c.Key))
                        ApplyBackspace(c.Key, content, field, sel, start);

                    else if (Is_a_Number(c.Key))
                        content[sel] += ApplyNumber(c.Key);

                    else if (Is_a_Letter(c.Key))
                    {
                        if (Console.CapsLock)
                        {
                            if ((c.Modifiers & ConsoleModifiers.Shift) == 0)//se non è shift
                                content[sel] += c.Key.ToString();
                            else
                                content[sel] += c.Key.ToString().ToLower();
                        }
                        else
                        {
                            if ((c.Modifiers & ConsoleModifiers.Shift) == 0)
                                content[sel] += c.Key.ToString().ToLower();
                            else
                                content[sel] += c.Key.ToString();
                        }
                    }

                    //se è una freccia o tab o invio
                    else
                    {
                        switch (c.Key)
                        {
                            case ConsoleKey.UpArrow:
                                sel--;
                                break;

                            case ConsoleKey.DownArrow:
                                sel++;
                                break;

                            case ConsoleKey.Tab:
                                sel++;
                                break;

                            case ConsoleKey.LeftArrow:
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                break;

                            case ConsoleKey.RightArrow:
                                Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                                break;

                            case ConsoleKey.Enter:
                                if (setAll)
                                {
                                    if (Control(field, content, start))
                                        return content;
                                    else break;
                                }
                                else
                                    return content;
                        }

                        //sistema l'indice della linea corrente
                        if (sel == -1)
                            sel = field.Length - 1;
                        else if (sel == field.Length)
                            sel = 0;
                    }
                    Write_I_Menu(sel, start, field, content, FieldForeground, FieldBackground, baseBG, baseFG);
                }
            }
        }

        private static void Write_I_Menu(int sel, int start, string[] field, string[] content, ConsoleColor FieldForeground, ConsoleColor FieldBackground, ConsoleColor baseBG, ConsoleColor baseFG)
        {
            Console.SetCursorPosition(0, start);
            //riscrive tutte le righe del menu più i contenuti inseriti
            for (int i = 0; i < field.Length; i++)
            {
                Console.BackgroundColor = FieldBackground;
                Console.ForegroundColor = FieldForeground;
                Console.Write(field[i] + ":");
                Console.BackgroundColor = baseBG;
                Console.ForegroundColor = baseFG;
                Console.Write(content[i] + "\n");
            }
            Console.SetCursorPosition(field[sel].Length + 1 + content[sel].Length, sel + start);
        }

        private static bool Control(string[] field, string[] content, int start)
        {
            foreach (string s in content)
            {
                if (s == "")
                {
                    Console.SetCursorPosition(0, 1 + start + field.Length);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("| ERROR |    NON POSSONO ESSERCI SPAZI VUOTI");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 1 + start + field.Length);
                    Console.Write("                                                   ");
                    return false;
                }
            }
            return true;
        }

        #region controllo tasti premuti

        //contralla che il tasto premuto sia una lettera
        private static bool Is_a_Letter(ConsoleKey c)
        {
            if (c != ConsoleKey.A & 
                c != ConsoleKey.B & 
                c != ConsoleKey.C & 
                c != ConsoleKey.D & 
                c != ConsoleKey.E & 
                c != ConsoleKey.F & 
                c != ConsoleKey.G & 
                c != ConsoleKey.H & 
                c != ConsoleKey.I & 
                c != ConsoleKey.J & 
                c != ConsoleKey.K & 
                c != ConsoleKey.L & 
                c != ConsoleKey.M & 
                c != ConsoleKey.N & 
                c != ConsoleKey.O & 
                c != ConsoleKey.P & 
                c != ConsoleKey.Q & 
                c != ConsoleKey.R & 
                c != ConsoleKey.S & 
                c != ConsoleKey.T & 
                c != ConsoleKey.U & 
                c != ConsoleKey.V & 
                c != ConsoleKey.W & 
                c != ConsoleKey.X & 
                c != ConsoleKey.Y & 
                c != ConsoleKey.Z)
                return false;
            else return true;
        }

        //contralla che il tasto premuto sia un numero
        private static bool Is_a_Number(ConsoleKey c)
        {
            if (c != ConsoleKey.D0 & 
                c != ConsoleKey.D1 & 
                c != ConsoleKey.D2 & 
                c != ConsoleKey.D3 & 
                c != ConsoleKey.D4 & 
                c != ConsoleKey.D5 & 
                c != ConsoleKey.D6 & 
                c != ConsoleKey.D7 & 
                c != ConsoleKey.D8 & 
                c != ConsoleKey.D9 & 
                c != ConsoleKey.NumPad0 & 
                c != ConsoleKey.NumPad1 & 
                c != ConsoleKey.NumPad2 & 
                c != ConsoleKey.NumPad3 & 
                c != ConsoleKey.NumPad4 & 
                c != ConsoleKey.NumPad5 & 
                c != ConsoleKey.NumPad6 & 
                c != ConsoleKey.NumPad7 & 
                c != ConsoleKey.NumPad8 & 
                c != ConsoleKey.NumPad9)
                return false;
            else return true;
        }

        //contralla che il tasto premuto sia 'space'
        private static bool It_Was_a_Mother_Fucking_Space(ConsoleKey c)
        {
            if (c == ConsoleKey.Spacebar)
                return true;
            else return false;
        }

        //controlla che il tasto premuto sia 'backspace'
        private static bool It_Was_a_BackSpace(ConsoleKey c)
        {
            if (c == ConsoleKey.Backspace)
                return true;
            else return false;
        }

        private static void ApplyBackspace(ConsoleKey c, string[] content, string[] field, int sel, int start)
        {
            try
            {
                content[sel] = content[sel].Substring(0, content[sel].Length - 1);
                Console.SetCursorPosition(field[sel].Length + 1 + content[sel].Length, sel + start);
                Console.Write("                                                    ");
            }
            catch { }
        }

        private static int ApplyNumber(ConsoleKey c)
        {
            switch (c)
            {
                case ConsoleKey.D0:
                    return 0;
                case ConsoleKey.D1:
                    return 1;
                case ConsoleKey.D2:
                    return 2;
                case ConsoleKey.D3:
                    return 3;
                case ConsoleKey.D4:
                    return 4;
                case ConsoleKey.D5:
                    return 5;
                case ConsoleKey.D6:
                    return 6;
                case ConsoleKey.D7:
                    return 7;
                case ConsoleKey.D8:
                    return 8;
                case ConsoleKey.D9:
                    return 9;
                case ConsoleKey.NumPad0:
                    return 0;
                case ConsoleKey.NumPad1:
                    return 1;
                case ConsoleKey.NumPad2:
                    return 2;
                case ConsoleKey.NumPad3:
                    return 3;
                case ConsoleKey.NumPad4:
                    return 4;
                case ConsoleKey.NumPad5:
                    return 5;
                case ConsoleKey.NumPad6:
                    return 6;
                case ConsoleKey.NumPad7:
                    return 7;
                case ConsoleKey.NumPad8:
                    return 8;
                case ConsoleKey.NumPad9:
                    return 9;
            }
            return 0;
        }
        #endregion

        #endregion
    }
}
