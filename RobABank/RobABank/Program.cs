using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BankOfBitsAndBytes
{
    class Program
    {
        static void Main(string[] args)
        {
            BankOfBitsNBytes bank = new BankOfBitsNBytes();
            BankRobber br = new BankRobber(bank);
            br.RobbTheBank();
            Console.ReadLine();
        }
    }

    class BankRobber
    {
        bool isReseting = false;
        BankOfBitsNBytes bank;
        float money;
        bool bankIsEmpty = false;
        int passwordSize = 1;
        bool sizeReached = false;
        List<Thread> threads = new List<Thread>();

        public BankRobber(BankOfBitsNBytes _bank)
        {
            bank = _bank;
        }

        public void RobbTheBank()
        {
            SetPasswordSize();
            threads = new List<Thread>();
            while (!bankIsEmpty)
            {
                if (!ThreadsStillRunning())
                {
                    char[] pwdGuess = new char[passwordSize];
                    int maxIndexCharPassword = passwordSize - 1;
                    for (int i = 0; i < BankOfBitsNBytes.acceptablePasswordChars.Length; i++)
                    {
                        char firstCharPassword = BankOfBitsNBytes.acceptablePasswordChars[i];
                        ThreadStart ts = new ThreadStart(() => { TryToRob(0, maxIndexCharPassword, firstCharPassword, pwdGuess); });
                        Thread t = new Thread(ts);
                        t.Start();
                        threads.Add(t);
                    }
                }
            }
        }

        private void SetPasswordSize()
        {
            while (passwordSize <= BankOfBitsNBytes.acceptablePasswordChars.Length && !sizeReached)
            {
                char[] pwd = new char[passwordSize];
                FillCharTable(pwd);
                if (bank.WithdrawMoney(pwd) == -1)
                {
                    passwordSize++;
                }
                else
                    sizeReached = true;
            }
        }

        private void TryToRob(int indexCharPassword, int maxIndexCharPassword, char firstCharPassword, char[] password)
        {
            if (indexCharPassword == 0)
            {
                password[indexCharPassword] = firstCharPassword;
                TryToRob(1, maxIndexCharPassword, firstCharPassword, password);
            }
            else
            {
                for (int i = 0; i < BankOfBitsNBytes.acceptablePasswordChars.Length; Interlocked.Increment(ref i))
                {
                    if (!isReseting)
                    {
                        password[indexCharPassword] = BankOfBitsNBytes.acceptablePasswordChars[i];
                        if (indexCharPassword != maxIndexCharPassword)
                            TryToRob(indexCharPassword + 1, maxIndexCharPassword, firstCharPassword, password);
                        else
                        {
                            int robberyTryMoneyAmount = bank.WithdrawMoney(password);
                            if (robberyTryMoneyAmount == 500)
                            {
                                money += robberyTryMoneyAmount;
                                Console.WriteLine("Quantity robbed : " + money);
                                isReseting = true;
                            }
                            else if (robberyTryMoneyAmount == 0)
                            {
                                bankIsEmpty = true;
                            }
                        }
                    }
                }
            }
        }

        private void FillCharTable(char[] t)
        {
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = BankOfBitsNBytes.acceptablePasswordChars[i];
            }
        }

        private bool ThreadsStillRunning()
        {
            foreach (Thread t in threads)
            {
                if (t.IsAlive)
                    return true;
            }
            isReseting = false;
            threads = new List<Thread>();
            return false;
        }
    }
}
