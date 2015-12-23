using System;
using System.Collections.Generic;
using System.Text;

namespace CeasarShiftCipherCSharp
{
    class CeasarShiftCipherEngine
    {
        public string Encrypt(string plainteks, int kunci)
        {
            //konversi string plainteks ke array character
            char[] plainChar = plainteks.ToCharArray();
            int[] ascii = new int[plainChar.Length];

            for (int count=0;count < plainChar.Length; count++)
            {
                //konversi char ke ASCII number
                ascii[count] = Convert.ToInt32(plainChar[count]);

                // Filter A-Z dan a-z 
                // A-Z di ASCII = 65-90
                // a-z di ASCII = 97-122
                if (ascii[count] >= 65 && ascii[count] <= 90)
                {
                    ascii[count] = ((ascii[count] - 65 + kunci) % 26) + 65;
                }
                else if (ascii[count] >= 97 && ascii[count] <= 122)
                {
                    ascii[count] = ((ascii[count] - 97 + kunci) % 26) + 97;
                }

                // konversi ASCII ke char
                plainChar[count] = ((char)(ascii[count]));
            }

            // mengembalikan array char ke string
            return new string(plainChar);
        }

        public string Decrypt(string cipherTeks, int kunci)
        {
            //konversi string cipherteks ke array character
            char[] cipherChar = cipherTeks.ToCharArray();
            int[] ascii = new int[cipherChar.Length + 1];

            for (int count = 0; count <= cipherChar.Length - 1; count++)
            {
                //konversi char ke ASCII number
                ascii[count] = Convert.ToInt32(cipherChar[count]);
                if (ascii[count] >= 65 & ascii[count] <= 90)
                {
                    ascii[count] = ((ascii[count] - 65 - (kunci % 26) + 26)) % 26 + 65;
                }
                else if (ascii[count] >= 97 & ascii[count] <= 122)
                {
                    ascii[count] = ((ascii[count] - 97 - (kunci % 26) + 26)) % 26 + 97;
                }

                //konversi ASCII ke char
                cipherChar[count] = Convert.ToChar(ascii[count]);
            }

            //mengembalikan array char ke string
            return new string(cipherChar);
        }
    }
}
