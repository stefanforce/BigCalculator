﻿using System;
using System.Collections;
using System.Text;

namespace BigCalculator.Service
{
    public class Compute : ICompute
    {
        private readonly List<char> operators = new List<char> { '+', '-', '*', '/', '^', '#' };

        public Dictionary<string, string> ComputeCalculus(string expression, Dictionary<string, string> terms)
        {
            char x;
            string firstOperand, secondOperand, firstOperandValue, secondOperandValue, operationResult;
            int counter = 1;
            Dictionary<string, string> results = new Dictionary<string, string>();

            Stack myStack = new Stack();
            Queue<char> postfixEpr = new Queue<char>();

            foreach (char c in expression)
            {
                postfixEpr.Enqueue(c);
            }

            while (postfixEpr.Count > 0)
            {
                x = postfixEpr.Dequeue();

                if (!operators.Contains(x))
                {
                    myStack.Push(x);
                }
                else
                {
                    if (x.Equals('#'))
                    {
                        firstOperand = myStack.Pop().ToString();

                        firstOperandValue = terms.ContainsKey(firstOperand) ? terms[firstOperand] : firstOperand;

                        operationResult = Sqrt(firstOperandValue);
                        results["operation " + counter] = "sqrt(" + firstOperand + ") =" + operationResult;
                    }
                    else
                    {
                        secondOperand = myStack.Pop().ToString();
                        firstOperand = myStack.Pop().ToString();

                        firstOperandValue = terms.ContainsKey(firstOperand) ? terms[firstOperand] : firstOperand;
                        secondOperandValue = terms.ContainsKey(secondOperand) ? terms[secondOperand] : secondOperand;

                        operationResult = ComputeOperation(firstOperandValue, secondOperandValue, x);
                        //Console.WriteLine("Operation " + counter + ":" + firstOperand + x + secondOperand + " = " + operationResult);
                        results["operation " + counter] = firstOperand + " " + x + " " + secondOperand + " = " + operationResult;
                    }
                    myStack.Push(operationResult);
                    counter++;
                }
            }
            var finalResult = myStack.Pop().ToString();
            results["final result"] = finalResult;
            return results;
        }

        private string ComputeOperation(string firstOperand, string secondOperand, char operatorType)
        {
            string result = "0";

            switch (operatorType)
            {
                case '+':
                    result = Sum(firstOperand, secondOperand);
                    break;
                case '*':
                    //result = Mul(firstOperand, secondOperand);
                    break;
                case '/':
                    //result = Div(firstOperand, secondOperand);
                    break;
                case '-':
                    result = Diff(firstOperand, secondOperand);
                    break;
                case '^':
                    result = Pow(firstOperand, secondOperand);
                    break;
            }
            return result;
        }

        private static bool IsSmaller(string str1, string str2)
        {
            int n1 = str1.Length, n2 = str2.Length;
            if (n1 < n2)
                return true;
            if (n2 < n1)
                return false;

            for (int i = 0; i < n1; i++)
                if (str1[i] < str2[i])
                    return true;
                else if (str1[i] > str2[i])
                    return false;

            return false;
        }

        private static bool IsSmallerOrEqual(string str1, string str2)
        {
            int n1 = str1.Length, n2 = str2.Length;
            if (n1 < n2)
                return true;
            if (n2 < n1)
                return false;

            for (int i = 0; i < n1; i++)
                if (str1[i] < str2[i])
                    return true;
                else if (str1[i] > str2[i])
                    return false;

            return true;
        }

        private static bool AreEqual(string str1, string str2)
        {
            int n1 = str1.Length, n2 = str2.Length;

            if (n1 < n2 || n2 < n1)
                return false;

            for (int i = 0; i < n1; i++)
                if (str1[i] < str2[i] || str1[i] > str2[i])
                    return false;

            return true;
        }

        public string Sum(string a, string b)
        {
            var sum = new StringBuilder();

            int carry = 0;

            if (a.Length != b.Length)
            {
                var maxLength = Math.Max(a.Length, b.Length);
                a = a.PadLeft(maxLength, '0');
                b = b.PadLeft(maxLength, '0');
            }

            for (int i = a.Length - 1; i >= 0; i--)
            {
                var digitSum = (a[i] - '0') + (b[i] - '0') + carry;

                if (digitSum > 9)
                {
                    carry = 1;
                    digitSum -= 10;
                }
                else
                {
                    carry = 0;
                }

                sum.Insert(0, digitSum);
            }

            if (carry == 1)
                sum.Insert(0, carry);

            return sum.ToString();
        }

        public string Mul(int[] a, int[] b)
        {
            int len_a = a.Length;
            int len_b = b.Length;
            if (a[0] == 0 || b[0] == 0)
                return "0";

            int[] result = new int[a.Length + b.Length];

            int last_a = 0;
            int last_b, i;

            for (i = len_a - 1; i >= 0; i--)
            {
                int carry = 0;
                int n1 = a[i];
                last_b = 0;

                for (int j = len_b - 1; j >= 0; j--)
                {
                    int n2 = b[j];

                    int sum = n1 * n2 + result[last_a + last_b] + carry;

                    carry = sum / 10;

                    result[last_a + last_b] = sum % 10;

                    last_b++;
                }

                if (carry > 0)
                    result[last_a + last_b] += carry;

                last_a++;
            }

            i = result.Length - 1;
            while (i >= 0 && result[i] == 0)
                i--;

            if (i == -1)
                return "0";

            String s = "";

            while (i >= 0)
                s += (result[i--]);

            return s;
        }

        public string Diff(string a, string b)
        {
            if (IsSmaller(a, b))
                return "-1";

            string result = "";

            int len_a = a.Length;
            int len_b = b.Length;
            int diff = len_a - len_b;

            int carry = 0;

            for (int i = len_b - 1; i >= 0; i--)
            {
                int sub = (((int)a[i + diff] - (int)'0')
                           - ((int)b[i] - (int)'0') - carry);
                if (sub < 0)
                {
                    sub = sub + 10;
                    carry = 1;
                }
                else
                    carry = 0;

                result += sub.ToString();
            }

            for (int i = len_a - len_b - 1; i >= 0; i--)
            {
                if (a[i] == '0' && carry > 0)
                {
                    result += "9";
                    continue;
                }
                int sub = (((int)a[i] - (int)'0') - carry);
                if (i > 0 || sub > 0)
                    result += sub.ToString();
                carry = 0;
            }

            char[] aa = result.ToCharArray();
            Array.Reverse(aa);
            var diffResult = new string(aa).TrimStart('0');
            return diffResult;
        }

        public List<int> FromDecimalToBinary(int[] a)
        {
            var bin = new List<int>();
            var prev = new int[100];
            var twoPows = new List<string>();
            twoPows.Add("1");
            var b = new int[] { 1 };
            var two = new int[] { 2 };

            if ((a.Length == 1 && a[0] == 0) || a.Length == 0)
            {
                bin.Add(0);

                return bin;
            }

            while (IsSmallerOrEqual(string.Join(string.Empty, b), string.Join(string.Empty, a)))
            {
                prev = b;
                var res = Mul(b, two);
                twoPows.Add(res);
                b = Array.ConvertAll(res.ToCharArray(), c => (int) Char.GetNumericValue(c));
            }

            twoPows.RemoveAt(twoPows.Count - 1);
            twoPows.Reverse();

            foreach (var pow in twoPows)
            {
                var aStr = string.Join(string.Empty, a);

                if (IsSmallerOrEqual(pow, aStr))
                {
                    var res = Diff(string.Join(string.Empty, a), pow);
                    a = Array.ConvertAll(res.ToCharArray(), c => (int) Char.GetNumericValue(c));
                    bin.Add(1);
                }
                else
                {
                    bin.Add(0);
                }
            }

            return bin;
        }

        public List<int> FromBinaryToDecimal(List<int> a)
        {
            var pow = new[] { 1 };
            var two = new[] { 2 };
            var result = new[] { 0 };
            var aReverse = a.ToArray().Reverse();

            foreach (var bit in aReverse)
            {
                if (bit == 1)
                {
                    var res = Sum(string.Join(string.Empty, result), string.Join(string.Empty, pow));
                    result = Array.ConvertAll(res.ToCharArray(), c => (int) Char.GetNumericValue(c));
                }

                var powStr = Mul(pow, two);
                pow = Array.ConvertAll(powStr.ToCharArray(), c => (int)Char.GetNumericValue(c));
            }

            return result.ToList();
        }

        public int[] Div2(int[] a, int[] b)
        {
            var denom = b.ToList();
            var current = new List<int>();
            var answer = new List<int>();
            current.Add(1);
            answer.Add(0);

            if (IsSmaller(string.Join(string.Empty, a), string.Join(string.Empty, b)))
            {
                return new[] { 0 };
            }

            if (AreEqual(string.Join(string.Empty, a), string.Join(string.Empty, b)))
            {
                return new[] { 1 };
            }

            var denomBin = FromDecimalToBinary(denom.ToArray());
            var currentBin = FromDecimalToBinary(current.ToArray());

            while (IsSmallerOrEqual(string.Join(string.Empty, denom), string.Join(string.Empty, a)))
            {
                denomBin.Add(0);
                currentBin.Add(0);

                denom = FromBinaryToDecimal(denomBin);
            }
            current = FromBinaryToDecimal(currentBin);

            denomBin.RemoveAt(denomBin.Count - 1);
            currentBin.RemoveAt(currentBin.Count - 1);

            current = FromBinaryToDecimal(currentBin);
            denom = FromBinaryToDecimal(denomBin);
            var answerBin = FromDecimalToBinary(answer.ToArray());

            while (currentBin.Count > 0)
            {
                if (IsSmallerOrEqual(string.Join(string.Empty, denom), string.Join(string.Empty, a)))
                {
                    var res = Diff(string.Join(string.Empty, a), string.Join(string.Empty, denom));
                    a = Array.ConvertAll(res.ToCharArray(), c => (int) Char.GetNumericValue(c));

                    //currentBin = ConvertToBinary(current.ToArray());

                    answerBin = BinaryOr(answerBin, currentBin);
                }

                answer = FromBinaryToDecimal(answerBin);
                denomBin.RemoveAt(denomBin.Count - 1);
                currentBin.RemoveAt(currentBin.Count - 1);

                current = FromBinaryToDecimal(currentBin);
                denom = FromBinaryToDecimal(denomBin);
            }

            return FromBinaryToDecimal(answerBin).ToArray();
        }

        public List<int> BinaryOr(List<int> a, List<int> b)
        {
            var len = a.Count > b.Count ? b.Count : a.Count;
            int i = 0;

            var invA = a.ToArray().Reverse().ToList();
            var invB = b.ToArray().Reverse().ToList();

            for (i = 0; i < len; i++)
            {
                invA[i] = invA[i] | invB[i];
            }

            if (a.Count < b.Count)
            {
                while (i < b.Count)
                {
                    invA.Add(invB[i]);
                    i++;
                }
            }

            return invA.ToArray().Reverse().ToList();
        }

        public string Div(int[] a, int[] b)
        {
            int b_int = 0;
            b_int = b.Aggregate((result, x) => result * 10 + x);
            string res = "";
            int idx = 0;
            int temp = a[idx];
            while (temp < b_int)
            {
                temp = temp * 10 + a[idx + 1];
                idx++;
            }
            ++idx;

            while (a.Length > idx)
            {
                res += (char)(temp / b_int + '0');

                temp = (temp % b_int) * 10 + a[idx];
                idx++;
            }
            res += (char)(temp / b_int + '0');

            if (res.Length == 0)
                return "-1";

            return res;
        }

        public string Pow(string a, string b)
        {
            int a_int = int.Parse(a);
            int b_int = int.Parse(b);
            int[] res = new int[99999999];
            int i = 1, k = 1, j;
            res[1] = 1;

            while (i <= b_int)
            {
                for (j = 1; j <= k; j++)
                    res[j] = res[j] * a_int;
                for (j = 1; j < k; j++)
                {
                    res[j + 1] = res[j + 1] + res[j] / 10;
                    res[j] = res[j] % 10;
                }
                while (res[j] > 9)
                {
                    k++;
                    res[k] = res[k] + res[j] / 10;
                    res[k - 1] = res[k - 1] % 10;
                    j++;
                }
                i++;
            }

            string s = "";
            for (i = k; i >= 1; i--)
                s += res[i];

            return s;
        }

        public string Sqrt(string a)
        {
            int a_int = int.Parse(a);
            int x = a_int;
            while (true)
            {
                int y = (x + a_int / x) / 2;
                if (y >= x)
                {
                    return x.ToString();
                }
                x = y;
            }
        }

    }
}