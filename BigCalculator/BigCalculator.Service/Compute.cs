﻿using System.Text;

namespace BigCalculator.Service
{
    public class Compute : ICompute
    {
        public int ComputeCalculus(int a, int b)
        {
            return a + b;
        }

        static bool isSmaller(string str1, string str2)
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

        static int toInt(string a)
        {
            int i, res = 0;
            for (i = 0; i < a.Length; i++)
                res = 10 * res + a[i];

            return res;
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

        public string Mul(string a, string b)
        {
            int len_a = a.Length;
            int len_b = b.Length;
            if (len_a == 0 || len_b == 0)
                return "0";

            int[] result = new int[len_a + len_b];

            int last_a = 0;
            int last_b = 0;
            int i;

            for (i = len_a - 1; i >= 0; i--)
            {
                int carry = 0;
                int n1 = a[i] - '0';

                last_b = 0;
          
                for (int j = len_b - 1; j >= 0; j--)
                {
                    int n2 = b[j] - '0';

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
            if (isSmaller(a, b))
                return "-1";

            String result = "";

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
            return new string(aa);
        }

        public string Div(string a, int b)  // couldn't make it work to divide by string
        {

            string res = "";

            int idx = 0;
            int temp = (int)(a[idx] - '0');
            while (temp < b)
            {
                temp = temp * 10 + (int)(a[idx + 1] - '0');
                idx++;
            }
            ++idx;

            while (a.Length > idx)
            {
                res += (char)(temp / b + '0');

                temp = (temp % b) * 10 + (int)(a[idx] - '0');
                idx++;
            }
            res += (char)(temp / b + '0');

            if (res.Length == 0)
                return "-1";

            return res;

        }

        public string Pow(string a, string b) // does't work 
        {
            int a_int = toInt(a);
            int b_int = toInt(b);
            int[] res = new int[99999999];
            int i=1, k=1, j;
            res[1] = 1;

            while(i<= b_int)
            {
                for (j = 1; j <= k; j++)
                    res[j] = res[j] * a_int;
                for (j = 1; j < k; j++)
                {
                    res[j + 1] = res[j + 1] + res[j] / 10;
                    res[j] = res[j] % 10;
                }
                while( res[j] > 9)
                {
                    k++;
                    res[k] = res[k] + res[j] / 10;
                    res[k - 1] = res[k - 1] % 10;
                    j++;
                }
                i++;
            }

            string s = "";
            for (i = 0; i < res.Length; i++)
                s += res[i];

            return s;
        }


    }
}