using DataBaseTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBaseTest
{
    public class Utilities
    {

        public const char PARAMS = '\uF71A';
        public const string COMPANY = "company";
        public const string DRIVER = "driver";
        public const string INTERNATIONAL = "international";
        public const string LOCAL_GOVERNMENT_ORIGIN = "local";
        public const string NATIONAL = "national";
        public const string VOTER = "voter";

        public const int MIN_BATCH_SIZE = 1;
        public const int MAX_BATCH_SIZE = 30;
        public const int ACCOUNT_NUM_LEN = 10;

        public const string Auth_1 = "Custom";
        public const string Auth_2 = "Custom1";
        public const string Auth_3 = "Custom2";
        public const string Auth_4 = "Custom3";
        public const string Auth_5 = "Custom4";
        public const string Auth_6 = "Custom5";
        public const string Policy1 = "Security";
        public const string Policy2 = "Maintenance";
        public const string Policy3 = "Operator";
        public const string Policy4 = "Administrator";
        public const string Policy5 = "User";

        public static ValueConverter<DateTime, DateTime> UtcConverter => new(toDb => toDb, fromDb => DateTime.SpecifyKind(fromDb, DateTimeKind.Utc));

        public static ValueConverter<List<string>, string> JsonStringConverter => new(toDb => Serialize(toDb), fromDb => Deserialize<List<string>>(fromDb), null);
        public static ValueConverter<Dictionary<string, string>, string> JsonStringConverter2 => new(toDb => Serialize(toDb), fromDb => Deserialize<Dictionary<string, string>>(fromDb), null);

        public static ValueConverter<TModel, string> JsonStringConverter3<TModel, TProvider>()
        {
            return new(model => Serialize(model), provider => Deserialize<TModel>(provider));
        }

        public static ValueConverter<ICurrency, string> JsonCurrencyConverter()
        {
            var val = new ValueConverter<ICurrency, string>(model => Serialize(model), provider => Deserialize<Currencies.MediumOfExchange>(provider));
            return val;
        }

        public static ValueComparer<List<TModel>> ListComparer<TModel>() => new((val1, val2) => val1.SequenceEqual(val2), val => Hash(val), val => val.ToList());
        public static ValueComparer<Queue<TModel>> QueueComparer<TModel> () => new((val1, val2) => val1.SequenceEqual(val2), val => Hash(val), val => val);
        public static ValueComparer<Dictionary<string, string>> DictionaryComparer => new((val1, val2) => IsEqual(val1, val2), val => Hash(val), val => val);
        public static ValueComparer<TModel> Comparer<TModel>() => new((val1, val2) => IsEqual(val1, val2), val => Hash(val), val => val);

        private static string Serialize<T>(T l)
        {
            return JsonSerializer.Serialize(l);
        }

        private static T Deserialize<T>(string s)
        {
            return JsonSerializer.Deserialize<T>(s);
        }

        public static string EmployeeIdDictionary
        {
            get => _idDict;
            private set { _idDict = value; }
        }

        private static string _idDict;

        public static string CustomerIdLookup
        {
            get => _idLookup;
            private set { _idLookup = value; }
        }

        private static string _idLookup;

        public static bool UpdateCustomerLookup(uint id, string proposedName)
        {
            Dictionary<uint, string> map;

            if (String.IsNullOrEmpty(CustomerIdLookup))
            {
                map = new Dictionary<uint, string>();
            }
            else
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(CustomerIdLookup));
                map = JsonSerializer.Deserialize<Dictionary<uint, string>>(decoded);
            }
            try
            {
                map.Add(id, proposedName);
            }
            catch
            {
                throw;
            }
            bool val = map.TryGetValue(id, out var s);

            var smap = JsonSerializer.Serialize(map);
            CustomerIdLookup = Convert.ToBase64String(Encoding.UTF8.GetBytes(smap));

            return val && s.Equals(proposedName);
        }

        public static bool UpdateLookup(uint id, string proposedName)
        {
            Dictionary<uint, string> map;

            if (String.IsNullOrEmpty(EmployeeIdDictionary))
            {
                map = new Dictionary<uint, string>();
            }
            else
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(EmployeeIdDictionary));
                //var decoded = ToString(Convert.FromBase64String(EmployeeIdDictionary));// System.Net.WebUtility.UrlDecode(EmployeeIdDictionary);
                map = JsonSerializer.Deserialize<Dictionary<uint, string>>(decoded);
            }
            try
            {
                map.Add(id, proposedName);
            }
            catch (Exception)
            {
                throw;
            }
            bool val = map.TryGetValue(id, out var s);

            var smap = JsonSerializer.Serialize(map);
            //Console.WriteLine(smap);
            EmployeeIdDictionary = Convert.ToBase64String(Encoding.UTF8.GetBytes(smap));// System.Net.WebUtility.UrlEncode(smap);

            return val && s.Equals(proposedName);
        }

        public static String ToString(byte[] b)
        {
            var sb = new StringBuilder();
            foreach (var item in b)
            {
                char c = Convert.ToChar(item);
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string that is the substring of the given string <paramref name="val"/>. The substring begins at the specified
        /// <c><paramref name="inclusiveBeginIndex"/></c> and extends to the character at index
        /// <c><paramref name="exclusiveEndIndex"/> - 1</c>. Thus the length of the substring is
        /// <c><paramref name="exclusiveEndIndex"/> - <paramref name="inclusiveBeginIndex"/></c>.
        /// <example>
        /// <code>
        /// Substring("hamburger", 4, 8) returns "urge"
        /// Substring("smiles", 1, 5) returns "mile"
        /// </code>
        /// </example>
        /// </summary>
        /// <remarks>
        /// This method was created because C# and .NET lacks the substring varaint of the JRE's string
        /// class and the code that uses this method was ported directly from java.
        /// </remarks>
        /// <param name="val">the string to be divided</param>
        /// <param name="inclusiveBeginIndex">the beginning index in the given string <paramref name="val"/>. This value is inclusive.</param>
        /// <param name="exclusiveEndIndex">the ending index in the given string <paramref name="val"/>. This value is exclusive.</param>
        /// <returns>the substring of <paramref name="val"/></returns>
        public static string Substring(string val, int inclusiveBeginIndex, int exclusiveEndIndex)
        {
            //return val.Substring(inclusiveBeginIndex, exclusiveEndIndex - inclusiveBeginIndex);
            return val[inclusiveBeginIndex..exclusiveEndIndex];
        }

        public static string SecretGen(DateTime d, object relatedObject, ref string secret)
        {
            if (secret.Length < 6 || secret.Length > 18)
                throw new ArgumentOutOfRangeException($"Password out of range: 6 \u2265 password Length =\u2264 18. Length: {secret.Length}");

            var secretPadding = new StringBuilder();
            secretPadding.Append(d.Month);
            secretPadding.Append(d.Day);
            secretPadding.Append($"{Convert.ToString(relatedObject.GetHashCode(), 16)}");
            secretPadding.Append(d.Year);
            secretPadding.Append($"{Convert.ToString(d.Millisecond | DateTime.Now.Millisecond, 16)}");

            var lengthOfStringInEachIndexInSecretAsList = new Random().Next(2, 7);
            var secretAsList = Split(ref secret, lengthOfStringInEachIndexInSecretAsList);

            var lengthOfStringInEachIndexInPaddingsAsList = new Random().Next(2, 4);
            /*Permanently converted!*/
            var paddings = Convert.ToBase64String(Encoding.UTF8.GetBytes(secretPadding.ToString()));
            var paddingsAsList = Split(ref paddings, lengthOfStringInEachIndexInPaddingsAsList);
            var lengthOfCharsAtLastIndexInPaddingsAsList = paddingsAsList[paddingsAsList.Count - 1].Length;
            var paddingsAsQueue = new Queue<String>(paddingsAsList);
            var isLeftToRight = new Random().Next(0, 1);//1 for true 0 for false
            var iteration = 0;// This is the length of the list representing the pure secret
            var lengthOfPaddingsAsList = paddingsAsQueue.Count;
            var isSecretLengthLongerThanPadding = false;
            var finalMixture = new List<String>();

            while (iteration < secretAsList.Count)
            {
                if (paddingsAsQueue.Count <= 0)
                    isSecretLengthLongerThanPadding = true;
                if (isLeftToRight == 1)
                {
                    if (!isSecretLengthLongerThanPadding)
                        finalMixture.Add(paddingsAsQueue.Dequeue());
                    finalMixture.Add(secretAsList[iteration++]);
                }
                else if (isLeftToRight == 0)
                {
                    if (!isSecretLengthLongerThanPadding)
                        finalMixture.Insert(0, paddingsAsQueue.Dequeue());
                    finalMixture.Insert(0, secretAsList[iteration++]);
                }
            }

            while (paddingsAsQueue.Count > 0)
                if (isLeftToRight == 1)
                    finalMixture.Add(paddingsAsQueue.Dequeue());
                else
                    finalMixture.Insert(0, paddingsAsQueue.Dequeue());

            finalMixture.Add(PARAMS.ToString());
            /*
             * Add all the parameters here
             */
            finalMixture.Add(lengthOfPaddingsAsList < 10 ? "0" + lengthOfPaddingsAsList : lengthOfPaddingsAsList.ToString());
            //Console.WriteLine("sbLength: " + lengthOfPaddingsAsList);
            finalMixture.Add(lengthOfCharsAtLastIndexInPaddingsAsList.ToString());
            //Console.WriteLine("lastIndexSplitSecretBuilder: " + lengthOfCharsAtLastIndexInPaddingsAsList);
            finalMixture.Add(secretAsList[^1].Length.ToString());
            //Console.WriteLine("lastIndexSecretSplitValue: " + secretAsList[^1].Length.ToString());
            finalMixture.Add(lengthOfStringInEachIndexInSecretAsList.ToString());
            //Console.WriteLine("secretSplitValue: " + lengthOfStringInEachIndexInSecretAsList);
            finalMixture.Add(lengthOfStringInEachIndexInPaddingsAsList.ToString());
            //Console.WriteLine("secretBuilderSplitValue: " + lengthOfStringInEachIndexInPaddingsAsList);
            finalMixture.Add(iteration.ToString());
            //Console.WriteLine("iteration: " + iteration);
            finalMixture.Add(isLeftToRight.ToString());
            //Console.WriteLine("isOrderly: " + isLeftToRight);
            finalMixture.Add(isSecretLengthLongerThanPadding ? "1" : "0");
            //Console.WriteLine("isOverflowing: " + isSecretLengthLongerThanPadding);
            //Console.WriteLine("List: " + ToString(finalMixture));
            secretPadding.Clear();
            foreach (String s in finalMixture)
                secretPadding.Append(s);
            //Temporal conversion
            //Console.WriteLine("before convert: " + secretPadding);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(secretPadding.ToString()));
        }

        public static async Task<string> SecretGen(string secret)
        {
            var random = new Random(secret.GetHashCode());
            int algorithm = random.Next(1, 50);
            string x = await Task.Run(() => SecretGenHelper(algorithm, ref secret));

            int algorithmPosition = random.Next(0, x.Length);
            return new StringBuilder(x)
                .Insert(algorithmPosition, algorithm)
                .Append(PARAMS)
                .Append(algorithmPosition)
                .Append(PARAMS)
                .Append(algorithm.ToString().Length)
                .ToString();
        }

        private static string SecretGenHelper(int algorithm, ref string secret)
        {
            string x = secret;
            for (int i = 0; i < algorithm; i++)
            {
                x = Convert.ToBase64String(Encoding.UTF8.GetBytes(x));
            }
            secret = null;
            return x;
        }

        private static List<string> Split(ref string val, int numOfPlaces)
        {
            var stringBuilder = new StringBuilder();
            var returnValue = new List<String>();
            for (int i = 0, j = 0; i < val.Length; i++, j++)
            {
                if (j >= numOfPlaces)
                {
                    returnValue.Add(stringBuilder.ToString());
                    stringBuilder = new StringBuilder();
                    j = 0;
                }
                var character = val[i];
                stringBuilder.Append(character);
            }
            if (stringBuilder.Length > 0)
                returnValue.Add(stringBuilder.ToString());
            val = null;
            return returnValue;
        }

        public static string Decrypt(ref string val)
        {
            var parameters = val[(val.IndexOf(PARAMS) + 1)..];//val.Substring(val.IndexOf(PARAMS) + 1)
            val = Substring(val, 0, val.IndexOf(PARAMS));//val.Substring(0, val.Length - parameters.Length - 1);//val.IndexOf(PARAMS));
            var lengthOfPaddingsAsList = Int32.Parse(Substring(parameters, 0, 2));
            //Int32.Parse(parameters.Substring(parameters.Length - 7).ToString());
            var lengthOfCharsAtLastIndexInPaddingsAsList = Int32.Parse(parameters[^7].ToString());
            var lengthOfCharsAtLastIndexInSecretAsList = Int32.Parse(parameters[^6].ToString());
            var lengthOfStringInEachIndexInSecretAsList = Int32.Parse(parameters[^5].ToString());
            var lengthOfStringInEachIndexInPaddingsAsList = Int32.Parse(parameters[^4].ToString());
            var iteration = Int32.Parse(parameters[^3].ToString());
            var isLeftToRight = parameters[^2] == '1';
            var isSecretLengthLongerThanPadding = parameters[^1] == '1';

            var secret = new StringBuilder();
            int x = 0, y = 0;

            if (isLeftToRight)
            {
                for (var i = 0; ; i++)
                {
                    if ((!isSecretLengthLongerThanPadding) && i >= iteration)
                        break;
                    else if (isSecretLengthLongerThanPadding && i >= lengthOfPaddingsAsList - 1)
                    {
                        x = ((lengthOfPaddingsAsList - 1) * lengthOfStringInEachIndexInPaddingsAsList) +
                            (i * lengthOfStringInEachIndexInSecretAsList) + lengthOfCharsAtLastIndexInPaddingsAsList;
                        //secret.Append(val.Substring(x, val.Length));
                        //secret.Append(val.Substring(x, lengthOfCharsAtLastIndexInPaddingsAsList));
                        secret.Append(Substring(val, x, val.Length));
                        break;
                    }

                    x = iteration == lengthOfPaddingsAsList && i == iteration - 1
                        ? val.Length - lengthOfCharsAtLastIndexInSecretAsList
                        : ((i + 1) * lengthOfStringInEachIndexInPaddingsAsList) + (i *
                            lengthOfStringInEachIndexInSecretAsList);
                    if (i == iteration - 1)
                    {
                        if (!isSecretLengthLongerThanPadding)
                            if (iteration == lengthOfPaddingsAsList)
                                y = val.Length;
                            else
                                y = x + lengthOfCharsAtLastIndexInSecretAsList;
                    }
                    else
                        y = x + lengthOfStringInEachIndexInSecretAsList;
                    //secret.Append(val.Substring(x, y));
                    //secret.Append(val.Substring(x, lengthOfStringInEachIndexInSecretAsList));
                    secret.Append(Substring(val, x, y));
                }
            }
            else
            {
                for (var i = 0; ; i++)
                {
                    if (i >= iteration && !isSecretLengthLongerThanPadding)
                        break;
                    else if (isSecretLengthLongerThanPadding && i >= lengthOfPaddingsAsList - 1)
                    {
                        y = val.Length - (((lengthOfPaddingsAsList - 1) * lengthOfStringInEachIndexInPaddingsAsList)
                            + (i * lengthOfStringInEachIndexInSecretAsList));
                        y = y - lengthOfCharsAtLastIndexInPaddingsAsList;
                        //var remainder = val.Substring(0, y);
                        //var remainder = val.Substring(0, lengthOfCharsAtLastIndexInPaddingsAsList);
                        var remainder = Substring(val, 0, y);
                        int ii = remainder.Length / lengthOfStringInEachIndexInSecretAsList, y1 = remainder.Length,
                            x1 = remainder.Length, iii = 0;
                        if (ii < 1)
                        {
                            secret.Append(remainder);
                            break;
                        }
                        while (true)
                        {
                            x1 -= lengthOfStringInEachIndexInSecretAsList;
                            //secret.Append(remainder.Substring(x1 < 0 ? 0 : x1, y1));
                            //secret.Append(remainder.Substring(x1 < 0 ? 0 : x1, lengthOfStringInEachIndexInSecretAsList));
                            secret.Append(Substring(remainder, x1 < 0 ? 0 : x1, y1));
                            y1 -= lengthOfStringInEachIndexInSecretAsList;
                            if (iii >= ii)
                            {
                                if ((double)remainder.Length / (double)lengthOfStringInEachIndexInSecretAsList > (double)ii * lengthOfStringInEachIndexInSecretAsList)
                                    //secret.Append(remainder.Substring(0, lengthOfCharsAtLastIndexInSecretAsList));
                                    //secret.Append(remainder.Substring(0, lengthOfCharsAtLastIndexInSecretAsList - appendedChars));
                                    secret.Append(Substring(remainder, 0, lengthOfCharsAtLastIndexInSecretAsList));
                                break;
                            }
                            iii++;
                        }
                        break;
                    }

                    y = iteration == lengthOfPaddingsAsList && i == iteration - 1 ? lengthOfCharsAtLastIndexInSecretAsList
                        : val.Length - (((i + 1) * lengthOfStringInEachIndexInPaddingsAsList) + (i * lengthOfStringInEachIndexInSecretAsList));
                    if (i == iteration - 1)
                    {
                        if (!isSecretLengthLongerThanPadding)
                            if (iteration == lengthOfPaddingsAsList)
                                x = 0;
                            else
                                x = y - lengthOfCharsAtLastIndexInSecretAsList;
                    }
                    else
                        x = y - lengthOfStringInEachIndexInSecretAsList;
                    //Console.WriteLine("Secret: " + secret);
                    //Console.WriteLine("val: " + val);
                    //secret.Append(val.Substring(x, y));
                    //secret.Append(val.Substring(x, lengthOfCharsAtLastIndexInSecretAsList));
                    secret.Append(Substring(val, x, y));
                }
            }

            val = null;
            return secret.ToString();
        }

        public static async Task<string> Decrypt(char[] val)
        {
            var s = new String(val);
            val = null;
            var parameters = s.Substring(s.IndexOf(PARAMS) + 1);
            int algorithmPosition = Int32.Parse(Substring(parameters, 0, parameters.IndexOf(PARAMS)));
            int algorithmLength = Int32.Parse(Substring(parameters, parameters.IndexOf(PARAMS) + 1, parameters.Length));
            s = Substring(s, 0, s.IndexOf(PARAMS));
            int algorithm = Int32.Parse(s.Substring(algorithmPosition, algorithmLength));
            var res = new StringBuilder(s).Remove(algorithmPosition, algorithmLength).ToString();
            //for (int i = 0; i < algorithm; i++)
            //{
            //    res = Encoding.UTF8.GetString(Convert.FromBase64String(res));
            //}

            //return res;
            return await Task.Run(() => DecryptHelper(algorithm, ref res));
        }

        public static string DecryptHelper(int algorithm, ref string secret)
        {
            string res = secret;
            for (int i = 0; i < algorithm; i++)
            {
                res = Encoding.UTF8.GetString(Convert.FromBase64String(res));
            }
            secret = null;
            return res;
        }

        public static Models.MaritalStatus GetMStatus(IFormCollection form, string key)
        {
            var x = ToProper(form[key].ToString());
            /*
            switch (x.Trim())
            {
                case "Single":
                    return Models.MaritalStatus.SINGLE;
                case "Married":
                    return Models.MaritalStatus.MARRIED;
                case "Divorced":
                    return Models.MaritalStatus.DIVORCED;
                case "Widowed":
                    return Models.MaritalStatus.WIDOWED;
                case "Engaged":
                    return Models.MaritalStatus.ENGAGED;
                default:
                    throw new ArgumentOutOfRangeException("Unknown Marital status", x);
            }
             */
            return x.Trim() switch
            {
                "Single" => Models.MaritalStatus.SINGLE,
                "Married" => Models.MaritalStatus.MARRIED,
                "Divorced" => Models.MaritalStatus.DIVORCED,
                "Widowed" => Models.MaritalStatus.WIDOWED,
                "Engaged" => Models.MaritalStatus.ENGAGED,
                _ => throw new ArgumentOutOfRangeException("Unknown Marital status", x),
            };
        }

        public static DateTime GetDate(string y, string m, string d)
        {
            try
            {
                return new DateTime(Int32.Parse(y), Int32.Parse(m), Int32.Parse(d), DateTime.Now.Hour,
                    DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond,
                    new GregorianCalendar(GregorianCalendarTypes.Localized), DateTimeKind.Utc);
            }
            catch (Exception)
            {
                Console.WriteLine("Year: " + y + ", Month: " + m + ", Day: " + d);
                throw;
            }
        }

        public static IDictionary<string, string> GetIds(IFormCollection form, string key, string numberKey)
        {
            var ids = new Dictionary<string, string>();
            var i = 0;
            while (true)
            {
                var x = form[$"{key} {i}"].ToString();
                x = ToProper(x);
                if (String.IsNullOrEmpty(x) || String.IsNullOrWhiteSpace(x))
                    break;
                switch (x.Trim())
                {
                    case "Voter":
                    case "National":
                    case "International":
                    case "Driver":
                    case "Local":
                    case "Company":
                        ids.Add(x.ToLower(), form[$"{numberKey} {i}"].ToString().Trim());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unknown Id type", x);
                }
                i++;
            }
            return ids.Any() ? ids : null;
        }

        public static IDictionary<string, string> GetMap(IFormCollection form, string key, string value)
        {
            var ids = new Dictionary<string, string>();
            var i = 0;
            while (true)
            {
                var x = form[$"{key} {i}"].ToString();
                if (String.IsNullOrEmpty(x) || String.IsNullOrWhiteSpace(x))
                    break;
                ids.Add(x.ToUpper(), form[$"{value} {i}"].ToString().Trim());
                i++;
            }
            return ids.Any() ? ids : null;
        }

        public static IEnumerable<string> GetMultipleValues(string name, IFormCollection f)
        {
            var i = 0;
            var l = new List<string>();
            while (true)
            {
                var v = f[$"{name} {i}"].ToString();
                if (String.IsNullOrEmpty(v) || String.IsNullOrWhiteSpace(v))
                    break;
                l.Add(v.Trim());
                i++;
            }
            return l.Any() ? l : null;
        }

        private static readonly char[] WhiteSpaceChars = new char[]
        {
            // See this link to get understanding of what's considered a white-space
            // characters in .Net 5.0.
            // https://docs.microsoft.com/en-us/dotnet/api/system.char.iswhitespace?view=net-5.0
            '\u0009'
            , '\u000A'
            , '\u000B'
            , '\u000C'
            , '\u000D'
            , '\u0085'

            , '\u2028'
            , '\u2029'

            , '\u0020'
            , '\u00A0'
            , '\u1680'
            , '\u2000'
            , '\u2001'
            , '\u2002'
            , '\u2003'
            , '\u2004'
            , '\u2005'
            , '\u2006'
            , '\u2007'
            , '\u2008'
            , '\u2009'
            , '\u200A'
            , '\u202F'
            , '\u205F'
            , '\u3000'
        };

        public static string ToProper(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = text.Trim();
            text = Regex.Replace(text, $"({string.Join('|', WhiteSpaceChars)})+", " ", RegexOptions.Singleline);


            CultureInfo culture_info = CultureInfo.InvariantCulture;
            TextInfo text_info = culture_info.TextInfo;
            text = text_info.ToTitleCase(text);

            if (!text.Contains('\''))
            {
                return text;
            }

            StringBuilder sb = new StringBuilder(text);
            // Capitalize letter after the apostrophe
            int i = sb.ToString().IndexOf('\'');
            while (i != -1 && i + 1 < sb.Length)
            {
                sb[i + 1] = Char.ToUpper(sb[i + 1], CultureInfo.InvariantCulture);
                i = sb.ToString().IndexOf('\'', i + 1);
            }

            return sb.ToString();
        }

        public static bool IsEqual<T>(IEnumerable<T> _1, IEnumerable<T> _2)
        {
            if (_1 == null)
                return _2 == null;
            else if (_1.Count() != _2.Count()) return false;
            foreach (var o in _1)
                if (!_2.Contains(o)) return false;
            return true;
        }

        public bool IsEqual<TKey, TValue>(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            // early-exit checks
            if (null == y)
                return null == x;
            if (null == x)
                return false;
            if (object.ReferenceEquals(x, y))
                return true;
            if (x.Count != y.Count)
                return false;

            // check keys are the same
            foreach (TKey k in x.Keys)
                if (!y.ContainsKey(k))
                    return false;

            // check values are the same
            foreach (TKey k in x.Keys)
                if (!x[k].Equals(y[k]))
                    return false;

            return true;
        }

        public static bool IsEqual<T>(T _1, T _2)
        {
            if (_1 is IComparable x && _2 is IComparable y)
                return x.CompareTo(y) == 0;

            return _1 == null ? _2 == null : _1.Equals(_2);
        }

        public static int Hash<T>(T o)
        {
            return o == null ? 0 : o.GetHashCode();
        }

        public static int CombineHash<T>(IEnumerable<T> val)
        {
            int hash = 0;
            foreach (var x in val)
            {
                hash &= Hash(x);
            }
            return hash;
        }

    }
}
