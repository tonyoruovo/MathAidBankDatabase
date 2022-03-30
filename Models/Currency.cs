using System;
using System.Collections.Generic;

namespace DataBaseTest.Models
{
    public class Currencies
    {

        private static readonly List<ICurrency> CURRENCIES;
        static Currencies()
        {
            CURRENCIES = new List<ICurrency>();
            CURRENCIES.Add(new MediumOfExchange("Afghanistan", "AFN", 971, 2, "Afghan Afghani", "\u006B"));
            CURRENCIES.Add(new MediumOfExchange("African Development Bank", "XUA", 965, 5, "ADB Unit of Account", null));
            CURRENCIES.Add(new MediumOfExchange("Albania", "ALL", 8, 2, "Albanian Lek", "\u004C\u0065\u006B"));
            CURRENCIES.Add(new MediumOfExchange("Algeria", "DZD", 12, 2, "Algerian Dinar", "DA"));
            CURRENCIES.Add(new MediumOfExchange("Angola", "AOA", 973, 2, "Angolan Kwanza", "Kz"));
            CURRENCIES.Add(new MediumOfExchange("Anguilla", "XCD", 951, 2, "East Carribean Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Argentina", "ARS", 32, 2, "Argentine Peso", "$"));
            CURRENCIES.Add(new MediumOfExchange("Armenia", "AMD", 51, 2, "Armenian Dram", "\u058F"));
            CURRENCIES.Add(new MediumOfExchange("Aruba", "AWG", 533, 2, "Aruban Guilder", "\u0192"));
            CURRENCIES.Add(new MediumOfExchange("Australia", "AUD", 36, 2, "Australian Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Azerbaijan", "AZN", 944, 2, "Azerbaijanian Manat", "\u20BC"));
            CURRENCIES.Add(new MediumOfExchange("Bahamas", "BSD", 44, 2, "Bahamian Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Bahrain", "BHD", 48, 3, "Bahraini Dinar", "BD"));
            CURRENCIES.Add(new MediumOfExchange("Bangladesh", "BDT", 50, 2, "Taka",
                    char.ConvertFromUtf32(2547)));
            CURRENCIES.Add(new MediumOfExchange("Barbados", "BBD", 52, 2, "Barbados Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Belarus", "BYR", 974, 0, "Belarrusian Ruble", "Br"));
            CURRENCIES.Add(new MediumOfExchange("Belize", "BZD", 84, 2, "Belize Dollar", "BZ$"));
            CURRENCIES.Add(new MediumOfExchange("Benin", "XOF", 952, 0, "BCEAO", "CFA"));
            CURRENCIES.Add(new MediumOfExchange("Bermuda", "BMD", 60, 2, "Bermudian Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Bhutan", "BTN", 64, 2, "Ngultrum", "Nu"));
            CURRENCIES.Add(new MediumOfExchange("Bolivia", "BOB", 68, 2, "Boliviano", "$B"));
            CURRENCIES.Add(new MediumOfExchange("Bosnia And Herzgovina", "BAM", 977, 2, "Convertible Mark", "KM"));
            CURRENCIES.Add(new MediumOfExchange("Botswana", "PWD", 72, 2, "Pula", "P"));
            CURRENCIES.Add(new MediumOfExchange("Norway", "NOK", 578, 2, "Norwegian Krone", "Kr"));
            CURRENCIES.Add(new MediumOfExchange("Brazil", "BBR", 986, 2, "Brazilian Real", "R$"));
            CURRENCIES.Add(new MediumOfExchange("Brunei Darussalam", "BND", 96, 2, "Brunei Dinar", "B$"));
            CURRENCIES.Add(new MediumOfExchange("Bulgaria", "BGN", 975, 2, "Bulgarian Lev", "\u043B\u0432"));
            CURRENCIES.Add(new MediumOfExchange("Burundi", "BIF", 108, 0, "Burundi Franc", "FBu"));
            CURRENCIES.Add(new MediumOfExchange("Cambodia", "KHR", 116, 2, "Riel", "\u17DB"));
            CURRENCIES.Add(new MediumOfExchange("Cameroon", "XAF", 950, 0, "BEAC", "FCFA"));
            CURRENCIES.Add(new MediumOfExchange("Canada", "CAD", 124, 2, "Canadian Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Cape Verde", "CVE", 132, 2, "Cape Verde Escudo", "$"));
            CURRENCIES.Add(new MediumOfExchange("Cayman Islands", "KYD", 136, 2, "Cayman Islands Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Chile", "CLP", 152, 0, "Chilean Peso", "$"));
            CURRENCIES.Add(new MediumOfExchange("Chile", "CLF", 990, 0, "Unidades de fomento", "UF"));
            CURRENCIES.Add(new MediumOfExchange("China", "CNY", 156, 2, "Yuan Renminbi", "\u00A5"));
            CURRENCIES.Add(new MediumOfExchange("Columbia", "COP", 170, 2, "Columbian Peso", "$"));
            CURRENCIES.Add(new MediumOfExchange("Columbia", "COU", 970, 2, "Unidad de Valor Real", null));
            CURRENCIES.Add(new MediumOfExchange("Comoros", "KMS", 174, 0, "Comoros Franc", "CF"));
            CURRENCIES
                    .Add(new MediumOfExchange("Congo, the Democratic Rebulic of", "CDF", 976, 2, "Congolese Franc", "FC"));
            CURRENCIES.Add(new MediumOfExchange("Costa Rica", "CRC", 188, 2, "Costa Rican Colon", "$"));
            CURRENCIES.Add(new MediumOfExchange("Crotia", "HRK", 191, 2, "Croatian Kuna", "Kn"));
            CURRENCIES.Add(new MediumOfExchange("Cuba", "CUP", 192, 2, "Cuban Peso", "\u20B1"));
            CURRENCIES.Add(new MediumOfExchange("Cuba", "CUC", 931, 2, "Peso Convertible", "CUC$"));
            CURRENCIES.Add(new MediumOfExchange("Curacao", "ANG", 532, 2, "Hetherlands Antilean Guilder", "NA\u0192"));
            CURRENCIES.Add(new MediumOfExchange("Czech Republic", "CZK", 203, 2, "Czech Koruna", "K\u010D"));
            CURRENCIES.Add(new MediumOfExchange("Denmark", "DKK", 208, 2, "Danish Krone", "Kr"));
            CURRENCIES.Add(new MediumOfExchange("Djibouti", "DJF", 262, 0, "Djiboutian Franc", "Fdj"));
            CURRENCIES.Add(new MediumOfExchange("Dominican Republic", "DOP", 214, 2, "Dominican Peso", "RD$"));
            CURRENCIES.Add(new MediumOfExchange("Egypt", "EGP", 818, 2, "Egyptian Pound", "£"));
            CURRENCIES.Add(new MediumOfExchange("El Salvador", "SVC", 222, 2, "El Salvador Colon", "$"));
            CURRENCIES.Add(new MediumOfExchange("Eritrea", "ERN", 232, 2, "Nafka", "Nkf"));
            CURRENCIES.Add(new MediumOfExchange("Ethiopia", "ETB", 230, 2, "Ethiopian Birr", "Br"));
            CURRENCIES.Add(new MediumOfExchange("European Union", "EUR", 978, 2, "European Euro", "\u20aC"));
            CURRENCIES
                    .Add(new MediumOfExchange("Falkland Islands (Malvinas)", "FKP", 238, 2, "Falkland Islands Pound", "£"));
            CURRENCIES.Add(new MediumOfExchange("Fiji", "FJD", 242, 2, "Fiji Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("French Polynesia", "XPF", 953, 0, "CFP Franc", "F"));
            CURRENCIES.Add(new MediumOfExchange("Gambia", "GMD", 270, 2, "Dalasi", "D"));
            CURRENCIES.Add(new MediumOfExchange("Georgia", "GEL", 981, 2, "Lari", "GEL"));
            CURRENCIES.Add(new MediumOfExchange("Ghana", "GHS", 936, 2, "Ghanian Cedi", "\u00A2"));
            CURRENCIES.Add(new MediumOfExchange("Gibraltar", "GIP", 292, 2, "Gibraltar Pound", "£"));
            CURRENCIES.Add(new MediumOfExchange("Guatemala", "GTQ", 320, 2, "Quetzal", "Q"));
            CURRENCIES.Add(new MediumOfExchange("Guinea", "GNF", 324, 0, "Guinea Franc", "FG"));
            CURRENCIES.Add(new MediumOfExchange("Guyana", "GYD", 328, 2, "Guyana Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Haiti", "HTG", 332, 2, "Gourde", "G"));
            CURRENCIES.Add(new MediumOfExchange("Honduras", "HNL", 340, 2, "Lempira", "L"));
            CURRENCIES.Add(new MediumOfExchange("Hong Kong", "HKD", 344, 2, "Hong Kong Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Hungary", "HUF", 348, 2, "Forint", "Ft"));
            CURRENCIES.Add(new MediumOfExchange("Iceland", "ISK", 352, 0, "Iceland Krona", "Kr"));
            CURRENCIES.Add(new MediumOfExchange("India", "INR", 358, 2, "Indian Rupee", "\u20B9"));
            CURRENCIES.Add(new MediumOfExchange("Indonesia", "IDR", 360, 2, "Rupiah", "Rp"));
            CURRENCIES.Add(new MediumOfExchange("International Monetary Fund (IMF)", "XDR", 960, 5,
                    "Special Draw Rights (SDR)", "SDR"));
            CURRENCIES.Add(new MediumOfExchange("Iran", "IRR", 364, 2, "Iranian Rial", "\uFDFC"));
            CURRENCIES.Add(new MediumOfExchange("Iraq", "IQD", 368, 3, "Iraqi Dinar", "IQD"));
            CURRENCIES.Add(new MediumOfExchange("Isreal", "ILS", 376, 2, "New Isreali Sheqel", "\u20AA"));
            CURRENCIES.Add(new MediumOfExchange("Jamaica", "JMD", 388, 2, "Jamaican Dollar", "J$"));
            CURRENCIES.Add(new MediumOfExchange("Japan", "JPY", 392, 0, "Japanese Yen", "\u00A5"));
            CURRENCIES.Add(new MediumOfExchange("Jordan", "JOD", 400, 3, "Jordanian Dinar", "JOD"));
            CURRENCIES.Add(new MediumOfExchange("Khazakstan", "KZT", 398, 2, "Khazakstani Tenge", "\u20B8"));
            CURRENCIES.Add(new MediumOfExchange("Kenya", "KES", 404, 2, "Kenyan Shilling", "KSh"));
            CURRENCIES.Add(new MediumOfExchange("Korea, Democratic Peoples Republic of", "KPW", 408, 2, "North Korean Won",
                    "\u20A9"));
            CURRENCIES.Add(new MediumOfExchange("Korea, Republic of", "KRW", 410, 0, "South Korean Won", "\u20A9"));
            CURRENCIES.Add(new MediumOfExchange("Kuwait", "KWD", 414, 3, "Kuwaiti Dinar", "KWD"));
            CURRENCIES.Add(new MediumOfExchange("Kyrgystan", "KGS", 417, 2, "Kygystani Som", "лв"));
            CURRENCIES.Add(new MediumOfExchange("Laos", "LAK", 418, 2, "Laos Kip", "\u20AD"));
            CURRENCIES.Add(new MediumOfExchange("Latvia", "LVL", 426, 2, "Latvian Lats", "Ls"));
            CURRENCIES.Add(new MediumOfExchange("Lebanon", "LBP", 422, 2, "Lebanese Pound", "L£"));
            CURRENCIES.Add(new MediumOfExchange("Lesotho", "LSL", 426, 2, "Lesotho Loti", "L"));
            CURRENCIES.Add(new MediumOfExchange("Liberia", "LRD", 430, 2, "Liberian Dollar", "L$"));
            CURRENCIES.Add(new MediumOfExchange("Libya", "LYD", 434, 3, "Libyan Dinar", "LD"));
            CURRENCIES.Add(new MediumOfExchange("Lithuania", "LTL", 440, 2, "Lithuanian Litas", "Lt"));
            CURRENCIES.Add(new MediumOfExchange("Macau", "MOP", 446, 2, "Macanesse Petaca", "P"));
            CURRENCIES.Add(new MediumOfExchange("Macedonia", "MKD", 807, 2, "Macedonian Denar", "ден"));
            CURRENCIES.Add(new MediumOfExchange("Madagascar", "MGA", 969, 2, "Malagasy Ariary", "Ar"));
            CURRENCIES.Add(new MediumOfExchange("Malawi", "MWK", 454, 2, "Malawian Kwacha", "MK"));
            CURRENCIES.Add(new MediumOfExchange("Malaysia", "MYR", 458, 2, "Malaysian Ringgit", "RM"));
            CURRENCIES.Add(new MediumOfExchange("Maldives", "MVR", 462, 2, "Maldivian Rifiyaa", "Rf"));
            CURRENCIES.Add(new MediumOfExchange("Mauritania", "MRO", 478, 2, "Mauritanian Ouguiya", "UM"));
            CURRENCIES.Add(new MediumOfExchange("Mauritius", "MUR", 480, 2, "Mauritian Rupee", "MUR"));
            CURRENCIES.Add(new MediumOfExchange("Mexico", "MXN", 484, 2, "Mexican Peso", "$"));
            CURRENCIES.Add(new MediumOfExchange("Mexico", "MXV", 979, 2, "Mexican Unidad de Inversion (UDI)", null));
            CURRENCIES.Add(new MediumOfExchange("Moldolva, Republic of", "MDL", 498, 2, "Moldovan Leu", "L"));
            CURRENCIES.Add(new MediumOfExchange("Mongolia", "MNT", 496, 2, "Mongolian Tugrik", "\u20AE"));
            CURRENCIES.Add(new MediumOfExchange("Morocco", "MAD", 504, 2, "Moroccan Dirham", "MAD"));
            CURRENCIES.Add(new MediumOfExchange("Mozambique", "MZN", 943, 2, "Mozambican Metical", "MT"));
            CURRENCIES.Add(new MediumOfExchange("Myanmar", "MMK", 104, 2, "Kyat", "K"));
            CURRENCIES.Add(new MediumOfExchange("Namibia", "NAD", 516, 2, "Namibian Dollar", "N$"));
            CURRENCIES.Add(new MediumOfExchange("Nepal", "NPR", 624, 2, "Nepalesse Rupee", "NPR"));
            CURRENCIES.Add(new MediumOfExchange("New Zealand", "NZD", 554, 2, "New Zealand Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Nicaragua", "NIO", 558, 2, "Nicaraguan Cordoba Oro", "C$"));
            CURRENCIES.Add(new MediumOfExchange("Nigeria", "NGN", 566, 2, "Nigerian Naira", "\u20A6"));
            CURRENCIES.Add(new MediumOfExchange("Oman", "OMR", 512, 3, "Omani Rial", "OMR"));
            CURRENCIES.Add(new MediumOfExchange("Pakistan", "PKR", 586, 2, "Pakistani Rupee", "OMR"));
            CURRENCIES.Add(new MediumOfExchange("Panama", "PAB", 590, 2, "Panamanian Balboa", "B/."));
            CURRENCIES.Add(new MediumOfExchange("Papua Ne Guinea", "PGK", 598, 2, "Papua new Guinea Kina", "K"));
            CURRENCIES.Add(new MediumOfExchange("Paraguay", "PYG", 600, 0, "Paraguayan Guarani", "\u20B2"));
            CURRENCIES.Add(new MediumOfExchange("Peru", "PEN", 604, 2, "Peruvian Nuevo Sol", "S/."));
            CURRENCIES.Add(new MediumOfExchange("Phillipines", "PHP", 608, 2, "Phillipines Peso", "\u20B1"));
            CURRENCIES.Add(new MediumOfExchange("Poland", "PLN", 985, 2, "Polish Zloty", "zł"));
            CURRENCIES.Add(new MediumOfExchange("Quatar", "QAR", 624, 2, "Quatari Rial", "QAR"));
            CURRENCIES.Add(new MediumOfExchange("Romania", "RON", 946, 2, "Romainian Leu", "L"));
            CURRENCIES.Add(new MediumOfExchange("Russia", "RUB", 643, 2, "Russian Ruble", "руб"));
            CURRENCIES.Add(new MediumOfExchange("Rwanda", "RWF", 646, 0, "Rwandan Franc", "RF"));
            CURRENCIES.Add(new MediumOfExchange("Saint Helena, Ascension and Tristan de Cuna", "SHP", 654, 2,
                    "Saint Helena Pound", "£"));
            CURRENCIES.Add(new MediumOfExchange("Samoa", "WST", 882, 2, "Samoan tālā", "T"));
            CURRENCIES.Add(new MediumOfExchange("Sao Tome and Principe", "STD", 676, 2, "Dobra", "Db"));
            CURRENCIES.Add(new MediumOfExchange("Saudi Arabia", "SAR", 682, 2, "Saudi Riyal", "SAR"));
            CURRENCIES.Add(new MediumOfExchange("Serbia", "RSD", 941, 2, "Serbian Dinar", "Дин."));
            CURRENCIES.Add(new MediumOfExchange("Seychelles", "SCR", 690, 2, "Seychelles Rupee", "SCR"));
            CURRENCIES.Add(new MediumOfExchange("Sierra Leon", "SLL", 694, 2, "Leon", "Le"));
            CURRENCIES.Add(new MediumOfExchange("Singapore", "SGD", 702, 2, "Singapore Dollar", "S$"));
            CURRENCIES.Add(new MediumOfExchange("Sistema Unitario de Compensacion Regional de Pagos (SUCRE)", "XSU", 994, 5,
                    "Sucre", "Sucre"));
            CURRENCIES.Add(new MediumOfExchange("Solomon Islands", "SBD", 90, 2, "Solomon Islands Dollar", "SI$"));
            CURRENCIES.Add(new MediumOfExchange("Somali", "SOS", 706, 2, "Somali Shilling", "So.Sh."));
            CURRENCIES.Add(new MediumOfExchange("South Africa", "ZAR", 710, 2, "South African Rand", "R"));
            CURRENCIES.Add(new MediumOfExchange("Sri Lanka", "LKR", 144, 2, "Sri Lankan Rupee", "LKR"));
            CURRENCIES.Add(new MediumOfExchange("Sudan", "SDG", 938, 2, "Sudanese Pound", "£Sd"));
            CURRENCIES.Add(new MediumOfExchange("Suriname", "SRD", 968, 2, "Surinamese Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Swaziland", "SZL", 748, 2, "Swazi lulangeni", "L"));
            CURRENCIES.Add(new MediumOfExchange("Sweeden", "SEK", 752, 2, "Sweedish Krona", "Kr"));
            CURRENCIES.Add(new MediumOfExchange("Switzerland", "CHF", 756, 2, "Swiss Franc", "Fr"));
            CURRENCIES.Add(new MediumOfExchange("Syria", "SYP", 760, 2, "Syrian Pound", "S£"));
            CURRENCIES.Add(new MediumOfExchange("Taiwan", "TWD", 901, 2, "New Taiwan Dollar", "NT$"));
            CURRENCIES.Add(new MediumOfExchange("Tajikistan", "TJS", 972, 2, "Tajikistani Somoni", "SM"));
            CURRENCIES.Add(new MediumOfExchange("Tanzania", "TZS", 834, 2, "Tanzanian Shilling", "TSh"));
            CURRENCIES.Add(new MediumOfExchange("Thailand", "THB", 764, 2, "Thai Baht", "\u0E3F"));
            CURRENCIES.Add(new MediumOfExchange("Trinidad and Tobago", "TTD", 780, 2, "Trinidad and Tobago Dollar", "TT$"));
            CURRENCIES.Add(new MediumOfExchange("Tunisia", "THD", 788, 3, "Tunisian Dinar", "THD"));
            CURRENCIES.Add(new MediumOfExchange("Turkey", "TRY", 949, 2, "Turkish Lira", "TL"));
            CURRENCIES.Add(new MediumOfExchange("Turkmenistan", "TMT", 934, 2, "New Manat", "m"));
            CURRENCIES.Add(new MediumOfExchange("Uganda", "UGX", 800, 2, "Ugandan Shilling", "USh"));
            CURRENCIES.Add(new MediumOfExchange("Ukraine", "UAH", 980, 2, "Ukrainian Hryvnia", "\u20B4"));
            CURRENCIES.Add(new MediumOfExchange("United Arab Emirates", "AED", 784, 2, "UAE Dirham", "AED"));
            CURRENCIES.Add(new MediumOfExchange("United Kingdom", "GBP", 826, 2, "Pound Sterling", "£"));
            CURRENCIES.Add(new MediumOfExchange("United States Of America", "USD", 840, 2, "United States Dollar", "$"));
            CURRENCIES.Add(new MediumOfExchange("Uruguay", "UYU", 858, 2, "Peso Uruguayo", "$U"));
            CURRENCIES.Add(
                    new MediumOfExchange("Uruguay", "UYI", 940, 0, "Uruguay Peso en Unidades Indexadas (URUIURUI)", null));
            CURRENCIES.Add(new MediumOfExchange("Uzbekistan", "UZS", 860, 2, "Uzbekistani Som", "лв"));
            CURRENCIES.Add(new MediumOfExchange("Vanatu", "VUV", 548, 0, "Vanatu Vatu", "VT"));
            CURRENCIES.Add(new MediumOfExchange("Venezuela", "VEF", 937, 2, "Bolivar Fuerte", "Bs"));
            CURRENCIES.Add(new MediumOfExchange("Vietnam", "VND", 704, 2, "Vietnamese Dong", "\u20AB"));
            CURRENCIES.Add(new MediumOfExchange("Yemen", "YER", 886, 2, "Yemeni Rial", "YER"));
            CURRENCIES.Add(new MediumOfExchange("Zambia", "ZMK", 894, 2, "Zambian Kwacha", "ZK"));
            CURRENCIES.Add(new MediumOfExchange("Zimbabwe", "ZWL", 932, 2, "Zimbabwean Dollar", "Z$"));
            CURRENCIES.Add(new MediumOfExchange("ZZ08_Gold", "XAU", 959, 5, "Gold", null));
            CURRENCIES.Add(new MediumOfExchange("ZZ09_Palladium", "XPD", 964, 5, "Palladium", null));
            CURRENCIES.Add(new MediumOfExchange("ZZ10_Platinium", "XPT", 962, 5, "Platinium", null));
            CURRENCIES.Add(new MediumOfExchange("ZZ11_Silver", "XAG", 961, 5, "Silver", null));
            CURRENCIES.Add(new MediumOfExchange("Crypto Currency", "XBT", -1, 5, "BitCoin", "BTC"));
        }

        public static IReadOnlyList<ICurrency> AvailableCurrencies()
        {
            return CURRENCIES.AsReadOnly();
        }

        public static ICurrency TryParse(int numericCode)
        {
            foreach (ICurrency c in CURRENCIES)
            {
                if (c.NumericCode == numericCode)
                    return c;
            }
            throw new ArgumentException("Unknown currency");
        }

        public static ICurrency TryParse(char first, char mid, char last)
        {
            string iso4217Code = new string(new char[] { first, mid, last }).ToUpper();
            foreach (ICurrency c in CURRENCIES)
            {
                if (c.ISOCode.CompareTo(iso4217Code) == 0)
                    return c;
            }
            throw new ArgumentException("Unknown currency");
        }

        public static ICurrency TryParse(string countryName)
        {
            foreach (ICurrency c in CURRENCIES)
            {
                if (c.CountryName != null && c.CountryName.Equals(countryName))
                    return c;
            }
            return null;
        }

        protected internal class MediumOfExchange : ICurrency
        {
            public MediumOfExchange(string countryName, string isoCode, int numericCode, int fractionalDigits, string displayName, string symbol)
            {
                CountryName = countryName;
                ISOCode = isoCode;
                NumericCode = numericCode;
                FractionalDigits = fractionalDigits;
                DisplayName = displayName;
                Symbol = symbol;
            }

            public string DisplayName
            {
                get;
            }

            public string ISOCode
            {
                get;
            }

            public string Symbol
            {
                get;
            }

            public string CountryName
            {
                get;
            }

            public int FractionalDigits
            {
                get;
            }

            public int NumericCode
            {
                get;
            }

            public int CompareTo(ICurrency to)
            {
                if (NumericCode > to.NumericCode)
                    return 1;
                else if (NumericCode < to.NumericCode)
                    return -1;
                else return ISOCode.CompareTo(to.ISOCode);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(DisplayName, ISOCode, Symbol, CountryName, FractionalDigits, NumericCode);
            }

            public override bool Equals(object obj)
            {
                return obj is ICurrency c ? CompareTo(c) == 0 : false;
            }
        }
    }
    public interface ICurrency : IComparable<ICurrency>
    {
        string DisplayName
        {
            get;
        }

        string ISOCode
        {
            get;
        }

        int FractionalDigits
        {
            get;
        }

        int NumericCode
        {
            get;
        }

        string Symbol
        {
            get;
        }

        string CountryName
        {
            get;
        }
    }
}
