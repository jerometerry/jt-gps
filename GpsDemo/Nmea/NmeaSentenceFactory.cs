using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeromeTerry.GpsDemo.Nmea
{
    /// <summary>
    /// NmeaSentenceFactory is used to create concrete classes from the generic NmeaSentence class
    /// </summary>
    public abstract class NmeaSentenceFactory
    {
        /// <summary>
        /// Hashtable of sentence id's to factories, so we can select the correct factory
        /// by the NMEA sentence id.
        /// </summary>
        private static Dictionary<string, NmeaSentenceFactory> _factories;

        /// <summary>
        /// Catchall factory for sentences that we don't currently support
        /// </summary>
        private static NmeaUnknownFactory _unknownFactory;

        /// <summary>
        /// Initialize the known factories
        /// </summary>
        static NmeaSentenceFactory()
        {
            _factories = new Dictionary<string, NmeaSentenceFactory>();
            _factories["GGA"] = new NmeaGgaFactory();
            _factories["GLL"] = new NmeaGllFactory();
            _unknownFactory = new NmeaUnknownFactory();
        }

        /// <summary>
        /// Get the NmeaSentenceFactory that can create concrete NmeaSentence classes
        /// from the given generic NmeaSentence.
        /// </summary>
        /// <param name="sentence">The generic NmeaSentence class to create a concrete class from</param>
        /// <returns>The NmeaSentenceFactory that can create concrete classes from the given
        /// generic NmeaSentence</returns>
        public static NmeaSentenceFactory GetFactory(NmeaSentence sentence)
        {
            if (sentence == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(sentence.SentenceId))
            {
                return _unknownFactory;
            }

            if (_factories.ContainsKey(sentence.SentenceId))
            {
                return _factories[sentence.SentenceId];
            }
            else
            {
                return _unknownFactory;
            }
        }

        /// <summary>
        /// Deriving NmeaSentenceFactory classes must implement this method to 
        /// create the concrete class from the generic class
        /// </summary>
        /// <param name="sentence">The generic NmeaSentect to create a concrete class for</param>
        /// <returns>The concrete NmeaSentence class</returns>
        public abstract NmeaSentence Create(NmeaSentence sentence);

        public static NmeaSentence CreateConcreteSentence(NmeaSentence sentence)
        {
            if (sentence == null)
            {
                return null;
            }

            string sentendId = sentence.SentenceId;
            NmeaSentenceFactory factory = GetFactory(sentence);
            return factory.Create(sentence);
        }
    }

    public class NmeaUnknownFactory : NmeaSentenceFactory
    {
        public override NmeaSentence Create(NmeaSentence sentence)
        {
            return sentence;
        }
    }

    public class NmeaGllFactory : NmeaSentenceFactory
    {
        public override NmeaSentence Create(NmeaSentence sentence)
        {
            return new NmeaGLL(sentence);
        }
    }

    public class NmeaGgaFactory : NmeaSentenceFactory
    {
        public override NmeaSentence Create(NmeaSentence sentence)
        {
            return new NmeaGGA(sentence);
        }
    }
}
