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

                        operationResult = Sqrt(ConvertStringToIntArray(firstOperandValue));
                        results["operation " + counter] = "sqrt(" + firstOperand + ") =" + operationResult;
                    }
                    else
                    {
                        secondOperand = myStack.Pop().ToString();
                        firstOperand = myStack.Pop().ToString();

                        firstOperandValue = terms.ContainsKey(firstOperand) ? terms[firstOperand] : firstOperand;
                        secondOperandValue = terms.ContainsKey(secondOperand) ? terms[secondOperand] : secondOperand;

                        operationResult = ComputeOperation(firstOperandValue, secondOperandValue, x);
                        if (operationResult.Equals("-1"))
                        {
                            results["error"] = "Negative result of subsctraction: " + firstOperand + " " + x + " " + secondOperand;
                            return results;
                        }
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

            int[] operand1 = ConvertStringToIntArray(firstOperand);
            int[] operand2 = ConvertStringToIntArray(secondOperand);

            switch (operatorType)
            {
                case '+':
                    result = Sum(operand1, operand2);
                    break;
                case '*':
                    result = Mul(operand1, operand2);
                    break;
                case '/':
                    result = Div(operand1, operand2);
                    break;
                case '-':
                    result = Diff(operand1, operand2);
                    break;
                case '^':
                    result = Pow(operand1, operand2);
                    break;
            }
            return result;
        }

        private static bool IsSmaller(int[] a, int[] b)
        {
            int n1 = a.Length, n2 = b.Length;
            if (n1 < n2)
                return true;
            if (n2 < n1)
                return false;

            for (int i = 0; i < n1; i++)
                if (a[i] < b[i])
                    return true;
                else if (a[i] > b[i])
                    return false;

            return false;
        }

        private static int[] ConvertStringToIntArray(string x)
        {
            int[] result = new int[] { 0 };

            if(!String.IsNullOrEmpty(x))
            {
                result = Array.ConvertAll(x.ToCharArray(), c => (int)Char.GetNumericValue(c));
            }

            return result;
        }

        /*private static int[] ConvertIntToIntArray(int number)
        {
            int[] result = number.ToString().Select(c => (int)Char.GetNumericValue(c)).ToArray();

            return result;
        }*/

        public string Sum(int[] a, int[] b)
        {
            Array.Reverse(a);
            Array.Reverse(b);

            int[] result = new int[a.Length + b.Length];

            int maxLength = Math.Max(a.Length, b.Length);

            for (int i = 0; i < maxLength; i++)
            {
                int lhs = (i < a.Length) ? a[i] : 0;
                int rhs = (i < b.Length) ? b[i] : 0;

                int sum = result[i] + lhs + rhs;
                result[i] = sum % 10;

                int carry = sum / 10;
                result[i + 1] = result[i + 1] + carry;
            }

            int j = result.Length - 1;
            while (j >= 0 && result[j] == 0)
                j--;

            if (j == -1)
                return "0";
            String s = "";

            while (j >= 0)
                s += (result[j--]);

            return s;
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

        public string Diff(int[] a, int[] b)
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
                int sub = a[i + diff] - b[i] - carry;
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
                int sub = (int)a[i] - carry;
                if (i > 0 || sub > 0)
                    result += sub.ToString();
                carry = 0;
            }

            char[] aa = result.ToCharArray();
            Array.Reverse(aa);
            return new string(aa).TrimStart('0');
        }

        public string Div(int[] a, int[] b)
        {
            int b_int = 0;
            b_int = b.Aggregate((result, x) => result * 10 + x); ;
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

        public string Pow(int[] a, int[] b)
        {
            string[] res = new string[99999999];
            int[] i = new int[b.Length];
            int j, k = 1;

            res[1] = "1";

            while (IsSmaller(i, b))
            {
                for (j = 1; j <= k; j++)
                {
                    var operand = ConvertStringToIntArray(res[j].ToString());
                    res[j] = Mul(operand, a);
                }

                for (j = 1; j < k; j++)
                {
                    var operand1 = ConvertStringToIntArray(res[j + 1]);
                    var operand2 = DivideBy10(res[j]);
                    res[j + 1] = Sum(operand1, ConvertStringToIntArray(operand2));
                    res[j] = Modulo10(res[j]);
                }
                while (res[j].Length > 1)
                {
                    k++;
                    var operand1 = ConvertStringToIntArray(res[k]);
                    var operand2 = DivideBy10(res[j]);
                    res[k] = Sum(operand1, ConvertStringToIntArray(operand2));
                    res[k - 1] = Modulo10(res[k - 1]);
                    j++;
                }
                i = ConvertStringToIntArray(Sum(i, new int[] { 1 }));
            }
            string result = "";
            for (j = k; j >= 1; j--)
                result += res[j];

            return result;
        }

        private static string DivideBy10(string number)
        {
            return number.Remove(number.Length - 1, 1);
        }

        private static string Modulo10(string number)
        {
            return (number[number.Length - 1]).ToString();
        }

        public string Sqrt(int[] a)
        {
            int[] res = new int[] { 0 };
            int[] one = new int[] { 1 };
            while (IsSmaller(ConvertStringToIntArray(Mul(res, res)), a))
            {
                string result = Sum(res, one);
                res = ConvertStringToIntArray(result);
            }
            if (Mul(res, res).Equals(string.Join("", a)))
                return string.Join("", res);
            return Diff(res, one);
        }

    }
}