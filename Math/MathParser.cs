/*
This file licenesed under The GNU General Public License (GPLv3)
Source: http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp
*/

using System;
using System.Collections.Generic;
using System.Collections;

namespace Unitilities.Utils
{
    public class MathParser
    {
		public enum Variables
		{
			A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
		}

		private Dictionary<Variables, double> _Parameters = new Dictionary<Variables, double>();
        private List<String> OperationOrder = new List<string>();

		public Dictionary<Variables, double> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        public MathParser()
        {
			OperationOrder.Add("^");
			OperationOrder.Add("/");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");
		}
		public double Calculate(string Formula)
        {
            try
            {
				string[] arr = Formula.Split("^/+-*()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (KeyValuePair<Variables, double> de in _Parameters)
                {
                    foreach(string s in arr)
                    {
                        if (s != de.Key.ToString() && s.EndsWith(de.Key.ToString()))
                        {
                            Formula = Formula.Replace(s, (Convert.ToDouble(s.Replace(de.Key.ToString(), "")) * de.Value).ToString());
                        }
                    }
					Formula = Formula.Replace(de.Key.ToString(), System.Math.Min(System.Math.Max(de.Value,0.0001), 10000.0).ToString());
                }
                while (Formula.LastIndexOf("(") > -1)
                {
                    int lastOpenPhrantesisIndex = Formula.LastIndexOf("(");
                    int firstClosePhrantesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenPhrantesisIndex);
					double result = ProcessOperation(Formula.Substring(lastOpenPhrantesisIndex + 1, firstClosePhrantesisIndexAfterLastOpened - lastOpenPhrantesisIndex - 1));
                    bool AppendAsterix = false;
                    if (lastOpenPhrantesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenPhrantesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenPhrantesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenPhrantesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstClosePhrantesisIndexAfterLastOpened + 1);

                }
                return ProcessOperation(Formula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured While Calculating '"+Formula+"'.", ex);
            }
        }

		private double ProcessOperation(string operation)
        {
			ArrayList arr = new ArrayList();
			string s = "";
			for (int i = 0; i < operation.Length; i++)
			{
				string currentCharacter = operation.Substring(i, 1);
				if (OperationOrder.IndexOf(currentCharacter) > -1)
				{
					if (s != "")
					{
						arr.Add(s);
					}
					arr.Add(currentCharacter);
					s = "";
				}
				else
				{
					s += currentCharacter;
				}
			}
			arr.Add(s);
			s = "";
			foreach (string op in OperationOrder)
			{

				while (arr.IndexOf(op) > -1)
				{
					int operatorIndex = arr.IndexOf(op);
					bool unaryOp = (operatorIndex <= 0);
					try {
						double digitBeforeOperator = unaryOp ? 0.0 : Convert.ToDouble(arr[operatorIndex - 1]);
						double digitAfterOperator = 0;
						if (arr[operatorIndex + 1].ToString() == "-")
						{
							arr.RemoveAt(operatorIndex + 1);
							digitAfterOperator = Convert.ToDouble(arr[operatorIndex + 1]) * -1;
						}
						else
						{
							digitAfterOperator = Convert.ToDouble(arr[operatorIndex + 1]);
						}
						arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
						if (!unaryOp) arr.RemoveAt(operatorIndex - 1);
						arr.RemoveAt(operatorIndex);
					} catch (Exception e) {
						throw new Exception("Error Occured While Processing Operation["+(operatorIndex-1)+"]: "+operation+" index "+arr[operatorIndex - 1], e);
					}
				}

			}
			return Convert.ToDouble(arr[0]);

        }
		public static double CalculateByOperator(double number1, double number2, string op)
        {
			if (op == "^")
			{
				return System.Math.Pow( number1, number2 );
			}
			else if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else
            {
                return 0;
            }
        }
    }
}
