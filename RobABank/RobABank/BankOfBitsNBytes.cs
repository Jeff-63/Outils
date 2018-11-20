using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankOfBitsAndBytes
{
    class BankOfBitsNBytes
    {
        public static readonly char[] acceptablePasswordChars = new char[]
        { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        int moneyAmt = 5000;
        char[] password;
        int passwordLength = 4;
        private static object myLock = new object();

        public BankOfBitsNBytes()
        {
            ResetPassword();
        }

        //make the thread safe
        public int WithdrawMoney(char[] _password)
        {
            if (_password.Length != password.Length)
            {
                FailedHackDetected();
                return -1;
            }

            Stack<int> weirdBug = new Stack<int>();
            bool passwordCorrect = true;
            for (int i = 0; i < _password.Length; ++i)
            {
                if (password[i] != _password[i])
                {
                    passwordCorrect = false;
                    FailedHackDetected();
                }
                else
                {
                    weirdBug.Push(i);
                }
            }


            if (passwordCorrect)
            {
                if (moneyAmt >= 500)
                {
                    lock (myLock)
                    {
                        moneyAmt -= 500;
                        ResetPassword();
                        return 500;
                    }
                }
                else
                {
                    Console.WriteLine("Not enough money in the bank!");
                    return 0;
                }
            }
            else
            {
                //A weird bug, when it fails, returns an int which reperesents the correct bits
                /*int toRet = 0;
                while (weirdBug.Count > 0)
                    toRet += (2 ^ weirdBug.Pop());
                Console.WriteLine("Pass: " + CharArrayToString(_password) + " vs " + CharArrayToString(password) + " yielded " + toRet);
                return toRet * -1;*/
                //Console.WriteLine(CharArrayToString(_password) + " fail !");
                return -2;
            }
        }

        private void FailedHackDetected()
        {
            //Console.Out.Write("failed");
        }

        private void ResetPassword()
        {
            password = GenerateRandomCharArray(passwordLength);
            /*Random r = new Random();
            
            password = new char[passwordLength];
            for (int i = 0; i < passwordLength; ++i)
            {
                int randomInt = (r.Next() % acceptablePasswordChars.Length);
                password[i] = acceptablePasswordChars[randomInt];
            }*/
            Console.Out.WriteLine("New password: " + CharArrayToString(password));
        }

        public static Random r = new Random(); //To prevent it being re-created every frame based on sys clock (Which would produce non-random number)
        public static char[] GenerateRandomCharArray(int length)
        {
            char[] toRet = new char[length];
            for (int i = 0; i < length; ++i)
            {
                int randomInt = (r.Next() % acceptablePasswordChars.Length);
                toRet[i] = acceptablePasswordChars[randomInt];
            }
            return toRet;
        }

        public static string CharArrayToString(char[] toString)
        {
            return new string(toString);
        }
    }
}
