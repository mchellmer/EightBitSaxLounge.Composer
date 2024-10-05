using System;
using System.Collections.Generic;

namespace EightBitSaxLounge.Composer.Mxl.Models
{
    public class ScaleGenerator
    {
        private static readonly Dictionary<int, string[]> CircleOfFifthsScales = new Dictionary<int, string[]>
        {
            { -7, ["Cb", "Db", "Eb", "Fb", "Gb", "Ab", "Bb"] },
            { -6, ["Gb", "Ab", "Bb", "Cb", "Db", "Eb", "F"] },
            { -5, ["Db", "Eb", "F", "Gb", "Ab", "Bb", "C"] },
            { -4, ["Ab", "Bb", "C", "Db", "Eb", "F", "G"] },
            { -3, ["Eb", "F", "G", "Ab", "Bb", "C", "D"] },
            { -2, ["Bb", "C", "D", "Eb", "F", "G", "A"] },
            { -1, ["F", "G", "A", "Bb", "C", "D", "E"] },
            { 0, ["C", "D", "E", "F", "G", "A", "B"] },
            { 1, ["G", "A", "B", "C", "D", "E", "F#"] },
            { 2, ["D", "E", "F#", "G", "A", "B", "C#"] },
            { 3, ["A", "B", "C#", "D", "E", "F#", "G#"] },
            { 4, ["E", "F#", "G#", "A", "B", "C#", "D#"] },
            { 5, ["B", "C#", "D#", "E", "F#", "G#", "A#"] },
            { 6, ["F#", "G#", "A#", "B", "C#", "D#", "E#"] },
            { 7, ["C#", "D#", "E#", "F#", "G#", "A#", "B#"] }
        };

        public static string[] GetScale(int circleOfFifths)
        {
            return CircleOfFifthsScales[circleOfFifths];
        }
    }
}