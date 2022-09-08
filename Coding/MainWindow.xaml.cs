using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Coding
{
    public partial class MainWindow : Window
    {
        private Dictionary<char, string> dictionary = new Dictionary<char, string>()
        {
            { '0', "0001" },
            { '1', "0010" },
            { '2', "0011" },
            { '3', "0100" },
            { '4', "0101" },
            { '5', "0110" },
            { '6', "0111" },
            { '7', "1000" },
            { '8', "1001" },
            { '9', "1010" },
            { '#', "1011" },
            { '?', "1100" }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = Input.Text;
            Bits.Text = "";
            EncodedText.Text = "";
            EncodedBits.Text = "";
            DecodedText.Text = "";
            Check.Text = "";
            Length1.Text = input.Length.ToString();

            if (input != "")
            {
                if (CheckValidText(input))
                {
                    byte[] bytes = Encode(input);

                    string encodedText = ConvertBytesToString(bytes);
                    EncodedText.Text = encodedText;
                    Length2.Text = encodedText.Length.ToString();

                    string decodedText = Decode(encodedText);
                    DecodedText.Text = decodedText;

                    Check.Text = (input == decodedText).ToString();
                }
                else
                {
                    MessageBox.Show("Введенный текст содержит недопустимые символы!");
                }
            }            
        }

        private bool CheckValidText(string text)
        {
            return !Regex.IsMatch(text, @"[^\d#?]+");
        }

        private byte[] Encode(string text)
        {
            List<string> bitsString = new List<string>();

            List<byte> bytes = new List<byte>();
            string[] splittedText = (from Match m in Regex.Matches(text.ToString(), @".{1," + 2 + "}") select m.Value).ToArray();
            foreach (string s in splittedText)
            {
                string bits;
                if (s.Length == 2)
                {
                    bits = dictionary[s[0]] + dictionary[s[1]];
                }
                else
                {
                    bits = "0000" + dictionary[s[0]];
                }
                bytes.Add(Convert.ToByte(bits, 2));

                bitsString.Add(bits);
            }

            Bits.Text = String.Join(" ", bitsString);

            return bytes.ToArray();
        }

        private string Decode(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            List<string> bitsString = new List<string>();
            foreach (byte b in bytes)
            {
                bitsString.Add(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            EncodedBits.Text = String.Join(" ", bitsString);

            StringBuilder endcodedText = new StringBuilder();
            foreach (byte b in bytes)
            {
                string s = Convert.ToString(b, 2).PadLeft(8, '0');
                string s1 = s.Substring(0, s.Length / 2);
                string s2 = s.Substring(s.Length / 2);

                if ((dictionary.ContainsValue(s1) || s1 == "0000") && (dictionary.ContainsValue(s2) || s2 == "0000"))
                {
                    if (dictionary.ContainsValue(s1))
                    {
                        endcodedText.Append(dictionary.FirstOrDefault(x => x.Value == s1).Key);
                    }
                    if (dictionary.ContainsValue(s2))
                    {
                        endcodedText.Append(dictionary.FirstOrDefault(x => x.Value == s2).Key);
                    }
                }
            }

            return endcodedText.ToString();
        }

        private string ConvertBytesToString(byte[] bytes)
        {
            string encodedText = Encoding.UTF7.GetString(bytes);
            //string encodedText = "";
            //foreach (byte b in bytes)
            //{
            //    encodedText += (char)b;
            //}

            return encodedText;
        }
    }
}
