using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// NmeaSentence is used to parse NMEA 0183 sentences
    /// 
    /// Here's an example of an NMEA sentence:
    ///     $GPGSA,A,3,04,05,,09,12,,,24,,,,,2.5,1.3,2.1*39
    /// 
    /// The general syntax of an NMEA sentence with checksum is
    ///     [0, 0]          $
    ///     [1, 2]          Talker ID
    ///     [3, 5]          Sentence ID
    ///     [6, 6]          ,
    ///     [7, n-4]        data fields (CSV)
    ///     [n-3, n-3]      *
    ///     [n-2, n-1]      checksum
    /// 
    /// The general syntax of an NMEA sentence without checksum is
    ///     [0, 0]          $
    ///     [1, 2]          Talker ID
    ///     [3, 5]          Sentence ID
    ///     [6, 6]          ,
    ///     [7, n-1]        data fields (CSV)
    /// </summary>
    public class NmeaSentence
    {
        public string Sentence { get; set; }
        public bool Valid { get; set; }
        public string TalkerId { get; set; }
        public string SentenceId { get; set; }
        public bool HasChecksum { get; set; }
        public byte Checksum { get; set; }
        
        public string[] Fields { get; set; }
        public int FieldCount { get; set; }

        public NmeaSentence()
        {
        }

        public NmeaSentence(NmeaSentence sentence)
        {
            this.Sentence = sentence.Sentence;
            this.Valid = sentence.Valid;
            this.TalkerId = sentence.TalkerId;
            this.SentenceId = sentence.SentenceId;
            this.HasChecksum = sentence.HasChecksum;
            this.Checksum = sentence.Checksum;
            this.Fields = sentence.Fields;
            this.FieldCount = sentence.FieldCount;
        }

        public NmeaSentence(string sentence)
        {
            this.Valid = false;
            this.Sentence = sentence;
            this.FieldCount = 0;
            this.HasChecksum = false;
            this.Checksum = 0;

            if (string.IsNullOrEmpty(sentence))
            {
                return;
            }

            int n = sentence.Length;

            if (n < 6)
            {
                // At a minimum, a sentence should start with something like $GPGGA
                // $, 2 char talker id, 3 char sentence id
                return;
            }

            this.TalkerId = sentence.Substring(1, 2);
            this.SentenceId = sentence.Substring(3, 3);

            int checksumIndex = sentence.LastIndexOf(Constants.NMEA_CHK_MARKER);
            this.HasChecksum = checksumIndex >= 0 && checksumIndex == n - 3;

            string checksum = null;

            int dataFieldStart = 7;
            int dataFieldEnd = n - 1;

            if (this.HasChecksum)
            {
                checksum = sentence.Substring(checksumIndex + 1);
                this.Checksum = byte.Parse(checksum, System.Globalization.NumberStyles.AllowHexSpecifier);
                dataFieldEnd = checksumIndex - 1;
            }

            int dataFieldLen = dataFieldEnd - dataFieldStart + 1;

            if (dataFieldEnd > dataFieldStart && dataFieldStart > 6 && dataFieldEnd < n)
            {
                string dataFields = sentence.Substring(dataFieldStart, dataFieldLen);

                this.Fields = dataFields.Split(',');
                if (this.Fields != null)
                {
                    this.FieldCount = this.Fields.Length;
                }
            }

            Console.WriteLine(sentence);
            if (this.HasChecksum)
            {
                Console.WriteLine("Talker ID: {0} Sentence ID: {1} Field Count {2} Checksum: {3}",
                    this.TalkerId, this.SentenceId, this.FieldCount, this.Checksum.ToString("X").PadLeft(2, '0'));
            }
            else
            {
                Console.WriteLine("Talker ID: {0} Sentence ID: {1} Field Count {2}",
                    this.TalkerId, this.SentenceId, this.FieldCount);
            }
        }
    }
}
