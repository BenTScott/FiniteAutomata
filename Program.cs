using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularExpressionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 100;
            Console.WriteLine("Use capital U for union and ALT+157 for Ø and only use lower case a-z for elements of sigma");
            Console.Write("Please enter a valid regular expression: ");
            string regExp = Console.ReadLine();
            RegularExpression rgRegExp = new RegularExpression(regExp);

            NFiniteAutomata nfa = FillNFA();

            Console.WriteLine("b");
            Console.WriteLine(nfa.CheckAcceptance("b"));
            Console.WriteLine("");
            Console.WriteLine("ab");
            Console.WriteLine(nfa.CheckAcceptance("ab"));
            Console.WriteLine("");
            Console.WriteLine("aab");
            Console.WriteLine(nfa.CheckAcceptance("aab"));
            Console.WriteLine("");
            Console.WriteLine("aaa");
            Console.WriteLine(nfa.CheckAcceptance("aaa"));
            Console.WriteLine("");
            Console.WriteLine("aaab");
            Console.WriteLine(nfa.CheckAcceptance("aaab"));
            Console.WriteLine("");
            Console.WriteLine("aaaa");
            Console.WriteLine(nfa.CheckAcceptance("aaaa"));

            Console.WriteLine("");
            Console.WriteLine("aaaaaaaaa");
            Console.WriteLine(nfa.CheckAcceptance("aaaaaaaaa"));
            Console.WriteLine("");

            Console.WriteLine("aaaaaaaaabbbbb");
            Console.WriteLine(nfa.CheckAcceptance("aaaaaaaaabbbbb"));

            Console.WriteLine("");
            Console.WriteLine("aaaabbbaab");
            Console.WriteLine(nfa.CheckAcceptance("aaaabbbaab"));
            Console.WriteLine("");

            Console.WriteLine("aaaabbbab");
            Console.WriteLine(nfa.CheckAcceptance("aaaabbbab"));


            Console.WriteLine("");




            Console.WriteLine("");
            string answer;
            Console.Write("Enter a word: ");
            answer = Console.ReadLine();

            while(answer!="END")
            {
                Console.WriteLine(nfa.CheckAcceptance(answer));
                Console.Write("Enter a word: ");
                answer = Console.ReadLine();
            }

            Console.Write("\nPress enter to close...");
            Console.ReadLine();
        }

        static private NFiniteAutomata FillNFA()
        {
            int noOfStates = 1;
            State[] states;
            bool valid = false;
            bool moreToAdd = true;
            int noOfLetters = 1;
            char letter;
            char[] alphabet;
            int noOfFinalStates = 1;
            int[] finalStates;
            int count = 0;

            noOfLetters = GetInteger("How many letters in the alphabet: ", 0, 25);

            alphabet = new char[noOfLetters];

            while (count <= noOfLetters - 1)
            {
                letter = GetLetter("Enter a letter of the alphabet (Lowercase and not e): ", false);
                if (alphabet.SubArray(0, count).Contains(letter))
                {
                    Console.WriteLine("That letter is already chosen");
                }
                else
                {
                    alphabet[count] = letter;
                    count += 1;
                }
            }

            noOfStates = GetInteger("How many states are there?: ", 1);

            states = new State[noOfStates];

            for (int i = 0; i <= noOfStates - 1; i++)
            {
                states[i] = new State(i);
            }

            for (int i = 0; i <= noOfStates - 1; i++)
            {

                if (i == 0)
                {
                    Console.WriteLine("State 0 (Initial State) -");
                }
                else
                {
                    Console.WriteLine("State {0:d} -", i);
                }
                moreToAdd = true;
                while (moreToAdd)
                {

                    letter = GetLetter("\tPlease enter the letter of an arrow from this state (END to finsh this state): ", true, "END", alphabet);

                    if (letter == 'E')
                    {
                        break;
                    }
                    else
                    {
                        states[i].Add(letter, states[GetInteger("\tWhat is the number of the state this should lead to? (States are indexed from 0): ", 0, states.GetUpperBound(0))]);
                    }

                }

            }



            noOfFinalStates = GetInteger("How many final states?: ", 0, noOfStates);


            finalStates = new int[noOfFinalStates];
            int state;

            for (int i = 0; i <= noOfFinalStates - 1; i++)
            {
                valid = false;
                do
                {
                    state = GetInteger("Enter a final state: ", 0, noOfStates - 1);

                    if ((i == 0) || (i > 0 && !finalStates.SubArray(0, i).Contains(state)))
                    {
                        finalStates[i] = state;
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                        Console.WriteLine("That is not a valid input, please try again!");
                    }
                } while (!valid);

            }


            return new NFiniteAutomata(states, alphabet, finalStates);

        }



        static char GetLetter(string prompt, bool eAllowed, string endString = "N/A", char[] alphabet = null)
        {

            bool valid = false;
            char letter = 'E';
            string answer;
            bool alphabetRequired = true;

            if (alphabet == null)
            {
                alphabetRequired = false;
            }

            do
            {
                try
                {
                    Console.Write(prompt);
                    answer = Console.ReadLine();

                    if (endString != "N/A" && answer == endString)
                    {
                        return 'E';
                    }

                    letter = Convert.ToChar(answer);
                    if (alphabetRequired == false)
                    {
                        if ((Char.IsLower(letter) && letter != 'e') || (eAllowed && letter == 'e'))
                        {
                            valid = true;
                        }
                        else
                        {
                            valid = false;
                            Console.WriteLine("That is not a valid input, please try again!");
                        }
                    }
                    else
                    {
                        if ((Char.IsLower(letter) && letter != 'e' && alphabet.Contains(letter)) || (eAllowed && letter == 'e'))
                        {
                            valid = true;
                        }
                        else
                        {
                            valid = false;
                            Console.WriteLine("That is not a valid input, please try again!");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("That is not a valid input, please try again!");
                }

            } while (!valid);

            return letter;

        }

        static int GetInteger(string prompt, int min, int max = int.MaxValue)
        {
            bool valid;
            int returnInt = 0;
            do
            {
                try
                {
                    Console.Write(prompt);
                    returnInt = Convert.ToInt32(Console.ReadLine());
                    if (returnInt <= max && returnInt >= min)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid input, please try again!");
                        valid = false;
                    }
                }
                catch
                {
                    Console.WriteLine("That is not a valid input, please try again!");
                    valid = false;
                }
            } while (!valid);

            return returnInt;
        }




    }

    public class RegularExpression
    {
        public List<RegularExpression> RegExpList;
        public string Op;

        public RegularExpression(string regExp)
        {
            RegExpList = new List<RegularExpression>();
            RegularExpression rgRegExp = null;
            Op = "";
            int charIndex = 0;
            while (charIndex <= regExp.Length - 1)
            {
                char letter = regExp[charIndex];
                switch (letter)
                {

                    case '(':
                        int closingBracketIndex = _FindBracketCounterPart(regExp, charIndex);
                        if (RegExpList.Count == 0)
                        {
                            if ((closingBracketIndex + 2 <= regExp.Length) && (regExp[closingBracketIndex + 1] == '*'))
                            {
                                if (closingBracketIndex + 3 <= regExp.Length)
                                {
                                    RegExpList.Add(new RegularExpression(regExp.Substring(charIndex, closingBracketIndex - charIndex + 1)));
                                }
                                else
                                {
                                    Op = "*";
                                    RegExpList.Add(new RegularExpression(regExp.Substring(charIndex + 1, closingBracketIndex - charIndex - 1)));
                                }
                                charIndex = closingBracketIndex + 2;
                            }
                            else
                            {
                                RegExpList.Add(new RegularExpression(regExp.Substring(charIndex + 1, closingBracketIndex - charIndex - 1)));
                                charIndex = closingBracketIndex + 1;
                            }

                        }
                        else
                        {
                            if ((closingBracketIndex + 2 <= regExp.Length) && (regExp[closingBracketIndex + 1] == '*'))
                            {
                                rgRegExp = new RegularExpression(regExp.Substring(charIndex, closingBracketIndex - charIndex + 1));
                                if (Op == "U" && regExp[charIndex - 1] != 'U')
                                {
                                    throw new System.ArgumentException("FAIL");
                                }
                                charIndex = closingBracketIndex + 2;
                            }
                            else
                            {
                                rgRegExp = new RegularExpression(regExp.Substring(charIndex + 1, closingBracketIndex - charIndex - 1));
                                if (Op == "U" && regExp[charIndex - 1] != 'U')
                                {
                                    throw new System.ArgumentException("FAIL");
                                }
                                charIndex = closingBracketIndex + 1;
                            }

                            if (Op == "C" || Op == "U" || Op == "")
                            {
                                if (Op == "")
                                {
                                    Op = "C";
                                }
                                RegExpList.Add(rgRegExp);
                                rgRegExp = null;
                            }
                            else
                            {
                                throw new System.ArgumentException("FAIL");
                            }
                        }

                        break;

                    case 'U':
                        if ((Op == "U" || Op == "") && regExp.Length != 0 && regExp.Length != charIndex)
                        {
                            Op = "U";
                            charIndex += 1;
                        }
                        else
                        {
                            throw new System.ArgumentException("FAIL");
                        }
                        break;

                    default:

                        if (letter == 'Ø' || char.IsLower(letter))
                        {
                            if (RegExpList.Count == 0)
                            {
                                if ((charIndex + 2 <= regExp.Length) && (regExp[charIndex + 1] == '*'))
                                {
                                    if (charIndex + 3 <= regExp.Length)
                                    {
                                        RegExpList.Add(new RegularExpression(regExp.Substring(charIndex, 2)));
                                    }
                                    else
                                    {
                                        Op = "*";
                                        RegExpList.Add(new RegularExpression(letter.ToString()));
                                    }
                                    charIndex += 2;
                                }
                                else
                                {
                                    if (regExp.Length == 1)
                                    {
                                        Op = letter.ToString();
                                    }
                                    else
                                    {
                                        RegExpList.Add(new RegularExpression(letter.ToString()));
                                    }

                                    charIndex += 1;
                                }

                            }
                            else
                            {
                                if ((charIndex + 2 <= regExp.Length) && (regExp[charIndex + 1] == '*'))
                                {
                                    rgRegExp = new RegularExpression(regExp.Substring(charIndex, 2));
                                    if (Op == "U" && regExp[charIndex - 1] != 'U')
                                    {
                                        throw new System.ArgumentException("FAIL");
                                    }
                                    charIndex += 2;
                                }
                                else
                                {
                                    rgRegExp = (new RegularExpression(letter.ToString()));
                                    if (Op == "U" && regExp[charIndex - 1] != 'U')
                                    {
                                        throw new System.ArgumentException("FAIL");
                                    }
                                    charIndex += 1;
                                }
                                if (Op == "C" || Op == "U" || Op == "")
                                {
                                    if (Op == "")
                                    {
                                        Op = "C";
                                    }
                                    RegExpList.Add(rgRegExp);
                                    rgRegExp = null;
                                }
                                else
                                {
                                    throw new System.ArgumentException("FAIL");
                                }
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("FAIL");
                        }

                        break;
                }
            }

        }

        /// <summary>
        /// Given a string and the index of a '(' in that string, the corresponding ')' will be found
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <returns>The index of the corresponding ')'</returns>
        private int _FindBracketCounterPart(string s, int index)
        {
            int bracketCount = 0;
            int closedBracketIndex = -1;
            for (int i = index; i <= s.Length; i++)
            {
                char letter = s[i];
                if (letter == '(')
                {
                    bracketCount++;
                }
                if (letter == ')')
                {
                    bracketCount--;
                }
                if (bracketCount == 0)
                {
                    closedBracketIndex = i;
                    break;
                }
            }
            return closedBracketIndex;
        }


    }

    class NFiniteAutomata
    {

        public State[] States;
        public char[] Alphabet;
        public int[] FinalStates;

        public NFiniteAutomata(State[] states, char[] alphabet, int[] finalstates)
        {
            States = states;
            Alphabet = alphabet;
            FinalStates = finalstates;
        }


        public int[] Delta(State state, char letter)
        {
            List<State> states = state.GetNewState(letter);
            if (states == null)
            {
                return null;
            }
            else
            {
                return states.GetStateNumbers();
            }
        }

        public bool CheckAcceptance(string word)
        {

            int[] states;

            for (int i = 0; i <= word.Length - 1; i++)
            {
                if (!Alphabet.Contains(word[i]))
                {
                    return false;
                }
            }




            states = Delta(States[0], 'e');
            if (states != null)
            {
                for (int i = 0; i <= states.GetUpperBound(0); i++)
                {
                    if (word.Length == 0)
                    {
                        if (FinalStates.Contains(states[i]))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (_CheckAcceptance(word, states[i])) return true;
                    }
                }
            }

            if (word.Length == 0)
            {
                if (FinalStates.Contains(0))
                {
                    return true;
                }
            }
            else
            {
                return _CheckAcceptance(word);
            }

            return false;
            

        }

        private bool _CheckAcceptance(string word, int startingStateIndex = 0)
        {

            int[] states;

            states = Delta(States[startingStateIndex], word[0]);
            if (states == null)
            {
                return false;
            }
            if (word.Length >= 2)
            {
                for (int i = 0; i <= states.GetUpperBound(0); i++)
                {
                    if (_CheckAcceptance(word.Substring(1), states[i])) return true;
                }
                return false;
            }
            else
            {
                for (int i = 0; i <= states.GetUpperBound(0); i++)
                {
                    if (FinalStates.Contains(states[i]))
                    {
                        return true;
                    }
                }
                return false;
            }

        }

    }


    public class State
    {
        public int StateNo;
        public MultiDictionary<char, State> Configurations;

        public State(int stateNumber)
        {
            StateNo = stateNumber;
            Configurations = new MultiDictionary<char, State>();
        }


        public void Add(char letter, State state)
        {
            Configurations.Add(letter, state);
        }

        public List<State> GetNewState(char letter)
        {
            if (Configurations.Keys.Contains(letter) == false)
            {
                return null;
            }
            else
            {
                
                List<State> intLttrList = Configurations[letter];

                List<State> finalList = new List<State>();

                for (int i = 0; i <= intLttrList.Count - 1; i++)
                {
                    finalList.Add(intLttrList[i]);
                }

                List<State> empWrdList;
                for (int i = 0; i <= intLttrList.Count-1; i++)
                {

                    empWrdList = GetEmptyWordChains(intLttrList[i]);
                    if (empWrdList != null)
                    {
                        finalList = finalList.Union(empWrdList).ToList();
                    }
                }

                return finalList;
            }
        }

        private List<State> GetEmptyWordChains(State state)
        {
            if (state.Configurations.Keys.Contains('e') == false)
            {
                return null;
            }
            else
            {
                List<State> stateList =  state.Configurations['e'];
                List<State> addedList = new List<State>();
                for (int i = 0; i <= stateList.Count - 1; i++)
                {
                    GetEmptyWordChains(stateList[i], addedList);
                }
                return addedList;
            }
        }

        private List<State> GetEmptyWordChains(State currentState, List<State> addedStates)
        {

            if (addedStates.GetStateNumbers().Contains(currentState.StateNo))
            {
                return null;
            }
            if (currentState.Configurations.Keys.Contains('e') != false)
            {
                List<State> stateList1;
                stateList1 = currentState.Configurations['e'];
                addedStates.Add(currentState);
                for (int i = 0; i <= stateList1.Count - 1; i++)
                {
                    GetEmptyWordChains(stateList1[i], addedStates);
                }
                
            }
            addedStates.Add(currentState);
            return addedStates;
        }


    }

    public class MultiDictionary<TKey, TVlaue>
        where TKey : System.IComparable
    {
        private List<TKey> keys;
        private List<TVlaue> values;

        public MultiDictionary()
        {
            keys = new List<TKey>();
            values = new List<TVlaue>();
        }

        public List<TKey> Keys
        {
            get
            {
                return keys;
            }

        }

        public List<TVlaue> Values
        {
            get
            {
                return values;
            }

        }

        public List<TVlaue> this[TKey key]
        {
            get
            {
                List<TVlaue> vals = new List<TVlaue>();

                for (int i = 0; i <= values.Count - 1; i++)
                {
                    if (key.CompareTo(keys[i]) == 0)
                    {
                        vals.Add(values[i]);
                    }
                }

                if (vals.Count == 0)
                {
                    throw new System.Collections.Generic.KeyNotFoundException("Key not found");
                }

                return vals;
            }

        }

        public void Add(TKey key, TVlaue value)
        {
            keys.Add(key);
            values.Add(value);
        }

    }

    public static class ClassExtensions
    {
        public static int[] GetStateNumbers(this List<State> m)
        {
            if (m == null)
            {
                throw new System.ArgumentNullException("State List", "List cannot be null");
            }
            int[] stateNos = new int[m.Count];
            for (int i = 0; i <= m.Count - 1; i++)
            {
                stateNos[i] = m[i].StateNo;
            }
            return stateNos;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }

}
